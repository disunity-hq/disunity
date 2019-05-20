using System;


namespace Disunity.Core {

    /// <summary>
    ///     Stores the exporter's settings.
    /// </summary>
    [Flags]
    public enum ContentType {
        PreloadAssemblies = 2,
        RuntimeAssemblies = 4,
        Prefabs = 8,
        Scenes = 16,
        Artifacts = 32,
    }

}