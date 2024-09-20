using AutoMapper;
using Makeup.Data;
using Makeup.Models;
using Makeup.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Makeup.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IMapper mapper;

		public UserController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
		{
			this.context = context;
			this.roleManager = roleManager;
			this.userManager = userManager;
			this.mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			var users = await userManager.Users.ToListAsync();
			var userViewModel = new List<ApplicationUserVM>();

			foreach (var user in users)
			{
				var usersVm = mapper.Map<ApplicationUserVM>(user);

     			var roles = await userManager.GetRolesAsync(user);
				usersVm.Roles = roles.ToList();

				userViewModel.Add(usersVm);
			}


			return View(userViewModel);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var roles = await roleManager.Roles.ToListAsync();

			var viewModel = new ApplicationUserCreateVM
			{
				Roles = roles.Select(role => new SelectListItem
				{
					Value = role.Name,
					Text = role.Name
				}).ToList(),
			};
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Create(ApplicationUserCreateVM userCreateVM)
		{
			if (!ModelState.IsValid)
			{
				return View(userCreateVM);
			}
			var user = mapper.Map<ApplicationUser>(userCreateVM);

			var result = await userManager.CreateAsync(user, userCreateVM.PasswordHash);

			if (!result.Succeeded)
			{
				return View(userCreateVM);
			}
			await userManager.AddToRolesAsync(user, userCreateVM.SelectedRoles);
			return RedirectToAction("Index");


		}
        public async Task<IActionResult> UserProfile(int id)
        {
			var userId = userManager.GetUserId(User);
			var user = await userManager.FindByIdAsync(userId);
			var viewModel = mapper.Map<ApplicationUserVM>(user);



			return View(viewModel);
        }
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> UserRegister()
		{
			var roles = await roleManager.Roles.ToListAsync();

			var viewModel = new ApplicationUserCreateVM
			{
				Roles = roles.Select(role => new SelectListItem
				{
					Value = role.Name,
					Text = role.Name
				}).ToList(),
			};
			return View(viewModel);
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> UserRegister(ApplicationUserCreateVM userCreateVM)
		{
			if (!ModelState.IsValid)
			{
				return View(userCreateVM);
			}
			
			var user = mapper.Map<ApplicationUser>(userCreateVM);

			var result = await userManager.CreateAsync(user, userCreateVM.PasswordHash);

			if (!result.Succeeded)
			{
				return View(userCreateVM);
			}
			string selectedRole = "User"; 

			if (selectedRole != null)
			{
				await userManager.AddToRoleAsync(user, selectedRole);
			}
			return RedirectToAction( "Index", "Home");


		}


	}
}
