using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/credentials")]
    public class CredentialsController : RepositoryApiController<Credential> {}
}