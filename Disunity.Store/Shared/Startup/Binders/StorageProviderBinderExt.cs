using System;

using BindingAttributes;

using Disunity.Store.Storage;
using Disunity.Store.Storage.Backblaze;
using Disunity.Store.Storage.Database;
using Disunity.Store.Storage.InMemory;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class StorageProviderBinderExt {

        public static void ConfigureStorageProvider(this IServiceCollection services, IConfiguration config) {
            Type implementationType;
            BindType bindingType = BindType.Singleton;

            switch (config["Storage:Provider"]) {
                case "B2":
                    implementationType = typeof(B2StorageProvider);
                    break;

                case "DB":
                    implementationType = typeof(DbStorageProvider);
                    bindingType = BindType.Scoped;
                    break;

                case "Memory":
                default:
                    implementationType = typeof(InMemoryStorageProvider);
                    break;
            }


            switch (bindingType) {
                case BindType.Singleton:
                    services.AddSingleton(typeof(IStorageProvider), implementationType);
                    break;

                case BindType.Transient:
                    services.AddTransient(typeof(IStorageProvider), implementationType);
                    break;

                case BindType.Scoped:
                    services.AddScoped(typeof(IStorageProvider), implementationType);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }

}