using Redack.DomainLayer.Models;
using System;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Models.Request.Post;

namespace Redack.ConsumeLayer
{
	public sealed class RedackClient : HttpClient
	{
		private readonly IsolatedStorageFile _store;

		public RedackClient(
			string name, 
			string host, 
			bool forceSSL = true, 
			HttpServer server = null) : base(server)
		{
			string protocol = forceSSL ? "https" : "http";

			this.BaseAddress = new Uri($"{protocol}://{host}/api");

			this._store = IsolatedStorageFile.GetStore(
				IsolatedStorageScope.User | 
				IsolatedStorageScope.Domain | 
				IsolatedStorageScope.Assembly, 
				null, 
				null);

			if (!_store.FileExists("client_informations"))
			{
				this.Register(name);
			}
		}

		public async Task<HttpResponseMessage> SendAsync(RestRequest request)
		{
			var builder = new UriBuilder(this.BaseAddress);
			builder.Path = $"{builder.Path}/{request.Resource}";

			request.Message.RequestUri = builder.Uri;

			return await base.SendAsync(request.Message);
		}

		private void Register(string name)
		{
			ApiKey apikey = new ApiKey
			{
				Key = ApiKey.GenerateKey(ApiKey.KeySize)
			};

			IEntityRequest content = new ApiKeyPostRequest();
			content.FromEntity(apikey);

			var request = new RestRequest(HttpMethod.Post, "apikeys");
			request.SetObjectContent<JsonMediaTypeFormatter>(content);

			var response = this.SendAsync(request).Result;

			// TODO : get apikey id

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

			response = this.SendAsync(request).Result;

			// TODO : get client response object
		}
	}
}
