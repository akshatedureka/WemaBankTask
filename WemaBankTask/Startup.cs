using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WemaBankTask.Common.Integration;
using WemaBankTask.Common.Mappings;
using WemaBankTask.Entities.IRepository;
using WemaBankTask.Entities.Repository;
using WemaBankTask.Extensions;
using WemaBankTask.Services;
using WemaBankTask.Services.IServices;

namespace WemaBankTask
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
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IThirdPatyIntegration, ThirdPartyIntegration>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IThirdpartyService, ThirdpartyService>();
            services.AddScoped(typeof(ICustomerRepository<>), typeof(CustomerRepository<>));
            services.ConfigureCors();
            services.ConfigureSwagger();
            services.ConfigureSqlServer(Configuration);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WemaBankTask"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
