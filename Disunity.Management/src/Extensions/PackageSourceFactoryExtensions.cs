using System;
using System.Collections.Generic;

using Disunity.Management.PackageSources;
using Disunity.Management.Util;


namespace Disunity.Management.Extensions {

    public static class PackageSourceFactoryExtensions {

        public static List<IPackageSource> InstantiateSources(this IPackageSourceFactory self, IList<string> sourceUris) {
            var packageSources = new List<IPackageSource>();
            
            if (sourceUris.Count == 0) {
                throw new InvalidOperationException("At least one package source must be specified");
            }

            foreach (var sourceUri in sourceUris) {
                var uriSources = self.InstantiateSource(sourceUri);
                packageSources.AddRange(uriSources);
                
            }

            return packageSources;
        }

    }

}