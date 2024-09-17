using IdentityServer4.Models;
using IdentityServer4;

namespace TestCase
{
    /// <summary>
    /// IdentityServer yapılandırması için gerekli kaynakları, kimlik kaynaklarını, API kapsamlarını ve istemcileri tanımlar.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// API kaynaklarını döndürür.
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
             new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        /// <summary>
        /// Kimlik kaynaklarını döndürür.
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
             new IdentityResources.Email(),
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{ "role"} }
        };

        /// <summary>
        /// API kapsamlarını döndürür.
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

        /// <summary>
        /// İstemci yapılandırmalarını döndürür.
        /// </summary>
        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientName="case",
                ClientId="WebClient",
                ClientSecrets= {new Secret("test".Sha256())},
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes={ IdentityServerConstants.LocalApi.ScopeName }
            },
        };
    }
}