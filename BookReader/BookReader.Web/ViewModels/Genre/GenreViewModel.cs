using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.Genre
{
	public class GenreViewModel
	{
		[Required]
		public string Name { get; set; }
	}
}
