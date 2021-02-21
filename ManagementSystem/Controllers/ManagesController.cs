using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystem.Models;
using Microsoft.AspNet.Identity;

namespace ManagementSystem.Controllers
{
    [Authorize] //Đã đăng nhập mới được tạo task
    public class ManagesController : Controller
    {
        private ManageDBContext db = new ManageDBContext();
        private ManageDBContext _context;
        public ManagesController()
        {
            _context = new ManageDBContext();
        }

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
               
                    .ToList();

            RoleLst.AddRange(RoleQry.Distinct());
            ViewBag.manageRole = new SelectList(RoleLst);

            if (!String.IsNullOrEmpty(searchString))
            {
                manages = db.User
                .Where(t => t.UserId == currentUserId)
                .Select(t => t.Manage)
                .Where(t => t.Name.Contains(searchString))
                .ToList();
                
            }

            if (!string.IsNullOrEmpty(manageRole))
            {
                manages = db.User
              .Where(t => t.UserId == currentUserId)
              .Select(t => t.Manage)
              .Where(t => t.Name.Contains(searchString))
              .ToList();
            }

            return View(manages);
        }

        // GET: Manages/Details/5
        public ActionResult Details(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            //var manages = _context.user
            //    .where(t => t.manageid == id && t.userid == currentuserid)
            //    .select(t => t.manage)

            //    .firstordefault();
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
        
       

        // POST: Manages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Date,Role,Age,Class")] Manage manage)
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
         public ActionResult Create()
        {
            return View();
           
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

           
            return View(manage);
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
