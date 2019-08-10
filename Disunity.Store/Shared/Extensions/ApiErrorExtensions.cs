using System.Collections;
using System.Collections.Generic;

using Disunity.Store.Errors;


namespace Disunity.Store.Extensions {

    public static class ApiErrorExtensions {

        public static AggregateError AsAggregate(this IEnumerable<ApiError> errors) {
            return new AggregateError(errors);
        }

    }

}