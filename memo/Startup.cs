using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(memo.Startup))]
namespace memo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
