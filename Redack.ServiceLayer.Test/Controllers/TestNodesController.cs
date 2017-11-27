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
	public class TestNodesController : BaseTestController<NodesController>
	{
		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

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
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["lambda"] as User);

			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode(false);

			var body = this.CreateBodyRequest<NodePostRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode(false);
			node.Name = null;

			var body = this.CreateBodyRequest<NodePostRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode();

			var body = this.CreateBodyRequest<NodePostRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var node = this.CreateNode(false);

			var body = this.CreateBodyRequest<NodePostRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["lambda"] as User);

			var node = this.CreateNode(false);

			var body = this.CreateBodyRequest<NodePostRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode();

			var body = this.CreateBodyRequest<NodePutRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Put, node.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode();

			var body = this.CreateBodyRequest<NodePutRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Put, int.MaxValue, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var node = this.CreateNode(false);

			var body = this.CreateBodyRequest<NodePutRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Put, node.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var node = this.CreateNode();

			var body = this.CreateBodyRequest<NodePutRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Put, node.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["lambda"] as User);

			var node = this.CreateNode();

			var body = this.CreateBodyRequest<NodePutRequest, Node>(node);

			var request = this.CreateRequest(HttpMethod.Put, node.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Node>()["users"]["lambda"] as User);

			var parameter = this.CreateNode().Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
