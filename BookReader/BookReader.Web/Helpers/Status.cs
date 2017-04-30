using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.Helpers
{
	public enum Status
	{
		[Display(Name = "Read")]
		Read = 1,
		[Display(Name = "To read")]
		ToRead = 2,
		[Display(Name = "Currently Reading")]
		CurrentlyReading = 3
	}
}
