using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace MauiBlazorEntraAuth.Security
{
    public class EntraAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public string accessToken;
        public string AccessToken => accessToken;
        private DateTimeOffset? accessTokenExpiration;
        private readonly IPublicClientApplication pca;
        private readonly string[] scopes = { "User.Read" };

        public EntraAuthStateProvider()
        {
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "854d789e-2c46-40d5-ab79-f068b27d813d",
                TenantId = "8f6bd982-92c3-4de0-985d-0e287c55e379",
                RedirectUri = "http://localhost:7777"
            };
            pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(pcaOptions)
                .WithAuthority(AzureCloudInstance.AzurePublic, pcaOptions.TenantId)
                .Build();
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync()
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var user = await LoginWithExternalProviderAsync();
                currentUser = user;

                return new AuthenticationState(currentUser);
            }
        }

        private async Task<ClaimsPrincipal> LoginWithExternalProviderAsync()
        {
            var result = await AcquireTokenAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Account.Username),
                new Claim("AccessToken", result.AccessToken),
            };

            var identity = new ClaimsIdentity(claims, "Custom");
            return new ClaimsPrincipal(identity);
        }

        public void Logout()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }

        private async Task<AuthenticationResult> AcquireTokenAsync()
        {
            var accounts = await pca.GetAccountsAsync();
            IAccount account = accounts.FirstOrDefault();

            AuthenticationResult result;
            try
            {
                if (account != null)
                {
                    result = await pca.AcquireTokenSilent(scopes, account)
                                      .ExecuteAsync();
                }
                else
                {
                    result = await pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                      .ExecuteAsync();
                }
            }
            catch (MsalUiRequiredException)
            {
                result = await pca.AcquireTokenInteractive(scopes)
                                  .WithAccount(accounts.FirstOrDefault())
                                  .ExecuteAsync();
            }

            accessToken = result.AccessToken;
            accessTokenExpiration = result.ExpiresOn;

            return result;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (accessTokenExpiration.HasValue && DateTimeOffset.UtcNow < accessTokenExpiration.Value.ToUniversalTime())
            {
                return accessToken;
            }
            await AcquireTokenAsync();
            return accessToken;
        }
    }
}
