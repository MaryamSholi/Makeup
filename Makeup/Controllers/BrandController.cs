using AutoMapper;
using Makeup.Data;
using Makeup.ViewModel;
using Makeup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Makeup.Controllers
{
	public class BrandController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;

		public BrandController(ApplicationDbContext context, IMapper mapper)
		{
			this.context = context;
			this.mapper = mapper;
		}
		public IActionResult Index()
		{
			var brands = context.Brands.ToList();
			var viewModel = mapper.Map<List<BrandVM>>(brands);
			return View(viewModel);
		}
		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View("Form");
		}
		[HttpPost]
		public IActionResult Create(BrandFormVM brandFormVM)
		{
			if (!ModelState.IsValid)
			{
				return View("Form", brandFormVM);
			}
			var brand = mapper.Map<Brand>(brandFormVM);

			try
			{
				context.Brands.Add(brand);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				ModelState.AddModelError("Name", "Brand Name Already Exist");
				return View("Form");
			}
		}
		[Authorize]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var brand = context.Brands.Find(id);
			if (brand == null)
			{
				return NotFound();
			}
			var viewModel = mapper.Map<BrandVM>(brand);


			return View("Form", viewModel);
		}
		[Authorize]
		[HttpPost]
		public IActionResult Edit(BrandVM brandVM)
		{
			if (!ModelState.IsValid)
			{
				return View("Form", brandVM);
			}
			var brand = context.Brands.Find(brandVM.Id);
			if (brand == null)
			{
				return NotFound();
			}

			brand.Name = brandVM.Name;
			brand.UpdatedOn = DateTime.Now;
			context.SaveChanges();

			return RedirectToAction("Index");

		}
		[Authorize]
		public IActionResult Details(int id)
		{
            var brand = context.Brands
				.Include(b=> b.Products)
				.FirstOrDefault(b => b.Id == id);
			if (brand == null)
			{
				return NotFound();
			}
			var viewModel = mapper.Map<BrandVM>(brand);
			return View(viewModel);
		}
		[Authorize]
		public IActionResult Delete(int id)
		{
			var brand = context.Brands.Find(id);
			if (brand == null)
			{
				return NotFound();
			}
			context.Remove(brand);
			context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
