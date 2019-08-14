using System.IO.Abstractions;

using Xunit;


namespace Disunity.Tests.Management {

    public static class Util {

        public static void AssertFileExists(IFileSystem fileSystem, string path) {
            Assert.True(fileSystem.File.Exists(path), $"Expected file {path} to exist");
        }
        
        public static void AssertFileExists(IFileSystem fileSystem, string path, string message) {
            Assert.True(fileSystem.File.Exists(path), message);
        }
        
        public static void AssertFileNotExists(IFileSystem fileSystem, string path) {
            Assert.False(fileSystem.File.Exists(path), $"Expected file {path} to exist");
        }
        
        public static void AssertFileNotExists(IFileSystem fileSystem, string path, string message) {
            Assert.False(fileSystem.File.Exists(path), message);
        }

    }

}