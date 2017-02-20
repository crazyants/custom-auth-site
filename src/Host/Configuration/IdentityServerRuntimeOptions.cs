
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Host.Configuration
{
    public class IdentityServerRuntimeOptions
    {
        public string CertificateFileName { get; set; }
        public string CertificatePassword { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<IdentityResource> IdentityResources { get; set; }
    }
}

