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
    public class TestClient : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var client = this.CreateClient();

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client.Id));
            }
        }

        [Fact]
        public void WithoutName()
        {
            var client = this.CreateClient(push: false);
            client.Name = null;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Insert(client);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutApiKey()
        {
            var client = this.CreateClient(push: false);
            client.ApiKey = null;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Insert(client);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithApiKeyCascadeDelete()
        {
            var apikey = this.CreateApiKey(false);
            var client = this.CreateClient(apikey);

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Delete(client);
                repository.Commit();

                Assert.IsNull(repository.GetById(client.Id));
            }

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNull(repository.GetById(apikey.Id));
            }
        }

        [Fact]
        public void WithIdentitiesCascadeDelete()
        {
            var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var client1 = this.CreateClient();
            var client2 = this.CreateClient();

            var identity1 = this.CreateIdentity(user1, client1);
            var identity2 = this.CreateIdentity(user1, client2);
            var identity3 = this.CreateIdentity(user2, client1);

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Delete(client1);
                repository.Commit();

                Assert.IsNull(repository.GetById(client1.Id));
                Assert.IsNotNull(repository.GetById(client2.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNotNull(repository.GetById(identity2.Id));
                Assert.IsNull(repository.GetById(identity3.Id));
            }
        }
    }
}
