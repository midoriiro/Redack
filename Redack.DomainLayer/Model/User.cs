using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Redack.DomainLayer.Model
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

        public virtual ICollection<Identity> Identities { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }

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
    }
}
