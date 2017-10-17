using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/permissions")]
    public class PermissionsController : RepositoryApiController<Permission> {}
}