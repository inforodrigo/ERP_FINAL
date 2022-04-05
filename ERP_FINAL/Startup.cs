using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERP_FINAL.Startup))]
namespace ERP_FINAL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
