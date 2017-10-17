using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : RepositoryApiController<User> {}
}