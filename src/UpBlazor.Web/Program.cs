using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UpBlazor.Web
{
    public class Program
    {
        // TODO: Refactor URLs to a constants class and remove magic strings
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}