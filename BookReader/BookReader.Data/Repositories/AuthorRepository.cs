﻿using System.Collections.Generic;
using System.Linq;
using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class AuthorRepository : Repository<Author>, IAuthorRepository
	{
		public AuthorRepository(BookReaderDbContext context)
			: base(context)
		{
		}

		public IEnumerable<string> GetAuthorNames()
		{
			var list = LoadList().Select(x => x.Name);
			return list;
		}
	}
}
