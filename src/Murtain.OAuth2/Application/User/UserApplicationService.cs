using Murtain.Caching;
using Murtain.Domain.UnitOfWork.Interception;
using Murtain.Exceptions;
using Murtain.Interception;
using Murtain.OAuth2.Core.Captcha;
using Murtain.OAuth2.SDK.Enum;
using Murtain.Runtime.GraphicGen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Murtain.OAuth2.Domain.Entities;
using Murtain.OAuth2.Core.User;
using Murtain.OAuth2.SDK.User;
using Murtain.Extensions.AutoMapper;
using IdentityServer4.Models;
using System.Linq;
using System.Security.Claims;
using IdentityModel;

namespace Murtain.OAuth2.ApplicationService.User
{

    [UnitOfWorkInceptor]
    public class UserApplicationService : IUserApplicationService
    {
        private readonly ICacheManager cacheManager;
        private readonly ICaptchaManager captchaManager;
        private readonly IUserManager userManager;

        public UserApplicationService(ICaptchaManager captchaManager, IUserManager userManager, ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
            this.captchaManager = captchaManager;
            this.userManager = userManager;
        }


        public async Task<CaptchaMessageCacheData> GetCaptchaMessageCacheDataAsync(GetCaptchaMessageCacheDataAsyncRequest input)
        {
            return await captchaManager.GetCaptchaMessageCacheDataAsync(input.CaptchaType, input.Mobile, input.Captcha);
        }

        public async Task<Domain.Entities.User> AuthenticateLocalAsync(SetPasswordAsyncRequest input)
        {
            return await userManager.AuthenticateLocalAsync(input.Mobile, input.Password);
        }


        public async Task<bool> CheckUserExsistAsync(string mobile)
        {
            var user = await userManager.FindAsync(mobile);
            return user != null;
        }


        public async Task<bool> SendMessageCaptchaAsync(SendMessageCaptchaAsyncRequest input)
        {
            await captchaManager.MessageCaptchaSendAsync(input.CaptchaType, input.Mobile, 10);
            return await Task.FromResult(true);
        }

        public async Task<byte[]> GetGraphicCaptchaAsync()
        {
            var strs = GraphicCaptchaManager.GetRandomCaptchString(6);
            cacheManager.Set($"Captcha:Graphic", string.Join("", strs));
            return await Task.FromResult(GraphicCaptchaManager.GetBytes(strs));
        }

        public Task<bool> CheckGraphicCaptchaAsync(string graphic_captcha)
        {
            return Task.FromResult<bool>(cacheManager.Get<string>($"Captcha:Graphic") == graphic_captcha);
        }

        public async Task SetPasswordAsync(SetPasswordAsyncRequest input)
        {
            await userManager.SetPasswordAsync(input.Mobile, input.Password);
        }

        public async Task<SDK.User.User> AuthenticateLocalAsync(string username, string password)
        {
            var user = await userManager.AuthenticateLocalAsync(username, password);
            return user?.MapTo<SDK.User.User>();
        }
        public async Task<SDK.User.User> GetUserProfileDataAsync(string subId)
        {
            var user = await userManager.GetProfileDataAsync(subId);
            return user?.MapTo<SDK.User.User>();
        }

        public async Task UpdatePropertyAsync(string subId, string property, string value)
        {
            var user = await userManager.GetProfileDataAsync(subId);

            if (!string.IsNullOrEmpty(value) && user.GetType().GetProperties().Any(x => x.Name == property))
            {
                user.GetType().GetProperty(property).SetValue(user, value);
                await userManager.UpdatePropertyAsync(user);
            }
        }
    }
}
