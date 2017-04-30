using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Helpers;
using BookReader.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
			var model = new LoginRegisterViewModel
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
				bool result = UserRepository.IsValidLogin(model.LoginViewModel.Email, model.LoginViewModel.Password);
				if (result)
				{
					User user = UserRepository.LoadList(u => u.Email == model.LoginViewModel.Email, u => u.Role).First();

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
				IEnumerable<string> emails = UserRepository.LoadList().Select(u => u.Email);
				if (emails.Contains(model.RegisterViewModel.Email))
				{
					ModelState.AddModelError("ExistentEmail", "This Email already exist.");
				}
				else
				{
					var user = new User
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

			List<Role> roles = RoleRepository.LoadList().ToList();
			model.RegisterViewModel.Roles = SelectListHelper.ToSelectListItem<Role>(roles, x => x.Name, x => x.Id.ToString());
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
			List<Role> roles = RoleRepository.LoadList().ToList();
			IList<SelectListItem> roleList = SelectListHelper.ToSelectListItem<Role>(roles, x => x.Name, x => x.Id.ToString());
			var model = new UserViewModel
			{
				Roles = roleList
			};

			return model;
		}
	}
}
