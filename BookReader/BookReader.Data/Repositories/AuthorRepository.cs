using BookReader.Data.Database;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories
{
	public class AuthorRepository : Repository<Author>, IAuthorRepository
	{
		public AuthorRepository(BookReaderDbContext context)
			: base(context)
		{
		}
	}
}
