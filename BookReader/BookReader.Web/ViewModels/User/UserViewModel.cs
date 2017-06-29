using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReader.Web.ViewModels.User
{
	public class UserViewModel
	{
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Firstname { get; set; }

		[Required]
		public string Lastname { get; set; }

		public int RoleId { get; set; }

		public IList<SelectListItem> Roles { get; set; }
	}
}
