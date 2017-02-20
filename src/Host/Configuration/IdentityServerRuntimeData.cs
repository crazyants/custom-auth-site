using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Host.Configuration
{
    public class IdentityServerRuntimeData
    {
        public IEnumerable<IdentityResource> IdentityResources { get; set; }
    }
}
