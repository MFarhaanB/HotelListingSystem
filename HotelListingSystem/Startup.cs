using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelListingSystem.Startup))]
namespace HotelListingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the dependencies
            services.AddScoped<SystemUserManager>();
            services.AddScoped<ApplicationSignInManager>();

            // ...
        }

    }
}
