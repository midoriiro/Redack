using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System.Net;
using System.Net.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestApiKeysController : BaseTestController<ApiKeysController>
	{
		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

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
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var apikey = this.CreateApiKey(false);

			var body = this.CreateBodyRequest<ApiKeyPostRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var apikey = this.CreateApiKey(false);
			apikey.Key = null;

			var body = this.CreateBodyRequest<ApiKeyPostRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var apikey = this.CreateApiKey();

			var body = this.CreateBodyRequest<ApiKeyPostRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var apikey = this.CreateApiKey(false);

			var body = this.CreateBodyRequest<ApiKeyPostRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var apikey = this.CreateApiKey(false);

			var body = this.CreateBodyRequest<ApiKeyPostRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var client = (Client)this.GetDataSet<ApiKey>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ApiKeyClientPutRequest, ApiKey>(client.ApiKey);

			var request = this.CreateRequest(HttpMethod.Put, client.ApiKey.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var client = (Client)this.GetDataSet<ApiKey>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ApiKeyClientPutRequest, ApiKey>(client.ApiKey);

			var request = this.CreateRequest(HttpMethod.Put, client.ApiKey.Id + 1, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var apikey = this.CreateApiKey(false);
			apikey.Client = this.CreateClient();

			var body = this.CreateBodyRequest<ApiKeyClientPutRequest, ApiKey>(apikey);

			var request = this.CreateRequest(HttpMethod.Put, apikey.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var client = (Client)this.GetDataSet<ApiKey>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ApiKeyClientPutRequest, ApiKey>(client.ApiKey);

			var request = this.CreateRequest(HttpMethod.Put, client.ApiKey.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["lambda"] as User);

			var client = (Client)this.GetDataSet<ApiKey>()["clients"]["client1"];

			var body = this.CreateBodyRequest<ApiKeyClientPutRequest, ApiKey>(client.ApiKey);

			var request = this.CreateRequest(HttpMethod.Put, client.ApiKey.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Delete, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<ApiKey>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<ApiKey>()["apikeys"]["apikey1"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
