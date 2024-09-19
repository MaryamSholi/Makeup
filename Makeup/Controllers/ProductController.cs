using AutoMapper;
using Makeup.Data;
using Makeup.ViewModel;
using Makeup.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Makeup.Controllers
{
	[Authorize]
	public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }
		[AllowAnonymous]
        public IActionResult Index()
		{
			var products = context.Products.ToList();
            var viewModel = mapper.Map<List<ProductVm>>(products);
            return View(viewModel);

		}
		public IActionResult Details(int id)
		{
			var product = context.Products
				.Include(p => p.Brand)
						  .Include(p => p.Category)
						  .FirstOrDefault(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			var viewModel = mapper.Map<ProductVm>(product);
			return View(viewModel);
		}
        [HttpGet]
        public IActionResult Create()
        {
            var brands = context.Brands.OrderBy(brand => brand.Name).ToList();
            var categories = context.Categories.OrderBy(category => category.Name).ToList();

            var brandList = brands.Select(brand => new SelectListItem
            {
                Value = brand.Id.ToString(),
                Text = brand.Name
            }).ToList();

            var categoryList = categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToList();

            var viewModel = new ProductFormVM
            {
                Brand = brandList,
                Category = categoryList,
            };

            return View("Create", viewModel);
        }
        [HttpPost]
        public IActionResult Create(ProductFormVM productFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productFormVM);
            }
            string imageName = null;
            if (productFormVM.ImageUrl != null)
            {
           
                imageName = Path.GetFileName(productFormVM.ImageUrl.FileName);
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "img", "Products", imageName);

                // Ensure the directory exists (optional, depending on your setup)
                if (!Directory.Exists(Path.Combine(webHostEnvironment.WebRootPath, "img", "Products")))
                {
                    Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, "img", "Products"));
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    productFormVM.ImageUrl.CopyTo(stream);
                }
            }

            var product = mapper.Map<Product>(productFormVM);
            product.ImageUrl = imageName;

            context.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var product = context.Products.Find(id);
			if (product == null)
			{
				return NotFound();
			}

			var brands = context.Brands.OrderBy(brand => brand.Name).ToList();
			var categories = context.Categories.OrderBy(category => category.Name).ToList();

			var brandList = brands.Select(brand => new SelectListItem
			{
				Value = brand.Id.ToString(),
				Text = brand.Name,
				Selected = brand.Id == product.BrandId
			}).ToList();

			var categoryList = categories.Select(category => new SelectListItem
			{
				Value = category.Id.ToString(),
				Text = category.Name,
				Selected = category.Id == product.CategoryId
			}).ToList();

			var viewModel = new ProductFormVM
			{
				id = product.Id,
				ProductName = product.ProductName,
				Description = product.Description,
				Price = product.Price,
				Brand = brandList,
				Category = categoryList,
			
			};


			return View("Create", viewModel);
		}
		[HttpPost]
		public IActionResult Edit(int id, ProductFormVM productFormVM)
		{
			if (!ModelState.IsValid)
			{
				var brands = context.Brands.OrderBy(brand => brand.Name).ToList();
				var categories = context.Categories.OrderBy(category => category.Name).ToList();

				productFormVM.Brand = brands.Select(brand => new SelectListItem
				{
					Value = brand.Id.ToString(),
					Text = brand.Name
				}).ToList();

				productFormVM.Category = categories.Select(category => new SelectListItem
				{
					Value = category.Id.ToString(),
					Text = category.Name
				}).ToList();

				return View(productFormVM);
			}

			var product = context.Products.Find(id);
			if (product == null)
			{
				return NotFound();
			}

			product.ProductName = productFormVM.ProductName;
			product.Description = productFormVM.Description;
			product.Price = productFormVM.Price;
			product.BrandId = productFormVM.BrandId;
			product.CategoryId = productFormVM.CategoryId;

			if (productFormVM.ImageUrl != null)
			{
				if (!string.IsNullOrEmpty(product.ImageUrl))
				{
					var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, "img", "Products", product.ImageUrl);
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				var imageName = Path.GetFileName(productFormVM.ImageUrl.FileName);
				var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "img", "Products", imageName);

				using (var stream = new FileStream(imagePath, FileMode.Create))
				{
					productFormVM.ImageUrl.CopyTo(stream);
				}

				product.ImageUrl = imageName;
			}

			context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
			var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "img", "Products", product.ImageUrl);
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}


			context.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index");


        }
    }
}
