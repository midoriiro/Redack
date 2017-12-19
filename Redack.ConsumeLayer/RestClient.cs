using Newtonsoft.Json;
using Redack.BridgeLayer.Messages.Request;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.DomainLayer.Models;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Redack.ConsumeLayer
{
	public sealed class RestClient : HttpClient
	{
		private readonly string _name;
		private readonly AppGuard _guard;
		public static Client Client;
		private static RestClient _instance;

		private RestClient(
			string name, 
			string host, 
			bool secureLayer = true)
		{
			this._name = name;
			this._guard = new AppGuard();

			string scheme = secureLayer ? "https" : "http";
			this.BaseAddress = new Uri($"{scheme}://{host}/api");
		}

		/*public RestClient(string name, HttpServer server) : base()
		{
			this._name = name;

			this.BaseAddress = new Uri("http://locahost/api");

			this.Initialyze();
		}*/

		public static RestClient CreateInstance(string name, string host, bool secureLayer = true)
		{
			return RestClient._instance ?? (RestClient._instance = new RestClient(name, host, secureLayer));
		}

		public static RestClient GetInstance()
		{
			return RestClient._instance;
		}

		public async void Initialyze()
		{
			string filename = $"{this._name}.client_informations";

			if (!this._guard.IsolatedStore.FileExists(filename))
			{
				RestClient.Client = await this.Register(this._name);

				using (var stream = this._guard.CreateStream(filename, FileMode.CreateNew))
				{
					string json = JsonConvert.SerializeObject(RestClient.Client);

					this._guard.Store(json, stream);
				}
			}
			else
			{
				using (var stream = this._guard.CreateStream(filename, FileMode.Open))
				{
					string json = this._guard.Restore(stream);

					RestClient.Client = JsonConvert.DeserializeObject<Client>(json);
				}
			}
		}

		public string GetHostname()
		{
			return this.BaseAddress.Host;
		}

		public async Task<HttpResponseMessage> SendAsync(RestRequest request)
		{
			var builder = new UriBuilder(this.BaseAddress);
			builder.Path = $"{builder.Path}/{request.Resource}";
			builder.Query = request.Query;

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
			request.SetObjectContent(content);

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
			request.SetObjectContent(content);

			if (response.StatusCode == HttpStatusCode.Created)
			{
				client.Id = int.Parse(response.Headers.Location.Segments[3]);
			}

			return client;
		}
	}
}
