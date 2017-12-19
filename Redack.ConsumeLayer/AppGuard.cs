using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Redack.DomainLayer.Models;

namespace Redack.ConsumeLayer
{
	public sealed class AppGuard : IDisposable
	{
		private readonly AesManaged _algorithm;
		public IsolatedStorageFile IsolatedStore { get; private set; }

		public AppGuard()
		{
			this.IsolatedStore = IsolatedStorageFile.GetStore(
				IsolatedStorageScope.User |
				IsolatedStorageScope.Domain |
				IsolatedStorageScope.Assembly,
				null,
				null);

			string filename = "credential.json";
			Dictionary<string, string> credential;

			if (!this.IsolatedStore.FileExists(filename))
			{
				credential = this.CreateCredential();

				using (var stream = new IsolatedStorageFileStream(filename, FileMode.CreateNew, this.IsolatedStore))
				{
					string json = JsonConvert.SerializeObject(credential);

					using (var writer = new StreamWriter(stream))
					{
						writer.Write(json);
					}
				}
			}
			else
			{
				using (var stream = new IsolatedStorageFileStream(filename, FileMode.Open, this.IsolatedStore))
				{
					using (var reader = new StreamReader(stream))
					{
						string json = reader.ReadToEnd();

						credential = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
					}
				}
			}

			var rng = new Rfc2898DeriveBytes(
				credential["password"], 
				Encoding.UTF8.GetBytes(credential["salt"]), 
				1);

			this._algorithm = new AesManaged
			{
				KeySize = 256,
				BlockSize = 128
			};

			this._algorithm.Key = rng.GetBytes(this._algorithm.KeySize / 8);
			this._algorithm.IV = rng.GetBytes(this._algorithm.BlockSize / 8);
			this._algorithm.Mode = CipherMode.CBC;
		}

		private Dictionary<string, string> CreateCredential()
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

			byte[] password = new byte[256];
			rng.GetBytes(password);

			byte[] salt = new byte[128];
			rng.GetBytes(salt);

			return new Dictionary<string, string>
			{
				{"password", Convert.ToBase64String(password) },
				{"salt", Convert.ToBase64String(salt) }
			};
		}

		public IsolatedStorageFileStream CreateStream(string filename, FileMode mode)
		{
			return new IsolatedStorageFileStream(filename, mode, this.IsolatedStore);
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
