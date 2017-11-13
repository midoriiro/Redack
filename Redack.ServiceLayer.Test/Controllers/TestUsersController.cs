using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestUsersController : BaseTestController<UsersController>
	{
		/*[Fact]
		public void GetAll_WithAuthentifiedUser()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<User>()["users"]["admin"] as User);

			var response = this.Call(e => e.GetAll()).Result;

			Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<User>>));
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
			this.CreateAuthentifiedUser(this.GetDataSet<User>()["users"]["lambda"] as User);

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.GetAll()).Result);
		}

		[Fact]
		public void Get_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<User>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var request = user.Id;

			var response = this.Call(e => e.Get(request)).Result;

			Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<User>));
		}

		[Fact]
		public void Get_WithInvalidRequest()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<User>()["users"]["admin"] as User);

			var response = this.Call(e => e.Get(int.MaxValue)).Result;

			Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
		}

		[Fact]
		public void Get_WithUnauthentifiedUser()
		{
			var request = this.GetDataSet<User>()["users"]["admin"];

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Get(request.Id)).Result);
		}

		[Fact]
		public void Post_WithDisabledAction()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<User>()["users"]["admin"] as User);

			var request = this.CreateUser(push: false);

			var response = this.Call(e => e.Post(request)).Result;

			Assert.IsInstanceOfType(response, typeof(NotFoundResult));
		}

		[Fact]
		public void Post_WithUnauthentifiedUser()
		{
			var request = this.CreateUser(push: false);

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Post(request)).Result);
		}

		[Fact]
		public void Put_WithAuthentifiedUser()
		{
			var user = (User)this.GetDataSet<User>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var request = user;

			var response = this.Call(e => e.Put(request.Id, request)).Result;

			Assert.IsInstanceOfType(response, typeof(StatusCodeResult));
		}

		[Fact]
		public void Put_WithInvalidRequest()
		{
			var user = (User)this.GetDataSet<User>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var request = user;

			var response = this.Call(e => e.Put(request.Id + 1, request)).Result;

			Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
		}

		[Fact]
		public void Put_WithNonExistingEntity()
		{
			this.CreateAuthentifiedUser(this.GetDataSet<User>()["users"]["admin"] as User);

			var request = this.CreateUser(push: false);

			var response = this.Call(e => e.Put(request.Id, request)).Result;

			Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
		}

		[Fact]
		public void Put_WithUnauthentifiedUser()
		{
			var user = (User)this.GetDataSet<User>()["users"]["lambda"];

			var request = user;

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Put(request.Id, request)).Result);
		}

		[Fact]
		public void Delete_WithDisabledAction()
		{
			var user = (User)this.GetDataSet<User>()["users"]["admin"];

			this.CreateAuthentifiedUser(user);

			var request = user.Id;

			var response = this.Call(e => e.Delete(request)).Result;

			Assert.IsInstanceOfType(response, typeof(NotFoundResult));
		}

		[Fact]
		public void Delete_WithUnauthentifiedUser()
		{
			var request = this.CreateUser(push: false);

			Assert.ThrowsException<InvalidOperationException>(() => this.Call(
				e => e.Delete(request.Id)).Result);
		}*/
	}
}
