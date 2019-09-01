namespace Disunity.Management.Models {

    /// <summary>
    /// Represents a unique identifier for generic packages
    /// </summary>
    public class PackageIdentifier {
        
        /// <summary>
        /// Id string for this package. Should be the minimal string necessary to uniquely identify this package
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Check whether the stored <see cref="Id"/> is validate. Default implementation always returns true
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate() => true;

    }

}