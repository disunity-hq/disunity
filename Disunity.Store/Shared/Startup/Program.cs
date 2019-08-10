using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Startup
{

    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseWebRoot("../Frontend/dist")
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
                              config.AddEnvironmentVariables();
                          })
                          .ConfigureLogging(f => f.AddConsole().AddDebug())
                          .UseStartup<Startup>();
        }

    }

}