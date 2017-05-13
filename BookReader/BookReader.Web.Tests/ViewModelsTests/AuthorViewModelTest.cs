using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BookReader.Web.ViewModels.Author;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookReader.Web.Tests.ViewModelsTests
{
	[TestClass]
	public class AuthorViewModelTest
	{
		[TestMethod]
		public void AuthorViewModelShouldBeInvalid_IfNameIsNull()
		{
			// Arrange
			var model = new AuthorViewModel { Name = null };

			// Act
			var validationResults = new List<ValidationResult>();
			var result = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

			// Assert
			Assert.IsFalse(result, "Validation result should be invalid");
			Assert.AreEqual(validationResults.Count, 1, "Expected 1 validation error");
			Assert.IsNotNull(validationResults.FirstOrDefault(r => r.MemberNames.Contains("Name")), "Member names should contains Name field");
		}
	}
}
