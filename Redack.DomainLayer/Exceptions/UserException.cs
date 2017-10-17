namespace Redack.DomainLayer.Exceptions
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
