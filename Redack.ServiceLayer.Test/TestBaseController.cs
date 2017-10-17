using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace Redack.ServiceLayer.Test
{
    public class TestBaseController<TController> : TestBase where TController : BaseApiController, new ()
    {
        protected TController Controller;

        public TestBaseController() : base()
        {
            this.Controller = new TController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        public JwtIdentity CreateJwtIdentity(User user = null, Client client = null)
        {
            user = user ?? this.CreateUser();
            client = client ?? this.CreateClient();

            return new JwtIdentity(this.CreateIdentity(user, client));
        }

        public void SetControllerIdentity(IIdentity identity)
        {
            this.Controller.User = new GenericPrincipal(identity, null);
        }

        public override void Dispose()
        {
            this.Controller.Dispose();

            base.Dispose();
        }
    }
}
