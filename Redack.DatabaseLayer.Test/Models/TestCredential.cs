using System.Data.Entity.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestCredential : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var credential = this.CreateCredential(push: false);
            var user = this.CreateUser(credential);

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
        public void WithoutLogin()
        {
            var credential = this.CreateCredential(push: false);
            credential.Login = null;

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Insert(credential);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutPassword()
        {
            var credential = this.CreateCredential(push: false);
            credential.Password = null;

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Insert(credential);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutPasswordConfirm()
        {
            var credential = this.CreateCredential(push: false);
            credential.PasswordConfirm = null;

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Insert(credential);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutSalt()
        {
            var credential = this.CreateCredential(push: false);
            credential.Salt = null;

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Insert(credential);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutApikey()
        {
            var credential = this.CreateCredential(push: false);
            credential.ApiKey = null;

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Insert(credential);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithApiKeyCascadeDelete()
        {
            var apikey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apikey, false);
            var user = this.CreateUser(credential);

            using (var repository = this.CreateRepository<Credential>())
            {
                repository.Delete(credential);
                repository.Commit();

                Assert.IsNull(repository.GetById(credential.Id));
            }

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNull(repository.GetById(apikey.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }
        }
    }
}