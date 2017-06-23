using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReader.Data.Models
{
	public class Book : ModelBase
	{
		public Book()
		{
			UserBooks = new List<UserBook>();
		}

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime Date { get; set; }

		public int AuthorId { get; set; }

		public int GenreId { get; set; }

		[ForeignKey("AuthorId")]
		public Author Author { get; set; }

		[ForeignKey("GenreId")]
		public Genre Genre { get; set; }

		public IList<UserBook> UserBooks { get; set; }
	}
}
