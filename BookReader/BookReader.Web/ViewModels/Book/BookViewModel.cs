using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReader.Web.ViewModels.Book
{
	public class BookViewModel
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description { get; set; }

		public int AuthorId { get; set; }

		public string AuthorName { get; set; }

		public int GenreId { get; set; }

		public string GenreName { get; set; }

		public string Date { get; set; }

		public bool IsUserBook { get; set; }

		public string Status { get; set; }

		public IList<SelectListItem> Genres { get; set; }
	}
}
