using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class TrainerToCoursesController : Controller
	{
		private ManageDBContext _context;
		public TrainerToCoursesController()
		{
			_context = new ManageDBContext();
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		// GET: TraineeToCourses
		public ActionResult Index(string searchTrainee)
		{
			var trainertocourses = _context.TrainerToCourses
								   .Include(tr => tr.Course)
								   .Include(tr => tr.Trainer);

			if (!String.IsNullOrEmpty(searchTrainee))
			{
				trainertocourses = trainertocourses.Where(
						s => s.Trainer.UserName.Contains(searchTrainee) ||
						s.Trainer.Email.Contains(searchTrainee));
			}

			return View(trainertocourses);
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			//Get Account Trainee
			var roleInDb = (from r in _context.Roles where r.Name.Contains("Trainer") select r)
									 .FirstOrDefault();

			var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
														 .Contains(roleInDb.Id))
														 .ToList();
			//Get Course
			var courses = _context.Courses.ToList();

			var viewModel = new TrainerToCourseViewModel
			{
				Courses = courses,
				Trainer = users,
				TrainerToCourse = new TrainerToCourse()
			};

			return View(viewModel);
		}


		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(TrainerToCourse trainerToCourse)
		{

			if (!ModelState.IsValid)
			{
				return View("~/Views/TrainerToCourseConditions/AssignNullTrainerCourse.cshtml");
			}

			//Check if Trainee Name or Course Name existed or not
			if (_context.TraineeToCourses.Any(c => c.TraineeId == trainerToCourse.TrainerId &&
												   c.CourseId == trainerToCourse.CourseId))
			{
				return View("~/Views/TrainerToCourseConditions/AssignExistTrainerCourse.cshtml");
			}

			var newTrainerToCourse = new TrainerToCourse
			{
				TrainerId = trainerToCourse.TrainerId,
				CourseId = trainerToCourse.CourseId
			};

			_context.TrainerToCourses.Add(newTrainerToCourse);
			_context.SaveChanges();
			return View("~/Views/TrainerToCourseConditions/AssignTrainerCourseSuccess.cshtml");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var trainerInDb = _context.TrainerToCourses.SingleOrDefault(trdb => trdb.Id == id);
			if (trainerInDb == null)
			{
				return HttpNotFound();
			}

			_context.TrainerToCourses.Remove(trainerInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Trainer")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var trainerToCourses = _context.TrainerToCourses
				.Where(c => c.TrainerId == userId)
				.Include(c => c.Course)
				.Include(c => c.Trainer)
				.ToList();

			return View(trainerToCourses);
		}
	}
}