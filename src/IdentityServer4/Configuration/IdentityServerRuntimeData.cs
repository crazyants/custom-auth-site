using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Configuration
{
    public class IdentityServerRuntimeData
    {
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<IdentityResource> IdentityResources { get; set; }
        public IEnumerable<ApiResource> ApiResources { get; set; }
    }
}
