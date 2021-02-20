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
    public class ManagesController : Controller
    {
        private ManageDBContext db = new ManageDBContext();
        private ApplicationDbContext _context;
        public ManagesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Manages
        public ActionResult Index(string manageRole, string searchString)
        {
            var RoleLst = new List<string>();

            var RoleQry = from d in db.Manages
                           orderby d.Role
                           select d.Role;

            RoleLst.AddRange(RoleQry.Distinct());
            ViewBag.manageRole = new SelectList(RoleLst);

            var manages = from m in db.Manages
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                manages = manages.Where(s => s.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(manageRole))
            {
                manages = manages.Where(x => x.Role == manageRole);
            }

            return View(manages);
        }

        // GET: Manages/Details/5
        public ActionResult Details(int? id)
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

        // GET: Manages/Create
        [Authorize] //Đã đăng nhập mới được tạo task
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Date,Role,Age,Class")] Manage manage)
        {
            if (ModelState.IsValid)
            {
                db.Manages.Add(manage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manage);
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
