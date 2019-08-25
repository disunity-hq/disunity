using System.Net.Http;

using Microsoft.Extensions.Configuration;


namespace Disunity.Client.v1 {

    public class ApiClient : IApiClient {

        public IDisunityClient DisunityClient { get; }
        public IModListClient ModListClient { get; }
        public IModPublishingClient ModPublishingClient { get; }
        public IOrgMemberClient OrgMemberClient { get; }
        public ITargetClient TargetClient { get; }
        public IUploadClient UploadClient { get; }

        public ApiClient(IConfiguration config, HttpClient httpClient) {
            HttpClient = httpClient;
            DisunityClient = new DisunityClient(config, httpClient);
            ModListClient = new ModListClient(config, httpClient);
            ModPublishingClient = new ModPublishingClient(config, httpClient);
            OrgMemberClient = new OrgMemberClient(config, httpClient);
            TargetClient = new TargetClient(config, httpClient);
            UploadClient = new UploadClient(config, httpClient);
        }

        public HttpClient HttpClient { get; }

    }

}