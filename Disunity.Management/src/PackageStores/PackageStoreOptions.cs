using System;


namespace Disunity.Management.PackageStores {

    public class PackageStoreOptions {

        public PackageStoreOptions() {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Path = System.IO.Path.Combine(appData, "disunity", "data");
        }

        public string Path { get; set; }

    }

}