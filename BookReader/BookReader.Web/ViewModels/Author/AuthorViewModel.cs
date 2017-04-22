using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.Author
{
	public class AuthorViewModel
	{
		[Required]
		public string Name { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Description { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Biography { get; set; }

		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Website { get; set; }
	}
}
