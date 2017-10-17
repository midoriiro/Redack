using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/clients")]
    public class ClientsController : RepositoryApiController<Client> {}
}