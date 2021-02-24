using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class TraineeToCoursesController : Controller
	{
		private ManageDBContext db;
		public TraineeToCoursesController()
		{
			db = new ManageDBContext();
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		// GET: TraineeToCourses
		public ActionResult Index(string searchTrainee)
		{
			var traineetocourses = db.TraineeToCourses
								   .Include(tr => tr.Course)
								   .Include(tr => tr.Trainee);

			if (!String.IsNullOrEmpty(searchTrainee))
			{
				traineetocourses = traineetocourses.Where(
						s => s.Trainee.UserName.Contains(searchTrainee) ||
						s.Trainee.Email.Contains(searchTrainee));
			}

			return View(traineetocourses);
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			//Get Account Trainee
			var roleInDb = (from r in db.Roles where r.Name.Contains("Trainee") select r)
									 .FirstOrDefault();

			var users = db.Users.Where(x => x.Roles.Select(y => y.RoleId)
														 .Contains(roleInDb.Id))
														 .ToList();
			//Get Course
			var courses = db.Courses.ToList();

			var viewModel = new TraineeToCourseViewModel
			{
				Courses = courses,
				Trainees = users,
				TraineeToCourse = new TraineeToCourse()
			};

			return View(viewModel);
		}


		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(TraineeToCourse traineeToCourse)
		{

			if (!ModelState.IsValid)
			{
				return View("~/Views/TraineeToCourseConditions/AssignNullTraineeCourse.cshtml");
			}

			//Check if Trainee Name or Course Name existed or not
			if (db.TraineeToCourses.Any(c => c.TraineeId == traineeToCourse.TraineeId &&
												   c.CourseId == traineeToCourse.CourseId))
			{
				return View("~/Views/TraineeToCourseConditions/AssignExistTraineeCourse.cshtml");
			}

			var newTraineeToCourse = new TraineeToCourse
			{
				TraineeId = traineeToCourse.TraineeId,
				CourseId = traineeToCourse.CourseId
			};

			db.TraineeToCourses.Add(newTraineeToCourse);
			db.SaveChanges();
			return View("~/Views/TraineeToCourseConditions/AssignTraineeCourseSuccess.cshtml");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var traineeInDb = db.TraineeToCourses.SingleOrDefault(trdb => trdb.Id == id);
			if (traineeInDb == null)
			{
				return HttpNotFound();
			}

			db.TraineeToCourses.Remove(traineeInDb);
			db.SaveChanges();
			return RedirectToAction("Index");
		}


		[HttpGet]
		[Authorize(Roles = "Trainee")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var traineeToCourses = db.TraineeToCourses
				.Where(c => c.TraineeId == userId)
				.Include(c => c.Course)
				.Include(c => c.Trainee)
				.ToList();

			return View(traineeToCourses);
		}
	}
}