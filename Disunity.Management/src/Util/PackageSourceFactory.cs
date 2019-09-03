using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BindingAttributes;

using Disunity.Management.Extensions;
using Disunity.Management.Options;
using Disunity.Management.PackageSources;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Disunity.Management.Util {

    public interface IPackageSourceFactory {

        List<Type> FindMatchingSourceTypes(string uri);
        
        IEnumerable<IPackageSource> InstantiateSource(string uri);

    }

    [AsSingleton(typeof(IPackageSourceFactory))]
    public class PackageSourceFactory : IPackageSourceFactory {

        private readonly IServiceProvider _serviceProvider;

        public readonly Dictionary<string, List<Type>> SourceTypeMap;

        public PackageSourceFactory(IServiceProvider serviceProvider, params Assembly[] assemblies) {
            _serviceProvider = serviceProvider;
            SourceTypeMap = PackageSourceAttribute.GetAvailableSourceTypes(assemblies);
        }

        [AsSingleton]
        public static PackageSourceFactory Create(IServiceProvider sp) {
            return new PackageSourceFactory(sp,
                                            Assembly.GetExecutingAssembly());
        }

        public List<Type> FindMatchingSourceTypes(string uri) {
            var matchingKey = "";

            foreach (var pair in SourceTypeMap) {
                if (uri.StartsWith(pair.Key) && matchingKey.Length < pair.Key.Length) {
                    matchingKey = pair.Key;
                }
            }

            return matchingKey.Length > 0 ? SourceTypeMap[matchingKey] : null;

        }

        public IEnumerable<IPackageSource> InstantiateSource(string uri) {
            var packageSourceTypes = FindMatchingSourceTypes(uri);

            if (packageSourceTypes == null) {
                throw new InvalidOperationException($"No matching PackageSource to handle {uri}");
            }

            var packageSources = new List<IPackageSource>();

            foreach (var type in packageSourceTypes) {
                var packageSource = _serviceProvider.Instantiate<IPackageSource>(type);
                packageSource.SourceUri = uri;

                packageSources.Add(packageSource);
            }

            return packageSources;
        }

    }

}