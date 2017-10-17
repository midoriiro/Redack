using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Entities;
using System.Web.Http.Results;
using Redack.Test.Lollipop.Models;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
    public class TestIdentitiesController : TestBaseController<IdentitiesController>
    {
        public SignUpRequest CreateValidSignUpRequest(
            User user = null, Client client = null, bool pushUser = true, bool pushClient = true)
        {
            user = user ?? this.CreateUser(push: pushUser);
            client = client ?? this.CreateClient(push: pushClient);

            var fixture = new Fixture();
            fixture.Customize(new SignUpRequestCustomization());

            var request = fixture.Create<SignUpRequest>();
            request.Client = client;
            request.Login = user.Credential.Login;
            request.Password = user.Credential.Password;
            request.PasswordConfirm = user.Credential.PasswordConfirm;

            return request;
        }

        public SignInRequest CreateSignInRequest(
            User user = null, Client client = null, bool pushUser = true, bool pushClient = true)
        {
            user = user ?? this.CreateUser(push: pushUser);
            client = client ?? this.CreateClient(push: pushClient);

            var fixture = new Fixture();
            fixture.Customize(new SignInRequestCustomization());

            var request = fixture.Create<SignInRequest>();
            request.Client = client;
            request.Login = user.Credential.Login;
            request.Password = user.Credential.Password;

            return request;
        }

        public ForgotPasswordRequest CreateForgotPasswordRequest(
            User user = null, Client client = null, bool pushUser = true, bool pushClient = true)
        {
            user = user ?? this.CreateUser(push: pushUser);
            client = client ?? this.CreateClient(push: pushClient);

            var fixture = new Fixture();
            fixture.Customize(new ForgotPasswordRequestCustomization());

            var request = fixture.Create<ForgotPasswordRequest>();
            request.Client = client;
            request.Login = user.Credential.Login;
            request.OldPassword = user.Credential.Password;

            return request;
        }

        [Fact]
        public void SignUp_WithValidRequest()
        {
            var request = this.CreateValidSignUpRequest(pushUser: false);

            var response = this.Controller.SignUp(request).Result;

            Assert.IsInstanceOfType(response, typeof(CreatedAtRouteNegotiatedContentResult<User>));
        }

        [Fact]
        public void SignUp_WithExistingUser()
        {
            var request = this.CreateValidSignUpRequest();

            var response = this.Controller.SignUp(request).Result;

            Assert.IsInstanceOfType(response, typeof(ConflictResult));
        }

        [Fact]
        public void SignUp_WithInvalidRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser(push: false);
            user.Credential.Password = fixture.Create<string>();
            user.Credential.PasswordConfirm = fixture.Create<string>();

            var request = this.CreateValidSignUpRequest(user, pushUser: false);

            var response = this.Controller.SignUp(request).Result;

            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [Fact]
        public void SignUp_WithNonExistingClient()
        {
            var request = this.CreateValidSignUpRequest(pushClient: false);

            var response = this.Controller.SignUp(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignUp_WithBlockedClient()
        {
            var request = this.CreateValidSignUpRequest();

            request.Client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(request.Client);
                repository.Commit();
            }

            var response = this.Controller.SignUp(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignIn_WithValidRequest()
        {
            var request = this.CreateSignInRequest();

            var response = this.Controller.SignIn(request).Result;

            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<TokenResponse>));
        }

        [Fact]
        public void SignIn_WithExistingIdentity()
        {
            var user = this.CreateUser();
            var identity = this.CreateIdentity(user);

            var request = this.CreateSignInRequest(user, identity.Client);

            var response = this.Controller.SignIn(request).Result;

            Assert.IsInstanceOfType(response, typeof(ConflictResult));
        }

        [Fact]
        public void SignIn_WithInvalidLoginRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var request = this.CreateSignInRequest(user);
            request.Login = fixture.Create<string>();

            var response = this.Controller.SignIn(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignIn_WithInvalidPasswordRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var request = this.CreateSignInRequest(user);
            request.Password = fixture.Create<string>();

            var response = this.Controller.SignIn(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignIn_WithBlockedClient()
        {
            var request = this.CreateSignInRequest();

            request.Client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(request.Client);
                repository.Commit();
            }

            var response = this.Controller.SignIn(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignIn_WithNonExistingClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new SignInRequestCustomization());

            var response = this.Controller.SignIn(fixture.Create<SignInRequest>()).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignOut_WithValidIdentity()
        {
            var apiKey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apiKey, false);
            var user = this.CreateUser(credential);
            var client = this.CreateClient();
            var identity = this.CreateIdentity(user, client);

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.SignOut().Result;

            Assert.IsInstanceOfType(response, typeof(OkResult));

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity.Id));
            }

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Credential>())
            {
                Assert.IsNotNull(repository.GetById(credential.Id));
            }

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNotNull(repository.GetById(apiKey.Id));
            }
        }

        [Fact]
        public void SignOut_WithNonExistingIdentity()
        {
            var response = this.Controller.SignOut().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void SignOut_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.SignOut().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void Refresh_WithValidIdentity()
        {
            this.SetControllerIdentity(new JwtIdentity(this.CreateIdentity()));

            var response = this.Controller.Refresh().Result;

            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<TokenResponse>));
        }

        [Fact]
        public void Refresh_WithNonExistingIdentity()
        {
            var response = this.Controller.Refresh().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void Refresh_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.Refresh().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void Refresh_WithBlockedClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = this.CreateIdentity();

            identity.Client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(identity.Client);
                repository.Commit();
            }

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.Refresh().Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void Refresh_WithExpiredToken()
        {
            var identity = this.CreateIdentity();

            identity.Access = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, -0.01);
            identity.Refresh = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, -0.01);

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.Refresh().Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void SignDown_WithValidIdentity()
        {
            var apiKey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apiKey, false);
            var user = this.CreateUser(credential);
            var client = this.CreateClient();
            var identity = this.CreateIdentity(user, client);

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.SignDown().Result;

            Assert.IsInstanceOfType(response, typeof(OkResult));

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity.Id));
            }

            using (var repository = this.CreateRepository<Client>())
            {
                Assert.IsNotNull(repository.GetById(client.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Credential>())
            {
                Assert.IsNull(repository.GetById(credential.Id));
            }

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNull(repository.GetById(apiKey.Id));
            }
        }

        [Fact]
        public void SignDown_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.SignDown().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void SignOutAll_WithValidIdentity()
        {
            var apiKey1 = this.CreateApiKey(false);
            var apiKey2 = this.CreateApiKey(false);
            var apiKey3 = this.CreateApiKey(false);
            var credential = this.CreateCredential(apiKey1, false);
            var user = this.CreateUser(credential);
            var client1 = this.CreateClient(apiKey2);
            var client2 = this.CreateClient(apiKey3);
            var identity1 = this.CreateIdentity(user, client1);
            var identity2 = this.CreateIdentity(user, client2);

            this.SetControllerIdentity(new JwtIdentity(identity1));

            var response = this.Controller.SignOutAll().Result;

            Assert.IsInstanceOfType(response, typeof(OkResult));

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

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsNotNull(repository.GetById(user.Id));
            }

            using (var repository = this.CreateRepository<Credential>())
            {
                Assert.IsNotNull(repository.GetById(credential.Id));
            }

            using (var repository = this.CreateRepository<ApiKey>())
            {
                Assert.IsNotNull(repository.GetById(apiKey1.Id));
                Assert.IsNotNull(repository.GetById(apiKey2.Id));
                Assert.IsNotNull(repository.GetById(apiKey3.Id));
            }
        }

        [Fact]
        public void SignOutAll_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.SetControllerIdentity(new JwtIdentity(identity));

            var response = this.Controller.SignDown().Result;

            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [Fact]
        public void ForgotPassword_WithValidRequestAndValidIdentity()
        {
            var identity = this.CreateIdentity();

            this.SetControllerIdentity(new JwtIdentity(identity));

            var request = this.CreateForgotPasswordRequest(identity.User);

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(OkResult));

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity.Id));
            }
        }

        [Fact]
        public void ForgotPassword_WithValidRequestAndZeroIdentity()
        {
            var request = this.CreateForgotPasswordRequest();

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [Fact]
        public void ForgotPassword_WithInvalidLoginRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var request = this.CreateForgotPasswordRequest(user);
            request.Login = fixture.Create<string>();

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void ForgotPassword_WithInvalidPasswordRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var request = this.CreateForgotPasswordRequest(user);
            request.OldPassword = fixture.Create<string>();

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void ForgotPassword_WithInvalidPasswordConfirmRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var request = this.CreateForgotPasswordRequest(user);
            request.NewPasswordConfirm = fixture.Create<string>();

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [Fact]
        public void ForgotPassword_WithNonExistingClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new ForgotPasswordRequestCustomization());

            var response = this.Controller.SignInForgotPassword(fixture.Create<ForgotPasswordRequest>()).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }

        [Fact]
        public void ForgotPassword_WithBlockedClient()
        {
            var request = this.CreateForgotPasswordRequest();
            request.Client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(request.Client);
                repository.Commit();
            }

            var response = this.Controller.SignInForgotPassword(request).Result;

            Assert.IsInstanceOfType(response, typeof(UnauthorizedResult));
        }
    }
}
