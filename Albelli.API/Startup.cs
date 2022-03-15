using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Albelli.Abstraction.Service;
using Albelli.Domain.Entity;
using Albelli.Infrastructure.MongoDB;
using Albelli.Service.Services;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Albelli.API
{
    public class Startup
    {
        //private ServiceSettings serviceSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo().AddMongoRepository<Order>("database");
            services.AddConsul();
            services.AddTransient<IOrderService, OrderService>();

            services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Albelli.API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Albelli.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            SeedConsulData(app);
        }

        private void SeedConsulData(IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var data = new Dictionary<string, double>()
                {
                    {"PhotoBook", 19},
                    {"Calendar", 10},
                    {"Canvas", 16},
                    {"Cards", 4.7},
                    {"Mug", 94}
                }
                ;
            var config = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(config);
            consulClient.KV.Put(new KVPair("ProductWidth") {Value = bytes}).Wait();
        }
    }
}