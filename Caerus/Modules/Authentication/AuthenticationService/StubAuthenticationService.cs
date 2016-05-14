﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caerus.Common.Modules.Authentication.Interfaces;

namespace AuthenticationService
{
    public class StubAuthenticationService: IAuthenticationService
    {
        public void ConfigureAuth(Owin.IAppBuilder app)
        {
            return;
        }
    }
}
