﻿using System.Collections.Generic;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IGenreRepository : IRepository<Genre>
	{
		List<string> GetGenreNames();
	}
}
