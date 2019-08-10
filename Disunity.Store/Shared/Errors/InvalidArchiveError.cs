namespace Disunity.Store.Errors {

    public class InvalidArchiveError : ApiError {

        public InvalidArchiveError(string message, string name = null, string context = null)
            : base(message, name, context) { }

    }

}