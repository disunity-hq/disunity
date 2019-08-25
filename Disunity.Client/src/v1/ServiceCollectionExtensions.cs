using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Client.v1 {

    public static class ServiceCollectionExtensions {

        public static void ConfigureApiClient(this IServiceCollection services) {
            services.AddSingleton(new HttpClient());
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<IDisunityClient, DisunityClient>();
            services.AddSingleton<ITargetClient, TargetClient>();
            services.AddSingleton<IUploadClient, UploadClient>();
            services.AddSingleton<IModListClient, ModListClient>();
            services.AddSingleton<IModPublishingClient, ModPublishingClient>();
            services.AddSingleton<IOrgMemberClient, OrgMemberClient>();
        }

    }

}