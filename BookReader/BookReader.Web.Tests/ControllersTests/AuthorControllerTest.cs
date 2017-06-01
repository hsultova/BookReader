using System.Collections.Generic;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Controllers;
using BookReader.Web.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookReader.Web.Tests.ControllersTests
{
	[TestClass]
	public class AuthorControllerTest
	{
		private Mock<IAuthorRepository> _mockAuthorRepository;
		private Mock<IUserBookRepository> _mockUserBookRepository;

		public AuthorControllerTest()
		{
			_mockAuthorRepository = new Mock<IAuthorRepository>();
			_mockUserBookRepository = new Mock<IUserBookRepository>();
		}

		[TestMethod]
		public void Create_ReturnsCreateView_IfModelStateIsInvalid()
		{
			//Arrange
			_mockAuthorRepository.Setup(x => x.LoadList()).Returns(new List<Author>());

			var authorController = new AuthorController(_mockAuthorRepository.Object, _mockUserBookRepository.Object);
			var author = new AuthorViewModel() { Name = "" };

			authorController.ModelState.AddModelError("RequiredName", "Name field is required.");

			//Act
			var result = authorController.Create(author) as ViewResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(null, result.ViewName);
		}

		[TestMethod]
		public void Create_ReturnsIndexView_IfModelStateIsValid()
		{
			//Arrange
			_mockAuthorRepository.Setup(x => x.LoadList()).Returns(new List<Author>());
			_mockAuthorRepository.Setup(x => x.GetAuthorNames()).Returns(new List<string>());

			var authorController = new AuthorController(_mockAuthorRepository.Object, _mockUserBookRepository.Object);
			var author = new AuthorViewModel() { Name = "Name" };

			//Act
			var result = authorController.Create(author) as RedirectToActionResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
		}
	}
}
