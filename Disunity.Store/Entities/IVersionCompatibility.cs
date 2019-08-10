using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities {

    public interface IVersionCompatibility<TVersion, TCompatible> {

        [Required] int VersionId { get; set; }
        TVersion Version { get; set; }

        int? MinCompatibleVersionId { get; set; }
        TCompatible MinCompatibleVersion { get; set; }

        int? MaxCompatibleVersionId { get; set; }
        TCompatible MaxCompatibleVersion { get; set; }

    }

}