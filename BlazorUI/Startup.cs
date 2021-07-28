// <copyright file="Startup.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace BlazorUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Blazored.Toast;
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Blazorise.RichTextEdit;
    using BlazorUI.Data;
    using BlazorUI.Services;
    using Common.Models.InterPageData;
    using Connectors.RedisCache;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddBlazorContextMenu();
            services.AddRazorPages().AddNewtonsoftJson();
            services.AddServerSideBlazor();
            services.AddBlazorDragDrop();
            services.AddBlazoredToast();
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = false; // Setting fale here to avoid tags
            }).AddBlazoriseRichTextEdit(options =>
            {
                options.UseBubbleTheme = true;
                options.UseShowTheme = true;
                options.DynamicLoadReferences = true;
            }).AddBootstrapProviders().AddFontAwesomeIcons();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            services.AddSingleton<WeatherForecastService>();
            services.AddAuthorization();
            services.AddCustomCors();
            services.AddCustomIntegrations(this.Configuration);

            services.AddScoped<ICacheHelper, MemoryCacheHelper>();

            // services.AddSingleton<ICacheHelper, MemoryCacheHelper>(); Caching reference add arne nai ho raaha
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    /// <summary>
    /// CustomExtensionMethods
    /// </summary>
    internal static partial class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
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
            // services.AddHttpClient<>().AddHttpMessageHandler<>();
            services.AddHttpClient<IBlazorService, BlazorService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiUrl:ServiceApi"].ToString());
            });

            services.AddScoped<InterpageData>();
            return services;
        }
    }
}
