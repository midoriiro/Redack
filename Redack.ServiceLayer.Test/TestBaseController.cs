using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop;

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
            user = user ?? this.CreateValidUser();
            client = client ?? this.CreateValidClient();

            return new JwtIdentity(this.CreateValidIdentity(user, client));
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
