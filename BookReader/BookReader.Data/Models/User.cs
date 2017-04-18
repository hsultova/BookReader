using System.ComponentModel.DataAnnotations.Schema;

namespace BookReader.Data.Models
{
	public class User : ModelBase
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public bool IsFirstTimeLoggedIn { get; set; }

		public int RoleId { get; set; }

		public string FullName
		{
			get
			{
				return Firstname + " " + Lastname;
			}
		}

		[ForeignKey("RoleId")]
		public Role Role { get; set; }
	}
}
