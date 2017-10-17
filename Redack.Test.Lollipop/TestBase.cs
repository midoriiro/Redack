using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Entity;
using System;
using System.Linq;
using Redack.Test.Lollipop.Configurations;
using Redack.Test.Lollipop.Entities;

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

        public Group CreateGroup(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new GroupCustomization());

            var group = fixture.Create<Group>();

            if (!push) return group;

            this.Context.Groups.Add(group);
            this.Context.SaveChanges();

            return group;
        }

        public Node CreateNode(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new NodeCustomization());

            var node = fixture.Create<Node>();

            if (!push) return node;

            this.Context.Nodes.Add(node);
            this.Context.SaveChanges();

            return node;
        }

        public DomainLayer.Models.Thread CreateThread(Node node = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ThreadCustomization());

            node = node ?? this.CreateNode();

            var thread = fixture.Create<DomainLayer.Models.Thread>();
            thread.Node = node;

            if (!push) return thread;

            this.Context.Threads.Add(thread);
            this.Context.SaveChanges();

            return thread;
        }

        public Message CreateMessage(
            User user = null, 
            DomainLayer.Models.Thread thread = null, 
            bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new MessageCustomization());

            user = user ?? this.CreateUser();
            thread = thread ?? this.CreateThread();

            var message = fixture.Create<Message>();
            message.Author = user;
            message.Thread = thread;

            if (!push) return message;

            this.Context.Messages.Add(message);
            this.Context.SaveChanges();

            return message;
        }

        public Permission CreatePermission<TEntity>(bool push = true) where TEntity : DomainLayer.Models.Entity
        {
            var fixture = new Fixture();
            fixture.Customize(new PermissionCustomization<TEntity>());

            var permission = fixture.Create<Permission>();

            if (!push) return permission;

            this.Context.Permissions.Add(permission);
            this.Context.SaveChanges();

            return permission;
        }

        public ApiKey CreateApiKey(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ApiKeyCustomization(256));

            var apiKey = fixture.Create<ApiKey>();

            if (!push) return apiKey;

            this.Context.ApiKeys.Add(apiKey);
            this.Context.SaveChanges();

            return apiKey;
        }

        public Credential CreateCredential(ApiKey apiKey = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new CredentialCustomization());

            apiKey = apiKey ?? this.CreateApiKey(push: false);

            var credential = fixture.Create<Credential>();
            credential.ApiKey = apiKey;

            if (!push) return credential;

            this.Context.Credentials.Add(credential);
            this.Context.SaveChanges();

            return credential;
        }

        public User CreateUser(Credential credential = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new UserCustomization());

            credential = credential ?? this.CreateCredential(push: false);

            var user = fixture.Create<User>();
            user.Credential = credential;

            if (!push) return user;

            this.Context.Users.Add(user);
            this.Context.SaveChanges();

            return user;
        }

        public Client CreateClient(ApiKey apiKey = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ClientCustomization());

            apiKey = apiKey ?? this.CreateApiKey(push: false);

            var client = fixture.Create<Client>();
            client.ApiKey = apiKey;

            if (!push) return client;

            var ll = this.Context.Clients.ToList();

            this.Context.Clients.Add(client);
            this.Context.SaveChanges();

            return client;
        }

        public Identity CreateIdentity(User user = null, Client client = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityCustomization());

            user = user ?? this.CreateUser();
            client = client ?? this.CreateClient();

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
