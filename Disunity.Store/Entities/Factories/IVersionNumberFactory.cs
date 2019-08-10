using System.Threading.Tasks;


namespace Disunity.Store.Entities.Factories {

    public interface IVersionNumberFactory {

        Task<VersionNumber> FindOrCreateVersionNumber(string versionString);
        Task<VersionNumber> FindOrCreateVersionNumber(VersionNumber versionNumber);

    }

}