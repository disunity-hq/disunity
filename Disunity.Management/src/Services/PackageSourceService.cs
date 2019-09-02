using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Management.Extensions;
using Disunity.Management.Models;
using Disunity.Management.Options;
using Disunity.Management.PackageSources;
using Disunity.Management.Util;

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

        public PackageSourceService(IServiceProvider serviceProvider, IOptions<ManagementOptions> options, IPackageSourceFactory packageSourceFactory) {
            _serviceProvider = serviceProvider;
            _options = options;
            _sources = packageSourceFactory.InstantiateSources(_options.Value.PackageSources);
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

    }

}