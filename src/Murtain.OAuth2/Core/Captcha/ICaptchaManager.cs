using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Murtain.OAuth2.SDK.Enum;
using Murtain.Dependency;

namespace Murtain.OAuth2.Core.Captcha
{
    public interface ICaptchaManager : ITransientDependency
    {
      
        Task MessageCaptchaSendAsync(MESSAGE_CAPTCHA_TYPE captcha, string mobile, int expiredTime);
     
        Task MessageCaptchaResendAsync(MESSAGE_CAPTCHA_TYPE captcha, string mobile, int expiredTime);

        Task<CaptchaMessageCacheData> GetCaptchaMessageCacheDataAsync(MESSAGE_CAPTCHA_TYPE type, string mobile, string captha);

        Task EmailCaptchaSendAsync(MESSAGE_CAPTCHA_TYPE type, string mobile, int expiredTime);
  
        Task EmailCaptchaResendAsync(MESSAGE_CAPTCHA_TYPE type, string mobile, int expiredTime);
     
        Task EmailCaptchaValidateAsync(MESSAGE_CAPTCHA_TYPE type, string email, string captha);
       
        Task<byte[]> GraphicCaptchaGenderatorAsync(string cookiename);
       
        Task<bool> GraphicCaptchaValidateAsync(string cookiename, string captcha);

    }
}
