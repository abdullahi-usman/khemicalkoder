using System.Security.Claims;
using System.Threading.Tasks;
using KhemicalKoder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace KhemicalKoder.Areas.Identity
{
    public class AdditionalUserClaimsFactory : UserClaimsPrincipalFactory<KhemicalKoderUser>
    {
        public AdditionalUserClaimsFactory(UserManager<KhemicalKoderUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(KhemicalKoderUser user)
        {
            var claimPrincipal = await base.CreateAsync(user);
            var identityClaim = (ClaimsIdentity) claimPrincipal.Identity;

            if (user.IsAdmin)
                identityClaim.AddClaim(new Claim("IsAdmin", "IsAdmin"));

            return claimPrincipal;
        }
    }
}