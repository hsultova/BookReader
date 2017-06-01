using System.Collections.Generic;
using System.Linq;
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

		public bool IsUserBook(int bookId, int userId)
		{
			List<int> userBooks = LoadList(x => x.UserId == userId).Select(x => x.BookId).ToList();
			bool isAdded = userBooks.Contains(bookId);

			return isAdded;
		}
	}
}
