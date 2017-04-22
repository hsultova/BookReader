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

		public bool IsValidLogin(string email, string password)
		{
			var users = LoadList(u => u.Email.ToUpper() == email.ToUpper() && u.Password == password);
			if (users.Count > 0)
			{
				return true;
			}

			return false;
		}
	}
}
