using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Disunity.Core.Archives {

    public static partial class Schema {

        public const string SLUG_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*$";
        private const string NAME_PATTERN = "^[a-zA-Z0-9 -_]+$";
        public const string WORD_PATTERN = "^[a-zA-Z0-9-_]+$";
        private const string CLASS_PATTERN = "^[a-zA-Z]+$";
        private const string PACKAGE_PATTERN = "^[a-z0-9]+(?:-[a-z0-9]+)*/[a-z0-9]+(?:-[a-z0-9]+)*$";
        public const string VERSION_PATTERN = "^[0-9]+\\.[0-9]+\\.[0-9]+$";
        private const string DLL_PATTERN = "^[a-zA-Z0-9-_]+\\.dll$";
        private const string TAG_PATTERN = "^[a-z]+(?:-[a-z]+)*$";

        private static object SchemaData() {
            return Object(new {
                ManifestVersion = Integer(1, 1),

                // identifiers
                OrgID = String(SLUG_PATTERN),
                ModID = String(SLUG_PATTERN),
                Version = String(VERSION_PATTERN),

                // information
                DisplayName = String(NAME_PATTERN),
                URL = String(format: "url"),
                Description = String(),
                Tags = Array(String(TAG_PATTERN)),
                ContentTypes = Integer(0, 62),

                // relations
                UnityVersions = Object(new {
                    MinVersion = String(),
                    MaxVersion = String()
                }),

                Targets = Object(
                    propertyNames: new {pattern = SLUG_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                Dependencies = Object(
                    propertyNames: new {pattern = PACKAGE_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                OptionalDependencies = Object(
                    propertyNames: new {pattern = PACKAGE_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                Incompatibilities = Object(
                    propertyNames: new {pattern = PACKAGE_PATTERN},
                    additionalProps: Object(new {
                        MinVersion = String(),
                        MaxVersion = String()
                    })),

                // assets
                Icon = String(),
                Readme = String(),
                Artifacts = Array(String()),
                PrefabBundles = Array(String()),
                SceneBundles = Array(String()),

                PreloadAssemblies = Array(String(DLL_PATTERN)),
                PreloadAssembly = String(CLASS_PATTERN),
                PreloadClass = String(CLASS_PATTERN),

                RuntimeAssemblies = Array(String(DLL_PATTERN)),
                RuntimeAssembly = String(CLASS_PATTERN),
                RuntimeClass = String(CLASS_PATTERN),

                ExtraData = Object(),

            }, dependencies: new {
                PreloadClass = new[] {"PreloadAssemblies", "PreloadAssembly"},
                PreloadAssembly = new[] {"PreloadAssemblies", "PreloadClass"},
                RuntimeClass = new[] {"RuntimeAssemblies", "RuntimeAssembly"},
                RuntimeAssembly = new[] {"RuntimeAssemblies", "RuntimeClass"}
            }, required: new[] {
                "OrgID", "ModID", "Version", "DisplayName", "URL", "Description", "ContentTypes"
            });
        }

        private static string SchemaJson(object data = null) {
            if (data == null) {
                data = SchemaData();
            }

            return JsonConvert.SerializeObject(data);
        }

        public static JSchema LoadSchema(string json = null) {
            if (json == null) {
                json = SchemaJson();
            }

            var schema = JSchema.Parse(json);
            schema.SchemaVersion = new Uri("http://json-schema.org/draft-04/schema#");
            return schema;
        }

        public static bool ValidateJson(string json, string schemaJson = null) {
            var validator = LoadSchema(schemaJson);
            var obj = JObject.Parse(json);
            return obj.IsValid(validator);
        }

    }

}