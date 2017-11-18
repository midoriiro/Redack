using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using Redack.DatabaseLayer.DataAccess;
using System.Linq;
using Redack.ServiceLayer.Controllers;

namespace Redack.ServiceLayer.Models.Request
{
    public class ForgotPasswordRequest : BaseRequest<Identity>
    {
        [Required]
        public int Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordConfirm { get; set; }

        public override Entity ToEntity(RedackDbContext context)
        {
            Client client;

            using (var repository = new Repository<Client>(context, false))
                client = repository
                    .Query(e => e.Id == this.Client)
                    .SingleOrDefault();

            User user;

            using (var repository = new Repository<User>(context, false))
                user = repository
                    .Query(e => e.Credential.Login == this.Login && e.Credential.Password == this.OldPassword)
                    .SingleOrDefault();

            if (user == null)
                return null;

            user = User.Update(
                user, 
                this.NewPassword, 
                this.NewPasswordConfirm, 
                IdentitiesController.KeySize);

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