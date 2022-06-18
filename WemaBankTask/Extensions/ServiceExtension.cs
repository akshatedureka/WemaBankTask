using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using WemaBankTask.Entities.DataContext;

namespace WemaBankTask.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
           services.AddCors(opt =>
           {
               opt.AddPolicy("CorsPolicy", builder =>

                   builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());

           });
        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration) =>
           services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
               b => b.MigrationsAssembly("WemaBankTask")));
        public static void ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Solar_Shops",
                    Description = "<h2>Description</h2><p>This is a test documentation.</p><br />" +
                    "><ul><li><strong>Authorization (header)</strong><ul><li>" +
                    "<p>The API uses no authorization presently.</p>" +
                    "</li></ul></li></ul>",                   
                    Contact = new OpenApiContact
                    {
                        Name = "Segun Olofinsawe",
                        Email = "sawesis1234@gmail.com",
                        Url = new Uri("https://linkedin.com/in/segunolofinsawe")

                    },
                   
                });
               
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "WemaBankTask.xml");
                //c.IncludeXmlComments(xmlPath);

            });

    }
}
