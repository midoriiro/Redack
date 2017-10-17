using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Linq;
using System.Security.Principal;

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

        public User GetUser()
        {
            User user;

            using (var repository = new Repository<User>())
            {
                user = repository.Query(e => e.Credential.ApiKey.Key == this.Identity.User.Credential.ApiKey.Key).Single();
            }

            return user;
        }
    }
}