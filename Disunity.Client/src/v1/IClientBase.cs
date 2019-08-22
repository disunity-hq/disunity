using System.Net.Http;


namespace Disunity.Client.v1 {

    public interface IClientBase {

        HttpClient HttpClient { get; }

    }

}