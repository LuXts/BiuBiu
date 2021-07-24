using BiuBiuServer.Authentication;
using LitJWT;
using LitJWT.Algorithms;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BiuBiuServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            // MagicOnion depends on ASP.NET Core gRPC service.
            services.AddMagicOnion()
                .AddJwtAuthentication<CustomJwtAuthenticationProvider>(options =>
                {
                    var preSharedKey = Convert.FromBase64String(Configuration.GetSection("BiuBiuServer:Secret").Value);
                    var algorithm = new HS512Algorithm(preSharedKey); // Use Symmetric algorithm (HMAC SHA-512)
                    options.Encoder = new JwtEncoder(algorithm);
                    options.Decoder = new JwtDecoder(new JwtAlgorithmResolver(algorithm));
                    options.Expire = TimeSpan.FromHours(2);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCertificateForwarding();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMagicOnionService();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}