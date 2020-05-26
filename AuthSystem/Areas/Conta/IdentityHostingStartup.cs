using System;
using AuthSystem.Areas.Conta.Pages;
using AuthSystem.Areas.Identity.Data;
using AuthSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AuthSystem.Areas.Identity.IdentityHostingStartup))]
namespace AuthSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthDbContextConnection")));

                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultUI()
                    .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
                    .AddDefaultTokenProviders();

                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AddAreaPageRoute("Conta", "/Conta", "");
                    });

                services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = $"/Conta/Login";
                    options.LogoutPath = $"/Conta/Logout";
                });
            });
        }
    }
}