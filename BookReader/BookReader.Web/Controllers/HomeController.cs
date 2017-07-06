using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookReader.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[Authorize]
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			try
			{
				string currentUser = User.Identity.IsAuthenticated ? @User.Identity.Name : "No User";

				IExceptionHandlerFeature feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
				Exception ex = feature?.Error;

				_logger.LogError("UNHANDLED_EXCEPTION", ex, "Unhandled exception for User={1}!", currentUser);
			}
			catch
			{
				//Do nothing.
			}
			return View();
		}
	}
}
