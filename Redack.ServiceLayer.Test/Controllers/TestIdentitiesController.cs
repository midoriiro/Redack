using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Entities;
using Redack.Test.Lollipop.Models;
using System.Net;
using System.Net.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.ServiceLayer.Test.Controllers
{
	public class TestIdentitiesController : BaseTestController<IdentitiesController>
    {
        public SignUpRequest CreateValidSignUpRequest(
            User user = null, Client client = null, bool pushUser = true, bool pushClient = true)
        {
            user = user ?? this.CreateUser(push: pushUser);
            client = client ?? this.CreateClient(push: pushClient);

            var fixture = new Fixture();
            fixture.Customize(new SignUpRequestCustomization());

            var request = fixture.Create<SignUpRequest>();
            request.Client = client.Id;
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
            request.Client = client.Id;
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
            request.Client = client.Id;
            request.Login = user.Credential.Login;
            request.OldPassword = user.Credential.Password;

            return request;
        }

        [Fact]
        public void SignUp_WithValidRequest()
        {
            var signUpRequest = this.CreateValidSignUpRequest(pushUser: false);

            var request = this.CreateRequest(HttpMethod.Post, body: signUpRequest, uriEndPoint: "signup");            
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void SignUp_WithExistingUser()
        {
            var signUpRequest = this.CreateValidSignUpRequest();

            var request = this.CreateRequest(HttpMethod.Post, body: signUpRequest, uriEndPoint: "signup");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public void SignUp_WithInvalidRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser(push: false);
            user.Credential.Password = fixture.Create<string>();
            user.Credential.PasswordConfirm = fixture.Create<string>();

            var signUpRequest = this.CreateValidSignUpRequest(user, pushUser: false);

            var request = this.CreateRequest(HttpMethod.Post, body: signUpRequest, uriEndPoint: "signup");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void SignUp_WithNonExistingClient()
        {
            var signUpRequest = this.CreateValidSignUpRequest(pushClient: false);

            var request = this.CreateRequest(HttpMethod.Post, body: signUpRequest, uriEndPoint: "signup");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignUp_WithBlockedClient()
        {
            var client = this.CreateClient();
            client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(client);
                repository.Commit();
            }

            var signUpRequest = this.CreateValidSignUpRequest(client: client);

            var request = this.CreateRequest(HttpMethod.Post, body: signUpRequest, uriEndPoint: "signup");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithValidRequest()
        {
            var signInRequest = this.CreateSignInRequest();

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithExistingIdentity()
        {
            var user = this.CreateUser();
            var identity = this.CreateIdentity(user);

            var signInRequest = this.CreateSignInRequest(user, identity.Client);

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithInvalidLoginRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var signInRequest = this.CreateSignInRequest(user);
            signInRequest.Login = fixture.Create<string>();

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithInvalidPasswordRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var signInRequest = this.CreateSignInRequest(user);
            signInRequest.Password = fixture.Create<string>();

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithBlockedClient()
        {
            var client = this.CreateClient();
            client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(client);
                repository.Commit();
            }

            var signInRequest = this.CreateSignInRequest(client: client);

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignIn_WithNonExistingClient()
        {
            var signInRequest = this.CreateSignInRequest(pushClient: false);

            var request = this.CreateRequest(HttpMethod.Post, body: signInRequest, uriEndPoint: "signin");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignOut_WithValidIdentity()
        {
            var apiKey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apiKey, false);
            var user = this.CreateUser(credential);
            var client = this.CreateClient();
            var identity = this.CreateIdentity(user, client);

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signout");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

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
            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signout");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignOut_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signout");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void Refresh_WithValidIdentity()
        {
            this.CreateAuthentifiedUser(this.CreateIdentity());

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "refresh");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Refresh_WithNonExistingIdentity()
        {
            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "refresh");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void Refresh_WithInvalidIdentity()
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            var identity = fixture.Create<Identity>();

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "refresh");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
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

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "refresh");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void Refresh_WithExpiredToken()
        {
            var identity = this.CreateIdentity();

            identity.Access = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, -0.01);
            identity.Refresh = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, -0.01);

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "refresh");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void SignDown_WithValidIdentity()
        {
            var apiKey = this.CreateApiKey(false);
            var credential = this.CreateCredential(apiKey, false);
            var user = this.CreateUser(credential);
            var client = this.CreateClient();
            var identity1 = this.CreateIdentity(user, client);
            var identity2 = this.CreateIdentity(user);

            this.CreateAuthentifiedUser(identity1);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signdown");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity1.Id));
                Assert.IsNull(repository.GetById(identity2.Id));
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

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signdown");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
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

            this.CreateAuthentifiedUser(identity1);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signout/all");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

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

            this.CreateAuthentifiedUser(identity);

            var request = this.CreateRequest(HttpMethod.Get, uriEndPoint: "signout/all");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithValidRequestAndValidIdentity()
        {
            var identity = this.CreateIdentity();

            this.CreateAuthentifiedUser(identity);

            var passwordRequest = this.CreateForgotPasswordRequest(identity.User);

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            using (var repository = this.CreateRepository<Identity>())
            {
                Assert.IsNull(repository.GetById(identity.Id));
            }
        }

        [Fact]
        public void ForgotPassword_WithValidRequestAndZeroIdentity()
        {
            var passwordRequest = this.CreateForgotPasswordRequest();

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithInvalidLoginRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var passwordRequest = this.CreateForgotPasswordRequest(user);
            passwordRequest.Login = fixture.Create<string>();

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithInvalidPasswordRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var passwordRequest = this.CreateForgotPasswordRequest(user);
            passwordRequest.OldPassword = fixture.Create<string>();

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithInvalidPasswordConfirmRequest()
        {
            var fixture = new Fixture();

            var user = this.CreateUser();

            var passwordRequest = this.CreateForgotPasswordRequest(user);
            passwordRequest.NewPasswordConfirm = fixture.Create<string>();

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithNonExistingClient()
        {
            var fixture = new Fixture();
            fixture.Customize(new ForgotPasswordRequestCustomization());

            var passwordRequest = fixture.Create<ForgotPasswordRequest>();

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void ForgotPassword_WithBlockedClient()
        {
            var client = this.CreateClient();
            client.IsBlocked = true;

            using (var repository = this.CreateRepository<Client>())
            {
                repository.Update(client);
                repository.Commit();
            }

            var passwordRequest = this.CreateForgotPasswordRequest(client: client);

            var request = this.CreateRequest(HttpMethod.Post, body: passwordRequest, uriEndPoint: "signin/forgotpassword");
            var response = this.Client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
