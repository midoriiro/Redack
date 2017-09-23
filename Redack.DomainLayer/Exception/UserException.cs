using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redack.DomainLayer.Exception
{
    public abstract class UserException : System.Exception
    {
        protected UserException(string message) : base(message) { }
    }

    public class UserLoginFormatException : UserException
    {
        public UserLoginFormatException() : base("Login format is invalid. Your login must be a valid email address.")
        {
        }
    }
}
