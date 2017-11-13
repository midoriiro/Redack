using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestGroupsController : BaseTestController<GroupsController>
	{
		/*[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var response = this.Call(e => e.GetAll()).Result;

			Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<Group>>));
		}

		[Fact]
		public void GetAll_WithUnauthentifiedUser()
		{
			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.GetAll()).Result);
		}

		[Fact]
		public void GetAll_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.GetAll()).Result);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var response = this.Call(e => e.Get(request)).Result;

			Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Group>));
		}

		[Fact]
		public void Get_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var response = this.Call(e => e.Get(int.MaxValue)).Result;

			Assert.IsInstanceOfType(response, typeof(NotFoundResult));
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Get(request)).Result);
		}

		[Fact]
		public void Get_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Get(request)).Result);
		}

		[Fact]
		public void Post_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.CreateGroup(false);

			var response = this.Call(e => e.Post(request)).Result;

			Assert.IsInstanceOfType(response, typeof(CreatedAtRouteNegotiatedContentResult<Group>));
		}

		[Fact]
		public void Post_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.CreateGroup(false);
			request.Name = null;

			var response = this.Call(e => e.Post(request)).Result;

			Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
		}

		[Fact]
		public void Post_WithExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"] as Group;

			var response = this.Call(e => e.Post(request)).Result;

			Assert.IsInstanceOfType(response, typeof(ConflictResult));
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Post(this.CreateGroup(false))).Result);
		}

		[Fact]
		public void Post_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Post(this.CreateGroup(false))).Result);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"] as Group;

			var response = this.Call(e => e.Put(request.Id, request)).Result;

			Assert.IsInstanceOfType(response, typeof(StatusCodeResult));
		}

		[Fact]
		public void Put_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"] as Group;

			var response = this.Call(e => e.Put(request.Id + 1, request)).Result;

			Assert.IsInstanceOfType(response, typeof(BadRequestResult));
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.CreateGroup(false);

			var response = this.Call(e => e.Put(request.Id, request)).Result;

			Assert.IsInstanceOfType(response, typeof(NotFoundResult));
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var request = this.GetDataSet<Group>()["groups"]["lambda"] as Group;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Put(request.Id, request)).Result);
		}

		[Fact]
		public void Put_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"] as Group;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Put(request.Id, request)).Result);
		}

		[Fact]
		public void Delete_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			var response = this.Call(e => e.Delete(request)).Result;

			Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Group>));
		}

		[Fact]
		public void Delete_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["admin"] as User);

			var response = this.Call(e => e.Delete(int.MaxValue)).Result;

			Assert.IsInstanceOfType(response, typeof(NotFoundResult));
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Delete(request)).Result);
		}

		[Fact]
		public void Delete_WithAuthentifiedUserAndWithoutPermissions()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<Group>()["users"]["lambda"] as User);

			var request = this.GetDataSet<Group>()["groups"]["lambda"].Id;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Delete(request)).Result);
		}*/
	}
}
