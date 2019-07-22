using System;


namespace Disunity.Editor {

    public class ExportValidationError : Exception {

        public ExportValidationError(string message) : base(message) {
        }

    }

}