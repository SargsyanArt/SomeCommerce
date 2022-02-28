using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SomeCommerce.DAL;

namespace SomeCommerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            Seed.EnsureSeedData(host.Services);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
