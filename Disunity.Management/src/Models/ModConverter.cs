using System;
using System.ComponentModel;
using System.Globalization;


namespace Disunity.Management.Models {
    
    public class ModConverter : TypeConverter {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return typeof(string).IsAssignableFrom(sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType.IsAssignableFrom(typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            var stringValue = value as string;
            var chunks = stringValue.Split('/');

            if (chunks.Length != 2) {
                throw GetConvertFromException(value);
            }

            var mod = new Mod {
                Owner = chunks[0],
                Name = chunks[1]
            };

            return mod;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (!(value is Mod mod)) {
                throw GetConvertToException(value, destinationType);
            }
            return $"{mod.Owner}/{mod.Name}";
        }

    }

}