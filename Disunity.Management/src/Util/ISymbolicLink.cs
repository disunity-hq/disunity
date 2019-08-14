namespace Disunity.Management.Util {

    public interface ISymbolicLink {

        void CreateDirectoryLink(string linkPath, string targetPath);

        void CreateFileLink(string linkPath, string targetPath);

        bool Exists(string path);

        string GetTarget(string path);

    }

}