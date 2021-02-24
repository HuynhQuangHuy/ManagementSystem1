using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class TrainingStaffsController : Controller
	{
		private ManageDBContext db;

		public TrainingStaffsController()
		{
			db = new ManageDBContext();
		}

		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Index()
		{
			//Láy giá trị từ bẳng AspNetUser và liên kết với bảng AspnetRole thông qua bảng AspNetUserRole
			var UserInfor = (from user in db.Users
							 select new
							 /*FROM-IN: xác định nguồn dữ liệu truy vấn (Users). 
                             Nguồn dữ liệu tập hợp những phần tử thuộc kiểu lớp triển khai giao diện IEnumrable*/
							 /*SELECT: chỉ ra các dữ liệu được xuất ra từ nguồn */
							 {
								 UserId = user.Id,
								 Username = user.UserName,
								 Emailaddress = user.Email,
								 RoleName = (from userRole in user.Roles
												 //JOIN kết hợp 2 trường dữ liệu tương ứng
											 join role in db.Roles //JOIN-IN: chỉ ra nguồn kết nối vs nguồn của FROM   
											 on userRole.RoleId          //ON: chỉ ra sự ràng buộc giữa các phần tử  
											 equals role.Id              //EQUALS: chỉ ra căn cứ vs ràng buộc (userRole.RoleId ~~ role.Id)
											 select role.Name).ToList()
							 }
							 ).ToList();



			// gắn giá trị của user with roleName vào UserInRolesViewModel()
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
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(string id)
		{
			var AccountInDB = db.Users.SingleOrDefault(p => p.Id == id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

			db.Users.Remove(AccountInDB);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		//Edit 
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(string id)
		{
			var AccountInDB = db.Users.SingleOrDefault(p => p.Id == id);
			if (AccountInDB == null)
			{
				return HttpNotFound();
			}
			return View(AccountInDB);
		}

		//EDIT
		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(ApplicationUser user)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var UsernameIsExist = db.Users.
								  Any(p => p.UserName.Contains(user.UserName));

			if (UsernameIsExist)
			{
				ModelState.AddModelError("UserName", "Username already existed");
				return View();
			}

			var AccountInDB = db.Users.SingleOrDefault(p => p.Id == user.Id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

			AccountInDB.UserName = user.UserName;



			db.SaveChanges();
			return RedirectToAction("Index");
		}


		//Change password
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult ChangePass(string id)
		{
			var AccountInDB = db.Users.SingleOrDefault(p => p.Id == id);

			if (AccountInDB == null)
			{
				return HttpNotFound();
			}

			//Khai báo biến var userId thuộc Curent.User.Identity và truy cập vào trường Id thông qua GetUserId       
			var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
			userId = AccountInDB.Id;
			if (userId != null) //Nếu userId tồn tại
			{
				//userManager bằng quản lý người dùng mới, mang dữ liệu mới 
				UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
				//Xoá password hiện tại của userManager 
				userManager.RemovePassword(userId);
				//Thay password mới "123456789" cho userManager
				String newPassword = "123456";
				userManager.AddPassword(userId, newPassword);
			}
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}