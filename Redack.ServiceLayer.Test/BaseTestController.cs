using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Http;
using Group = Redack.DomainLayer.Models.Group;

namespace Redack.ServiceLayer.Test
{
	public class BaseTestController<TController> : BaseTest where TController : BaseApiController, new ()
	{
		protected TController Controller;
		protected HttpClient Client;
		private AuthenticationHeaderValue _auth;

		private Dictionary<string, Dictionary<string, Entity>> _dataSet;

		public BaseTestController() : base()
		{
			this.Controller = new TController()
			{
				Request = new HttpRequestMessage(),
				Configuration = new HttpConfiguration()
			};			

			this.Client = new HttpClient(this.Server);
		}

		public Dictionary<string, Dictionary<string, Entity>> GetDataSet<TEntity>() where TEntity : Entity
		{
			if (this._dataSet != null)
				return this._dataSet;

			var permissions = this.CreatePermissions<TEntity>();

			var apikeys = new List<ApiKey>
			{
				this.CreateApiKey(),
				this.CreateApiKey(),
				this.CreateApiKey(),
				this.CreateApiKey()
			};

			var credentialAdmin = this.CreateCredential(apikeys[0], push: false);
			var credentialLambda = this.CreateCredential(apikeys[1], push: false);

			var userAdmin = this.CreateUser(credentialAdmin);
			var userMod = this.CreateUser();
			var userLambda = this.CreateUser(credentialLambda);

			var groupAdmin = this.CreateGroup();
			groupAdmin.Name = "Administrator";
			groupAdmin.Permissions.Add(permissions["Create"]);
			groupAdmin.Permissions.Add(permissions["Retrieve"]);
			groupAdmin.Permissions.Add(permissions["Update"]);
			groupAdmin.Permissions.Add(permissions["Delete"]);
			groupAdmin.Users.Add(userAdmin);

			var groupModerator = this.CreateGroup();
			groupModerator.Name = "Moderator";
			groupModerator.Permissions.Add(permissions["Create"]);
			groupModerator.Permissions.Add(permissions["Retrieve"]);
			groupModerator.Permissions.Add(permissions["Update"]);
			groupModerator.Permissions.Add(permissions["Delete"]);
			groupModerator.Users.Add(userMod);

			var groupLambda = this.CreateGroup();
			groupLambda.Name = "Lambda";
			groupLambda.Users.Add(userLambda);

			using (var repository = new Repository<Group>())
			{
				repository.Update(groupAdmin);
				repository.Update(groupModerator);
				repository.Update(groupLambda);

				repository.Commit();
			}

			var client1 = this.CreateClient(apikeys[2]);
			var client2 = this.CreateClient(apikeys[3]);

			this._dataSet = new Dictionary<string, Dictionary<string, Entity>>()
			{
				{
					"permissions", new Dictionary<string, Entity>()
					{
						{ "create", permissions["Create"] },
						{ "retrieve", permissions["Retrieve"] },
						{ "update", permissions["Update"] },
						{ "delete", permissions["Delete"] }
					}
				},
				{
					"users", new Dictionary<string, Entity>()
					{
						{ "admin", userAdmin },
						{ "mod", userMod },
						{ "lambda", userLambda }
					}
				},
				{
					"credentials", new Dictionary<string, Entity>()
					{
						{ "credential1", userAdmin.Credential },
						{ "credential2", userLambda.Credential }
					}
				},
				{
					"groups", new Dictionary<string, Entity>()
					{
						{ "admin", groupAdmin },
						{ "moderator", groupModerator },
						{ "lambda", groupLambda }
					}
				},
				{
					"clients", new Dictionary<string, Entity>()
					{
						{ "client1", client1 },
						{ "client2", client2 }
					}
				},
				{
					"apikeys", new Dictionary<string, Entity>()
					{
						{ "apikey1", apikeys[0] },
						{ "apikey2", apikeys[1] },
						{ "apikey3", apikeys[2] },
						{ "apikey4", apikeys[3] },
					}
				}
			};

			return this._dataSet;
		}

		public JwtIdentity CreateJwtIdentity(User user = null, Client client = null)
		{
			user = user ?? this.CreateUser();
			client = client ?? this.CreateClient();

			return new JwtIdentity(this.CreateIdentity(user, client));
		}

		public AuthenticationHeaderValue CreateAuthenticationHeader(Identity identity)
		{
			return new AuthenticationHeaderValue("Basic", identity.Access);
		}

		public void CreateAuthentifiedUser(User user, Client client = null)
		{
			this._auth = this.CreateAuthenticationHeader(this.CreateIdentity(user, client));
		}

        public void CreateAuthentifiedUser(Identity identity)
        {
            this._auth = this.CreateAuthenticationHeader(identity);
        }        

		public HttpRequestMessage CreateRequest(HttpMethod method, int? id = null, IEntityRequest body = null, string uriEndPoint = null)
		{
			var request = new HttpRequestMessage()
			{
				RequestUri = new Uri(this.GetUrIFromMethodName(method, this.GetEntityNameFromClassName(), id) + "/" + uriEndPoint),
				Method = method,
			};

			if (body != null)
			{
				request.Content = new ObjectContent(body.GetType(), body, new JsonMediaTypeFormatter());
				request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
			}

			request.Headers.Authorization = this._auth;
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return request;
		}

		private string GetEntityNameFromClassName()
		{
			var className = typeof(TController).Name;
			return Regex.Match(className, "([a-zA-Z]+s)Controller").Groups[1].Value.ToLower();
		}

		private string GetUrIFromMethodName(HttpMethod method, string entity, int? id = null)
		{
			var uri = $"http://localhost/api/{entity}";

			if (id == null && method != HttpMethod.Put && method != HttpMethod.Delete)
				return uri;

			return $"{uri}/{id}";
		}

		public void SetControllerIdentity(IIdentity identity)
		{
			this.Controller.User = new GenericPrincipal(identity, null);
		}

		public override void Dispose()
		{
			this.Client.Dispose();
			this.Controller.Dispose();

			base.Dispose();
		}
	}
}
