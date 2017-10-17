namespace Redack.DomainLayer.Exceptions
{
    public abstract class CredentialException : System.Exception
    {
        protected CredentialException(string message) : base(message) { }
    }

    public class CredentialPasswordConfirmException : CredentialException
    {
        public CredentialPasswordConfirmException() : base("Password confirmation does not match")
        {
        }
    }
}
