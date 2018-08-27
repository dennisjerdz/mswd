using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MSWD.Startup))]
namespace MSWD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
