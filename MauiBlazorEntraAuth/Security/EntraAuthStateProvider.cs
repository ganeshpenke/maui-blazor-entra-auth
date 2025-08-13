using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace MauiBlazorEntraAuth.Security
{
    public class EntraAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly Task<AuthenticationState> authenticationState;
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

            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "your-client-id",
                TenantId = "your-tenant-id",
                RedirectUri = "http://localhost:5002",
            };

            var pca = PublicClientApplicationBuilder.CreateWithApplicationOptions(pcaOptions).Build();
            var scopes = new[] { "User.Read" };
            var accounts = await pca.GetAccountsAsync();
            var result = await pca.AcquireTokenInteractive(scopes)
                                   .WithAccount(accounts.FirstOrDefault())
                                   .ExecuteAsync();

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

    }
}
