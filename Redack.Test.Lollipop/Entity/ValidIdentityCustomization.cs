using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidIdentityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new ValidApiKeyCustomization(256));
            fixture.Customize(new ValidClientCustomization());
        }
    }
}
