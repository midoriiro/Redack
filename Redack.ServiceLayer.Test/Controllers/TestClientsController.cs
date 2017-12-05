using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Net;
using System.Net.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestClientsController : BaseTestController<ClientsController>
	{
		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void GetAll_WithUnauthentifiedUser()
		{
			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void GetAll_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey, false);

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var client = this.CreateClient(push: false);
			client.Name = null;

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var client = this.GetDataSet<Client>()["clients"]["client1"] as Client;

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey, false);

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey, false);

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var client = (Client)this.GetDataSet<Client>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ClientPutRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Put, client.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var client =  (Client)this.GetDataSet<Client>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ClientPutRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Put, int.MaxValue, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var client = this.CreateClient(push: false);

			var body = this.CreateBodyRequest<ClientPutRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Put, client.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var client = (Client)this.GetDataSet<Client>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ClientPutRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Put, client.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var client = (Client)this.GetDataSet<Client>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ClientPutRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Put, client.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Delete, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<Client>()["clients"]["client1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void SignIn_WithValidRequest()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey);

			var body = this.CreateBodyRequest<ClientSignInRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			request.RequestUri = new Uri("http://localhost/api/clients/signin");

			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void SignIn_WithInvalidRequest()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey);

			var body = this.CreateBodyRequest<ClientSignInRequest, Client>(client) as ClientSignInRequest;
			body.Name = null;

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			request.RequestUri = new Uri("http://localhost/api/clients/signin");

			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void SignIn_WithInvalidName()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey);

			var body = this.CreateBodyRequest<ClientSignInRequest, Client>(client) as ClientSignInRequest;
			body.Name = "Redack";

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			request.RequestUri = new Uri("http://localhost/api/clients/signin");

			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void SignIn_WithInvalidPassPhrase()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey);

			var body = this.CreateBodyRequest<ClientSignInRequest, Client>(client) as ClientSignInRequest;
			body.PassPhrase = "Redack";

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			request.RequestUri = new Uri("http://localhost/api/clients/signin");

			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void SignIn_WithBlocked()
		{
			var apikey = (ApiKey)this.GetDataSet<Client>()["apikeys"]["apikey1"];
			var client = this.CreateClient(apikey);

			client.IsBlocked = true;

			using (var repository = this.CreateRepository<Client>())
			{
				repository.Update(client);
				repository.Commit();
			}

			var body = this.CreateBodyRequest<ClientSignInRequest, Client>(client) as ClientSignInRequest;

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			request.RequestUri = new Uri("http://localhost/api/clients/signin");

			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
