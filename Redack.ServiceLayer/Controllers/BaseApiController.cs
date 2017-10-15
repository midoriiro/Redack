using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;

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