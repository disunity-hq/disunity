namespace Disunity.Store.Errors {

    public class ArchiveLoadError : ApiError {

        public ArchiveLoadError(string message, string name = null, string context = null) : base(
            message, name, context) { }

    }

}