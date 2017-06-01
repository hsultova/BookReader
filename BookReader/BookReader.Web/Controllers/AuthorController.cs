using System.Collections.Generic;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Helpers;
using BookReader.Web.ViewModels.Author;
using BookReader.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReader.Web.Controllers
{
	public class AuthorController : Controller
	{
		private IAuthorRepository _authorRepository;
		private IUserBookRepository _userBookRepository;

		public AuthorController(IAuthorRepository authorRepository,
			IUserBookRepository userBookRepository)
		{
			_authorRepository = authorRepository;
			_userBookRepository = userBookRepository;
		}

		[Authorize]
		[HttpGet]
		public IActionResult Index()
		{
			IList<Author> authors = _authorRepository.LoadList();
			return View(authors);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Create(AuthorViewModel model)
		{
			if (ModelState.IsValid)
			{
				List<string> authorNames = _authorRepository.GetAuthorNames();
				if (authorNames.Contains(model.Name))
				{
					ModelState.AddModelError("DuplicateAuthorName", "The Author name already exist.");
				}
				else
				{
					var author = new Author
					{
						Name = model.Name,
						Description = model.Description,
						Biography = model.Biography,
						Website = model.Website
					};

					_authorRepository.Add(author);
					return RedirectToAction("Index", "Author");
				}
			}

			return View(model);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Details(int id)
		{
			var books = new List<BookViewModel>();
			var userId = UserHelper.GetCurrentUserId(HttpContext);
			Author author = _authorRepository.Load(id, null, "Books", "Books.Genre");

			foreach (var book in author.Books)
			{
				books.Add(new BookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					Description = book.Description,
					AuthorName = book.Author.Name,
					GenreName = book.Genre.Name,
					Date = book.Date.ToString(),
					IsUserBook = _userBookRepository.IsUserBook(book.Id, userId)
				});
			}

			var model = new AuthorViewModel
			{
				Id = author.Id,
				Name = author.Name,
				Biography = author.Biography,
				Description = author.Description,
				Website = author.Website,
				Books = books
			};

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			Author author = _authorRepository.Load(id);
			var model = new AuthorViewModel
			{
				Id = author.Id,
				Name = author.Name,
				Biography = author.Biography,
				Description = author.Description,
				Website = author.Website
			};

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Edit(AuthorViewModel model)
		{
			if (ModelState.IsValid)
			{
				List<string> authorNames = _authorRepository.GetAuthorNames();

				Author author = _authorRepository.Load(model.Id);
				author.Name = model.Name;
				author.Description = model.Description;
				author.Biography = model.Biography;
				author.Website = model.Website;

				_authorRepository.Save(author);

				return RedirectToAction("Index", "Author");

			}

			return View(model);
		}

		[Authorize(Policy = BookReaderPolicies.AdminPolicy)]
		[HttpPost]
		public IActionResult Delete(int id)
		{
			Author author = _authorRepository.Load(id, null, "Books");

			_authorRepository.Remove(author);

			return RedirectToAction("Index", "Author");
		}
	}
}
