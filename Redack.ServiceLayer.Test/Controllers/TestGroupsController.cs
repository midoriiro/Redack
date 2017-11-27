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
	public class TestGroupsController : BaseTestController<GroupsController>
	{
		[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

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
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var request = this.CreateRequest(HttpMethod.Get);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Get_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Get, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Get, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = this.CreateGroup(false);

			var body = this.CreateBodyRequest<GroupPostRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = this.CreateGroup(false);
			group.Name = null;

			var body = this.CreateBodyRequest<GroupPostRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = (Group)this.GetDataSet<Group>()["groups"]["lambda"];

			var body = this.CreateBodyRequest<GroupPostRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var group = this.CreateGroup(false);

			var body = this.CreateBodyRequest<GroupPostRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var group = this.CreateGroup();

			var body = this.CreateBodyRequest<GroupPostRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Post, body: body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = (Group)this.GetDataSet<Group>()["groups"]["lambda"];

			var body = this.CreateBodyRequest<GroupPutRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Put, group.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public void Put_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = (Group)this.GetDataSet<Group>()["groups"]["lambda"];

			var body = this.CreateBodyRequest<GroupPutRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Put, int.MaxValue, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var group = this.CreateGroup(false);

			var body = this.CreateBodyRequest<GroupPutRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Put, group.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var group = (Group)this.GetDataSet<Group>()["groups"]["lambda"];

			var body = this.CreateBodyRequest<GroupPutRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Put, group.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var group = (Group)this.GetDataSet<Group>()["groups"]["lambda"];

			var body = this.CreateBodyRequest<GroupPutRequest, Group>(group);

			var request = this.CreateRequest(HttpMethod.Put, group.Id, body);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
		
		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public void Delete_WithInvalidId()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.CreateRequest(HttpMethod.Delete, int.MaxValue);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var parameter = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var request = this.CreateRequest(HttpMethod.Delete, parameter);
			var response = this.Client.SendAsync(request).Result;

			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
