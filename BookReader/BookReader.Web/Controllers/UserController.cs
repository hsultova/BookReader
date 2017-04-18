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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			var result = UserRepository.IsValidLogin(model.Email, model.Password);
			if (result)
			{
				var user = UserRepository.LoadList(u => u.Email == model.Email, u => u.Role).First();
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

			ModelState.AddModelError("InvalidLogin", "The email or password is invalid.");
			return View(model);
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
				var emails = UserRepository.LoadList().Select(u => u.Email);
				if (emails.Contains(model.Email))
				{
					ModelState.AddModelError("ExistentEmail", "This Email already exist.");
				}
				else
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
