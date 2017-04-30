using System.Collections.Generic;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IAuthorRepository : IRepository<Author>
	{
		List<string> GetAuthorNames();
	}
}
