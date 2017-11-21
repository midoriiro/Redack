﻿using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;
using System;

namespace Redack.Test.Lollipop.Entities
{
    public class ClientCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new ApiKeyCustomization(256));
            fixture.Customize(new IgnorePropertiesCustomization(new[] {"Identities"}));
            fixture.Customize<Client>(e => e.With(p => p.IsBlocked, false));
			fixture.Customize<Client>(e => e.With(p => p.Salt, Convert.ToBase64String(Client.CreateRandomSalt())));
        }
    }
}
