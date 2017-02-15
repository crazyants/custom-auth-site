using System;
using System.Collections.Generic;

namespace IdentityServer.Configuration
{
    //TODO:ba I rippred out unused, ok??  Keep Auth entries???
    public class IdentityServerRuntimeOptions
    {
        public string CertificateFileName { get; set; }
        public string CertificatePassword { get; set; }

        public TimeSpan AuthenticationCookieExpiration { get; set; } = TimeSpan.FromHours(10);
        public bool AuthenticationCookieSlidingExpiration { get; set; } = false;
    }
}
