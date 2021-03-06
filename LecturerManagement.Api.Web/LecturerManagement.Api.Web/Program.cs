using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LecturerManagement.Api.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            try
            {
                var result = WebHost.CreateDefaultBuilder(args)
                      .UseContentRoot(Directory.GetCurrentDirectory())
                      .ConfigureAppConfiguration((builderContext, config) =>
                      {
                          IHostingEnvironment env = builderContext.HostingEnvironment;
                          config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                      })
                      .UseStartup<Startup>();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
