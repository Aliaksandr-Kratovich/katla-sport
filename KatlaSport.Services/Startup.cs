using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KatlaSport.Services.StartupOwin))]

namespace KatlaSport.Services
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
