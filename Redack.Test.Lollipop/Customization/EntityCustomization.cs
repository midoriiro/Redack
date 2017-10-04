using AutoFixture.AutoEF;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.Test.Lollipop.Customization
{
    public class EntityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new AutoFixture.AutoEF.EntityCustomization(
                new DbContextEntityTypesProvider(typeof(RedackDbContext))));
        }
    }
}
