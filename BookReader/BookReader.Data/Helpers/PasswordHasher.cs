using System;
using System.Security.Cryptography;

namespace BookReader.Data.Helpers
{
	public static class PasswordHasher
	{
		private const int SaltByteLength = 24;
		private const int DerivedKeyLength = 24;

		public static string CreatePasswordHash(string password)
		{
			var salt = GenerateRandomSalt();
			var iterationCount = GetIterationCount();
			var hashValue = GenerateHashValue(password, salt, iterationCount);
			var iterationCountBtyeArr = BitConverter.GetBytes(iterationCount);
			var valueToSave = new byte[SaltByteLength + DerivedKeyLength + iterationCountBtyeArr.Length];

			Buffer.BlockCopy(salt, 0, valueToSave, 0, SaltByteLength);
			Buffer.BlockCopy(hashValue, 0, valueToSave, SaltByteLength, DerivedKeyLength);
			Buffer.BlockCopy(iterationCountBtyeArr, 0, valueToSave, salt.Length + hashValue.Length, iterationCountBtyeArr.Length);

			return Convert.ToBase64String(valueToSave);
		}

		private static byte[] GenerateRandomSalt()
		{
			var generator = RandomNumberGenerator.Create();
			var salt = new byte[SaltByteLength];

			generator.GetBytes(salt);

			return salt;
		}

		private static byte[] GenerateHashValue(string password, byte[] salt, int iterationCount)
		{
			byte[] hashValue;
			var valueToHash = string.IsNullOrEmpty(password) ? string.Empty : password;
			using (var pbkdf2 = new Rfc2898DeriveBytes(valueToHash, salt, iterationCount))
			{
				hashValue = pbkdf2.GetBytes(DerivedKeyLength);
			}

			return hashValue;
		}

		public static bool VerifyPassword(string passwordGuess, string actualSavedHashResults)
		{
			//ingredient #1: password salt byte array
			var salt = new byte[SaltByteLength];

			//ingredient #2: byte array of password
			var actualPasswordByteArr = new byte[DerivedKeyLength];

			//convert actualSavedHashResults to byte array
			var actualSavedHashResultsBtyeArr = Convert.FromBase64String(actualSavedHashResults);

			//ingredient #3: iteration count
			var iterationCountLength = actualSavedHashResultsBtyeArr.Length - (salt.Length + actualPasswordByteArr.Length);
			var iterationCountByteArr = new byte[iterationCountLength];

			Buffer.BlockCopy(actualSavedHashResultsBtyeArr, 0, salt, 0, SaltByteLength);
			Buffer.BlockCopy(actualSavedHashResultsBtyeArr, SaltByteLength, actualPasswordByteArr, 0, actualPasswordByteArr.Length);
			Buffer.BlockCopy(actualSavedHashResultsBtyeArr, (salt.Length + actualPasswordByteArr.Length), iterationCountByteArr, 0, iterationCountLength);
			var passwordGuessByteArr = GenerateHashValue(passwordGuess, salt, BitConverter.ToInt32(iterationCountByteArr, 0));

			return AreHashEqual(passwordGuessByteArr, actualPasswordByteArr);
		}

		private static bool AreHashEqual(byte[] passwordGuess, byte[] actualPassword)
		{
			uint difference = (uint)passwordGuess.Length ^ (uint)actualPassword.Length;
			for (var i = 0; i < passwordGuess.Length && i < actualPassword.Length; i++)
			{
				difference |= (uint)(passwordGuess[i] ^ actualPassword[i]);
			}

			return difference == 0;
		}

		private static int GetIterationCount()
		{
			return 24000;
		}
	}
}