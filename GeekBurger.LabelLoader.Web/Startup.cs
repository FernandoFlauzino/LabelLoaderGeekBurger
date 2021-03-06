﻿using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Application.Interface.Api;
using GeekBurger.LabelLoader.Web.Application.Service;
using GeekBurger.LabelLoader.Web.Infra.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using AutoMapper;
using System;

namespace GeekBurger.LabelLoader.Web
{
    public class Startup
    {



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "GeekBurger LabelLoader",
                    Version = "v1"
                });
            });

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IIngredientsRepository, IngredientsRepository>();
            services.AddScoped<ILabelLoaderService, LabelLoaderService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddScoped<ILababelLoaderChangedService, LabelLoaderChangedService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabelLoader");
            });

        }
    }
}
