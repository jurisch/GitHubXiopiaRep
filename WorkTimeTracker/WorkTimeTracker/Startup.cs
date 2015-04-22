using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkTimeTracker.Startup))]
namespace WorkTimeTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
