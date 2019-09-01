using System;
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
        public string RootPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "disunity");

    }

}