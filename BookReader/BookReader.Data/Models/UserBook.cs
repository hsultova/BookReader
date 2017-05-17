using System.ComponentModel.DataAnnotations.Schema;

namespace BookReader.Data.Models
{
	public class UserBook : ModelBase
	{
		public string Status { get; set; }

		public int UserId { get; set; }

		public int BookId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		[ForeignKey("BookId")]
		public Book Book { get; set; }
	}
}
