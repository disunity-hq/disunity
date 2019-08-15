using System.IO.Abstractions;

using Xunit;


namespace Disunity.Tests.Management {

    public static class Util {

        public static void AssertFileExists(IFileSystem fileSystem, string path) {
            AssertFileExists(fileSystem, path, $"Expected file {path} to exist");
        }

        public static void AssertFileExists(IFileSystem fileSystem, string path, string message) {
            Assert.True(fileSystem.File.Exists(path), message);
        }

        public static void AssertFileNotExists(IFileSystem fileSystem, string path) {
            AssertFileNotExists(fileSystem, path, $"Expected file {path} to exist");
        }

        public static void AssertFileNotExists(IFileSystem fileSystem, string path, string message) {
            Assert.False(fileSystem.File.Exists(path), message);
        }

        public static void AssertDirectoryExists(IFileSystem fileSystem, string path) {
            AssertDirectoryExists(fileSystem, path, $"Expected file {path} to exist");
        }

        public static void AssertDirectoryExists(IFileSystem fileSystem, string path, string message) {
            Assert.True(fileSystem.Directory.Exists(path), message);
        }

        public static void AssertDirectoryNotExists(IFileSystem fileSystem, string path) {
            AssertDirectoryNotExists(fileSystem, path, $"Expected file {path} to exist");
        }

        public static void AssertDirectoryNotExists(IFileSystem fileSystem, string path, string message) {
            Assert.False(fileSystem.Directory.Exists(path), message);
        }

    }

}