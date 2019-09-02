using System;
using System.Collections.Generic;
using System.IO;

using BindingAttributes;


namespace Disunity.Management.Options {

    [Options]
    public class ManagementOptions {

        /// <summary>
        /// Root path for all files managed by disunity.
        /// </summary>
        /// <remarks>
        /// Defaults to `<see cref="Environment.SpecialFolder.ApplicationData"/>/disunity`
        /// </remarks>
        public string RootPath { get; set; }

        /// <summary>
        /// A list of Uri's to be used as package sources
        /// </summary>
        public List<string> PackageSources { get; set; }

        public ManagementOptions() {
            RootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "disunity");
            PackageSources = new List<string> {
                "disunity://api/v1"
            };
        }

    }

}