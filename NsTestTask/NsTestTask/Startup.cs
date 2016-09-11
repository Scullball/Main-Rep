using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NsTestTask.Startup))]
namespace NsTestTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
