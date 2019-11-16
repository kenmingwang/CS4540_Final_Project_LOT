/* 
 * Name:Ken Wang
 * uID: u1193853
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CS4540_A2.Areas.Identity.Data;
using CS4540_A2.Data;
using CS4540_A2.Models;
using CS4540A2.Migrations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS4540_A2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            // Initialize the Roles when the app runs
            // Refering to this blog : https://gooroo.io/GoorooTHINK/Article/17333/Custom-user-roles-and-rolebased-authorization-in-ASPNET-core/28352#.XYnJbyhKiUk
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Migrate and seed Courses and LOS
                    var LOSContext = services.GetRequiredService<LOSContext>();
                    LOSContext.Database.Migrate();

                    LOSDbInitializer.Initialize(LOSContext);

                    // Migrate and seed User and Roles
                    var UserContext = services.GetRequiredService<UserRoleDBContext>();
                    UserContext.Database.Migrate();

                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    Seed.CreateUserRoles(serviceProvider).Wait();     
                    

                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while creating roles");
                }
            }

            host.Run();
                
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
