﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace Caerus.Common.Modules.Authentication.Interfaces
{
    public interface IAuthenticationService {
        void ConfigureAuth(IAppBuilder app);
    }
}
