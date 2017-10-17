using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redack.DomainLayer.Model;

namespace Redack.DomainLayer.Exception
{
    public abstract class MessageException : System.Exception
    {
        protected MessageException(string message) : base(message) { }
    }
}
