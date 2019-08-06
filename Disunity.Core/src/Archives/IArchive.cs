using System.Collections.Generic;
using System.IO;


namespace Disunity.Core.Archives {

    public interface IArchive {

        bool HasEntry(string filename);
        bool HasReadme();
        bool HasManifest();
        bool HasArtifact(string filename);
        bool HasPreloadAssembly(string filename);
        bool HasRuntimeAssembly(string filename);
        bool HasPrefabBundle(string filename);
        bool HasSceneBundle(string filename);

        Manifest GetManifest();
        Stream GetReadme();
        Stream GetEntry(string filename);
        Stream GetArtifact(string filename);
        Stream GetPreloadAssembly(string filename);
        Stream GetRuntimeAssembly(string filename);
        Stream GetPrefabBundle(string filename);
        Stream GetSceneBundle(string filename);
        
        IEnumerable<string> ArtifactPaths { get; }
        IEnumerable<string> PreloadAssemblyPaths { get; }
        IEnumerable<string> RuntimeAssemblyPaths { get; }
        IEnumerable<string> PrefabBundlePaths { get; }
        IEnumerable<string> SceneBundlePaths { get; }
        Manifest Manifest { get; }
        string Readme { get; }

    }

}