using System;
using System.Collections.Generic;
using System.Linq;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Helpers;
using BookReader.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReader.Web.Controllers
{
	public class BookController : Controller
	{
		private IBookRepository _bookRepository;
		private IGenreRepository _genreRepository;
		private IAuthorRepository _authorRepository;

		public BookController(IBookRepository bookRepository,
			IGenreRepository genreRepository,
			IAuthorRepository authorRepository)
		{
			_bookRepository = bookRepository;
			_genreRepository = genreRepository;
			_authorRepository = authorRepository;
		}

		[Authorize]
		[HttpGet]
		public IActionResult Create(int authorId)
		{
			List<Genre> genres = _genreRepository.LoadList().ToList();
			Author author = _authorRepository.Load(authorId);

			var model = new BookViewModel
			{
				Genres = SelectListHelper.ToSelectListItem<Genre>(genres, x => x.Name, x => x.Id.ToString()),
				AuthorId = authorId,
				AuthorName = author.Name
			};

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Create(BookViewModel model)
		{
			if (ModelState.IsValid)
			{
				var book = new Book
				{
					Title = model.Title,
					Description = model.Description,
					Date = DateTime.Today,
					GenreId = model.GenreId,
					Status = model.Status.ToString(),
					AuthorId = model.AuthorId
				};

				_bookRepository.Add(book);
				//TODO: Add a new user book with created book ID and current user ID

				return RedirectToAction("Details", "Author", new { Id = model.AuthorId });
			}

			List<Genre> genres = _genreRepository.LoadList().ToList();
			model.Genres = SelectListHelper.ToSelectListItem<Genre>(genres, x => x.Name, x => x.Id.ToString());

			return View(model);
		}
	}
}
