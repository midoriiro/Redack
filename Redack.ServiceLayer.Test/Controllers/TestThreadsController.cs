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
using Redack.ServiceLayer.Models.Request.Put;
using Redack.ServiceLayer.Models.Request.Post;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestThreadsController : BaseTestController<ThreadsController>
	{
		public TestThreadsController()
		{
			this.CreateThread();
		}

		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void GetAll_WithUnauthentifiedUser()
		{
			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void GetAll_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var parameter = this.CreateThread().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.CreateThread().Id; ;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["lambda"] as User);

			var parameter = this.CreateThread().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread(push: false);

			var body = this.CreateBodyRequest<ThreadPostRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread(push: false);
			thread.Description = null;

			var body = this.CreateBodyRequest<ThreadPostRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread();

			var body = this.CreateBodyRequest<ThreadPostRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var thread = this.CreateThread(push: false);

			var body = this.CreateBodyRequest<ThreadPostRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["lambda"] as User);

			var thread = this.CreateThread(push: false);

			var body = this.CreateBodyRequest<ThreadPostRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread();

			var body = this.CreateBodyRequest<ThreadPutRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Put, thread.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread();

			var body = this.CreateBodyRequest<ThreadPutRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Put, int.MaxValue, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var thread = this.CreateThread(push: false);

			var body = this.CreateBodyRequest<ThreadPutRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Put, thread.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var thread = this.CreateThread();

			var body = this.CreateBodyRequest<ThreadPutRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Put, thread.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["lambda"] as User);

			var thread = this.CreateThread();

			var body = this.CreateBodyRequest<ThreadPutRequest, Thread>(thread);

			var request = this.CreateRequest(HttpMethod.Put, thread.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var parameter = this.CreateThread().Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Delete, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.CreateThread().Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Thread>()["users"]["lambda"] as User);

			var parameter = this.CreateThread().Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
