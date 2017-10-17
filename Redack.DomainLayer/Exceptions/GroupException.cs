using Redack.DomainLayer.Models;

namespace Redack.DomainLayer.Exceptions
{
    public abstract class GroupException : System.Exception
    {
        protected GroupException(string message) : base(message) {}
    }

    public class GroupUserAlreadyExist : GroupException
    {
        public GroupUserAlreadyExist(Group group, User user) : base($"Group '{@group.Name}' already contains the user '{user.Alias}'.")
        {
        }
    }

    public class GroupUserNotFoundException : GroupException
    {
        public GroupUserNotFoundException(Group group, User user) : base($"Group '{@group.Name}' does not contains user '{user.Alias}'.")
        {
        }
    }
}