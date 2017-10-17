using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidPermissionCustomization<TEntity> : BaseValidEntityCustomization, ICustomization where TEntity : DomainLayer.Model.Entity
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