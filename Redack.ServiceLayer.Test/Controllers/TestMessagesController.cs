using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
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
	public class TestMessagesController : BaseTestController<MessagesController>
	{
		[Fact]
		public void GetAll_WithUnauthentifiedUser()
		{
			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.CreateMessage();

			var request = this.CreateRequest(HttpMethod.Get, parameter.Id);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var node = this.CreateNode();
			var thread = this.CreateThread(node);
			var message = this.CreateMessage(user, thread, false);

			var body = this.CreateBodyRequest<MessagePostRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Message>()["users"]["admin"] as User);

			var message = this.CreateMessage(push: false);
			message.Text = null;

			var body = this.CreateBodyRequest<MessagePostRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Message>()["users"]["admin"] as User);

			var message = this.CreateMessage();

			var body = this.CreateBodyRequest<MessagePostRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var message = this.CreateMessage(push: false);

			var body = this.CreateBodyRequest<MessagePostRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var message = this.CreateMessage(user);

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidRequest()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var message = this.CreateMessage(user);
			message.Text = null;

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var message = this.CreateMessage(user);
			message.Id = int.MaxValue;

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Message>()["users"]["admin"] as User);

			var message = this.CreateMessage(push: false);

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var message = this.CreateMessage();

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithUserIsNotOwner()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var message = this.CreateMessage();

			var body = this.CreateBodyRequest<MessagePutRequest, Message>(message);

			var request = this.CreateRequest(HttpMethod.Put, message.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<Message>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var parameter = this.CreateMessage(user).Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Message>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Delete, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.CreateMessage().Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUserIsNotOwner()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Message>()["users"]["lambda"] as User);

			var parameter = this.CreateMessage().Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
