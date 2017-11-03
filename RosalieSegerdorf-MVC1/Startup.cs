using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RosalieSegerdorf_MVC1.Startup))]
namespace RosalieSegerdorf_MVC1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
