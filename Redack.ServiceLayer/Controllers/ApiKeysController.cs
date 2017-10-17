using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/apikeys")]
    public class ApiKeysController : RepositoryApiController<ApiKey> {}
}