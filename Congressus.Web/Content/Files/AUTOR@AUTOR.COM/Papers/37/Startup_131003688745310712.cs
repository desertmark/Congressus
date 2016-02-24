using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Congressus.Web.Startup))]
namespace Congressus.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
