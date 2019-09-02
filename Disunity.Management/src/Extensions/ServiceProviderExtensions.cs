using System;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Management.Extensions {

    public static class ServiceProviderExtensions {

        
        public static T Instantiate<T>(this IServiceProvider sp) {
            return Instantiate<T>(sp, typeof(T));
        }
        
        public static T Instantiate<T>(this IServiceProvider sp, Type type) {
            return (T) Instantiate(sp, type);
        }
        
        public static object Instantiate(this IServiceProvider sp, Type type) {
            if (!type.IsClass || type.IsAbstract) {
                throw new ArgumentException($"{type.Name} must be a non-abstract class", nameof(type));
            }

            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (ctors.Length == 0) {
                throw new ArgumentException($"{type.Name} must have a valid public constructor");
            }

            foreach (var ctor in ctors) {
                var constructed = TryInstantiateObject(sp, ctor);

                if (constructed != null) {
                    return constructed;
                }
            }
            
            throw new ArgumentException($"No suitable constructor found for {type.Name}", nameof(type));
        }

        private static object TryInstantiateObject(IServiceProvider serviceProvider, ConstructorInfo ctor) {
            var argInfo = ctor.GetParameters();
            var args = new object[argInfo.Length];

            foreach (var argType in argInfo) {
                var arg = serviceProvider.GetService(argType.ParameterType);
                if (arg == null) return null;
                args[argType.Position] = arg;
            }
            
            return ctor.Invoke(args);
        }
        

    }

}