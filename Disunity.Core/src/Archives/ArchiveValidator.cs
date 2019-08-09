using System;
using System.IO;
using System.Linq;

using Disunity.Core.Archives.Exceptions;


namespace Disunity.Core.Archives {

    public static class ArchiveValidator {
        
        public static void ValidateArtifacts(IArchive archive) {
            var errors = (from path in archive.Manifest.Artifacts
                          let fullPath = Path.Combine("artifacts", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingEntryException(archive, EntryType.Artifact, fullPath)).ToList();

            if (errors.Any()) {
                throw new AggregateException(errors);
            }
        }

        public static void ValidatePrefabBundles(IArchive archive) {
            var errors = (from path in archive.Manifest.PrefabBundles
                          let fullPath = Path.Combine("prefabs", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingEntryException(archive, EntryType.PrefabBundle, fullPath)).ToList();

            if (errors.Any()) {
                throw new AggregateException(errors);
            }
        }

        public static void ValidateSceneBundles(IArchive archive) {
            var errors = (from path in archive.Manifest.SceneBundles
                          let fullPath = Path.Combine("scenes", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingEntryException(archive, EntryType.SceneBundle, fullPath)).ToList();

            if (errors.Any()) {
                throw new AggregateException(errors);
            }
        }

        public static void ValidatePreloadAssemblies(IArchive archive) {
            var errors = (from path in archive.Manifest.PreloadAssemblies
                          let fullPath = Path.Combine("preload", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingEntryException(archive, EntryType.PreloadAssembly, fullPath)).ToList();

            if (errors.Any()) {
                throw new AggregateException(errors);
            }
        }

        public static void ValidateRuntimeAssemblies(IArchive archive) {
            var errors = (from path in archive.Manifest.RuntimeAssemblies
                          let fullPath = Path.Combine("runtime", path)
                          let entry = archive.GetEntry(fullPath)
                          where entry == null
                          select new MissingEntryException(archive, EntryType.RuntimeAssembly, fullPath)).ToList();

            if (errors.Any()) {
                throw new AggregateException(errors);
            }
        }

        public static void Validate(IArchive archive) {
            ValidateArtifacts(archive);
            ValidatePrefabBundles(archive);
            ValidateSceneBundles(archive);
            ValidatePreloadAssemblies(archive);
            ValidateRuntimeAssemblies(archive);
        }

    }

}