using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using UnityEngine;

using static System.StringComparison;


namespace Disunity.Editor {

    public static class StoreClient {

        public static List<Dependency> FetchDependencies() {
            var url = "http://thunderstore.io/api/v1/package";
            var webReq = (HttpWebRequest) WebRequest.Create(url);
            var content = new MemoryStream();

            using (var response = webReq.GetResponse()) {
                using (var responseStream = response.GetResponseStream()) {
                    responseStream.CopyTo(content);
                }
            }

            var reader = new StreamReader(content);
            content.Seek(0, SeekOrigin.Begin);
            var text = $"{{\"items\": {reader.ReadToEnd()}}}";
            var items = JsonUtility.FromJson<Dependencies>(text).items;

            foreach (var dep in items) {
                foreach (var version in dep.versions) {
                    version.owner = dep.owner;
                }
            }

            return items;
        }

        [Serializable]
        public class DependencyVersion : IComparable {

            public string full_name;

            public string name;
            public string owner;
            public string version_number;

            public int CompareTo(object other) {
                var value = other as DependencyVersion;

                if (value == null) {
                    return -1;
                }

                return string.Compare(version_number, value.version_number, CurrentCulture);
            }

        }

        [Serializable]
        public class Dependency : IComparable {

            public string full_name;

            public string name;
            public string owner;
            public List<DependencyVersion> versions;

            public int CompareTo(object other) {
                var value = other as Dependency;

                if (value == null) {
                    return -1;
                }

                return string.Compare(full_name, value.full_name, CurrentCulture);
            }

        }

        [Serializable]
        public class Dependencies {

            public List<Dependency> items;

        }

    }

}