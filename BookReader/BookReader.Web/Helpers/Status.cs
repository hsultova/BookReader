using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.Helpers
{
	public enum Status
	{
		[Display(Name = "Read")]
		Read,
		[Display(Name = "To read")]
		ToRead,
		[Display(Name = "Currently Reading")]
		CurrentlyReading
	}
}
