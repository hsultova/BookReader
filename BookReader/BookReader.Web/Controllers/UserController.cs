using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookReader.Data.Models;
using BookReader.Data.Repositories;
using BookReader.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BookReader.Web.Helpers;

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
		public IActionResult Login()
		{
			var model = new LoginRegisterViewModel()
			{
				RegisterViewModel = BuildRegisterViewModel()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = UserRepository.IsValidLogin(model.LoginViewModel.Email, model.LoginViewModel.Password);
				if (result)
				{
					var user = UserRepository.LoadList(u => u.Email == model.LoginViewModel.Email, u => u.Role).First();
					var claims = new List<Claim> {
						new Claim(ClaimTypes.Sid, user.Id.ToString()),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Name, user.FullName),
						new Claim(ClaimTypes.Role, user.Role.Name)
					};
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("InvalidLogin", "The email or password is invalid.");
					model.RegisterViewModel = BuildRegisterViewModel();
					return View(model);
				}
			}

			model.RegisterViewModel = BuildRegisterViewModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Login", "User");
		}

		[HttpPost]
		public IActionResult Register(LoginRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var emails = UserRepository.LoadList().Select(u => u.Email);
				if (emails.Contains(model.RegisterViewModel.Email))
				{
					ModelState.AddModelError("ExistentEmail", "This Email already exist.");
				}
				else
				{
					var user = new User()
					{
						Email = model.RegisterViewModel.Email,
						Password = model.RegisterViewModel.Password,
						Firstname = model.RegisterViewModel.Firstname,
						Lastname = model.RegisterViewModel.Lastname,
						IsFirstTimeLoggedIn = true,
						RoleId = model.RegisterViewModel.RoleId
					};

					UserRepository.Add(user);
					return RedirectToAction("Index", "Home");
				}
			}
			model.RegisterViewModel.Roles = BuildRoleSelectList();
			model.IsRegisterActive = true;
			return View("Login", model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult List()
		{
			return View();
		}

		private UserViewModel BuildRegisterViewModel()
		{
			var roles = BuildRoleSelectList();
			var model = new UserViewModel()
			{
				Roles = roles
			};

			return model;
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
