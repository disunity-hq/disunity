using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Schema;


namespace Disunity.Core.Archives.Exceptions {

    public class SchemaValidationException : BaseDisunityException {

        public IEnumerable<ValidationError> Errors { get; }

        public SchemaValidationException(IEnumerable<ValidationError> errors) {
            Errors = errors;

        }

    }

}