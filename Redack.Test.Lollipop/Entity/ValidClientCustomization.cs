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
    public class ValidClientCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new ValidApiKeyCustomization(256));
            fixture.Customize<Client>(e => e.With(p => p.IsBlocked, false));
        }
    }
}
