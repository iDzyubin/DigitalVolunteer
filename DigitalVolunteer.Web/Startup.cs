using System;
using DigitalVolunteer.Core.DataAccess;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Repositories;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Core.Validators;
using DigitalVolunteer.DBUpdate;
using DigitalVolunteer.Web.Filters;
using FluentValidation.AspNetCore;
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
                options.CheckConsentNeeded    = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );

            var expireTimeSpan = TimeSpan.FromDays( 30 );
            services.AddAuthentication( CookieAuthenticationDefaults.AuthenticationScheme )
                    .AddCookie( options =>
                     {
                         options.LoginPath  = "/Account/Login";
                         options.LogoutPath = "/Account/Logout";
                     } );

            var dbConnStr = Configuration.GetConnectionString( "DefaultConnection" );
            InitDB( dbConnStr );
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings( dbConnStr );
            services.AddSingleton<MainDb>();


            var smtpSettings = Configuration.GetSection( "SmtpClientSetting" ).Get<SmtpSettings>();
            services.AddSingleton( smtpSettings );
            services.AddScoped<NotificationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddScoped<CategoryService>();
            services.AddScoped<UserService>();
            services.AddScoped<GreetingService>();
            services.AddSingleton<PasswordHashService>();
            services.AddScoped<TaskService>();

            services
               .AddMvc( o => o.Filters.Add<UserClaimsFilter>() )
               .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>() )
               .SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
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

        private void InitDB( string connectionString )
        {
            var migrator = new MigratorRunner( connectionString );
            migrator.Run();
        }
    }
}