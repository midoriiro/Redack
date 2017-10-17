using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;
using Redack.Test.Lollipop.Entity;

namespace Redack.Test.Lollipop.Entities
{
    public class IdentityCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            Fixture fixtureUser = new Fixture();
            fixtureUser.Customize(new UserCustomization());

            Fixture fixtureClient = new Fixture();
            fixtureClient.Customize(new ClientCustomization());

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
