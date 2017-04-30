﻿using System.Collections.Generic;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Helpers;
using BookReader.Web.ViewModels.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReader.Web.Controllers
{
	public class AuthorController : Controller
	{
		private IAuthorRepository AuthorRepository;

		public AuthorController(IAuthorRepository authorRepository)
		{
			this.AuthorRepository = authorRepository;
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
				List<string> authorNames = AuthorRepository.GetAuthorNames();
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

					AuthorRepository.Add(author);
					return RedirectToAction("Index", "Home");
				}
			}

			return View(model);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Details(int id)
		{
			Author author = AuthorRepository.Load(id, null, "Books", "Books.Genre");
			var model = new AuthorViewModel
			{
				Id = author.Id,
				Name = author.Name,
				Biography = author.Biography,
				Description = author.Description,
				Website = author.Website,
				Books = author.Books
			};

			return View(model);
		}
	}
}
