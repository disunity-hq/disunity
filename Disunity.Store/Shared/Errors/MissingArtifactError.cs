using Newtonsoft.Json;


namespace Disunity.Store.Errors {

    public class MissingArtifactError : ApiError {
        
        [JsonProperty]
        public string Filename { get; }

        public MissingArtifactError(string filename)
            : base($"Manifest mentions artifact not present in archive") {
            Filename = filename;
        }

        public override string Info =>
            "The manifest's `Archive` section lists a file not found within the `archive` directory of the mod archive.";

    }

}