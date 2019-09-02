using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Disunity.Management.PackageStores;


namespace Disunity.Management.PackageSources {

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class PackageSourceAttribute:Attribute {

        /// <summary>
        /// Usually just a protocol, but can be as much of a URI segment as necessary
        /// </summary>
        public string UriPrefix { get; set; }
        
        /// <summary>
        /// A set of types of <see cref="IPackageStore"/>s that are provided by this source. Leave blank for all
        /// </summary>
        public ISet<Type> AvailableStores { get; }

        /// <summary>
        /// Construct a new Package Source to handle the given URI's
        /// </summary>
        /// <param name="uriPrefix">The URI prefix this source can handle</param>
        /// <param name="storeTypes"></param>
        /// <exception cref="ArgumentException"></exception>
        public PackageSourceAttribute(string uriPrefix, params Type[] storeTypes) {
            if (string.IsNullOrWhiteSpace(uriPrefix)) {
                throw new ArgumentException("uriPrefix cannot be null or blank", nameof(uriPrefix));
            }

            if (!storeTypes.All(t => typeof(IPackageStore).IsAssignableFrom(t))) {
                throw new ArgumentException("all storeTypes must be a defendants of IPackageStore");
            }
            
            UriPrefix = uriPrefix;
            AvailableStores = new HashSet<Type>(storeTypes);
        }
        
        public static Dictionary<string, List<Type>> GetAvailableSourceTypes(params Assembly[] assemblies) {
            if (assemblies.Length == 0) assemblies = new[] {Assembly.GetExecutingAssembly()};
            var sourceTypeMap = new Dictionary<string, List<Type>>();

            var types = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => typeof(IPackageSource).IsAssignableFrom(t));

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