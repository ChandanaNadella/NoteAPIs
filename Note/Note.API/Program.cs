using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Note.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //.NET CORE 2.1 way
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseIISIntegration()
                .UseStartup<Startup>()
                 .UseNLog();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                 .Build();
    }

}


