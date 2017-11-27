using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using Redack.DatabaseLayer.DataAccess;
using System.Linq;

namespace Redack.ServiceLayer.Models.Request.Post
{
    public class SignInRequest : BasePostRequest<Identity>
    {
        [Required]
        public int Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public override Entity ToEntity(RedackDbContext context)
        {
            User user;

            using (var repository = new Repository<User>(context, false))
                user = repository
                    .Query(e => e.Credential.Login == this.Login && e.Credential.Password == this.Password)
                    .SingleOrDefault();

            Client client;

            using (var repository = new Repository<Client>(context, false))
                client = repository
                    .Query(e => e.Id == this.Client)
                    .SingleOrDefault();

            return new Identity()
            {
                Client = client,
                User = user
            };
        }

        public override void FromEntity(Entity entity)
        {
            throw new System.NotImplementedException();
        }        
    }
}