using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using System.Data.Entity.Validation;
using System.Linq;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestUser : TestBase
    {
        [Fact]
        public void Valid()
        {
            var user = this.CreateUser();

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }
        }

        [Fact]
        public void WithoutAlias()
        {
            var user = this.CreateUser(push: false);
            user.Alias = null;

            using (var repository = this.CreateRepository<User>())
            {
                repository.Insert(user);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutCredential()
        {
            var user = this.CreateUser(push: false);
            user.Credential = null;

            using (var repository = this.CreateRepository<User>())
            {
                repository.Insert(user);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithCredentialCascadeDelete()
        {
            var credential = this.CreateCredential(push: false);
            var user = this.CreateUser(credential);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Credential>())
            {
                Assert.IsNull(repository.GetById(credential.Id));
            }
        }

        [Fact]
        public void WithPermissionsCascadeDelete()
        {
            var permission1 = this.CreatePermission<User>();
            var permission2 = this.CreatePermission<User>();
            var permission3 = this.CreatePermission<User>();

            var user1 = this.CreateUser();
            user1.Permissions.Add(permission1);
            user1.Permissions.Add(permission2);
            user1.Permissions.Add(permission3);

            var user2 = this.CreateUser();
            user2.Permissions.Add(permission1);
            user2.Permissions.Add(permission2);
            user2.Permissions.Add(permission3);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Update(user1);
                repository.Update(user2);
                repository.Commit();

                repository.Delete(user1);
                repository.Commit();

                Assert.IsNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            using (var repository = this.CreateRepository<Permission>())
            {
                Assert.IsFalse(repository.All().Any(e => e.Users.Any(p => p.Id == user1.Id)));

                Assert.IsNotNull(repository.GetById(permission1.Id));
                Assert.IsNotNull(repository.GetById(permission2.Id));
                Assert.IsNotNull(repository.GetById(permission3.Id));
            }
        }

        [Fact]
        public void WithIdentitiesCascadeDelete()
        {
            var user = this.CreateUser();
            var client1 = this.CreateClient();
            var client2 = this.CreateClient();

            var identity1 = this.CreateIdentity(user, client1);
            var identity2 = this.CreateIdentity(user, client2);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNull(repository.GetById(identity2.Id));
            }

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client1.Id));
                Assert.IsNotNull(repository.GetById(client2.Id));
            }
        }

        [Fact]
        public void WithMessagesCascadeDelete()
        {
            var user = this.CreateUser();

            var message1 = this.CreateMessage(user);
            var message2 = this.CreateMessage(user);
            var message3 = this.CreateMessage(user);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Update(user);
                repository.Commit();

                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Message>())
            {
                Assert.IsNotNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
                Assert.IsNotNull(repository.GetById(message3.Id));
            }
        }

        [Fact]
        public void WithGroupCascadeDelete()
        {
            var group = this.CreateGroup();

            var user = this.CreateUser();
            user.Groups.Add(group);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Update(user);
                repository.Commit();

                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Group>())
            {
                Assert.IsNotNull(repository.GetById(group.Id));
            }
        }
    }
}
