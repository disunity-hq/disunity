using System;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class AutoMapperBinderExt {

        public static void ConfigureAutoMapper(this IServiceCollection services) {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

    }

}