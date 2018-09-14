using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fri_pm_music_store.Startup))]
namespace fri_pm_music_store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
