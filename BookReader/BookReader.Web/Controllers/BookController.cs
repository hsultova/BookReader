using Microsoft.AspNetCore.Mvc;

namespace BookReader.Web.Controllers
{
	public class BookController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
