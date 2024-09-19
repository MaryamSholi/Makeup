using Makeup.Data;
using Makeup.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Makeup.Controllers
{
	[Authorize]
	public class RolesController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly RoleManager<IdentityRole> roleManager;

		public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
		{
			this.context = context;
			this.roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			var roles = await roleManager.Roles.ToListAsync();
			var roleVm = roles.Select(role => new RoleVM
			{
				Name = role.Name,
			}).ToList();

			return View(roleVm);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(RoleVM roleVM)
		{
			if (!ModelState.IsValid)
			{
				return View(roleVM);
			}
			var result = await roleManager.CreateAsync(new IdentityRole(roleVM.Name));
			return RedirectToAction("Index");
		}
	}
}
