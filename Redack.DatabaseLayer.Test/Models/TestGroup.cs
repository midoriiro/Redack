using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
	public class TestGroup : BaseTest
	{
		[Fact]
		public void Valid()
		{
			var group = this.CreateGroup();

			using (var repository = this.CreateRepository<Group>())
			{
				Assert.IsNotNull(repository.GetById(group.Id));
			}
		}

		[Fact]
		public void WithoutName()
		{
			var group = this.CreateGroup(false);
			group.Name = null;

			using (var repository = this.CreateRepository<Group>())
			{
				repository.Insert(group);

				Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
			}
		}

		[Fact]
		public void WithUsersCascadeDelete()
		{
			var user1 = this.CreateUser();
			var user2 = this.CreateUser();
			var user3 = this.CreateUser();

			var group1 = this.CreateGroup();
			group1.Users.Add(user1);
			group1.Users.Add(user2);

			var group2 = this.CreateGroup();
			group2.Users.Add(user2);
			group2.Users.Add(user3);

			using (var repository = this.CreateRepository<Group>())
			{
				repository.Update(group1);
				repository.Update(group2);
				repository.Commit();

				repository.Delete(group1);
				repository.Commit();

				Assert.IsNull(repository.GetById(group1.Id));
				Assert.IsNotNull(repository.GetById(group2.Id));
			}

			using (var repository = this.CreateRepository<User>())
			{
				Assert.IsFalse(repository.All().Any(
					e => e.Groups.Any(
						p => p.Id == group1.Id)));

				Assert.IsNotNull(repository.GetById(user1.Id));
				Assert.IsNotNull(repository.GetById(user2.Id));
				Assert.IsNotNull(repository.GetById(user3.Id));
			}

			Assert.AreEqual(0, user1.Groups.Count);
			Assert.AreEqual(1, user2.Groups.Count);
			Assert.AreEqual(1, user3.Groups.Count);
		}

		[Fact]
		public void WithPermissionsCascadeDelete()
		{
			var permission1 = this.CreatePermission<Group>();
			var permission2 = this.CreatePermission<Group>();
			var permission3 = this.CreatePermission<Group>();

			var group1 = this.CreateGroup();
			group1.Permissions.Add(permission1);
			group1.Permissions.Add(permission2);

			var group2 = this.CreateGroup();
			group2.Permissions.Add(permission2);
			group2.Permissions.Add(permission3);

			using (var repository = this.CreateRepository<Group>())
			{
				repository.Update(group1);
				repository.Update(group2);
				repository.Commit();

				repository.Delete(group1);
				repository.Commit();

				Assert.IsNull(repository.GetById(group1.Id));
				Assert.IsNotNull(repository.GetById(group2.Id));
			}

			using (var repository = this.CreateRepository<Permission>())
			{
				Assert.IsFalse(repository.All().Any(
					e => e.Groups.Any(
						p => p.Id == group1.Id)));

				Assert.IsNotNull(repository.GetById(permission1.Id));
				Assert.IsNotNull(repository.GetById(permission2.Id));
				Assert.IsNotNull(repository.GetById(permission3.Id));
			}

			Assert.AreEqual(0, permission1.Groups.Count);
			Assert.AreEqual(1, permission2.Groups.Count);
			Assert.AreEqual(1, permission3.Groups.Count);
		}
	}
}
