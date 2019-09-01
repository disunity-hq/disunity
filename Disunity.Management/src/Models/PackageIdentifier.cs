namespace Disunity.Management.Models {

    public class PackageIdentifier {
        
        public string Id { get; set; }

        public virtual bool Validate() => true;

    }

}