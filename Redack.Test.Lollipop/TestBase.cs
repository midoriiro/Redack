using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web.Http;
using NMemory.Linq;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Configuration;
using Redack.Test.Lollipop.Entity;
using Thread = System.Threading.Thread;

namespace Redack.Test.Lollipop
{
    public class TestBase : IDisposable
    {
        protected readonly RedackDbContext Context;

        public TestBase()
        {
            EffortProviderFactory.ResetDb();

            var factory = new EffortProviderFactory();
            this.Context = new RedackDbContext(factory.CreateConnection(""));
        }

        public Group CreateValidGroup(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidGroupCustomization());

            var group = fixture.Create<Group>();

            if (!push) return group;

            this.Context.Groups.Add(group);
            this.Context.SaveChanges();

            return group;
        }

        public Node CreateValidNode(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidNodeCustomization());

            var node = fixture.Create<Node>();

            if (!push) return node;

            this.Context.Nodes.Add(node);
            this.Context.SaveChanges();

            return node;
        }

        public DomainLayer.Model.Thread CreateValidThread(Node node = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidThreadCustomization());

            node = node ?? this.CreateValidNode();

            var thread = fixture.Create<DomainLayer.Model.Thread>();
            thread.Node = node;

            if (!push) return thread;

            this.Context.Threads.Add(thread);
            this.Context.SaveChanges();

            return thread;
        }

        public Message CreateValidMessage(
            User user = null, 
            DomainLayer.Model.Thread thread = null, 
            bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidMessageCustomization());

            user = user ?? this.CreateValidUser();
            thread = thread ?? this.CreateValidThread();

            var message = fixture.Create<Message>();
            message.Author = user;
            message.Thread = thread;

            if (!push) return message;

            this.Context.Messages.Add(message);
            this.Context.SaveChanges();

            return message;
        }

        public Permission CreateValidPermission<TEntity>(bool push = true) where TEntity : DomainLayer.Model.Entity
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidPermissionCustomization<TEntity>());

            var permission = fixture.Create<Permission>();

            if (!push) return permission;

            this.Context.Permissions.Add(permission);
            this.Context.SaveChanges();

            return permission;
        }

        public ApiKey CreateValidApiKey(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidApiKeyCustomization(256));

            var apiKey = fixture.Create<ApiKey>();

            if (!push) return apiKey;

            this.Context.ApiKeys.Add(apiKey);
            this.Context.SaveChanges();

            return apiKey;
        }

        public Credential CreateValidCredential(ApiKey apiKey = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidCredentialCustomization());

            apiKey = apiKey ?? this.CreateValidApiKey(push: false);

            var credential = fixture.Create<Credential>();
            credential.ApiKey = apiKey;

            if (!push) return credential;

            this.Context.Credentials.Add(credential);
            this.Context.SaveChanges();

            return credential;
        }

        public User CreateValidUser(Credential credential = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidUserCustomization());

            credential = credential ?? this.CreateValidCredential(push: false);

            var user = fixture.Create<User>();
            user.Credential = credential;

            if (!push) return user;

            this.Context.Users.Add(user);
            this.Context.SaveChanges();

            return user;
        }

        public Client CreateValidClient(ApiKey apiKey = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidClientCustomization());

            apiKey = apiKey ?? this.CreateValidApiKey(push: false);

            var client = fixture.Create<Client>();
            client.ApiKey = apiKey;

            if (!push) return client;

            var ll = this.Context.Clients.ToList();

            this.Context.Clients.Add(client);
            this.Context.SaveChanges();

            return client;
        }

        public Identity CreateValidIdentity(User user = null, Client client = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidIdentityCustomization());

            user = user ?? this.CreateValidUser();
            client = client ?? this.CreateValidClient();

            var identity = fixture.Create<Identity>();
            identity.User = user;
            identity.Client = client;
            identity.Access = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, 5);
            identity.Refresh = JwtTokenizer.Encode(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, 1000);

            if (!push) return identity;

            this.Context.Identities.Add(identity);
            this.Context.SaveChanges();

            return identity;
        }

        public virtual void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
