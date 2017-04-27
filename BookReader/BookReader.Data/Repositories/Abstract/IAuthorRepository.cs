﻿using System.Collections.Generic;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IAuthorRepository : IRepository<Author>
	{
		IEnumerable<string> GetAuthorNames();
	}
}