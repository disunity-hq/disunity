using System.Collections.Generic;

using Disunity.Core.Archives;
using Disunity.Management.Models;


namespace Disunity.Management.Util {

    /// <summary>
    /// A utility for resolving version requirements into concrete <see cref="VersionSet"/>s 
    /// </summary>
    public interface IResolver {

        VersionSet ResolveVersions(Dictionary<ModIdentifier, VersionRange> modVersionRequirements);

    }

}