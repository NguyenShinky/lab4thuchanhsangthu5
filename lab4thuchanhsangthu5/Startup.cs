using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab4thuchanhsangthu5.Startup))]
namespace lab4thuchanhsangthu5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
