using Microsoft.CodeAnalysis.Text;

using Newtonsoft.Json;


namespace Disunity.Store.Errors {

    public class ManifestParseError : ApiError {

        [JsonProperty] public string Path { get; }
        [JsonProperty] public string Source { get; }
        [JsonProperty] public int LineNumber { get; }
        [JsonProperty] public int LinePosition { get; }

        public ManifestParseError(JsonReaderException error)
            : base(error.Message) {
            Path = error.Path;
            Source = error.Source;
            LineNumber = error.LineNumber;
            LinePosition = error.LinePosition;
        }

    }

}