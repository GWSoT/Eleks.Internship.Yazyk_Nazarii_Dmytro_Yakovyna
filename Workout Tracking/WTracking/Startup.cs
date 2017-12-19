using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WTracking.Data;
using WTracking.Models;
using WTracking.Services;
using Google.Apis.Fitness.v1;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using WTracking;
using WTracking.Hubs;

namespace WTracking
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })

            .AddGoogle(googleOption =>
            {
                googleOption.ClientId = Configuration["Authentication:Google:client_id"];
                googleOption.ClientSecret = Configuration["Authentication:Google:client_secret"];
            })

            .AddGoogle("GoogleFit", "Google Fit", o =>
            {
                o.ClientId = Configuration["Authentication:GoogleFit:client_id"];
                o.ClientSecret = Configuration["Authentication:GoogleFit:client_secret"];
                o.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/fetch-google");
                o.SaveTokens = true;
                o.Scope.Add(FitnessService.Scope.FitnessLocationRead);
                o.Scope.Add(FitnessService.Scope.FitnessActivityRead);
            })
             
            .AddFacebook(o =>
            {
                o.AppId = Configuration["Authentication:Facebook:AppId"];
                o.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IApiFetcher, ApiFetcher>();
            services.AddSignalR();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSignalR(config =>
            {
                config.MapHub<Chat>("chat");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "profile",
                    template: "{uniqueId?}",
                    defaults: new { controller = "Profile", action = "Index" });
            });
        }
    }
}
