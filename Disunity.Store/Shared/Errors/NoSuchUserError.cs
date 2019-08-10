using System.Net;


namespace Disunity.Store.Errors {

    public class NoSuchUserError : ApiError {

        public NoSuchUserError(string message, string name = null, string context = null) :
            base(message, name, context) {
            StatusCode = HttpStatusCode.NotFound;
        }

    }

}