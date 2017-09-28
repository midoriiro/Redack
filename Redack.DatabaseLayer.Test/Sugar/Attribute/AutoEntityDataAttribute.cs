using AutoFixture.AutoEF;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.DatabaseLayer.Test.Sugar.Attribute
{
    public class AutoEntityDataAttribute : AutoDataAttribute
    {
        public AutoEntityDataAttribute()
            : base(new Fixture()
                .Customize(new EntityCustomization(new DbContextEntityTypesProvider(typeof(RedackDbContext)))))
        { }
    }
}
