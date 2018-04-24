using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Murtain.OAuth2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Data
{
    public class ApplicationDbContextSeed
    {

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole<string>> _roleManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {

            if (!context.UserRoles.Any())
            {
                _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();

                var role = new IdentityRole<string>()
                {
                    Name = "Administrators",
                    NormalizedName = "Administrators",
                };

                await _roleManager.CreateAsync(role);
            }

            if (!context.Users.Any())
            {
                _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var defaultUser = new ApplicationUser()
                {
                    UserName = "Administrator",
                    Email = "392327013@qq.com",
                    NormalizedUserName = "admin",
                    SecurityStamp = "admin",
                    Avatar = "http://video.jessetalk.cn/files/user/2018/04-24/205959f1439c148108.jpg"
                };

                await _userManager.AddToRoleAsync(defaultUser, "Administrators");
                await _userManager.CreateAsync(defaultUser, "123456");
            }
        }
    }
}
