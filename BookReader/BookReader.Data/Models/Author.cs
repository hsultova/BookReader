using System.Collections.Generic;

namespace BookReader.Data.Models
{
	public class Author :ModelBase
	{
		public Author()
		{
			Books = new List<Book>();
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public string Biography { get; set; }

		public string Website { get; set; }

		public IList<Book> Books { get; set; }
	}
}
