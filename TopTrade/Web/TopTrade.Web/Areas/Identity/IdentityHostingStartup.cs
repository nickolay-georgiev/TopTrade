
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TopTrade.Web.Areas.Identity.IdentityHostingStartup))]

namespace TopTrade.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
