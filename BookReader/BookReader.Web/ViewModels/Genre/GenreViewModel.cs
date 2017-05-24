using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.Genre
{
	public class GenreViewModel
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
