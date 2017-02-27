// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace RelativityAuthenticationBridge.UI
{
    public class LoggedOutModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; }
    }
}