using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using Redack.DatabaseLayer.DataAccess;
using Redack.ServiceLayer.Models.Request;
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
			var client = this.CreateClient(push: false);

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Client>()["users"]["lambda"] as User);

			var client = this.CreateClient(push: false);

			var body = this.CreateBodyRequest<ClientPostRequest, Client>(client);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
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

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
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
	}
}
