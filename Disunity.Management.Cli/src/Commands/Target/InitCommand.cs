using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Client.v1;
using Disunity.Client.v1.Models;
using Disunity.Core;
using Disunity.Management.Cli.Commands.Options;
using Disunity.Management.Factories;
using Disunity.Management.PackageStores;
using Disunity.Management.Util;


namespace Disunity.Management.Cli.Commands.Target {

    public class InitCommand : CommandBase<TargetInitOptions> {

        private readonly IFileSystem _fileSystem;
        private readonly ITargetClient _targetClient;
        private readonly IPackageStore _disunityStore;
        private readonly ITargetFactory _targetFactory;
        private readonly IDisunityClient _disunityClient;
        private readonly Crypto _crypto;

        public InitCommand(ILogger logger, IFileSystem fileSystem, ITargetClient targetClient, IPackageStore disunityStore, ITargetFactory targetFactory, IDisunityClient disunityClient, Crypto crypto) : base(logger) {
            _fileSystem = fileSystem;
            _targetClient = targetClient;
            _disunityStore = disunityStore;
            _targetFactory = targetFactory;
            _disunityClient = disunityClient;
            _crypto = crypto;
        }

        protected override async Task Execute(CancellationToken cancellationToken) {
            // 0. Hash target
            var targetHash = await _crypto.HashFile(Options.ExecutablePath, _fileSystem, cancellationToken);
            // 1. Get target info from store
            var targetInfo = await _targetClient.FindTargetByHashAsync(targetHash, cancellationToken);
            // 2. Calculate disunity distro version
            var disunityVersion = GetMatchingDisunityVersion(targetInfo);
            var disunityPackage = $"disunity_{disunityVersion}";
            // 3. Add target disunity version to store
            await _disunityStore.DownloadPackage(disunityPackage, cancellationToken: cancellationToken);
            // 4. Create managed target dir
            var target = await _targetFactory.CreateManagedTarget(Options.ExecutablePath, targetInfo.DisplayName, targetInfo.Slug);
            // 5. Install disunity version to default profile
            await _disunityStore.CreatePackageReference(disunityPackage, _fileSystem.Path.Combine(target.TargetMeta.ManagedPath, "profiles", "active", "distro"));
            // 6. Add and configure doorstop to target install
            throw new System.NotImplementedException();
        }

        private async Task<string> GetMatchingDisunityVersion(TargetVersionDto targetInfo) {
            var request = ResolveCompatibleVersion(targetInfo);

            if (request != null) {
                return request.ToString(3);
            }

            var disunityVersions = await _disunityClient.GetDisunityVersionsAsync();
            var latestVersion = disunityVersions.First();
            return latestVersion.VersionNumber;
        }

        private Version ResolveCompatibleVersion(TargetVersionDto targetInfo) {
            Version minTargetVersion = null;
            Version maxTargetVersion = null;
            Version request = null;
            Version.TryParse(targetInfo.MinCompatibleVersion, out minTargetVersion);
            Version.TryParse(targetInfo.MaxCompatibleVersion, out maxTargetVersion);
            Version.TryParse(Options.DistroVersion, out request);
            var comparer = Comparer<Version>.Default;
            request = comparer.Min(request, maxTargetVersion);
            request = comparer.Max(request, minTargetVersion);
            return request;
        }

    }

}