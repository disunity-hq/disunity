using System;


namespace Disunity.Cecil {

    /// <summary>
    ///     Filter mode for finding RuntimeAssemblies.
    /// </summary>
    [Flags]
    public enum AssemblyFilter {

        ApiAssemblies = 1,
        DisunityAssemblies = 2,
        ModAssemblies = 4

    }

}