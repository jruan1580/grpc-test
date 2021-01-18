using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineShop.Api.Services;

namespace OnlineShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton(CreateUserGrpcClient());
            services.AddSingleton(CreateCartGrpcClient());
            services.AddSingleton(CreateCatalogGrpcClient());

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<ICartService, CartService>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public User.Grpc.User.UserClient CreateUserGrpcClient()
        {
            var url = Configuration.GetSection("GrpcServices:User:URL").Value;

            var channel = GrpcChannel.ForAddress(url);

            return new User.Grpc.User.UserClient(channel);
        }

        public Catalog.Grpc.Catalog.CatalogClient CreateCatalogGrpcClient()
        {
            var url = Configuration.GetSection("GrpcServices:Catalog:URL").Value;

            var channel = GrpcChannel.ForAddress(url);

            return new Catalog.Grpc.Catalog.CatalogClient(channel);
        }

        public Cart.Grpc.Cart.CartClient CreateCartGrpcClient()
        {
            var url = Configuration.GetSection("GrpcServices:Cart:URL").Value;

            var channel = GrpcChannel.ForAddress(url);

            return new Cart.Grpc.Cart.CartClient(channel);
        }
    }
}
