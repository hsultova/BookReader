namespace BookReader.Web.ViewModels
{
	public class LoginRegisterViewModel
	{
		public LoginViewModel LoginViewModel { get; set; }

		public UserViewModel RegisterViewModel { get; set; }

		public bool IsRegisterActive { get; set; }
	}
}
