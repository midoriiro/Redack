using System.Data.Entity.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestApiKey : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var apikey = this.CreateApiKey();

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNotNull(repository.GetById(apikey.Id));
            }
        }

        [Fact]
        public void WithoutKey()
        {
            var apikey = this.CreateApiKey(false);
            apikey.Key = null;

            using (var repository = this.CreateRepository<ApiKey>())
            {
                repository.Insert(apikey);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithCredentialCascadeDelete()
        {
            var apikey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apikey, false);
            var user = this.CreateUser(credential);

            using (var repository = this.CreateRepository<ApiKey>())
            {
                repository.Delete(apikey);
                repository.Commit();

                Assert.IsNull(repository.GetById(apikey.Id));
            }

            using (var repository = this.CreateRepository<Credential>())
            {
                Assert.IsNotNull(repository.GetById(credential.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }
        }

        [Fact]
        public void WithClientCascadeDelete()
        {
            var apikey = this.CreateApiKey(false);
            var client = this.CreateClient(apikey);

            using (var repository = this.CreateRepository<ApiKey>())
            {
                repository.Delete(apikey);
                repository.Commit();

                Assert.IsNull(repository.GetById(apikey.Id));
            }

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client.Id));
            }
        }
    }
}