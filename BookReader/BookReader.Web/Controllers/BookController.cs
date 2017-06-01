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
		private IUserBookRepository _userBookRepository;

		public BookController(IBookRepository bookRepository,
			IGenreRepository genreRepository,
			IAuthorRepository authorRepository,
			IUserBookRepository userBookRepository)
		{
			_bookRepository = bookRepository;
			_genreRepository = genreRepository;
			_authorRepository = authorRepository;
			_userBookRepository = userBookRepository;
		}

		[Authorize]
		[HttpGet]
		public IActionResult Index()
		{
			var model = new List<BookViewModel>();
			IList<Book> books = _bookRepository.LoadList(null, x => x.Author, x => x.Genre);

			foreach (var book in books)
			{
				model.Add(new BookViewModel
				{
					Title = book.Title,
					AuthorName = book.Author.Name,
					GenreName = book.Genre.Name,
					Description = book.Description,
					Date = book.Date.ToString()
				});
			}

			return View(model);
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
					Date = DateTime.Now,
					GenreId = model.GenreId,
					AuthorId = model.AuthorId
				};

				_bookRepository.Add(book);

				return RedirectToAction("Details", "Author", new { Id = model.AuthorId });
			}

			List<Genre> genres = _genreRepository.LoadList().ToList();
			model.Genres = SelectListHelper.ToSelectListItem<Genre>(genres, x => x.Name, x => x.Id.ToString());

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public IActionResult CreateUserBook(int authorId, int bookId, string status)
		{
			var userId = UserHelper.GetCurrentUserId(HttpContext);
			var userBook = new UserBook
			{
				Status = status,
				BookId = bookId,
				UserId = userId
			};

			_userBookRepository.Add(userBook);

			return RedirectToAction("Details", "Author", new { Id = authorId });
		}

		[Authorize]
		[HttpGet]
		public IActionResult UserBookList()
		{
			var model = new List<BookViewModel>();
			var userId = UserHelper.GetCurrentUserId(HttpContext);
			IList<UserBook> userBooks = _userBookRepository.LoadList(x => x.UserId == userId);
			IList<Book> books = _bookRepository.LoadList(null, x => x.Author, x => x.Genre);

			//My books - books with status
			//TODO: Refactoring
			var myBooks = books.Join(userBooks,
				book => book.Id,
				userBook => userBook.BookId,
				(book, userBook) => new { Book = book, UserBook = userBook }).Select(b => new
				{
					b.Book.Title,
					b.Book.Description,
					b.Book.Date,
					b.Book.Author,
					b.Book.Genre,
					b.Book.Id,
					b.UserBook.Status
				});

			foreach (var book in myBooks)
			{
				model.Add(new BookViewModel
				{
					Title = book.Title,
					AuthorName = book.Author.Name,
					GenreName = book.Genre.Name,
					Description = book.Description,
					Date = book.Date.ToString(),
					Status = book.Status
				});
			}

			return View("Index", model);
		}
	}
}
