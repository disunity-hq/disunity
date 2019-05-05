using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Disunity.Shared;
using Mono.Cecil;


namespace Disunity.Cecil {

    /// <summary>
    ///     Utility for finding RuntimeAssemblies.
    /// </summary>
    public class AssemblyUtility {

        /// <summary>
        ///     Find dll files in a directory and its sub directories.
        /// </summary>
        /// <param name="path">The directory to search in.</param>
        /// <returns>A List of paths to found RuntimeAssemblies.</returns>
        public static List<string> GetAssemblies(string path, AssemblyFilter assemblyFilter) {
            var assemblies = new List<string>();

            GetAssemblies(assemblies, path, assemblyFilter);

            return assemblies;
        }

        public static List<string> GetAssemblyTypes(string path) {
            var asm = AssemblyDefinition.ReadAssembly(path);
            return asm.MainModule.Types.Select(o => o.FullName).ToList();
        }

        public static void GetAssemblies(List<string> assemblies, string path, AssemblyFilter assemblyFilter) {
            var assemblyFiles = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);

            foreach (var assembly in assemblyFiles) {
                AssemblyDefinition assemblyDefinition;

                try {
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(assembly);
                }
                catch {
                    LogUtility.LogDebug($"Couldn't read assembly: {assembly}");
                    continue;
                }

                var name = assemblyDefinition.Name.Name;

                assemblyDefinition.Dispose();

                if (name.StartsWith("Disunity.")) {
                    if ((assemblyFilter & AssemblyFilter.DisunityAssemblies) != 0) {
                        LogUtility.LogDebug($"Adding assembly: {name}");
                        assemblies.Add(assembly);
                    }

                    continue;
                }

                if (name.Contains("Mono.Cecil")) {
                    if ((assemblyFilter & AssemblyFilter.DisunityAssemblies) != 0) {
                        LogUtility.LogDebug($"Adding assembly: {name}");
                        assemblies.Add(assembly);
                    }

                    continue;
                }

                //if(CodeSettings.apiAssemblies.Contains(name))
                //{
                //    if((assemblyFilter & AssemblyFilter.ApiAssemblies) != 0)
                //    {
                //        LogUtility.LogDebug($"Adding assembly: {name}");
                //        assemblies.Add(assembly);
                //    }

                //    continue;
                //}

                if ((assemblyFilter & AssemblyFilter.ModAssemblies) != 0) {
                    LogUtility.LogDebug($"Adding assembly: {name}");
                    assemblies.Add(assembly);
                }
            }
        }

    }

}