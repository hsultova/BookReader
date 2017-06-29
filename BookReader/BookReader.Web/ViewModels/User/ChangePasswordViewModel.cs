using System.ComponentModel.DataAnnotations;

namespace BookReader.Web.ViewModels.User
{
	public class ChangePasswordViewModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Confirm Password")]
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Passwords does not match.")]
		public string ConfirmPassword { get; set; }
	}
}
