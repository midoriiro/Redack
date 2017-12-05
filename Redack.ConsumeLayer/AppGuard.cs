using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Redack.ConsumeLayer
{
	public sealed class AppGuard : IDisposable
	{
		private readonly AesManaged _algorithm;

		public AppGuard()
		{
			string keyPassword = "redack.password";
			string keySalt = "redack.salt";

			var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

			string password;
			string salt;

			try
			{
				password = config.AppSettings.Settings[keyPassword].Value;
				salt = config.AppSettings.Settings[keySalt].Value;
			}
			catch (NullReferenceException)
			{
				string[] credential = this.CreateCredential();
				password = credential[0];
				salt = credential[1];

				config.AppSettings.Settings.Add(keyPassword, password);
				config.AppSettings.Settings.Add(keySalt, salt);
				config.Save(ConfigurationSaveMode.Minimal);
			}

			var rng = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 1);

			this._algorithm = new AesManaged
			{
				KeySize = 256,
				BlockSize = 128
			};

			this._algorithm.Key = rng.GetBytes(this._algorithm.KeySize / 8);
			this._algorithm.IV = rng.GetBytes(this._algorithm.BlockSize / 8);
			this._algorithm.Mode = CipherMode.CBC;
		}

		private string[] CreateCredential()
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

			byte[] password = new byte[256];
			rng.GetBytes(password);

			byte[] salt = new byte[128];
			rng.GetBytes(salt);

			return new[]
			{
				Convert.ToBase64String(password),
				Convert.ToBase64String(salt)
			};
		}

		private void EncryptToStream(string data, Stream stream)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			if (data.Length <= 0)
				throw new ArgumentException(nameof(data));
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			var transform = this._algorithm.CreateEncryptor();

			using (var memory = new MemoryStream())
			{
				using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write))
				{
					using (var writer = new StreamWriter(crypto))
					{
						writer.Write(data);
					}

					if (stream.CanWrite)
					{
						byte[] encrypted = memory.ToArray();

						stream.Write(encrypted, 0, encrypted.Length);
					}
				}
			}
		}

		private string DecryptToStream(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			byte[] encrypted = new byte[stream.Length];

			if (stream.CanRead)
				stream.Read(encrypted, 0, encrypted.Length);

			string decrypted = null;

			var transform = this._algorithm.CreateDecryptor();

			using (var memory = new MemoryStream(encrypted))
			{
				using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Read))
				{
					using (StreamReader reader = new StreamReader(crypto))
					{
						decrypted = reader.ReadToEnd();
					}
				}
			}

			return decrypted;
		}

		public void Store(string data, Stream stream)
		{
			this.EncryptToStream(data, stream);
		}

		public string Restore(Stream stream)
		{
			return this.DecryptToStream(stream);
		}

		public void Dispose()
		{
			this._algorithm.Dispose();
		}
	}
}
