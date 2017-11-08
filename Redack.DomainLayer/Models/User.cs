using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    [Table("Users")]
    public class User : Entity
    {
        [Required(ErrorMessage = "The alias field is required")]
        [MaxLength(15, ErrorMessage = "Type less than 15 characters")]
        [MinLength(3, ErrorMessage = "Type at least 3 characters")]
        [Index]
        public string Alias { get; set; }

        public string IdentIcon { get; set; }

        [Required(ErrorMessage = "The state field is required")]
        public bool IsEnabled { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The credential field is required")]
        public virtual Credential Credential { get; set; }

        public virtual ICollection<Identity> Identities { get; set; }
        public virtual IList<Message> Messages { get; set; } = new List<Message>();
        public virtual IList<Group> Groups { get; set; } = new List<Group>();
        public virtual IList<Permission> Permissions { get; set; } = new List<Permission>();

        public User()
        {
            this.IsEnabled = true;
        }

        public static User Create(string login, string password, string passwordConfirm, int keySize)
        {
            var user = new User()
            {
                Alias = Guid.NewGuid().ToString().Substring(0, 15),
                Credential = new Credential()
                {
                    Login = login,
                    Password = password,
                    PasswordConfirm = passwordConfirm,
                    ApiKey = new ApiKey()
                    {
                        Key = ApiKey.GenerateKey(keySize)
                    }
                }
            };

            user.Credential.ToHash();

            return user;
        }

        public static User Update(User user, string password, string passwordConfirm, int keySize)
        {
            user.Credential.Password = password;
            user.Credential.PasswordConfirm = passwordConfirm;
            user.Credential.ApiKey.Key = ApiKey.GenerateKey(keySize);

            user.Credential.ToHash();

            return user;
        }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            var q = query as IQueryable<User>;

            return (q ?? throw new InvalidOperationException()).Where(e => e.IsEnabled);
        }

        public override List<Entity> Delete()
        {
            foreach (var group in this.Groups)
            {
                for (int i = 0; i < group.Users.Count; i++)
                {
                    var user = group.Users.ElementAt(i);

                    if (user.Id == this.Id)
                        group.Users.RemoveAt(i);
                }
            }

            this.Groups.Clear();

            foreach (var permission in this.Permissions)
            {
                for (int i = 0; i < permission.Users.Count; i++)
                {
                    var user = permission.Users.ElementAt(i);

                    if (user.Id == this.Id)
                        permission.Users.RemoveAt(i);
                }
            }

            this.Permissions.Clear();

            this.Messages.Clear();

            this.IsEnabled = false;
            this.CanBeDeleted = false;

            return null;
        }
    }
}
