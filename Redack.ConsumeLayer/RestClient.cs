using Redack.DomainLayer.Models;
using System;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Models.Request.Post;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Redack.ConsumeLayer
{
	public sealed class RedackClient : HttpClient
	{
		private string _name;
		private IsolatedStorageFile _store;
		private Client _client;

		public RedackClient(
			string name, 
			string host, 
			bool secureLayer = true)
		{
			this._name = name;

			string scheme = secureLayer ? "https" : "http";

			this.BaseAddress = new Uri($"{scheme}://{host}/api");

			this.Initialyze();
		}

		public RedackClient(string name, HttpServer server) : base(server)
		{
			this._name = name;

			this.BaseAddress = new Uri("http://locahost/api");

			this.Initialyze();
		}

		private void Initialyze()
		{
			this._store = IsolatedStorageFile.GetStore(
				IsolatedStorageScope.User |
				IsolatedStorageScope.Domain |
				IsolatedStorageScope.Assembly,
				typeof(System.Security.Policy.Url), 
				typeof(System.Security.Policy.Url));

			string filename = $"{this._name}.client_informations";

			using (var guard = new AppGuard())
			{
				if (!_store.FileExists(filename))
				{
					this._client = this.Register(this._name).Result;

					using (var stream = new IsolatedStorageFileStream(filename, FileMode.CreateNew, this._store))
					{
						string json = JsonConvert.SerializeObject(this._client);

						guard.Store(json, stream);
					}
				}
				else
				{
					using (var stream = new IsolatedStorageFileStream(filename, FileMode.Open, this._store))
					{
						string json = guard.Restore(stream);

						this._client = JsonConvert.DeserializeObject<Client>(json);
					}
				}
			}
		}

		public async Task<HttpResponseMessage> SendAsync(RestRequest request)
		{
			var builder = new UriBuilder(this.BaseAddress);
			builder.Path = $"{builder.Path}/{request.Resource}";

			request.Message.RequestUri = builder.Uri;

			return await this.SendAsync(request.Message);
		}

		public async Task<HttpResponseMessage> SendAsync(RestPromise promise)
		{
			return promise.Execute(await this.SendAsync(promise.Request));
		}

		private async Task<Client> Register(string name)
		{
			ApiKey apikey = new ApiKey
			{
				Key = ApiKey.GenerateKey(ApiKey.KeySize)
			};

			IEntityRequest content = new ApiKeyPostRequest();
			content.FromEntity(apikey);

			var request = new RestRequest(HttpMethod.Post, "apikeys");
			request.SetObjectContent<JsonMediaTypeFormatter>(content);

			var response = await this.SendAsync(request);

			if (response.StatusCode == HttpStatusCode.Created)
			{
				apikey.Id = int.Parse(response.Headers.Location.Segments[3]);
			}

			Client client = new Client()
			{
				Name = name,
				PassPhrase = ApiKey.GenerateKey(ApiKey.KeySize),
				ApiKey = apikey
			};

			content = new ClientPostRequest();
			content.FromEntity(client);

			request = new RestRequest(HttpMethod.Post, "clients");
			request.SetObjectContent<JsonMediaTypeFormatter>(content);

			if (response.StatusCode == HttpStatusCode.Created)
			{
				client.Id = int.Parse(response.Headers.Location.Segments[3]);
			}

			return client;
		}
	}
}
