using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;

namespace ManagementSystem.Controllers
{
	public class TrainerProfilesController : Controller
	{
		private ManageDBContext db;
		public TrainerProfilesController()
		{
			db = new ManageDBContext();
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		// GET: TrainerProfiles
		public ActionResult Index(string searchTrainerProfile)
		{
			var trainerProfiles = db.TrainerProfiles.Include(tp => tp.Trainer);

			if (!String.IsNullOrEmpty(searchTrainerProfile))
			{
				trainerProfiles = trainerProfiles.Where(
						s => s.Trainer.UserName.Contains(searchTrainerProfile) ||
						s.Trainer.Email.Contains(searchTrainerProfile));
			}
			return View(trainerProfiles);
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			//Get Account Trainer
			var roleInDb = (from r in db.Roles where r.Name.Contains("Trainer") select r)
									 .FirstOrDefault();

			var users = db.Users.Where(x => x.Roles.Select(y => y.RoleId)
														 .Contains(roleInDb.Id))
														 .ToList();

			var trainerProfiles = db.TrainerProfiles.ToList();

			var trainerProfile = new TrainerProfile
			{
				Trainers = users,

			};
			return View(trainerProfile);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(TrainerProfile trainerProfile)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/TrainerProfileConditions/CreateNullTrainerProfile.cshtml");
			}

			//Check if Trainer Profile existed or not
			if (db.TrainerProfiles.Any(c => c.TrainerId == trainerProfile.TrainerId))
			{
				return View("~/Views/TrainerProfileConditions/CreateExistTrainerProfile.cshtml");
			}
			var getTrainerProfile = new TrainerProfile
			{
				TrainerId = trainerProfile.TrainerId,
				Full_Name = trainerProfile.Full_Name,
				External_Internal = trainerProfile.External_Internal,
				Education = trainerProfile.Education,
				Working_Place = trainerProfile.Working_Place,
				Phone_Number = trainerProfile.Phone_Number
			};

			db.TrainerProfiles.Add(getTrainerProfile);
			db.SaveChanges();
			return View("~/Views/TrainerProfileConditions/CreateTrainerProfileSuccess.cshtml");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var trainerProfileInDb = db.TrainerProfiles.SingleOrDefault(trdb => trdb.Id == id);
			if (trainerProfileInDb == null)
			{
				return HttpNotFound();
			}

			db.TrainerProfiles.Remove(trainerProfileInDb);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff, Trainer")]
		public ActionResult Edit(int id)
		{
			var trainerProfileInDb = db.TrainerProfiles.SingleOrDefault(trdb => trdb.Id == id);

			if (trainerProfileInDb == null)
			{
				return HttpNotFound();
			}

			return View(trainerProfileInDb);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff, Trainer")]
		public ActionResult Edit(TrainerProfile trainerProfile)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var trainerProfileInDb = db.TrainerProfiles.SingleOrDefault(trdb => trdb.Id == trainerProfile.Id);

			if (trainerProfileInDb == null)
			{
				return HttpNotFound();
			}

			trainerProfileInDb.TrainerId = trainerProfile.TrainerId;
			trainerProfileInDb.Full_Name = trainerProfile.Full_Name;
			trainerProfileInDb.External_Internal = trainerProfile.External_Internal;
			trainerProfileInDb.Education = trainerProfile.Education;
			trainerProfileInDb.Phone_Number = trainerProfile.Phone_Number;
			trainerProfileInDb.Working_Place = trainerProfile.Working_Place;
			trainerProfileInDb.Phone_Number = trainerProfile.Phone_Number;
			db.SaveChanges();

			if (User.IsInRole("TrainingStaff"))
			{
				return RedirectToAction("Index");
			}

			if (User.IsInRole("Trainer"))
			{
				return RedirectToAction("Mine");
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Trainer")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var trainerProfiles = db.TrainerProfiles
				.Where(c => c.TrainerId == userId)
				.Include(c => c.Trainer)
				.ToList();

			return View(trainerProfiles);
		}
	}
}