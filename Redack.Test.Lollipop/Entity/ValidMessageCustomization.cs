using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Entity
{
    class ValidMessageCustomization : BaseValidEntityCustomization, ICustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new IgnorePropertiesCustomization(new []
            {
                "Thread",
                "Author",
                "RevisionHistory"
            }));
        }
    }
}
