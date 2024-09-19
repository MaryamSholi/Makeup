using AutoMapper;
using Makeup.Data;
using Makeup.Models;
using Makeup.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Makeup.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMapper mapper )
		{
			_logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

		public IActionResult Index()
		{
            var categories = context.Categories.ToList();
            var viewModel = mapper.Map<List<CategoryVM>>(categories);
            return View(viewModel);
        }

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
