using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookReader.Web.Controllers
{
	public class UserController : Controller
	{
		// GET: /<controller>/
		public IActionResult List()
		{
			return View();
		}
	}
}
