using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redack.DomainLayer.Exception
{
    public abstract class CredentialException : System.Exception
    {
        protected CredentialException(string message) : base(message) { }
    }

    public class CredentialPasswordConfirmException : CredentialException
    {
        public CredentialPasswordConfirmException() : base("The password and password confirmation does not match")
        {
        }
    }
}
