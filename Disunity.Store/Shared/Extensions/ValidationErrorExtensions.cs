using System.Collections.Generic;
using System.Linq;

using Disunity.Store.Errors;

using Newtonsoft.Json.Schema;

namespace Disunity.Store.Extensions {

    public static class ValidationErrorExtensions {

        public static SchemaError ToSchemaException(this ValidationError error) {
            return SchemaError.FromValidationError(error);
        }

        public static IEnumerable<SchemaError> ToSchemaExceptions(this IList<ValidationError> errors) {
            return errors.Select(e => e.ToSchemaException());
        }

        public static AggregateError AsAggregate(this IList<ValidationError> errors) {
            return new AggregateError(errors.ToSchemaExceptions());
        }

    }

}