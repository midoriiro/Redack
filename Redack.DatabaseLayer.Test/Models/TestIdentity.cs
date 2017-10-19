using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestIdentity : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var identity = this.CreateIdentity();

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNotNull(repository.GetById(identity.Id));
            }
        }

        [Fact]
        public void WithoutUser()
        {
            var identity = this.CreateIdentity(push: false);
            identity.User = null;

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Insert(identity);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutClient()
        {
            var identity = this.CreateIdentity(push: false);
            identity.Client = null;

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Insert(identity);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutTokenAccess()
        {
            var identity = this.CreateIdentity(push: false);
            identity.Access = null;

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Insert(identity);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutTokenRefresh()
        {
            var identity = this.CreateIdentity(push: false);
            identity.Refresh = null;

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Insert(identity);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithUserCascadeDelete()
        {
            var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var identity1 = this.CreateIdentity(user1);
            var identity2 = this.CreateIdentity(user1);
            var identity3 = this.CreateIdentity(user2);

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Delete(identity1);
                repository.Commit();

                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNotNull(repository.GetById(identity2.Id));
                Assert.IsNotNull(repository.GetById(identity3.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            Assert.AreEqual(1, user1.Identities.Count);
            Assert.AreEqual(1, user2.Identities.Count);
        }

        [Fact]
        public void WithClientCascadeDelete()
        {
            var client1 = this.CreateClient();
            var client2 = this.CreateClient();

            var identity1 = this.CreateIdentity(client: client1);
            var identity2 = this.CreateIdentity(client: client2);
            var identity3 = this.CreateIdentity(client: client1);

            using (var repository = this.CreateRepository<Identity>())
            {
                repository.Delete(identity1);
                repository.Commit();

                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNotNull(repository.GetById(identity2.Id));
                Assert.IsNotNull(repository.GetById(identity3.Id));
            }

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client1.Id));
                Assert.IsNotNull(repository.GetById(client2.Id));
            }

            Assert.AreEqual(1, client1.Identities.Count);
            Assert.AreEqual(1, client2.Identities.Count);
        }
    }
}