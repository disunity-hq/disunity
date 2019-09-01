using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using BindingAttributes;

using Disunity.Core.Archives;
using Disunity.Management.Models;
using Disunity.Management.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Disunity.Management.Managers {

    public class Profile : IProfile {

        private readonly IOptions<ManagementOptions> _options;
        private readonly IFileSystem _fileSystem;

        private Profile(ProfileMeta meta, IOptions<ManagementOptions> options, IFileSystem fileSystem) {
            _options = options;
            _fileSystem = fileSystem;
            ProfileMeta = meta;
        }

        public ProfileMeta ProfileMeta { get; }

        public string ProfilePath => _fileSystem.Path.Combine(_options.Value.RootPath, "profiles", ProfileMeta.Id.ToString());
        public IDictionary<ModIdentifier, VersionRange> ModVersions { get; }

        [Factory]
        public static Func<ProfileMeta, IProfile> CreateProfile(IServiceProvider sp) {
            var options = sp.GetRequiredService<IOptions<ManagementOptions>>();
            var fileSystem = sp.GetRequiredService<IFileSystem>();
            return meta => new Profile(meta, options, fileSystem);
        }

        public void Delete() {
            _fileSystem.Directory.Delete(ProfilePath, true);
        }

    }

}