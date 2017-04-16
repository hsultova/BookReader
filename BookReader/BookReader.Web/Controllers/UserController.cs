using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookReader.Data.Models;
using BookReader.Data.Repositories;
using BookReader.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookReader.Web.Controllers
{
	public class UserController : Controller
	{
		private IUserRepository UserRepository;
		private IRoleRepository RoleRepository;

		public UserController(IUserRepository userRepository,
			IRoleRepository roleRepository)
		{
			UserRepository = userRepository;
			RoleRepository = roleRepository;
		}

		[HttpGet]
		public IActionResult Register()
		{
			var roles = BuildRoleSelectList();
			var model = new UserViewModel()
			{
				Roles = roles
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Register(UserViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new User()
				{
					Email = model.Email,
					Password = model.Password,
					Firstname = model.Firstname,
					Lastname = model.Lastname,
					IsFirstTimeLoggedIn = true,
					RoleId = model.RoleId
				};

				UserRepository.Add(user);
				return RedirectToAction("Index", "Home");
			}
			model.Roles = BuildRoleSelectList();
			return View(model);
		}

		[HttpGet]
		public IActionResult List()
		{
			return View();
		}

		private List<SelectListItem> BuildRoleSelectList()
		{
			IList<SelectListItem> roleSelectList = new List<SelectListItem>();
			var roles = RoleRepository.LoadList();

			foreach (var role in roles)
			{
				roleSelectList.Add(new SelectListItem()
				{
					Text = role.Name,
					Value = role.Id.ToString()
				});
			}

			return roleSelectList.ToList();
		}
	}
}
