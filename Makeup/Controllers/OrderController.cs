using AutoMapper;
using Makeup.Data;
using Makeup.ViewModel;
using Makeup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using static System.Reflection.Metadata.BlobBuilder;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Makeup.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
		private readonly UserManager<ApplicationUser> userManager;

		public OrderController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
		{
			this.context = context;
			this.mapper = mapper;
			this.userManager = userManager;
		}
		public IActionResult Index()
		{
            var orders = context.Orders
                .Include(o => o.Product)  
                .Where(o => o.UserId == userManager.GetUserId(User))
                .ToList();
			var ordervm = orders.Select(order => new OrderVM
			{
				Id = order.Id,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				Qty = order.Qty,
				Price = order.Price,
				TotalPrice = order.TotalPrice,
				Product = order.Product.ProductName,
			}).ToList();

            return View(ordervm);
		}
		[HttpGet]
		public IActionResult Create(int id)
		{
			var product = context.Products.Find(id);
			if (product == null)
			{
				return NotFound();
			}

			var viewModel = new OrderCreateVM
			{
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Qty = 1, 
                UserId = userManager.GetUserId(User)
            };
			return View(viewModel);
		}
		[HttpPost]
		public IActionResult Create(OrderCreateVM orderCreateVM)
		{
			if (!ModelState.IsValid)
			{
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);  
                }
                return View("Index", orderCreateVM);
			}
            var product = context.Products.Find(orderCreateVM.ProductId);
            if (product == null) { return NotFound(); }
            var newStock = product.Stock - orderCreateVM.Qty;
            if (orderCreateVM.Qty > product.Stock)
            {
				TempData["ErrorMessage"] = $"The requested quantity exceeds the available stock .";
				return RedirectToAction("Create", orderCreateVM);
            }

            var order = new Order
			{
                ProductId = orderCreateVM.ProductId,
                Qty = orderCreateVM.Qty,
                Price = orderCreateVM.Price,
                TotalPrice = orderCreateVM.TotalPrice,
                OrderStatus = 1,  
                UserId = orderCreateVM.UserId,
                OrderDate = DateTime.Now
            }; 
			

			product.Stock = newStock;
			context.Products.Update(product);


			context.Orders.Add(order);
			context.SaveChanges();



			return RedirectToAction("Index");

		}
        public IActionResult UserOrders()
        {
            var orders = context.Orders
                .Include(o => o.Product)
				.Include(o => o.User)
                .Where(o => o.UserId == userManager.GetUserId(User))
                .ToList();
            var ordervm = orders.Select(order => new OrderAdminVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                Qty = order.Qty,
                Price = order.Price,
                TotalPrice = order.TotalPrice,
                Product = order.Product.ProductName,
				User = order.User.UserName,
				Email = order.User.Email,
                Address = order.User.Address
            }).ToList();

            return View(ordervm);
        }
		[HttpGet]
		public IActionResult StatusUpdate(int id)
		{
			var order = context.Orders
				   .Include(o => o.Product)
				   .Include(o => o.User) 
				   .FirstOrDefault(o => o.Id == id);

			if (order == null)
			{
				return NotFound();
			}

			var model = new OrderAdminVM
			{
				Id = order.Id,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				Qty = order.Qty,
				Price = order.Price,
				TotalPrice = order.TotalPrice,
				Product = order.Product.ProductName,
				User = order.User.UserName,
				Email = order.User.Email,
				Address = order.User.Address
			};

			return View(model);
		}
		[HttpPost]
		public IActionResult StatusUpdate(OrderAdminVM orderAdminVM)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				foreach (var error in errors)
				{
					Console.WriteLine(error.ErrorMessage);  
				}
				return View("StatusUpdate", orderAdminVM);
			}
			var order = context.Orders.Find(orderAdminVM.Id);
			if (order == null)
			{
				return NotFound();
			}

			order.OrderStatus = orderAdminVM.OrderStatus;
			context.SaveChanges();

			return RedirectToAction("UserOrders");
		}


	}
}
