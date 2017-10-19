using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using System.Data.Entity.Validation;
using System.Linq;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestUser : BaseTest
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
                Assert.IsFalse(repository.All().Any(
                    e => e.Users.Any(
                        p => p.Id == user1.Id)));

                Assert.IsNotNull(repository.GetById(permission1.Id));
                Assert.IsNotNull(repository.GetById(permission2.Id));
                Assert.IsNotNull(repository.GetById(permission3.Id));
            }

            Assert.AreEqual(1, permission1.Users.Count);
            Assert.AreEqual(1, permission2.Users.Count);
            Assert.AreEqual(1, permission3.Users.Count);
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

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client1.Id));
                Assert.IsNotNull(repository.GetById(client2.Id));
            }

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.User.Id == user.Id));

                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNull(repository.GetById(identity2.Id));
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
                Assert.IsFalse(repository.All().Any(
                    e => e.Author.Id == user.Id));

                Assert.IsNotNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
                Assert.IsNotNull(repository.GetById(message3.Id));
            }
        }

        [Fact]
        public void WithGroupsCascadeDelete()
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

            using (var repository = this.CreateRepository<User>())
            {
                repository.Update(user1);
                repository.Update(user2);
                repository.Update(user3);
                repository.Commit();

                repository.Delete(user1);
                repository.Commit();

                Assert.IsNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
                Assert.IsNotNull(repository.GetById(user3.Id));
            }

            using (var repository = this.CreateRepository<Group>())
            {
                Assert.IsTrue(repository.All().Any(
                    e => e.Id == group1.Id));

                Assert.IsNotNull(repository.GetById(group1.Id));
                Assert.IsNotNull(repository.GetById(group2.Id));
            }

            Assert.AreEqual(1, group1.Users.Count);
            Assert.AreEqual(2, group2.Users.Count);
        }

        [Fact]
        public void WithMessageRevisionsCascadeDelete()
        {
            /*var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var message1 = this.CreateMessage(user1);
            var message2 = this.CreateMessage(user2);

            var revision1 = this.CreateMessageRevision(user1, message1);
            var revision2 = this.CreateMessageRevision(user1, message1);
            var revision3 = this.CreateMessageRevision(user2, message2);

            using (var repository = this.CreateRepository<User>())
            {
                repository.Delete(user1);
                repository.Commit();

                Assert.IsNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Editor.Id == user1.Id));

                var lol = repository.GetById(revision1.Id, revision1.EditorId, revision1.MessageId, revision1.Date);

                Assert.IsTrue(repository.All().Any(e => e.Id == revision1.Id));
                Assert.IsTrue(repository.All().Any(e => e.Id == revision2.Id));
                Assert.IsTrue(repository.All().Any(e => e.Id == revision3.Id));
            }

            Assert.AreEqual(0, message1.Revisions.Count);
            Assert.AreEqual(1, message2.Revisions.Count);*/

            Assert.IsTrue(false);
        }
    }
}
