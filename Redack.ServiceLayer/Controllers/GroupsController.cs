using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/groups")]
    public class GroupsController : RepositoryApiController<Group> {}
}