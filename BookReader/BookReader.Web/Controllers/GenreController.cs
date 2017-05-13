using System.Collections.Generic;
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
		private IGenreRepository _genreRepository;

		public GenreController(IGenreRepository genreRepository)
		{
			_genreRepository = genreRepository;
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
				List<string> genreNames = _genreRepository.GetGenreNames();

				if (genreNames.Contains(model.Name))
				{
					ModelState.AddModelError("DuplicateGenreName", "The Genre name already exist.");
				}
				else
				{
					var genre = new Genre { Name = model.Name };

					_genreRepository.Add(genre);
					return RedirectToAction("Index", "Home");
				}
			}

			return View(model);
		}
	}
}
