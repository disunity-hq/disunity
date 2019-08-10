using Disunity.Store.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class DbContextBinderExt {

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration config) {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

        }

    }

}