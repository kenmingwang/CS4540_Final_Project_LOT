using System;
using CS4540_A2.Models;
using CS4540_A2.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CS4540_A2.Areas.Identity.IdentityHostingStartup))]
namespace CS4540_A2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserRoleDBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserRoleDBContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(config => { config.SignIn.RequireConfirmedEmail = true; })
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<UserRoleDBContext>();

                 services.AddTransient<IEmailSender, EmailSender>();
            });

        }
    }
}