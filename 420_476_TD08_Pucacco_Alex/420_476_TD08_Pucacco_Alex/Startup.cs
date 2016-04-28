using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_420_476_TD08_Pucacco_Alex.Startup))]
namespace _420_476_TD08_Pucacco_Alex
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
