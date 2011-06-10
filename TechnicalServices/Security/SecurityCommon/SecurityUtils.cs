using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TechnicalServices.Security.SecurityCommon
{
	public class SecurityUtils
	{
		public static byte[] PasswordToHash(string password)
		{
			string hashName = CngAlgorithm.MD5.Algorithm;
			using (HashAlgorithm hash = HashAlgorithm.Create(hashName))
			{
				byte[] data = Encoding.Default.GetBytes(password);
				return hash.ComputeHash(data);
			}
		}

		[Obsolete]
		public static string PasswordToBase64(string password)
		{
			byte[] data = PasswordToHash(password);
			return Convert.ToBase64String(data);
		}
	}
}
