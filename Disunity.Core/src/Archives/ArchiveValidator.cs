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

        public static void Validate(IArchive archive) {
            ValidateArtifacts(archive);
        }

    }

}