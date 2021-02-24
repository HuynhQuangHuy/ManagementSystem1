using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class CourseController : Controller
	{
		private ManageDBContext db;

		public CourseController()
		{
			db = new ManageDBContext();
		}

		// Courses/Index
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Index(string searchCourse)
		{
			var courses = db.Courses.Include(co => co.Category);

			if (!String.IsNullOrEmpty(searchCourse))
			{
				courses = courses.Where(
					s => s.Name.Contains(searchCourse) ||
					s.Category.Name.Contains(searchCourse));
			}

			return View(courses);
		}

		
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			var viewModel = new CourseCategoryViewModel
			{
				Categories = db.Categories.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/CourseConditions/CreateNullCourse.cshtml");
			}

			//Check if Course Name existed or not
			if (db.Courses.Any(c => c.Name == course.Name &&
										  c.CategoryId == course.CategoryId))
			{
				return View("~/Views/CourseConditions/CreateExistCourse.cshtml");
			}

			var newCourse = new Course
			{
				Name = course.Name,
				Descriptions = course.Descriptions,
				CategoryId = course.CategoryId,
			};

			db.Courses.Add(newCourse);
			db.SaveChanges();

			return View("~/Views/CourseConditions/CreateCourseSuccess.cshtml");
		}

		// Edit Course (Courses/Edit/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(int id)
		{
			var courseInDb = db.Courses.SingleOrDefault(co => co.Id == id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			var viewModel = new CourseCategoryViewModel
			{
				Course = courseInDb,
				Categories = db.Categories.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/CourseConditions/EditNullCourse.cshtml");
			}

			var courseInDb = db.Courses.SingleOrDefault(co => co.Id == course.Id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			courseInDb.Name = course.Name;
			courseInDb.Descriptions = course.Descriptions;
			courseInDb.CategoryId = course.CategoryId;

			db.SaveChanges();
			return View("~/Views/CourseConditions/EditCourseSuccess.cshtml");
		}

		// Delete Course (Courses/Delete/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var courseInDb = db.Courses.SingleOrDefault(co => co.Id == id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			db.Courses.Remove(courseInDb);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}