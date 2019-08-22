using System.Net.Http;


namespace Disunity.Client.v1 {

    class Client : IClient {

        public IDisunityClient DisunityClient { get; }
        public IModListClient ModListClient { get; }
        public IModPublishingClient ModPublishingClient { get; }
        public IOrgMemberClient OrgMemberClient { get; }
        public ITargetClient TargetClient { get; }
        public IUploadClient UploadClient { get; }

        public Client(string baseUrl, HttpClient httpClient) {
            HttpClient = httpClient;
            DisunityClient = new DisunityClient(baseUrl, httpClient);
            ModListClient = new ModListClient(baseUrl, httpClient);
            ModPublishingClient = new ModPublishingClient(baseUrl, httpClient);
            OrgMemberClient = new OrgMemberClient(baseUrl, httpClient);
            TargetClient = new TargetClient(baseUrl, httpClient);
            UploadClient = new UploadClient(baseUrl, httpClient);
        }

        public HttpClient HttpClient { get; }

    }

}