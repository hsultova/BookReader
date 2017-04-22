using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReader.Data.Models
{
	public class Book : ModelBase
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public string Status { get; set; }

		public DateTime Date { get; set; }

		public int AuthorId { get; set; }

		public int GenreId { get; set; }

		[ForeignKey("AuthorId")]
		public Author Author { get; set; }

		[ForeignKey("GenreId")]
		public Genre Genre { get; set; }
	}
}
