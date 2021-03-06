﻿using Ploeh.AutoFixture;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.Test.Lollipop.Customizations;
using Redack.Test.Lollipop.Entities;

namespace Redack.Test.Lollipop.Models
{
	public class SignInRequestCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            fixture.Customize(new EmailAddressCustomization<SignInRequest>("Login"));
            fixture.Customize(new ClientCustomization());
        }
    }
}
