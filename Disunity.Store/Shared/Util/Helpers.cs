using System.Collections.Generic;

using Disunity.Store.Entities;


namespace Disunity.Store.Util {

    public static class Helpers {

        public static Dictionary<string, string> RouteParamsFromModVersion(ModVersion version) {
            return version == null ? null : new Dictionary<string, string>() {
                {"ownerSlug", version.Mod.Owner.Slug},
                {"modSlug", version.Mod.Slug},
                {"versionNumber", version.VersionNumber}
            };
        }
        
        public static Dictionary<string, string> RouteParamsFromMod(Mod mod) {
            return mod == null ? null : new Dictionary<string, string>() {
                {"ownerSlug", mod.Owner.Slug},
                {"modSlug", mod.Slug},
            };
        }

    }

}