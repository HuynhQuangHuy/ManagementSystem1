using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Controllers
{
	public class TopicsController : Controller
	{
		private ManageDBContext db;

		public TopicsController()
		{
			db = new ManageDBContext();
		}

		// Topics/Index
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Index(string searchTopic)
		{
			var topics = db.Topics.Include(t => t.Course);

			if (!String.IsNullOrEmpty(searchTopic))
			{
				topics = topics.Where(
					s => s.Name.Contains(searchTopic) ||
					s.Course.Name.Contains(searchTopic));
			}

			return View(topics);
		}

		// Create Topic (Topics/Create)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			var viewModel = new TopicCourseViewModel
			{
				Courses = db.Courses.ToList()
			};
			return View(viewModel);
		}


		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/TopicConditions/CreateNullTopic.cshtml");
			}

			//Check if Topic Name existed or not
			if (db.Topics.Any(c => c.Name == topic.Name &&
										  c.CourseId == topic.CourseId))
			{
				return View("~/Views/TopicConditions/CreateExistTopic.cshtml");
			}

			var newTopic = new Topic
			{
				Name = topic.Name,
				Descriptions = topic.Descriptions,
				CourseId = topic.CourseId,
			};

			db.Topics.Add(newTopic);
			db.SaveChanges();

			return View("~/Views/TopicConditions/CreateTopicSuccess.cshtml");
		}

		// Edit Topic (Topics/Edit/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(int id)
		{
			var topicInDb = db.Topics.SingleOrDefault(t => t.Id == id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			var viewModel = new TopicCourseViewModel
			{
				Topic = topicInDb,
				Courses = db.Courses.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/TopicConditions/EditNullTopic.cshtml");
			}

			var topicInDb = db.Topics.SingleOrDefault(t => t.Id == topic.Id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			topicInDb.Name = topic.Name;
			topicInDb.Descriptions = topic.Descriptions;
			topicInDb.CourseId = topic.CourseId;

			db.SaveChanges();
			return View("~/Views/TopicConditions/EditTopicSuccess.cshtml");
		}

		// Delete Topic (Topics/Delete/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var topicInDb = db.Topics.SingleOrDefault(t => t.Id == id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			db.Topics.Remove(topicInDb);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}