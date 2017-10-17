using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop;
using System.Data.Entity.Validation;
using System.Linq;
using Xunit;
using Thread = System.Threading.Thread;

namespace Redack.DatabaseLayer.Test.Entity
{
    public class TestUser : TestBase
    {
        [Fact]
        public void Valid()
        {
            var user = this.CreateValidUser();

            using (var repository = new Repository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }
        }

        [Fact]
        public void WithoutAlias()
        {
            var user = this.CreateValidUser(push: false);
            user.Alias = null;

            using (var repository = new Repository<User>())
            {
                repository.Insert(user);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutCredential()
        {
            var user = this.CreateValidUser(push: false);
            user.Credential = null;

            using (var repository = new Repository<User>())
            {
                repository.Insert(user);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithCredentialCascadeDelete()
        {
            var credential = this.CreateValidCredential(push: false);
            var user = this.CreateValidUser(credential);

            using (var repository = new Repository<User>())
            {
                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = new Repository<Credential>())
            {
                Assert.IsNull(repository.GetById(credential.Id));
            }
        }

        [Fact]
        public void WithPermissionsCascadeDelete()
        {
            var permission1 = this.CreateValidPermission<User>();
            var permission2 = this.CreateValidPermission<User>();
            var permission3 = this.CreateValidPermission<User>();

            var user1 = this.CreateValidUser();
            user1.Permissions.Add(permission1);
            user1.Permissions.Add(permission2);
            user1.Permissions.Add(permission3);

            var user2 = this.CreateValidUser();
            user2.Permissions.Add(permission1);
            user2.Permissions.Add(permission2);
            user2.Permissions.Add(permission3);

            using (var repository = new Repository<User>())
            {
                repository.Update(user1);
                repository.Update(user2);
                repository.Commit();

                repository.Delete(user1);
                repository.Commit();

                Assert.IsNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            using (var repository = new Repository<Permission>(this.Context))
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
            var user = this.CreateValidUser();
            var client1 = this.CreateValidClient();
            var client2 = this.CreateValidClient();

            var identity1 = this.CreateValidIdentity(user, client1);
            var identity2 = this.CreateValidIdentity(user, client2);

            using (var repository = new Repository<User>())
            {
                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = new Repository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNull(repository.GetById(identity2.Id));
            }

            using (var repository = new Repository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client1.Id));
                Assert.IsNotNull(repository.GetById(client2.Id));
            }
        }

        [Fact]
        public void WithMessagesCascadeDelete()
        {
            var user = this.CreateValidUser();

            var message1 = this.CreateValidMessage(user);
            var message2 = this.CreateValidMessage(user);
            var message3 = this.CreateValidMessage(user);

            using (var repository = new Repository<User>())
            {
                repository.Update(user);
                repository.Commit();

                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = new Repository<Message>())
            {
                Assert.IsNotNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
                Assert.IsNotNull(repository.GetById(message3.Id));
            }
        }

        [Fact]
        public void WithGroupCascadeDelete()
        {
            var group = this.CreateValidGroup();

            var user = this.CreateValidUser();
            user.Groups.Add(group);

            using (var repository = new Repository<User>(this.Context))
            {
                repository.Update(user);
                repository.Commit();

                repository.Delete(user);
                repository.Commit();

                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = new Repository<Group>())
            {
                Assert.IsNotNull(repository.GetById(group.Id));
            }
        }
    }
}
