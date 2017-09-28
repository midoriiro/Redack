﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test.Sugar.Entity
{
    class ValidUserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new OmitOnRecursionCustomization(2));
            fixture.Customize(new ValidCredentialCustomization());
        }
    }
}
