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
using BookReader.Data.Helpers;

namespace BookReader.Web.Controllers
{
	public class UserController : Controller
	{
		private IUserRepository _userRepository;
		private IRoleRepository _roleRepository;

		public UserController(IUserRepository userRepository,
			IRoleRepository roleRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
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

		[ServiceFilter(typeof(TransactionFilterAttribute))]
		[HttpPost]
		public async Task<IActionResult> Login(LoginRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				bool result = _userRepository.IsValidLogin(model.LoginViewModel.Email, model.LoginViewModel.Password);
				if (result)
				{
					User user = _userRepository.LoadList(u => u.Email == model.LoginViewModel.Email, u => u.Role).First();

					var claims = new List<Claim> {
						new Claim(ClaimTypes.Sid, user.Id.ToString()),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Name, user.FullName),
						new Claim(ClaimTypes.Role, user.Role.Name)
					};
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

					if (user.IsFirstTimeLoggedIn)
					{
						user.IsFirstTimeLoggedIn = false;
						_userRepository.Save(user);

						return RedirectToAction("ChangePassword", "User");
					}

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

		[ServiceFilter(typeof(TransactionFilterAttribute))]
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Login", "User");
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Create()
		{
			var model = new LoginRegisterViewModel();
			model.RegisterViewModel = BuildRegisterViewModel();

			return View(model);
		}

		[ServiceFilter(typeof(TransactionFilterAttribute))]
		[HttpPost]
		public IActionResult Register(LoginRegisterViewModel model)
		{
			int userId = UserHelper.GetCurrentUserId(HttpContext);

			if (ModelState.IsValid)
			{
				IEnumerable<string> emails = _userRepository.LoadList().Select(u => u.Email);
				if (emails.Contains(model.RegisterViewModel.Email))
				{
					ModelState.AddModelError("ExistentEmail", "This Email already exist.");
				}
				else
				{
					string randomPassword = _userRepository.GenerateRandomPassword();

					var user = new User
					{
						Email = model.RegisterViewModel.Email,
						Password = PasswordHasher.CreatePasswordHash(randomPassword),
						Firstname = model.RegisterViewModel.Firstname,
						Lastname = model.RegisterViewModel.Lastname,
						IsFirstTimeLoggedIn = true
					};

					if (userId == 0)
					{
						user.RoleId = 2; //TODO: Add to const
						_userRepository.SendPasswordToEmail(user.Email, randomPassword);
						_userRepository.Add(user);

						return RedirectToAction("Index", "Home");
					}
					else
					{
						user.RoleId = model.RegisterViewModel.RoleId;
						_userRepository.SendPasswordToEmail(user.Email, randomPassword);
						_userRepository.Add(user);

						return RedirectToAction("Index", "User");
					}
				}
			}

			List<Role> roles = _roleRepository.LoadList().ToList();
			model.RegisterViewModel.Roles = SelectListHelper.ToSelectListItem<Role>(roles, x => x.Name, x => x.Id.ToString());
			model.IsRegisterActive = true;

			if (userId == 0)
			{
				return View("Login", model);
			}
			else
			{
				return View("Create", model);
			}
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Index()
		{
			IList<User> users = _userRepository.LoadList();
			return View(users);
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[ServiceFilter(typeof(TransactionFilterAttribute))]
		[HttpPost]
		public IActionResult ForgotPassword(string email)
		{
			User user = _userRepository.LoadList(x => x.Email == email).FirstOrDefault();

			if (user != null)
			{
				string randomPassword = _userRepository.GenerateRandomPassword();
				user.Password = PasswordHasher.CreatePasswordHash(randomPassword);
				_userRepository.SendPasswordToEmail(email, randomPassword);
				_userRepository.Save(user);

				ModelState.AddModelError("SentPassword", "A new password has been sent to your email address.");

				return RedirectToAction("Login");
			}

			ModelState.AddModelError("NoExistedEmail", "The email address " + email + " is not registered. Please try again.");

			return View("ForgotPassword");

		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userId = UserHelper.GetCurrentUserId(HttpContext);
				User user = _userRepository.Load(userId);

				user.Password = PasswordHasher.CreatePasswordHash(model.Password);
				_userRepository.Save(user);

				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			User user = _userRepository.Load(id);
			var model = new UserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				Firstname = user.Firstname,
				Lastname = user.Lastname
			};

			return View(model);
		}

		[ServiceFilter(typeof(TransactionFilterAttribute))]
		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Edit(UserViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = _userRepository.Load(model.Id);
				user.Email = model.Email;
				user.Firstname = model.Firstname;
				user.Lastname = model.Lastname;

				_userRepository.Save(user);

				return RedirectToAction("Index", "User");

			}

			return View(model);
		}

		private UserViewModel BuildRegisterViewModel()
		{
			List<Role> roles = _roleRepository.LoadList().ToList();
			IList<SelectListItem> roleList = SelectListHelper.ToSelectListItem<Role>(roles, x => x.Name, x => x.Id.ToString());
			var model = new UserViewModel
			{
				Roles = roleList
			};

			return model;
		}
	}
}