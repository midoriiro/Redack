using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Security
{
    public class JwtIdentity : GenericIdentity
    {
        public int Id { get; }

        public JwtIdentity(User user) : base(user.Alias)
        {
            this.Id = user.Id;
        }

        public User GetUser()
        {
            User user;

            using (var repository = new Repository<User>())
            {
                user = repository.GetById(this.Id);
            }

            return user;
        }

        public Identity GetIdentity()
        {
            Identity identity;

            using (var repository = new Repository<Identity>())
            {
                identity = repository.Query(e => e.ApiKey.Id == this.GetUser().Credential.ApiKey.Id).Single();
            }

            return identity;
        }
    }
}