using System.Collections.Generic;
using System.Linq;


namespace Disunity.Management.Models {

    public class VersionSet {

        public int Id { get; set; }

        public List<Package> Packages {
            get => VersionSetPackages.Select(m => m.Package).ToList();
            set { VersionSetPackages = value.Select(p => new VersionSetPackage {Package = p, VersionSet = this}).ToList(); }
        }

        public List<VersionSetPackage> VersionSetPackages { get; set; }

    }

}