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

		[Authorize]
		[HttpGet]
		public IActionResult Index()
		{
			IList<Genre> genres = _genreRepository.LoadList();
			return View(genres);
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
					return RedirectToAction("Index", "Genre");
				}
			}

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			Genre genre = _genreRepository.Load(id);
			var model = new GenreViewModel
			{
				Id = genre.Id,
				Name = genre.Name
			};

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Edit(GenreViewModel model)
		{
			if (ModelState.IsValid)
			{
				List<string> genreNames = _genreRepository.GetGenreNames();

				Genre genre = _genreRepository.Load(model.Id);
				genre.Name = model.Name;

				_genreRepository.Save(genre);

				return RedirectToAction("Index", "Genre");
			}

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Delete(int id)
		{
			Genre genre = _genreRepository.Load(id);

			_genreRepository.Remove(genre);

			return RedirectToAction("Index", "Genre");
		}
	}
}
