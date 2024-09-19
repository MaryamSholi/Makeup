using AutoMapper;
using Makeup.Data;
using Makeup.ViewModel;
using Makeup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Makeup.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;

		public CategoryController(ApplicationDbContext context, IMapper mapper)
		{
			this.context = context;
			this.mapper = mapper;
		}
		public IActionResult Index()
		{
			var categories = context.Categories.ToList();
			var viewModel = mapper.Map<List<CategoryVM>>(categories);
			return View(viewModel);
		}
		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View("Form");
		}
		[Authorize]
		[HttpPost]
		public IActionResult Create(CategoryFormVM categoryFormVM)
		{
			if (!ModelState.IsValid)
			{
				return View("Form", categoryFormVM);
			}
			var category = mapper.Map<Category>(categoryFormVM);

			try
			{
				context.Categories.Add(category);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				ModelState.AddModelError("Name", "Category Name Already Exist");
				return View("Form");
			}
		}
		[Authorize]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var category = context.Categories.Find(id);
			if (category == null)
			{
				return NotFound();
			}
			var viewModel = mapper.Map<CategoryVM>(category);


			return View("Form", viewModel);
		}
		[Authorize]
		[HttpPost]
		public IActionResult Edit(CategoryVM categoryVM)
		{
			if (!ModelState.IsValid)
			{
				return View("Form", categoryVM);
			}
			var category = context.Categories.Find(categoryVM.Id);
			if (category == null)
			{
				return NotFound();
			}

			category.Name = categoryVM.Name;
			category.UpdatedOn = DateTime.Now;
			context.SaveChanges();

			return RedirectToAction("Index");

		}
		[Authorize]
		public IActionResult Details(int id)
		{
			var category = context.Categories
				.Include(b => b.Products)
				.FirstOrDefault(b => b.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			var viewModel = mapper.Map<CategoryVM>(category);
			return View(viewModel);
		}
		[Authorize]
		public IActionResult Delete(int id)
		{
			var category = context.Categories.Find(id);
			if (category == null)
			{
				return NotFound();
			}
			context.Remove(category);
			context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
