using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Navigation properties
        [Required(ErrorMessage = "The credential field is required")]
        public virtual Credential Credential { get; set; }

        public virtual ICollection<Identity> Identities { get; set; } = new List<Identity>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

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

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            this.Groups.Clear();
            this.Messages.Clear();
            this.Identities.Clear();
            this.Permissions.Clear();
        }
    }
}
