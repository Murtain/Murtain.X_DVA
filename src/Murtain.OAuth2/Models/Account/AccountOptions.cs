using System;

namespace Murtain.OAuth2.Configuration
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;

        // to enable windows authentication, the host (IIS or IIS Express) also must have 
        // windows auth enabled.
        public static bool WindowsAuthenticationEnabled = true;
        public static bool IncludeWindowsGroups = false;
        // specify the Windows authentication scheme and display name
        public static readonly string WindowsAuthenticationSchemeName = "Windows";

        public static string InvalidCredentialsErrorMessage = "账号或密码错误";
        public static string InvalidGraphicCaptcha = "图形验证码错误";
        public static string InvalidMessageCaptcha = "短信验证码错误";
        public static string MessageCaptchaExpired = "短信验证已失效";
        public static string PasswordMustBeConsistent = "密码不一致";
        public static string MessageCaptchaSendFailed = "短信发送失败";
        public static string MobileExists = "手机号已存在";
        public static string MobileNotExists = "手机号不存在";
    }
}
