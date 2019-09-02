using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Management.Attributes;
using Disunity.Management.Extensions;
using Disunity.Management.Models;
using Disunity.Management.Options;
using Disunity.Management.PackageSources;

using Microsoft.Extensions.Options;


namespace Disunity.Management.Services {

    /// <summary>
    /// Service that houses all <see cref="IPackageSource"/>s and provides methods to interact with them
    /// </summary>
    public interface IPackageSourceService {

        /// <summary>
        /// Goes through the list of package sources until it is successfully able to 
        /// </summary>
        /// <param name="packageIdentifier"></param>
        /// <returns></returns>
        Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier);

    }

    [AsSingleton(typeof(IPackageSourceService))]
    public class PackageSourceService : IPackageSourceService {

        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ManagementOptions> _options;
        private readonly List<IPackageSource> _sources;
        private Dictionary<string, List<Type>> _sourceTypeMap;

        public PackageSourceService(IServiceProvider serviceProvider, IOptions<ManagementOptions> options) {
            _serviceProvider = serviceProvider;
            _options = options;
            _sourceTypeMap = GetAvailableSourceTypes(Assembly.GetExecutingAssembly());
            _sources = InstantiateSources();
        }

        public async Task<Stream> GetPackageImportStream(PackageIdentifier packageIdentifier) {
            foreach (var packageSource in _sources) {
                if (!await packageSource.CanHandlePackage(packageIdentifier)) continue;
                var importStream = await packageSource.GetPackageImportStream(packageIdentifier);

                if (importStream != null) {
                    return importStream;
                }
            }

            return null;
        }

        private List<IPackageSource> InstantiateSources() {
            var packageSources = new List<IPackageSource>();

            var sourceUris = _options.Value.PackageSources;

            if (sourceUris.Count == 0) {
                throw new InvalidOperationException("At least one package source must be specified");
            }

            foreach (var sourceUri in sourceUris) {
                var packageSourceTypes = FindMatchingSourceTypes(sourceUri);

                if (packageSourceTypes == null) {
                    throw new InvalidOperationException($"No matching PackageSource to handle {sourceUri}");
                }

                foreach (var type in packageSourceTypes) {
                    var packageSource = _serviceProvider.Instantiate<IPackageSource>(type);
                    packageSource.SourceUri = sourceUri;

                    packageSources.Add(packageSource);
                }
            }

            return packageSources;
        }

        private List<Type> FindMatchingSourceTypes(string uri) {
            var uriSegments = uri.Split('/');

            // Might want to make this >= if we want to support a PackageSource catching all requests...
            for (var numSegments = uriSegments.Length; numSegments > 0; --numSegments) {
                var searchString = string.Join("/", uriSegments, 0, numSegments);
                if (!_sourceTypeMap.ContainsKey(searchString)) continue;
                return _sourceTypeMap[searchString];
            }

            return null;
        }

        private static Dictionary<string, List<Type>> GetAvailableSourceTypes(params Assembly[] assemblies) {
            var sourceTypeMap = new Dictionary<string, List<Type>>();

            var types = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.IsSubclassOf(typeof(IPackageSource)));

            foreach (var type in types) {
                foreach (var attr in type.GetCustomAttributes<PackageSourceAttribute>()) {
                    List<Type> prefixSources;

                    if (!sourceTypeMap.TryGetValue(attr.UriPrefix, out prefixSources)) {
                        prefixSources = new List<Type>();
                        sourceTypeMap.Add(attr.UriPrefix, prefixSources);
                    }

                    prefixSources.Add(type);

                }
            }

            return sourceTypeMap;
        }

    }

}