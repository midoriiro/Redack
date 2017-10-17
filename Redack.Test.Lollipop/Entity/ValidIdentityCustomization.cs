using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidIdentityCustomization : BaseValidEntityCustomization, ICustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            Fixture fixtureUser = new Fixture();
            fixtureUser.Customize(new ValidUserCustomization());

            Fixture fixtureClient = new Fixture();
            fixtureClient.Customize(new ValidClientCustomization());

            fixture.Customize<Identity>(e => e
            .Without(p => p.User)
            .Without(p => p.Client)
            .Without(p => p.Access)
            .Without(p => p.Refresh)
            .Do(o =>
                {
                    o.User = fixtureUser.Create<User>();
                    o.Client = fixtureClient.Create<Client>();
                    o.Access = JwtTokenizer.Encode(o.User.Credential.ApiKey.Key, o.Client.ApiKey.Key, 10);
                    o.Refresh = JwtTokenizer.Encode(o.User.Credential.ApiKey.Key, o.Client.ApiKey.Key, 100);
                }));
        }
    }
}
