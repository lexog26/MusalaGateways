using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusalaGateways.Api
{
    /// <summary>
    /// Identity server in memory configs
    /// </summary>
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("gatewaysApi", "Gateways"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                //Users Api
                new Client
                {
                    //clientId/clientSecret for authentication
                    ClientId = "MusalaGatewaysApiSwagger",

                    ClientName = "SwaggerUI Client",

                    //secret for authentication
                    ClientSecrets =
                    {
                        new Secret("musalaGatewaysSecret".Sha256())
                    },
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    //Scopes that client has access to
                    AllowedScopes = { "gatewaysApi" }
                }
            };
    }
}
