using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Redack.ConsumeLayer
{
	public sealed class AppGuard : IDisposable
	{
		private readonly string _appFolder;
		private readonly string _tmpFolder;
		private byte[] _key;

		public AppGuard(string key)
		{
			string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			this._appFolder = Path.Combine(localAppDataDirectory, "RedackAppGuard");

			Directory.CreateDirectory(this._appFolder);

			this._tmpFolder = Path.Combine(Path.GetTempPath(), "RedackAppGuard");

			Directory.CreateDirectory(this._tmpFolder);

			this._key = Encoding.UTF8.GetBytes(key);

			this.Store(this._key);
		}

		private byte[] CreateRandomEntropy()
		{
			byte[] entropy = new byte[16];

			new RNGCryptoServiceProvider(entropy);

			return entropy;
		}

		private void EncryptToMemory(byte[] data, MemoryProtectionScope scope)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			else if (data.Length <= 0)
				throw new ArgumentException(nameof(data));

			ProtectedMemory.Protect(data, scope);
		}

		private void DecryptToMemory(byte[] data, MemoryProtectionScope scope)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			else if (data.Length <= 0)
				throw new ArgumentException(nameof(data));

			ProtectedMemory.Unprotect(data, scope);
		}

		private int EncryptToStream(byte[] data, byte[] entropy, Stream stream, DataProtectionScope scope)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));
			else if (data.Length <= 0)
				throw new ArgumentException(nameof(data));
			else if (entropy == null)
				throw new ArgumentNullException(nameof(entropy));
			else if (entropy.Length <= 0)
				throw new ArgumentException(nameof(entropy));
			else if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			byte[] encrypted = ProtectedData.Protect(data, entropy, scope);

			int length = 0;

			if (stream.CanWrite && encrypted != null)
			{
				stream.Write(encrypted, 0, encrypted.Length);
				length = encrypted.Length;
			}

			return length;
		}

		private byte[] DecryptToStream(byte[] entropy, Stream stream, DataProtectionScope scope)
		{
			if (entropy == null)
				throw new ArgumentNullException(nameof(entropy));
			else if (entropy.Length <= 0)
				throw new ArgumentException(nameof(entropy));
			else if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			byte[] buffer = new byte[stream.Length];
			byte[] decrypted;

			if(stream.CanRead)
			{
				stream.Read(buffer, 0, buffer.Length);

				decrypted = ProtectedData.Unprotect(buffer, entropy, scope);
			}
			else
			{
				throw new IOException("Could not read stream");
			}

			return decrypted;
		}

		public void Store(string filename, byte[] data)
		{
			FileStream stream = new FileStream(Path.Combine(this._appFolder, filename), FileMode.OpenOrCreate);

			this.Restore(this._key);

			this.EncryptToStream(data, this._key, stream, DataProtectionScope.CurrentUser);

			this.Store(this._key);
		}

		public void Store(byte[] data)
		{
			this.EncryptToMemory(data, MemoryProtectionScope.SameProcess);
		}

		public byte[] Restore(string filename)
		{
			FileStream stream = new FileStream(Path.Combine(this._appFolder, filename), FileMode.OpenOrCreate);

			byte[] data;

			this.Restore(this._key);

			try
			{
				data = this.DecryptToStream(this._key, stream, DataProtectionScope.CurrentUser);
			}
			catch(IOException e)
			{
				throw e;
			}
			finally
			{
				this.Store(this._key);
			}

			return data;
		}

		public void Restore(byte[] data)
		{
			this.DecryptToMemory(data, MemoryProtectionScope.SameProcess);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this._key = null;
					GC.Collect();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
