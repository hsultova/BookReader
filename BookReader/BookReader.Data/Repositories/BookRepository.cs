using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class BookRepository : Repository<Book>, IBookRepository
	{
		public BookRepository(BookReaderDbContext context)
			: base(context)
		{
		}
	}
}
