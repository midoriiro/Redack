using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Configurations;
using Redack.Test.Lollipop.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Redack.Test.Lollipop.Data;

namespace Redack.Test.Lollipop
{
	public class BaseTest
    {
        protected readonly EffortProviderFactory Factory;
        protected RedackDbContext Context;
		protected HttpServer Server;

		public BaseTest()
        {
            EffortProviderFactory.ResetDb();

            this.Factory = new EffortProviderFactory();
            this.Context = new RedackDbContext(this.Factory.CreateConnection(""));
		}

	    public void InitialyzeServer()
	    {
		    var config = new HttpApiConfiguration
		    {
			    DbConnection = this.Factory.CreateConnection("")
		    };

		    WebApiConfig.Register(config);

		    this.Server = new HttpServer(config);
		}

		public IEntityRequest CreateBodyRequest<TRequest, TEntity>(TEntity entity) where TRequest : IEntityRequest, new() where TEntity : Entity
		{
			var request = new TRequest();
			request.FromEntity(entity);

			return request;
		}

		public RedackDbContext CreateContext()
        {
            return new RedackDbContext(this.Factory.CreateConnection(""));
        }

	    public DummyDbContext CreateDummyContext()
	    {
		    return new DummyDbContext(this.Factory.CreateConnection(""));
	    }

		public Repository<TEntity> CreateRepository<TEntity>() where TEntity : DomainLayer.Models.Entity
        {
            return new Repository<TEntity>(this.CreateContext());
        }

        public Repository<TEntity> CreateRepository<TEntity>(RedackDbContext context, bool disposable = true) where TEntity : DomainLayer.Models.Entity
        {
            return new Repository<TEntity>(context, disposable);
        }

        public Group CreateGroup(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new GroupCustomization());

            var group = fixture.Create<Group>();

            if (!push) return group;

            using (var repository = this.CreateRepository<Group>(this.Context, false))
            {
                repository.Insert(group);
                repository.Commit();
            }

            return group;
        }

        public Node CreateNode(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new NodeCustomization());

            var node = fixture.Create<Node>();

            if (!push) return node;

            using (var repository = this.CreateRepository<Node>(this.Context, false))
            {
                repository.Insert(node);
                repository.Commit();
            }

            return node;
        }

        public Thread CreateThread(Node node = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ThreadCustomization());

            node = node ?? this.CreateNode();

            var thread = fixture.Create<Thread>();
            thread.Node = node;

            if (!push) return thread;

            using (var repository = this.CreateRepository<Thread>(this.Context, false))
            {
                repository.Insert(thread);
                repository.Commit();
            }

            return thread;
        }

        public Message CreateMessage(
            User user = null, 
            Thread thread = null, 
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

            using (var repository = this.CreateRepository<Message>(this.Context, false))
            {
                repository.Insert(message);
                repository.Commit();
            }

            return message;
        }

        public MessageRevision CreateMessageRevision(
            User user = null, 
            Message message = null, 
            bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new MessageRevisionCustomization());

            user = user ?? this.CreateUser();
            message = message ?? this.CreateMessage();

            var revision = fixture.Create<MessageRevision>();
            revision.Editor = user;
            revision.Message = message;

            if (!push) return revision;

            using (var repository = this.CreateRepository<MessageRevision>(this.Context, false))
            {
                repository.Insert(revision);
                repository.Commit();
            }

            return revision;
        }

        public Permission CreatePermission<TEntity>(string codename = null, bool push = true) where TEntity : DomainLayer.Models.Entity
        {
            var fixture = new Fixture();
            fixture.Customize(new PermissionCustomization<TEntity>());

            var permission = fixture.Create<Permission>();
            permission.Codename = codename ?? permission.Codename;

            if (!push) return permission;

            using (var repository = this.CreateRepository<Permission>(this.Context, false))
            {
                repository.Insert(permission);
                repository.Commit();
            }

            return permission;
        }

        public Dictionary<string, Permission> CreatePermissions<TEntity>(bool push = true) where TEntity : DomainLayer.Models.Entity
        {
            Dictionary<string, Permission> result = new Dictionary<string, Permission>
            {
                {"Create", this.CreatePermission<TEntity>("Create", push)},
                {"Retrieve", this.CreatePermission<TEntity>("Retrieve", push)},
                {"Update", this.CreatePermission<TEntity>("Update", push)},
                {"Delete", this.CreatePermission<TEntity>("Delete", push)}
            };

            return result;
        }

        public ApiKey CreateApiKey(bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ApiKeyCustomization(256));

            var apiKey = fixture.Create<ApiKey>();

            if (!push) return apiKey;

            using (var repository = this.CreateRepository<ApiKey>(this.Context, false))
            {
                repository.Insert(apiKey);
                repository.Commit();
            }

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

            using (var repository = this.CreateRepository<Credential>(this.Context, false))
            {
                repository.Insert(credential);
                repository.Commit();
            }

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

            using (var repository = this.CreateRepository<User>(this.Context, false))
            {
                repository.Insert(user);
                repository.Commit();
            }

            return user;
        }

        public Client CreateClient(ApiKey apiKey = null, bool push = true)
        {
            var fixture = new Fixture();
            fixture.Customize(new ClientCustomization());

            apiKey = apiKey ?? this.CreateApiKey(push: false);

            var client = fixture.Create<Client>();
            client.ApiKey = apiKey;

			int id = client.Id;

			client = Client.Create(client.Name, client.PassPhrase, client.ApiKey);
			client.Id = id;
			client.IsBlocked = false;

			if (!push) return client;

            using (var repository = this.CreateRepository<Client>(this.Context, false))
            {
                repository.Insert(client);
                repository.Commit();
            }

            return client;
        }

        public Identity CreateIdentity(
            User user = null, 
            Client client = null, 
            bool push = true)
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

            using (var repository = this.CreateRepository<Identity>(this.Context, false))
            {
                repository.Insert(identity);
                repository.Commit();
            }

            return identity;
        }

        public virtual void Dispose()
        {
            this.Context.Dispose();
			this.Server.Dispose();
		}
    }
}
