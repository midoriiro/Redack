using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;

namespace Redack.Test.Lollipop.Entities
{
    public class IdentityCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            Fixture fixtureUser = new Fixture();
            fixtureUser.Customize(new UserCustomization());

            var user = fixtureUser.Create<User>();

            Fixture fixtureClient = new Fixture();
            fixtureClient.Customize(new ClientCustomization());

            var client = fixtureClient.Create<Client>();

            fixture.Customize<Identity>(e => e
            .Without(p => p.User)
            .Without(p => p.Client)
            .Without(p => p.Access)
            .Without(p => p.Refresh)
            .Do(o =>
                {
                    o.User = user;
                    o.Client = client;
                    o.Access = JwtTokenizer.Encode(o.User.Credential.ApiKey.Key, o.Client.ApiKey.Key, 10);
                    o.Refresh = JwtTokenizer.Encode(o.User.Credential.ApiKey.Key, o.Client.ApiKey.Key, 100);
                }));
        }
    }
}
