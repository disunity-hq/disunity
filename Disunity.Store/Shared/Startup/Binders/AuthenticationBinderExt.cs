using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class AuthenticationBinderExt {

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config) {
            var authenticationBuilder = services.AddAuthentication();
            var githubClientId = config.GetValue<string>("Auth:Github:ClientId");
            var githubClientSecret = config.GetValue<string>("Auth:Github:ClientSecret");
            var discordClientId = config.GetValue<string>("Auth:Discord:ClientId");
            var discordClientSecret = config.GetValue<string>("Auth:Discord:ClientSecret");

            if (!string.IsNullOrWhiteSpace(githubClientId) && !string.IsNullOrWhiteSpace(githubClientSecret)) {
                authenticationBuilder.AddGitHub(options => {
                    options.ClientId = githubClientId;
                    options.ClientSecret = githubClientSecret;
                });
            }

            if (!string.IsNullOrWhiteSpace(discordClientId) && !string.IsNullOrWhiteSpace(discordClientSecret)) {
                authenticationBuilder.AddDiscord(options => {
                    options.ClientId = discordClientId;
                    options.ClientSecret = discordClientSecret;
                });
            }

        }

    }

}