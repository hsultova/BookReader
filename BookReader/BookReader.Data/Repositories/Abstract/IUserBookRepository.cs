﻿using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IUserBookRepository : IRepository<UserBook>
	{
		//Is a book added to my books list
		bool IsUserBook(int bookId, int userId);
	}
}
