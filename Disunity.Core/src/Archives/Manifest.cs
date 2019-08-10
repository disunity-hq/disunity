using System.Collections.Generic;

using Disunity.Core.Exceptions;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Disunity.Core.Archives {

    public partial class Manifest {

        public string ModID;

        public string OrgID;
        public string URL;
        public string Version;

        public Dictionary<string, VersionRange> OptionalDependencies;
        public List<string> Artifacts;
        public int ContentTypes;
        public Dictionary<string, VersionRange> Dependencies;
        public string Description;

        public string DisplayName;

        public object ExtraData;

        public string Icon;
        public Dictionary<string, VersionRange> Incompatibilities;
        public List<string> PrefabBundles;

        public List<string> PreloadAssemblies;
        public string PreloadAssembly;
        public string PreloadClass;
        public string Readme;

        public List<string> RuntimeAssemblies;
        public string RuntimeAssembly;
        public string RuntimeClass;
        public List<string> SceneBundles;
        public List<string> Tags;
        public Dictionary<string, VersionRange> Targets;

        public VersionRange UnityVersions;

    }

    public partial class Manifest {

        public Manifest() {
            Artifacts = new List<string>();
            Tags = new List<string>();
            UnityVersions = new VersionRange();
            Targets = new Dictionary<string, VersionRange>();
            Dependencies = new Dictionary<string, VersionRange>();
            OptionalDependencies = new Dictionary<string, VersionRange>();
            Incompatibilities = new Dictionary<string, VersionRange>();
        }

        /// <summary>
        /// Validate some JSON against the manifest JSONSchema.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="json"></param>
        /// <exception cref="ApiException"></exception>
        public static void ValidateJson(string json) {
            var schema = Schema.LoadSchema();
            var obj = JObject.Parse(json);

            obj.IsValid(schema, out IList<ValidationError> errors);

            if (errors.Count > 0) {
                throw new SchemaValidationException(errors);
            }
        }

    }

}