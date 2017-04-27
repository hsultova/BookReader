using System.Linq;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Helpers;
using BookReader.Web.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReader.Web.Controllers
{
	public class GenreController : Controller
	{
		private IGenreRepository GenreRepository;

		public GenreController(IGenreRepository genreRepository)
		{
			this.GenreRepository = genreRepository;
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Create(GenreViewModel model)
		{
			if (ModelState.IsValid)
			{
				var genreNames = GenreRepository.GetGenreNames();
				if (genreNames.Contains(model.Name))
				{
					ModelState.AddModelError("DuplicateGenreName", "The Genre name already exist.");
				}
				else
				{
					var author = new Genre() { Name = model.Name };

					GenreRepository.Add(author);
					return RedirectToAction("Index", "Home");
				}
			}

			return View(model);
		}
	}
}
