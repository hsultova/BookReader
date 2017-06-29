using System.Threading.Tasks;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}
