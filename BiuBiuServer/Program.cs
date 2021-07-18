using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiuBiuShare.Tool;

namespace BiuBiuServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ulong id1=IdManagement.GenId(1);
            ulong id2=IdManagement.GenId(2);
            ulong id3=IdManagement.GenId(3);
            ulong id4=IdManagement.GenId(4);
            Console.WriteLine(Convert.ToString((long)id1,2));
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
