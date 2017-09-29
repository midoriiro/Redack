using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using Redack.DatabaseLayer.Test.Sugar.Specimen;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test.Sugar.Entity
{
    class ValidCredentialCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new OmitOnRecursionCustomization(2));
            fixture.Customize(new EmailAddressCustomization<Credential>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<Credential>(
                "Password", "PasswordConfirm"));
        }
    }
}
