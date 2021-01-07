using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Accessors;
using Cart.Domain.Services;
using Cart.Grpc.Services;
using Cart.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cart.Grpc
{
    public class Startup
    {
        public IGrpcServiceAccessor GrpcServiceAccessor { get; set; }

        public Startup(IConfiguration configuration)
        {
            GrpcServiceAccessor = new GrpcServiceAccessor(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddSingleton<ICartsRepository, CartsRepository>();
            services.AddSingleton(GrpcServiceAccessor.GetCatalogGrpcClient());
            services.AddSingleton(GrpcServiceAccessor.GetUserGrpcClient());
            services.AddSingleton<ICartService, Domain.Services.CartService>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.CartService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }             
    }
}
