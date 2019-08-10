using System;

using Disunity.Store.Errors;


namespace Disunity.Store.Exceptions {

    public class ApiException : Exception {

        public ApiError Error { get; }

        public ApiException(ApiError error) {
            Error = error;
        }

    }

}