using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class TrainerToTopicsController : Controller
	{
		private ManageDBContext db;

		public TrainerToTopicsController()
		{
			db = new ManageDBContext();
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		// GET: TrainerToTopics
		public ActionResult Index(string searchTrainer)
		{
			var trainertotopicss = db.TrainerToTopics
								   .Include(tr => tr.Topic)
								   .Include(tr => tr.Trainer);

			if (!String.IsNullOrEmpty(searchTrainer))
			{
				trainertotopicss = trainertotopicss.Where(
						s => s.Trainer.UserName.Contains(searchTrainer) ||
						s.Trainer.Email.Contains(searchTrainer));
			}

			return View(trainertotopicss);
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

			//Get Topic
			var topics = db.Topics.ToList();

			var viewModel = new TrainerToTopicViewModel
			{
				Topics = topics,
				Trainers = users,
				TrainerToTopic = new TrainerToTopic()
			};

			return View(viewModel);
		}


		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(TrainerToTopic trainerToTopic)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/TrainerToTopicConditions/AssignNullTrainerTopic.cshtml");
			}

			//Check if Trainer Name or Topic Name existed or not
			if (db.TrainerToTopics.Any(c => c.TrainerId == trainerToTopic.TrainerId &&
												  c.TopicId == trainerToTopic.TopicId))
			{
				return View("~/Views/TrainerToTopicConditions/AssignExistTrainerTopic.cshtml");
			}

			var newTrainerToTopic = new TrainerToTopic
			{
				TrainerId = trainerToTopic.TrainerId,
				TopicId = trainerToTopic.TopicId
			};

			db.TrainerToTopics.Add(newTrainerToTopic);
			db.SaveChanges();
			return View("~/Views/TrainerToTopicConditions/AssignTrainerTopicSuccess.cshtml");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var trainerInDb = db.TrainerToTopics.SingleOrDefault(trdb => trdb.Id == id);
			if (trainerInDb == null)
			{
				return HttpNotFound();
			}

			db.TrainerToTopics.Remove(trainerInDb);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		
		[HttpGet]
		[Authorize(Roles = "Trainer")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var trainerToTopics = db.TrainerToTopics
				.Where(c => c.TrainerId == userId)
				.Include(c => c.Topic)
				.Include(c => c.Trainer)
				.ToList();

			return View(trainerToTopics);
		}
	}
}