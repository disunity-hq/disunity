using System;
using System.Linq;

using BindingAttributes;

using Disunity.Core.Archives;
using Disunity.Core.Exceptions;
using Disunity.Store.Errors;
using Disunity.Store.Exceptions;
using Disunity.Store.Extensions;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


namespace Disunity.Store.Util {

    [AsSingleton]
    public class ApiArchiveValidator {

        private readonly ILogger<ApiArchiveValidator> _logger;

        public ApiArchiveValidator(ILogger<ApiArchiveValidator> logger) {
            _logger = logger;
        }

        public ApiError FromMissingEntryException(MissingEntryException e) {
            switch (e.EntryType) {
                case EntryType.Artifact: {
                    return new MissingArtifactError(e.Path);
                }

                default: {
                    // TODO Handle other error cases
                    break;
                }
            }

            throw e;
        }

        private ApiError FromSchemaValidationException(SchemaValidationException schemaValidationException) {
            return schemaValidationException.Errors
                                            .Select(SchemaError.FromValidationError)
                                            .AsAggregate();
        }

        private ApiError FromAggregateException(AggregateException aggregateException) {
            var errors = aggregateException.InnerExceptions.Select(ConvertException);
            return new AggregateError(errors);
        }

        private ApiError FromJsonReaderException(JsonReaderException jsonReaderException) {
            return new ManifestParseError(jsonReaderException);
        }

        public ApiError ConvertException(Exception exception) {
            if (exception is MissingEntryException missingEntryException) {
                return FromMissingEntryException(missingEntryException);
            }

            if (exception is SchemaValidationException schemaValidationException) {
                return FromSchemaValidationException(schemaValidationException);
            }

            if (exception is JsonReaderException jsonReaderException) {
                return FromJsonReaderException(jsonReaderException);
            }

            if (exception is AggregateException aggregateException) {
                return FromAggregateException(aggregateException);
            }

            throw exception;
        }

        public void Validate(ZipArchive archive) {
            try {
                ArchiveValidator.Validate(archive);
            }
            catch (BaseDisunityException e) {
                throw ConvertException(e).ToExec();
            }
            catch (AggregateException e) {
                throw ConvertException(e).ToExec();
            }
        }

    }

}