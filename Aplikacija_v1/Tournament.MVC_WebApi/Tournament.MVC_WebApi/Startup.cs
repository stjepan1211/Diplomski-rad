using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tournament.MVC_WebApi.Startup))]
namespace Tournament.MVC_WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
