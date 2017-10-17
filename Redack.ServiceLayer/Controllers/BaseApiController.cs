using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;
using System;
using System.Linq;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    public class BaseApiController : ApiController
    {
        protected RedackDbContext Context { get; }

        public BaseApiController() : base()
        {
            this.Context = new RedackDbContext();
        }

        public Identity GetIdentity()
        {
            var jwtIdentity = this.User.Identity as JwtIdentity;

            Identity identity = jwtIdentity?.Identity;

            if (identity == null || !this.Context.Identities.Any(e => e.Id == identity.Id))
                throw new InvalidOperationException();

            return identity;
        }
    }
}