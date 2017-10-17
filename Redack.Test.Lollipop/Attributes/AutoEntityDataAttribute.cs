using AutoFixture.AutoEF;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.Test.Lollipop.Attributes
{
    public class AutoEntityDataAttribute : AutoDataAttribute
    {
        public AutoEntityDataAttribute()
            : base(new Fixture()
                .Customize(new EntityCustomization(new DbContextEntityTypesProvider(typeof(RedackDbContext)))))
        { }
    }
}
