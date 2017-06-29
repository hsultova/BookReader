using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IUserRepository : IRepository<User>
	{
		bool IsValidLogin(string email, string password);

		void SendPasswordToEmail(string email, string password);

		string GenerateRandomPassword();
	}
}
