using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookReader.Data.Repositories;

namespace BookReader.Web.Tests
{
	[TestClass]
	public class AuthorControllerTest
	{
		private Mock<IAuthorRepository> mockAuthorRepository;

		public AuthorControllerTest()
		{
			mockAuthorRepository = new Mock<IAuthorRepository>();
		}
	}
}
