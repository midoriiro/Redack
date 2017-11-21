using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using RestSharp;
using System;
using System.IO.IsolatedStorage;

namespace Redack.ConsumeLayer
{
	public sealed class RedackClient : RestClient
	{
		private static readonly IsolatedStorageFile _store = = IsolatedStorageFile.GetStore(
			IsolatedStorageScope.User |
			IsolatedStorageScope.Assembly |
			IsolatedStorageScope.Application,
			null,
			null);

		public RedackClient(string name, string host, bool forceSSL = true)
		{
			string protocol = forceSSL ? "https" : "http";

			this.BaseHost = host;
			this.BaseUrl = new Uri($"{protocol}://{this.BaseHost}/api");

			if (!_store.FileExists("client_informations"))
			{
				this.Register(name);
			}
		}

		private void Register(string name)
		{
			ApiKey apikey = new ApiKey();
			apikey.Key = ApiKey.GenerateKey(ApiKey.KeySize);			

			var request = new RestRequest("apikeys", Method.POST);
			request.AddJsonBody(apikey);

			var response = this.Execute(request);

			// TODO : get apikey id

			Client client = new Client()
			{
				Name = name,
				PassPhrase = ApiKey.GenerateKey(ApiKey.KeySize),
				ApiKey = apikey
			};

			request = new RestRequest("clients", Method.POST);
			request.AddJsonBody(client);

			response = this.Execute(request);

			// TODO : get client response object
		}
	}
}
