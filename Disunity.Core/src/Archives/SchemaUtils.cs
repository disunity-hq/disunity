using System.Dynamic;


namespace Disunity.Core.Archives {

    public static partial class Schema {

        public static ExpandoObject EO() {
            return new ExpandoObject();
        }

        public static object Integer(int? min = null, int? max = null,
                                     int? exclusiveMin = null,
                                     int? exclusiveMax = null) {
            dynamic eo = EO();
            eo.type = "integer";

            if (min != null) {
                eo.minimum = min;
            }

            if (max != null) {
                eo.maximum = max;
            }

            if (exclusiveMin != null) {
                eo.exclusiveMinimum = exclusiveMin;
            }

            if (exclusiveMax != null) {
                eo.exclusiveMax = exclusiveMax;
            }

            return eo;
        }

        public static object String(string pattern = null, string format = null, int maxLength = 128) {
            dynamic eo = EO();
            eo.type = "string";
            eo.maxLength = maxLength;

            if (pattern != null) {
                eo.pattern = pattern;
            }

            if (format != null) {
                eo.format = format;
            }

            return eo;
        }

        public static object Object(object properties = null,
                                    object propertyNames = null,
                                    object additionalProps = null,
                                    object dependencies = null,
                                    string[] required = null) {
            dynamic eo = EO();
            eo.type = "object";

            if (properties != null) {
                eo.properties = properties;
            }

            if (propertyNames != null) {
                eo.propertyNames = propertyNames;
            }

            if (additionalProps != null) {
                eo.additionalProperties = additionalProps;
            }

            if (dependencies != null) {
                eo.dependencies = dependencies;
            }

            if (required != null) {
                eo.required = required;
            }

            return eo;
        }

        public static object Array(object items) {
            dynamic eo = EO();
            eo.type = "array";
            eo.items = items;
            return eo;
        }

    }

}