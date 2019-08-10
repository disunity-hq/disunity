using Disunity.Store.Entities;


namespace Disunity.Store.Data {

    public interface IVersionModel {

        int VersionNumberId { get; set; }

        VersionNumber VersionNumber { get; set; }

    }

}