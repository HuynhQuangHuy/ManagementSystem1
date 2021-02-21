using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;

namespace ManagementSystem.Controllers
{
     //Đã đăng nhập mới được tạo task, chỉ user mới được access
    public class ManagesController : Controller
    {
        private ManageDBContext db = new ManageDBContext();
        

        // GET: Manages
        public ActionResult Index(string manageRole, string searchString)
        {
            var currentUserId = User.Identity.GetUserId();

            var RoleLst = new List<string>();

            var RoleQry = from d in db.Manages
                           orderby d.Role
                           select d.Role;
          var manages = db.User
                .Where(t => t.UserId == currentUserId)
                    .Select(t => t.Manage)
                    .Include(t => t.Category)
                    .ToList();

            RoleLst.AddRange(RoleQry.Distinct());
            ViewBag.manageRole = new SelectList(RoleLst);

            if (!String.IsNullOrEmpty(searchString))
            {
                 manages = db.User
                .Where(t => t.UserId == currentUserId)
                .Select(t => t.Manage)
                .Where(t => t.Name.Contains(searchString))
                .Include(t => t.Category)
                .ToList();
                
            }

            if (!string.IsNullOrEmpty(manageRole))
            {
                manages = db.User
              .Where(t => t.UserId == currentUserId)
              .Select(t => t.Manage)
              .Where(t => t.Name.Contains(searchString))
              .Include(t => t.Category)
              .ToList();
            }

            return View(manages);
        }

        // GET: Manages/Details/5
        public ActionResult Details(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            var manages = db.User
               .Where(t => t.ManageId == id && t.UserId == currentUserId)
                   .Select(t => t.Manage)
                   .Include(t => t.Category)
                   .FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manages.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            return View(manage);
        }

        // GET: Manages/Create
        
        public ActionResult Create()
        {
            var viewModel = new ManageCategoriesViewModel()
            {
                Categories = db.Categories.ToList()
            };
            return View(viewModel);
        }

        // POST: Manages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Date,Role,Age,Class,CategoryId")] Manage manage)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var manageCreated = new Manage();
            manageCreated.Name = manage.Name;
            manageCreated.Date = manage.Date;
            manageCreated.Role = manage.Role;
            manageCreated.Age = manage.Age;
            manageCreated.Class = manage.Class;
            manageCreated.CategoryId = manage.CategoryId;

            db.Manages.Add(manageCreated);
            // Add to User table
            var currentUserId = User.Identity.GetUserId();

            var manageUser = new ManageUser()
            {
                ManageId = manageCreated.ID,
                UserId = currentUserId
            };

            db.User.Add(manageUser);

            db.SaveChanges();


            return RedirectToAction("Index");
        }
        

        // GET: Manages/Edit/5
        public ActionResult Edit(int? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manages.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            return View(manage);
        }

        // POST: Manages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Date,Role,Age,Class")] Manage manage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var currentUserId = User.Identity.GetUserId();

            var todoUserInDb = db.User
                .SingleOrDefault(t => t.UserId == currentUserId && t.ManageId== manage.ID);

            if (todoUserInDb == null) return HttpNotFound();

            var manageInDb = db.Manages.SingleOrDefault(t => t.ID == manage.ID);

            manageInDb.Name = manage.Name;
            manageInDb.Date = manage.Date;
            manageInDb.Age = manage.Age;
            manageInDb.Role= manage.Role;
            manageInDb.Class = manage.Class;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Manages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manage manage = db.Manages.Find(id);
            if (manage == null)
            {
                return HttpNotFound();
            }
            return View(manage);
        }

        // POST: Manages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manage manage = db.Manages.Find(id);
            db.Manages.Remove(manage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
