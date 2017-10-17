using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : RepositoryApiController<Message> {}
}