using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace BiuBiuServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Initialization initialization = new Initialization();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                            listenOptions.UseHttps("./BiuBiuServer.pfx",
                                "ABaABaABa");
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}