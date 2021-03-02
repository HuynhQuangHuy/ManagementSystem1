using ManagementSystem.Models;
using ManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementSystem.Controllers
{
   
	[Authorize(Roles = "Admin")]
	public class AdminsController : Controller
	{
		private ManageDBContext _context;

		public AdminsController()
		{
			_context = new ManageDBContext();
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Profiless()
		{
	
			var UserInfor = (from user in _context.Users
							 select new
							
							 {
								 UserId = user.Id,
								 Username = user.UserName,
								 Emailaddress = user.Email,
								 RoleName = (from userRole in user.Roles
											 join role in _context.Roles 
											 on userRole.RoleId          
											 equals role.Id              
											 select role.Name).ToList()
							 }
							 ).ToList();
			
			var UserWithRole = UserInfor.Select(p => new UserWithRoleViewModel()
			{
				UserId = p.UserId,
				Username = p.Username,
				Email = p.Emailaddress,
				Role = string.Join(",", p.RoleName)
			}
												);

			return View(UserWithRole);
		}
		//DELETE ACCOUNT
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(string id)
		{
			var AccountInDB = _context.Users.SingleOrDefault(p => p.Id == id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

			_context.Users.Remove(AccountInDB);
			_context.SaveChanges();
			return RedirectToAction("Profiless");
		}

		//Edit 
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(string id)
		{
			var AccountInDB = _context.Users.SingleOrDefault(p => p.Id == id);
			if (AccountInDB == null)
			{
				return HttpNotFound();
			}
			return View(AccountInDB);
		}

		//EDIT
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(ApplicationUser user)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var UsernameIsExist = _context.Users.
								  Any(p => p.UserName.Contains(user.UserName));

			if (UsernameIsExist)
			{
				ModelState.AddModelError("UserName", "Username already existed");
				return View();
			}

			var AccountInDB = _context.Users.SingleOrDefault(p => p.Id == user.Id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

			AccountInDB.UserName = user.UserName;

			_context.SaveChanges();
			return RedirectToAction("Profiless");
		}


		//Change password
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult ChangePass(string id)
		{
			var AccountInDB = _context.Users.SingleOrDefault(p => p.Id == id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

   
			//var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
			var userId = AccountInDB.Id;
			if (userId != null) 
			{

				UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
	
				userManager.RemovePassword(userId);
				
				String newPassword = "123456";
				userManager.AddPassword(userId, newPassword);
			}
			_context.SaveChanges();
			return RedirectToAction("Profiless");
		}
	}
}
