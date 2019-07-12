using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.DataAccess;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Repositories;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Core.Validators;
using DigitalVolunteer.DBUpdate;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalVolunteer.API
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.Configure<CookiePolicyOptions>( options =>
            {
                options.CheckConsentNeeded    = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );


            services.AddAuthentication( CookieAuthenticationDefaults.AuthenticationScheme )
                    .AddCookie( options =>
                     {
                         options.LoginPath  = "/Account/Login";
                         options.LogoutPath = "/Account/Logout";
                     } );

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();


            var smtpSettings = Configuration.GetSection( "SmtpClientSetting" ).Get<SmtpSettings>();
            services.AddSingleton( smtpSettings );
            services.AddSingleton<PasswordHashService>();
            services.AddScoped<NotificationService>();
            services.AddTransient<UserService>();
            services.AddTransient<TaskService>();


            services.AddScoped<ValidateCategoryExistsAttribute>();
            services.AddScoped<ValidateUserExistsAttribute>();
            services.AddScoped<UserRegisteredValidatorAttribute>();


            services.AddMvc( options => options.Filters.Add( new ModelValidatorAttribute() ) )
                    .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>() )
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );


            var connectionString = Configuration.GetConnectionString( "DefaultConnection" );
            InitDB( connectionString );
            LinqToDB.Data.DataConnection.DefaultSettings = new Linq2DbSettings( connectionString );
            services.AddSingleton<MainDb>();


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles( configuration => { configuration.RootPath = "ClientApp/build"; } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{action=Index}/{id?}" );
            } );

            app.UseSpa( spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if( env.IsDevelopment() )
                {
                    spa.UseReactDevelopmentServer( npmScript: "start" );
                }
            } );
        }


        private void InitDB( string connectionString )
        {
            var migrator = new MigratorRunner( connectionString );
            migrator.Run();
        }
    }
}