using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
    public class PermissionCustomization<TEntity> : BaseEntityCustomization where TEntity : DomainLayer.Models.Entity
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize<Permission>(
                e => e.With(p => p.ContentType, typeof(TEntity).Name));
            fixture.Customize(new IgnorePropertiesCustomization(new[]
            {
                "Users",
                "Groups"
            }));
        }
    }
}