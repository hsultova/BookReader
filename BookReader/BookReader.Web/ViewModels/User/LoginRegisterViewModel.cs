namespace BookReader.Web.ViewModels.User
{
	public class LoginRegisterViewModel
	{
		public LoginViewModel LoginViewModel { get; set; }

		public UserViewModel RegisterViewModel { get; set; }

		public bool IsRegisterActive { get; set; }
	}
}
