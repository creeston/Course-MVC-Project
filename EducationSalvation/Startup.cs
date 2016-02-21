using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EducationSalvation.Startup))]
namespace EducationSalvation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}