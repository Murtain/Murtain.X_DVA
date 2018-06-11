using IdentityServer4.Services;
using Murtain.Domain;
using Murtain.Domain.UnitOfWork.Interception;
using Murtain.OAuth2.Core.Captcha;
using Murtain.OAuth2.SDK.Enum;
using Murtain.OAuth2.SDK.User;
using System;
using System.Threading.Tasks;

namespace Murtain.OAuth2.ApplicationService.User
{

    public interface IUserApplicationService : IApplicationService
    {
        Task<bool> SendMessageCaptchaAsync(SendMessageCaptchaAsyncRequest input);
        Task<CaptchaMessageCacheData> GetCaptchaMessageCacheDataAsync(GetCaptchaMessageCacheDataAsyncRequest input);
        Task SetPasswordAsync(SetPasswordAsyncRequest input);
        Task<byte[]> GetGraphicCaptchaAsync();
        Task<bool> CheckUserExsistAsync(string mobile);
        Task<bool> CheckGraphicCaptchaAsync(string graphic_captcha);
        Task<SDK.User.User> AuthenticateLocalAsync(string username, string password);
        Task<SDK.User.User> GetUserProfileDataAsync(string subId);
        Task UpdatePropertyAsync(string subId, string property, string value);
    }
}
