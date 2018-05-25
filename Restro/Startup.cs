using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Restro.Startup))]
namespace Restro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
