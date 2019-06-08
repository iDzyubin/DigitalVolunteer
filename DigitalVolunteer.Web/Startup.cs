using DigitalVolunteer.Core.Contexts;
using DigitalVolunteer.Core.DomainModels;
using DigitalVolunteer.Core.Repositories;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Core.Validators;
using DigitalVolunteer.Web.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.Configure<CookiePolicyOptions>( options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded    = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );

            services.AddTransient<IAccountRepository, MoqAccountRepository>();
            services.AddTransient<IExecutorRepository, MoqExecutorRepository>();
            services.AddTransient<IRepository<Category>, CategoryRepository>();

            services.AddTransient<GreetingService>();
            services.AddTransient<ExecutorService>();

            var connectionString = Configuration.GetConnectionString( "DefaultConnection" );
            services.AddDbContext<ApplicationDbContext>( builder => builder.UseNpgsql( connectionString ) );

            services.AddMvc()
                    .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>() )
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
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
                app.UseExceptionHandler( "/Home/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}