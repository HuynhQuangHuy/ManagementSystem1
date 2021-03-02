using System;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Models;

namespace ManagementSystem.Controllers
{
	public class CategoriesController : Controller
	{
		private ManageDBContext _context;

		public CategoriesController()
		{
			_context = new ManageDBContext();
		}

		// Categories/Index
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Index(string searchCategory)
		{
			var categories = _context.Categories.ToList();

			if (!String.IsNullOrEmpty(searchCategory))
			{
				categories = categories.FindAll(s => s.Name.Contains(searchCategory));
			}

			return View(categories);
		}

		// Create Category (Categories/Create)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/CategoryCondition/CreateNullCategory.cshtml");
			}

			//Check if Category Name existed or not
			if (_context.Categories.Any(c => c.Name.Contains(category.Name)))
			{
				return View("~/Views/CategoryCondition/CreateExistCategory.cshtml");
			}

			var newCategory = new Category
			{
				Name = category.Name,
				Descriptions = category.Descriptions
			};

			_context.Categories.Add(newCategory);
			_context.SaveChanges();
			return View("~/Views/CategoryCondition/CreateCategorySuccess.cshtml");


		}

		// Edit Category (Categories/Edit/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == id);

			if (categoryInDb == null)
			{
				return HttpNotFound();
			}
			return View(categoryInDb);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Edit(Category category)
		{

			if (!ModelState.IsValid)
			{
				return View();
			}

			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);

			if (categoryInDb == null)
			{
				return HttpNotFound();
			}

			categoryInDb.Name = category.Name;
			categoryInDb.Descriptions = category.Descriptions;

			_context.SaveChanges();
			return View("~/Views/CategoryCondition/EditCategorySuccess.cshtml");
		}

		// Delete Category (Categories/Delete/Id/...)
		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Delete(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == id);

			if (categoryInDb == null)
			{
				return HttpNotFound();
			}

			_context.Categories.Remove(categoryInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}