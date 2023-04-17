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
    }
}
