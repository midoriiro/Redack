using System.Data.Entity;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Linq;
using System.Security.Principal;
using System.Web.Http.Controllers;
using Redack.ServiceLayer.Controllers;

namespace Redack.ServiceLayer.Security
{
    public class JwtIdentity : GenericIdentity
    {
        public Identity Identity { get; }

        public JwtIdentity(Identity identity) : base(identity.Client.Name)
        {
            this.Identity = identity;
        }

        public IPrincipal GetPrincipal()
        {
            return new GenericPrincipal(this, null);
        }

        public User GetUser(HttpActionContext actionContext)
        {
	        BaseApiController controller = (BaseApiController)actionContext.ControllerContext.Controller;

			User user;

            using (var repository = new Repository<User>(controller.Context, false))
            {
                user = repository
                    .All()
                    .Where(e => e.Credential.ApiKey.Key == this.Identity.User.Credential.ApiKey.Key)
                    .Include(e => e.Groups.Select(p => p.Permissions))
                    .Include(e => e.Permissions)
                    .Single();
            }

            return user;
        }
    }
}