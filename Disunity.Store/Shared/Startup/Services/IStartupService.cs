using Microsoft.AspNetCore.Builder;


namespace Disunity.Store.Startup.Services {

    public interface IStartupService {

        void Startup(IApplicationBuilder app);

    }

}