using System.ComponentModel.DataAnnotations;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Linq;
using Redack.ServiceLayer.Controllers;

namespace Redack.ServiceLayer.Models.Request.Post
{
    public class SignUpRequest : BasePostRequest<Identity>
    {
        [Required]
        public int Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }

        public override Entity ToEntity(RedackDbContext context)
        {
            User user = User.Create(
                this.Login, 
                this.Password, 
                this.PasswordConfirm, 
                ApiKey.KeySize);

            Client client;

            using (var repository = new Repository<Client>(context, false))
                client = repository
                    .Query(e => e.Id == this.Client)
                    .SingleOrDefault();

            return new Identity()
            {
                Client = client,
                User = user,
            };
        }

        public override void FromEntity(Entity entity)
        {
            throw new System.NotImplementedException();
        }        
    }
}