using BookReader.Data.Database;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(BookReaderDbContext context)
			: base(context)
		{
		}
	}
}
