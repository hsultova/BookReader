using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class UserBookRepository : Repository<UserBook>, IUserBookRepository
	{
		public UserBookRepository(BookReaderDbContext context)
			: base(context)
		{
		}
	}
}
