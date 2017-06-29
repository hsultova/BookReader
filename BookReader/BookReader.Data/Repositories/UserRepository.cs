using System;
using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private IEmailSender _emailSender;

		public UserRepository(BookReaderDbContext context, IEmailSender emailSender)
			: base(context)
		{
			_emailSender = emailSender;
		}

		public string GenerateRandomPassword()
		{
			int passwordLength = 10;

			string lower = "abcdefghijklmnopqrstuvwxyz";
			string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string digits = "0123456789";
			string symbols = "!#$%&*/?@";
			string allowedChars = lower + upper + digits + symbols;

			char[] chars = new char[passwordLength];

			var random = new Random();

			chars[random.Next(0, passwordLength)] = lower[random.Next(0, lower.Length)];
			chars[random.Next(0, passwordLength)] = upper[random.Next(0, upper.Length)];
			chars[random.Next(0, passwordLength)] = digits[random.Next(0, digits.Length)];
			chars[random.Next(0, passwordLength)] = symbols[random.Next(0, symbols.Length)];

			for (int i = 0; i < passwordLength; i++)
			{
				if (chars[i] == '\0')
				{
					chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
				}
			}

			return new string(chars);
		}

		public bool IsValidLogin(string email, string password)
		{
			var users = LoadList(u => u.Email.ToUpper() == email.ToUpper() && u.Password == password);
			if (users.Count > 0)
			{
				return true;
			}

			return false;
		}

		public async void SendPasswordToEmail(string email, string password)
		{
			string subject = "Account information";
			string message = "Your password is " + password;

			await _emailSender.SendEmailAsync(email, subject, message);
		}
	}
}
