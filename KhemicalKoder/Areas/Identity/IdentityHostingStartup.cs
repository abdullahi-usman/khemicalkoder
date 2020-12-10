using KhemicalKoder.Areas.Identity;
using KhemicalKoder.Data;
using KhemicalKoder.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace KhemicalKoder.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<IUserClaimsPrincipalFactory<KhemicalKoderUser>, AdditionalUserClaimsFactory>();

                services.AddAuthorization(options =>
                    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("IsAdmin")));
            });
        }
    }
}