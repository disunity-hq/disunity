using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Errors {

    public class AggregateError : ApiError, IEnumerable<ApiError> {

        private readonly IEnumerable<ApiError> _innerErrors;

        public AggregateError(IEnumerable<ApiError> innerErrors) : base(null) {
            _innerErrors = innerErrors;
        }

        public IEnumerator<ApiError> GetEnumerator() {
            foreach (var error in _innerErrors) {
                if (error is AggregateError innerAggregate) {
                    foreach (var aggregateError in innerAggregate) {
                        yield return aggregateError;
                    }
                } else {
                    yield return error;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override ObjectResult ToObjectResult() {
            var groups = new Dictionary<string, object>();

            foreach (var error in this) {
                dynamic group;

                if (groups.ContainsKey(error.Name)) {
                    group = groups[error.Name];
                    group.Items.Add(error);
                } else {
                    group = new {Info = error.Info, Items = new List<ApiError>() {error}};
                }

                groups[error.Name] = group;
            }

            return new ObjectResult(new { errors = groups }) {
                StatusCode = (int) StatusCode
            };
        }

    }

}