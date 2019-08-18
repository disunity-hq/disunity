using System.Collections.Generic;

using Disunity.Client.v1;
using Disunity.Core.Archives;

using Newtonsoft.Json;


namespace Disunity.Management.Models {

    [JsonObject(MemberSerialization.OptIn)]
    public class ModVersion {

        [JsonProperty]public Mod Mod { get; set; }

        [JsonProperty]public string VersionNumber { get; set; }

        public string Path { get; set; }

        public Dictionary<Mod, VersionRange> Dependencies { get; set; }

    }

}