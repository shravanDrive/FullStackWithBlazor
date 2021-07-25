using Common.Repositories.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.PropertyLookup.Models;
using Services.PropertyLookup.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.PropertyLookup.Services;
using Services.PropertyLookup.Controllers;
using Services.Common.ExceptionFilter;
using Connectors.RedisCache;

namespace Services.PropertyLookup
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
            services.AddCustomMvc(this.Configuration)
                .AddCustomRepository(this.Configuration)
                .AddCustomIntegrations(this.Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Property Lookup", Version = "v1" });
                //var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("./swagger/v1/swagger.json","Proprety Lookup");
            });
        }

        /// <summary>
        /// Custom Repository Implementation Added
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>

    }

    /// <summary>
    /// CustomExtensionMethods How is that this method of add custom repository deosnot get added in startup class but if you make new class then it gets added
    /// </summary>
    internal static partial class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var DBOptions = new DbContextOptionsBuilder<PropertyDBContext>().UseSqlServer(
                configuration.GetConnectionString(Constants.Database.ConnectionString),
                sqlServerOptionsAction: SqlOptions =>
                {
                    SqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }).Options;

            var propertyContext = new PropertyDBContext(DBOptions);
            services.AddScoped<IRepositoryBase<SampleTable>>(x =>
            {
                return new RepositoryBase<SampleTable>(propertyContext);
            });
            services.AddScoped<IRepositoryBase<MyModel>>(x =>
            {
                return new RepositoryBase<MyModel>(propertyContext);  
            });
            services.AddSingleton<IRepositoryBase<OpParameterModel>>(x =>
            {
                return new RepositoryBase<OpParameterModel>(propertyContext);  
            });

            return services;
        }

        /// <summary>
        /// AddCustomIntegrations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<ICacheHelper, MemoryCacheHelper>()
                .AddScoped<IPropertyLookupService, PropertyLookupService>();
            return services;
        }

        /// <summary>
        /// Adds the custom MVC configurations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddApplicationPart(typeof(PropertyLookupController).Assembly); // added for configuration with xunit
            return services;
        }
    }
}
