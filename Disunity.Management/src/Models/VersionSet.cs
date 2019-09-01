using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
#pragma warning disable 618


namespace Disunity.Management.Models {

    public class VersionSet {

        /// <summary>
        /// Internal id of the version set. Used only for reference linking
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A list of all packages. For now, to add or remove a package, make a new list and assign to to this property
        /// </summary>
        public IImmutableList<Package> Packages {
            get => ImmutableList.Create(VersionSetPackages.Select(m => m.Package).ToArray());
            set { VersionSetPackages = value.Select(p => new VersionSetPackage {Package = p, VersionSet = this}).ToList(); }
        }

        /// <summary>
        /// Internal list of packages used by the database system. There is not need to modify this list directly
        /// </summary>
        [Obsolete("`VersionSetPackages` is an implementation detail. Please use `Packages` instead")]
        public List<VersionSetPackage> VersionSetPackages { get; set; }

    }

}