using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MusalaGateways.Api.Configurations;
using MusalaGateways.BusinessLogic.Configurations.Mapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.BusinessLogic.Services;
using MusalaGateways.DataLayer.Context;
using MusalaGateways.DataLayer.Repository;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork;
using MusalaGateways.DataLayer.UnitOfWork.Interface;

namespace MusalaGateways.Api
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
            //Swagger
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc(SwaggerConfiguration.DocNameV1,
                    new OpenApiInfo
                    {
                        Title = SwaggerConfiguration.DocInfoTitle,
                        Version = "v1",
                        Description = SwaggerConfiguration.DocInfoDescription,
                        Contact = new OpenApiContact
                        {
                            Name = SwaggerConfiguration.ContactName,
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

            //Mapper
            services.AddAutoMapper(typeof(MusalaMapperProfile));

            //Context
            string connectionString = Configuration.GetConnectionString("MusalaConnectionString");
            bool inMemoryDb = Configuration.GetValue<bool>("InMemoryDB");

            if(inMemoryDb)
            {
                services.AddDbContext<MusalaContext>(options =>
                options.UseInMemoryDatabase("musala"));
                services.AddScoped<MusalaContext>();
            }
            else
            {
                services.AddDbContext<MusalaContext>(options =>
                options.UseSqlServer(connectionString));
            }

            //Unit of work
            services.AddScoped<IUnitOfWork, EntityFrameworkUnitOfWork<MusalaContext>>();

            //Repositories
            services.AddScoped<IRepository, ContextRepository<MusalaContext>>();

            //Services
            services.AddScoped<IGatewayService, GatewayService>();
            services.AddScoped<IDeviceService, DeviceService>();


            services.AddControllers();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
