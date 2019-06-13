using System;
using DigitalVolunteer.Core.DataAccess;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Repositories;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalVolunteer.Web
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services )
        {
            services.Configure<CookiePolicyOptions>( options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );

            var expireTimeSpan = TimeSpan.FromDays( 30 );
            services.AddAuthentication( CookieAuthenticationDefaults.AuthenticationScheme )
                .AddCookie( options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                } );

            var dbConnStr = Configuration.GetConnectionString( "DefaultConnection" );
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings( dbConnStr );
            services.AddSingleton<MainDb>();

            services.AddSingleton<PasswordHashService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserService>();

            services.AddMvc( o =>
            {
                o.Filters.Add<UserClaimsFilter>();
            } ).SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}
