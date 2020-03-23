
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityService
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("gateway", "API Gateway"),
                new ApiResource("users", "Users service"),
                new ApiResource("review", "Review service"),
                new ApiResource("watching", "Watching service"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "react_client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {
                        "http://localhost:3000/signInCallback.html",
                        "http://localhost:3000/signInSilentCallback.html",
                    },
                    PostLogoutRedirectUris = {"http://localhost:3000/"},
                    AllowedCorsOrigins = {"http://localhost:3000" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "users",
                        "review",
                        "gateway",
                        "watching"
                    },
                    AccessTokenLifetime = 3600,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                }
            };
        }
    }
}