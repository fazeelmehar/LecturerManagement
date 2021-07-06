using MediatR;
using Microsoft.AspNetCore.Builder;
using Intellix.Infrastructure.Services;
using LecturerManagement.Core.Lecturer;
using LecturerManagement.Domain.Mapping;
using LecturerManagement.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LecturerManagement.Api.Web
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
            services.AddOptions();
            services.AddSingleton(Configuration);
            services.AddSwaggerDocs();
            services.RegisterHandlers();
            Bootstrapper.RegisterEF(services, Configuration);
            services.RegisterApplication();
            services.AddEntityServices();
            services.CorsConfiguration();
            services.AddMediatR();
            services.AddDomainAutoMapper();
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<ApiBehaviorOptions>(o =>
              o.InvalidModelStateResponseFactory = a => new UnprocessableEntityObjectResult(new ErrorModel(a.ModelState))
          );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Terminal")),
            //    RequestPath = new PathString("/Terminal")
            //});
            app.UseSwagger();
            app.RegisterApplications();

            app.UseResponseCompression();
            app.UseMvc();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
        }
    }
}
