using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestPermission : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var permission = this.CreatePermission<Permission>();

            using (var repository = this.CreateRepository<Permission>())
            {
                Assert.IsNotNull(repository.GetById(permission.Id));
            }
        }

        [Fact]
        public void WithoutCodename()
        {
            var permission = this.CreatePermission<Permission>(push: false);
            permission.Codename = null;

            using (var repository = this.CreateRepository<Permission>())
            {
                repository.Insert(permission);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutHelpText()
        {
            var permission = this.CreatePermission<Permission>(push: false);
            permission.HelpText = null;

            using (var repository = this.CreateRepository<Permission>())
            {
                repository.Insert(permission);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutContentType()
        {
            var permission = this.CreatePermission<Permission>(push: false);
            permission.ContentType = null;

            using (var repository = this.CreateRepository<Permission>())
            {
                repository.Insert(permission);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithGroupsCascadeDelete()
        {
            var permission1 = this.CreatePermission<Permission>();
            var permission2 = this.CreatePermission<Permission>();
            var permission3 = this.CreatePermission<Permission>();

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
            }

            using (var repository = this.CreateRepository<Permission>())
            {
                repository.Delete(permission1);
                repository.Commit();

                Assert.IsNull(repository.GetById(permission1.Id));
                Assert.IsNotNull(repository.GetById(permission2.Id));
                Assert.IsNotNull(repository.GetById(permission3.Id));
            }

            using (var repository = this.CreateRepository<Group>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Permissions.Any(
                        p => p.Id == permission1.Id)));

                Assert.IsNotNull(repository.GetById(group1.Id));
                Assert.IsNotNull(repository.GetById(group2.Id));
            }

            Assert.AreEqual(1, group1.Permissions.Count);
            Assert.AreEqual(2, group2.Permissions.Count);
        }

        [Fact]
        public void WithUsersCascadeDelete()
        {
            var permission1 = this.CreatePermission<Permission>();
            var permission2 = this.CreatePermission<Permission>();
            var permission3 = this.CreatePermission<Permission>();

            var user1 = this.CreateUser();
            user1.Permissions.Add(permission1);
            user1.Permissions.Add(permission2);

            var user2 = this.CreateUser();
            user2.Permissions.Add(permission2);
            user2.Permissions.Add(permission3);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Update(user1);
                repository.Update(user2);
                repository.Commit();
            }

            using (var repository = this.CreateRepository<Permission>())
            {
                repository.Delete(permission1);
                repository.Commit();

                Assert.IsNull(repository.GetById(permission1.Id));
                Assert.IsNotNull(repository.GetById(permission2.Id));
                Assert.IsNotNull(repository.GetById(permission3.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Permissions.Any(
                        p => p.Id == permission1.Id)));

                Assert.IsNotNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            Assert.AreEqual(1, user1.Permissions.Count);
            Assert.AreEqual(2, user2.Permissions.Count);
        }
    }
}