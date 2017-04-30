using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.Author
{
	public class AuthorViewModel
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Biography { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Website { get; set; }

		public IList<Data.Models.Book> Books { get; set; }
	}
}
