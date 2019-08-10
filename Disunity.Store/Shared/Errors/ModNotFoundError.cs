namespace Disunity.Store.Errors {

    public class ModNotFoundError : ApiError {

        public ModNotFoundError(string message, string name = null, string context = null)
            : base(message, name, context) { }

    }

}