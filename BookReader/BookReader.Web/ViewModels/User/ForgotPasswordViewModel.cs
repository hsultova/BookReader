using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.User
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
