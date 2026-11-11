using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IOT_HJPlat.Startup))]
namespace IOT_HJPlat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
