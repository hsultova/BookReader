using System.Collections.Generic;
using System.Linq;
using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class GenreRepository : Repository<Genre>, IGenreRepository
	{
		public GenreRepository(BookReaderDbContext context)
			: base(context)
		{
		}

		public List<string> GetGenreNames()
		{
			var list = LoadList().Select(x => x.Name).ToList();
			return list;
		}
	}
}
