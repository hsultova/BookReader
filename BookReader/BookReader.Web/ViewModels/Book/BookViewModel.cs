using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReader.Web.ViewModels.Book
{
	public class BookViewModel
	{
		[Required]
		public string Title { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description { get; set; }

		public int AuthorId { get; set; }

		public string AuthorName { get; set; }

		public int GenreId { get; set; }

		public IList<SelectListItem> Genres { get; set; }
	}
}
