using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop;
using Redack.Test.Lollipop.Configuration;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;
using Redack.Test.Lollipop.Model;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
    public class TestIdentitiesController : TestBase
    {
        private readonly RedackDbContext _context;
        private readonly IdentitiesController _controller;

        public TestIdentitiesController()
        {
            this._context = new RedackDbContext();

            this._controller = new IdentitiesController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        public User CreateValidUser()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidUserCustomization());

            var user = fixture.Create<User>();

            var repository = new Repository<User>(this._context);
            repository.Insert(user);
            repository.Commit();

            return user;
        }

        public Client CreateValidClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidClientCustomization());

            var client = fixture.Create<Client>();

            var repository = new Repository<Client>(this._context);
            repository.Insert(client);
            repository.Commit();

            return client;
        }

        public Identity CreateValidIdentity(User user = null, Client client = null)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidIdentityCustomization());

            var tokenizer = new JwtTokenizer();

            user = user ?? this.CreateValidUser();
            client = client ?? this.CreateValidClient();

            var identity = fixture.Create<Identity>();
            identity.ApiKey = user.Credential.ApiKey;
            identity.Client = client;
            identity.Access = tokenizer.Encode(identity, 5);
            identity.Refresh = tokenizer.Encode(identity, 1000);

            var repository = new Repository<Identity>(this._context);
            repository.Insert(identity);
            repository.Commit();

            return identity;
        }

        [Fact]
        public void SignUp_WithValidRequest()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignUpRequest>());

            var client = this.CreateValidClient();

            var credential = fixture.Create<CredentialSignUpRequest>();
            credential.Client = client;

            var response = this._controller.SignUp(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<User>));
        }

        [Fact]
        public void SignUp_WithExistingUser()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignUpRequest>());

            var user = this.CreateValidUser();

            var client = this.CreateValidClient();

            var credential = fixture.Create<CredentialSignUpRequest>();
            credential.Client = client;
            credential.Login = user.Credential.Login;
            credential.Password = user.Credential.Password;
            credential.PasswordConfirm = user.Credential.PasswordConfirm;

            var response = this._controller.SignUp(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [Theory, AutoData]
        public void SignUp_WithInvalidCredential()
        {
            Fixture fixture = new Fixture();
            fixture.Customize(new IgnorePropertiesCustomization(new[]
            {
                "PasswordConfirm"
            }));

            var response = this._controller.SignUp(fixture.Create<CredentialSignUpRequest>());
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [Theory, AutoData]
        public void SignUp_WithInvalidClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignUpRequest>());

            var credential = fixture.Create<CredentialSignUpRequest>();
            credential.Client = fixture.Create<Client>();

            var response = this._controller.SignUp(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [Theory, AutoData]
        public void SignIn_WithValidRequest()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignInRequest>());

            var user = this.CreateValidUser();

            var client = this.CreateValidClient();

            var credential = fixture.Create<CredentialSignInRequest>();
            credential.Client = client;
            credential.Login = user.Credential.Login;
            credential.Password = user.Credential.Password;

            var response = this._controller.SignIn(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TokenResponse>));
        }

        [Theory, AutoData]
        public void SignIn_WithExistingIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignInRequest>());

            var user = this.CreateValidUser();

            var identity = this.CreateValidIdentity(user);

            var credential = fixture.Create<CredentialSignInRequest>();
            credential.Client = identity.Client;
            credential.Login = user.Credential.Login;
            credential.Password = user.Credential.Password;

            var response = this._controller.SignIn(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [Theory, AutoData]
        public void SignIn_WithInvalidCredential()
        {
            Fixture fixture = new Fixture();
            fixture.Customize(new IgnorePropertiesCustomization(new[]
            {
                "Password"
            }));

            var response = this._controller.SignIn(fixture.Create<CredentialSignInRequest>());
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [Theory, AutoData]
        public void SignIn_WithInvalidClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialRequestCustomization<CredentialSignInRequest>());

            var credential = fixture.Create<CredentialSignInRequest>();
            credential.Client = fixture.Create<Client>();

            var response = this._controller.SignIn(credential);
            var result = response.Result;

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }
    }
}
