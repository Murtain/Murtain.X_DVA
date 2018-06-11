using IdentityServer4.Services;
using Murtain.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;
using System.Threading.Tasks;
using System.Linq;
using Murtain.OAuth2.Core.User;
using System.Security.Claims;
using IdentityModel;

namespace Murtain.OAuth2.ApplicationService.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IUserManager userManager;
        public ProfileService(IUserManager userManager)
        {
            this.userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var user = await userManager.GetProfileDataAsync(subId);

            if (user != null)
            {
                context.IssuedClaims = await GetClaimsFromUserAsync(user);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;

            var subId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var user = await userManager.GetProfileDataAsync(subId);

            context.IsActive = user != null;
        }

        private Task<List<Claim>> GetClaimsFromUserAsync(Domain.Entities.User user)
        {
            if (user == null)
            {
                return Task.FromResult(new List<Claim>());
            }

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.SubjectId),
                new Claim(JwtClaimTypes.Name,user.Name),
                new Claim(JwtClaimTypes.NickName,user.NickName),

                new Claim("avatar",user.Avatar)
            };

            return Task.FromResult(claims);
        }
    }
}
