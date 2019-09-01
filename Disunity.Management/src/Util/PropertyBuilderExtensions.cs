using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Newtonsoft.Json;


namespace Disunity.Management.Util {

    public static class PropertyBuilderExtensions {

        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> builder) {
            return builder.HasConversion(
                d => JsonConvert.SerializeObject(d, Formatting.None),
                s => JsonConvert.DeserializeObject<T>(s));
        }

    }

}