using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using Redack.ServiceLayer.Models.Request;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestCredentialsController : BaseTestController<CredentialsController>
	{
		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Credential>()["users"]["admin"] as User);

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
			this.CreateAuthentifiedUser(this.GetDataSet<Credential>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<Credential>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var parameter = user.Credential.Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Credential>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<Credential>()["users"]["admin"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithDisabledAction()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Credential>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Post);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var request = this.CreateRequest(HttpMethod.Post);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<Credential>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var credential = user.Credential;

			var body = this.CreateBodyRequest<CredentialPutRequest, Credential>(credential);

			var request = this.CreateRequest(HttpMethod.Put, credential.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			var user = (User)this.GetDataSet<Credential>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var credential = user.Credential;

			var body = this.CreateBodyRequest<CredentialPutRequest, Credential>(credential);

			var request = this.CreateRequest(HttpMethod.Put, int.MaxValue, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Credential>()["users"]["admin"] as User);

			var credential = this.CreateCredential(push: false);

			var body = this.CreateBodyRequest<CredentialPutRequest, Credential>(credential);

			var request = this.CreateRequest(HttpMethod.Put, credential.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var user = (User)this.GetDataSet<Credential>()["users"]["lambda"];

			var credential = user.Credential;

			var body = this.CreateBodyRequest<CredentialPutRequest, Credential>(credential);

			var request = this.CreateRequest(HttpMethod.Put, credential.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithDisabledAction()
		{
			var user = (User)this.GetDataSet<Credential>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var parameter = user.Credential.Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.CreateCredential(push: false).Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
