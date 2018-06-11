using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Murtain.OAuth2.Configuration;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4;
using Murtain.OAuth2.Models.Passport;
using Murtain.OAuth2.Attributes;

namespace Murtain.OAuth2.Controllers
{
    [SecurityHeaders]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
    public class PassportController : Controller
    {

        //private readonly IUserApplicationService userApplicationService;

        public PassportController()
        {
            //this.userApplicationService = userApplicationService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var vm = await BuiderProfileViewModelAsync();
            return View(vm);
        }
        public IActionResult Security()
        {
            return View();
        }
        public IActionResult Support()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Property(PorfileInput input)
        {
            if (!ModelState.IsValid)
            {
                var vm = await BuiderProfileViewModelAsync();
                return View(vm);
            }
            //await userApplicationService.UpdatePropertyAsync(input.SubjectId, input.Property, input.Value);
            return RedirectToAction("Profile", "Passport");
        }

        private async Task<ProfileViewModel> BuiderProfileViewModelAsync()
        {
            //var user = await userApplicationService.GetUserProfileDataAsync(User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);

            return new ProfileViewModel
            {
                //Name = user.Name,
                //NickName = user.NickName,
                //Address = user.Country + " " + user.Province + " " + user.City + " " + user.Address,
                //Email = user.Email,
                //Gender = user.Gender,
                //Birthday = user.Birthday,
                //SubjectId = user.SubjectId,
                //Mobile = user.Mobile,
                //CreateTime = user.CreateTime
            };
        }
    }
}