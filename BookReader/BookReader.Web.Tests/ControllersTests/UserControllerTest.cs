using System.Collections.Generic;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;
using BookReader.Web.Controllers;
using BookReader.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookReader.Web.Tests.ControllersTests
{
	[TestClass]
	public class UserControllerTest
	{
		private Mock<IUserRepository> _mockUserRepository;
		private Mock<IRoleRepository> _mockRoleRepository;

		public UserControllerTest()
		{
			_mockUserRepository = new Mock<IUserRepository>();
			_mockRoleRepository = new Mock<IRoleRepository>();
		}

		[TestMethod]
		public void Login_ShouldReturnLoginRegisterViewModel()
		{
			//Arrange
			_mockRoleRepository.Setup(x => x.LoadList()).Returns(new List<Role>());
			var userController = new UserController(_mockUserRepository.Object, _mockRoleRepository.Object);

			//Act
			var result = userController.Login() as ViewResult;

			//Assert
			Assert.IsInstanceOfType(result.Model, typeof(LoginRegisterViewModel), "Model should be LoginRegisterViewModel");
		}

		[TestMethod]
		public void List_ShouldReturnUserList()
		{
			// Arrange
			var users = new List<User>() {
				new User() {
					Email = "email",
					RoleId = 1,
					Password= "123",
					Firstname = "Test User",
					Lastname = "Test"
				},
				new User() {
					Email = "email2",
					RoleId = 2,
					Password= "123",
					Firstname = "Test User 2",
					Lastname = "Test 2"
				}
			};
			_mockUserRepository.Setup(m => m.LoadList()).Returns(users);

			var controller = new UserController(_mockUserRepository.Object, _mockRoleRepository.Object);

			// Act
			var result = controller.List() as ViewResult;

			// Assert
			Assert.AreEqual(users, result.Model, "List method should return list of users");
		}
	}
}
