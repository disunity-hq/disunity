using System.Collections.Generic;
using System.IO;


namespace Disunity.Core {

    public abstract class Mod {

        private readonly List<string> _artifactFiles = new List<string>();

        /// <summary>
        ///     Initialize a new Mod with a path to a mod file.
        /// </summary>
        /// <param name="infoPath">The path to a mod file</param>
        protected Mod(string infoPath) {
            Info = ModInfo.Load(infoPath);
            InstallPath = Path.GetDirectoryName(infoPath);
        }

        public Dictionary<string, string> Artifacts { get; } = new Dictionary<string, string>();
        public ILogger Log { get; protected set; }

        /// <summary>
        /// Where the Mod is installed on disk.
        /// </summary>
        public string InstallPath { get; private set; }

        /// <summary>
        ///     This mod's Info.
        /// </summary>
        public ModInfo Info { get; }

        /// <summary>
        ///     Is the mod valid? A Mod becomes invalid when it is no longer being managed by the ModManager,
        ///     when any of its resources is missing or can't be loaded.
        /// </summary>
        public bool IsValid { get; protected set; } = true;

        protected virtual bool CheckArtifacts(ContentType contentTypes) {
            if (contentTypes.HasFlag(ContentType.Artifacts) && Info.Artifacts.Length == 0) {
                Log.LogError($"Mod advertises artifact files but none listed in metadata. {(int) contentTypes}");
            }

            var returnValue = true;

            foreach (var path in Info.Artifacts) {
                var fullPath = Path.Combine(InstallPath, "artifacts", path);

                if (File.Exists(fullPath)) {
                    _artifactFiles.Add(fullPath);
                    continue;
                }

                Log.LogError($"Couldn't find artifact: {fullPath}");
                returnValue = false;
            }

            return returnValue;
        }

        protected bool CheckResources() {
            var contentTypes = (ContentType) Info.ContentTypes;
            return CheckArtifacts(contentTypes);
        }

        protected virtual void LoadArtifacts() {
            var artifactPath = Path.Combine(InstallPath, "artifacts/");

            foreach (var fullPath in _artifactFiles) {
                var relativePath = fullPath.Substring(artifactPath.Length);
                Artifacts[relativePath] = fullPath;
            }
        }

        protected void LoadResources() {
            IsValid = CheckResources();

            if (IsValid) {
                LoadArtifacts();
            }
        }

    }

}