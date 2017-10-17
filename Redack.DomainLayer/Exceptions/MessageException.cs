namespace Redack.DomainLayer.Exceptions
{
    public abstract class MessageException : System.Exception
    {
        protected MessageException(string message) : base(message) { }
    }
}
