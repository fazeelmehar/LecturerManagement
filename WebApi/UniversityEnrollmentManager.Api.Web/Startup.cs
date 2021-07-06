using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using UniversityEnrollmentManager.Api.Web.Extensions;
using UniversityEnrollmentManager.Core.Enrollments;
using UniversityEnrollmentManager.Data.Context;
using UniversityEnrollmentManager.Data.Settings;
using UniversityEnrollmentManager.Infrastructure.UnitOfWork;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Api.Web
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
            //MvcOptions.EnableEndpointRouting = false;

            services.AddOptions();
            services.AddSingleton(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "University Enrollment Manager",
                        Version = "v1",
                        Description = "Lecturer Management Documentation"
                    });
                c.EnableAnnotations();
            });
            services.RegisterHandlers();

            AppStartup.RegisterEF(services, Configuration);
            services.RegisterApplication();
            services.AddEntityServices();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
                   .AllowCredentials()
                   .SetPreflightMaxAge(TimeSpan.FromMinutes(5))
               );
            });

            services.AddMediatR();
            services.AddAutoMapper(typeof(Index));

            services.Configure<ApiBehaviorOptions>(o =>
              o.InvalidModelStateResponseFactory = a => new UnprocessableEntityObjectResult(new Utils.ErrorModel(a.ModelState))
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // We wouldn't do this normally.
            app.UseCors("AllowAll");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerDocs();
            app.UseMvc();

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
        }
    }
}
