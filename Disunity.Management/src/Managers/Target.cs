using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.Linq;

using BindingAttributes;

using Disunity.Management.Models;
using Disunity.Management.Services;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management.Managers {

    public class Target : ITarget {

        private readonly IFileSystem _fileSystem;
        private readonly ObservableHashSet<IProfile> _profileSet;

        private Target(TargetMeta targetMeta, IFileSystem fileSystem, Func<ProfileMeta, IProfile> profileFactory) {
            _fileSystem = fileSystem;
            TargetMeta = targetMeta;

            
            var profiles = targetMeta.Profiles
                                    .Select(tp => tp.ProfileMeta)
                                    .Select(profileFactory);
            _profileSet = new ObservableHashSet<IProfile>(profiles);
        }

        public TargetMeta TargetMeta { get; }
        public ISet<IProfile> Profiles => _profileSet;
        public IProfile ActiveProfile { get; set; }

        [Factory]
        public static Func<TargetMeta, ITarget> CreateTarget(IServiceProvider sp) {
            var fileSystem = sp.GetRequiredService<IFileSystem>();
            var profileFactory = sp.GetRequiredService<Func<ProfileMeta, IProfile>>();
            return meta => new Target(meta, fileSystem, profileFactory);
        }
        

        public void Delete() {
            _fileSystem.Directory.Delete(TargetMeta.ManagedPath, true);
        }

    }

}