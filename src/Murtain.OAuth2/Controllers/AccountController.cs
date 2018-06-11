using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Murtain.Extensions.AutoMapper;
using Murtain.Extensions;
using Murtain.OAuth2.Configuration;
using Murtain.OAuth2.Models;
using Murtain.OAuth2.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Murtain.OAuth2.Domain.Aggregates.User;
using Murtain.OAuth2.Application;
using Murtain.OAuth2.Models.Enum;
using Murtain.OAuth2.Providers;

namespace Murtain.OAuth2.Controllers
{
    /// <summary>
    /// This controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILoginApplicationService<ApplicationUser> _loginApplicationService;
        private readonly IClientStore _clientStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TestUserStore _store;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            //IHttpContextAccessor httpContextAccessor,
            IAuthenticationSchemeProvider schemeProvider
            //IEventService events,
            //ITempDataProvider tempDataProvider,
            //ILoginApplicationService<ApplicationUser> loginApplicationService,
            //UserManager<IdentityUser> userManager,
            //TestUserStore store

            )
        {
            _interaction = interaction;
            _clientStore = clientStore;
            //_httpContextAccessor = httpContextAccessor;
            //_interaction = interaction;
            _schemeProvider = schemeProvider;
            //_eventService = events;
            //_userManager = userManager;
            //_loginApplicationService = loginApplicationService;

            //_store = store;
        }
        /// <summary>
        /// Render the page for login
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
            }
            TempData["ReturnUrl"] = returnUrl;
            return View(vm);
        }
        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("account/login")]
        public async Task<IActionResult> Login(LoginViewModel model, string button)
        {
            if (button != "login")
            {
                // the user clicked the "cancel" button
                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (!ModelState.IsValid)
            {
                // something went wrong, show form with error
                var vm = await BuildLoginViewModelAsync(model);
                await ModelStateCleanAsync(vm);
                return View(vm);
            }

            // validate username/password against in-memory store

            var user = await _loginApplicationService.FindByUsernameAsync(model.Username);

            if (!await _loginApplicationService.ValidateCredentialsAsync(user, model.Password))
            {

                await _eventService.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);

                // something went wrong, show form with error
                var vm = await BuildLoginViewModelAsync(model);
                return View(vm);
            }

            await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.SecurityStamp, user.UserProperty?.NickName));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (AccountOptions.AllowRememberLogin && model.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration),
                };
            };

            // issue authentication cookie with subject ID and username
            await HttpContext.SignInAsync(
                    user.SecurityStamp,
                    user.SecurityStamp,
                    props,
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Name, user.UserProperty?.NickName)
            );

            // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
            if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("~/");
        }

        /////// <summary>
        /////// Render the page for validate user identity
        /////// </summary>
        /////// <returns></returns>
        ////[Route("account/validate-id")]
        ////public async Task<IActionResult> ValidateID(string returnUrl, string type)
        ////{
        ////    var vm = await BuildValidateIdViewModelAsync(returnUrl, Enum.Parse<SDK.Enum.MESSAGE_CAPTCHA_TYPE>(type).ToString());
        ////    return View(vm);
        ////}
        /////// <summary>
        /////// Post processing of validate user identity 
        /////// </summary>
        /////// <param name="input"></param>
        /////// <returns></returns>
        ////[HttpPost]
        ////[Route("account/validate-id")]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> ValidateID(ValidateIdInput input)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        var vm = await BuildValidateIdViewModelAsync(input);
        ////        await ModelStateCleanAsync(vm);
        ////        return View(vm);
        ////    }


        ////    var mobileExsits = await userApplicationService.CheckUserExsistAsync(input.Mobile);
        ////    if (mobileExsits && input.CaptchaType == SDK.Enum.MESSAGE_CAPTCHA_TYPE.REGISTER)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Mobile, "exsits mobile"));
        ////        ModelState.AddModelError("", AccountOptions.MobileExists);

        ////        var vm = await BuildValidateIdViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    if (!mobileExsits && input.CaptchaType == SDK.Enum.MESSAGE_CAPTCHA_TYPE.RETRIEVE_PASSWORD)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Mobile, "mobile not exsits "));
        ////        ModelState.AddModelError("", AccountOptions.MobileNotExists);

        ////        var vm = await BuildValidateIdViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    if (!await userApplicationService.CheckGraphicCaptchaAsync(input.GraphicCaptcha))
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.GraphicCaptcha, "invalid graphic captcha"));
        ////        ModelState.AddModelError("", AccountOptions.InvalidGraphicCaptcha);
        ////        var vm = await BuildValidateIdViewModelAsync(input);
        ////        return View(vm);
        ////    }


        ////    if (!await userApplicationService.SendMessageCaptchaAsync(new SDK.User.SendMessageCaptchaAsyncRequest
        ////    {
        ////        CaptchaType = input.CaptchaType,
        ////        Mobile = input.Mobile
        ////    }))
        ////    {

        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.GraphicCaptcha, "message code send failed"));
        ////        ModelState.AddModelError("", AccountOptions.MessageCaptchaSendFailed);
        ////        var vm = await BuildValidateIdViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    TempData["Mobile"] = input.Mobile;
        ////    TempData["GraphicCaptcha"] = input.GraphicCaptcha;
        ////    TempData["CaptchaType"] = input.CaptchaType.ToString();
        ////    return RedirectToAction("validate-captcha", "account", new { returnUrl = input.ReturnUrl });
        ////}

        /////// <summary>
        /////// Render the validate message captcha page 
        /////// </summary>
        /////// <returns></returns>
        ////[Route("account/validate-captcha")]
        ////public async Task<IActionResult> ValidateCaptcha(string returnUrl)
        ////{
        ////    var vm = await BuildValidateCaptchaViewModelAsync(returnUrl, TempData["Mobile"] as string, TempData["CaptchaType"] as string, TempData["GraphicCaptcha"] as string);
        ////    return View(vm);
        ////}
        /////// <summary>
        /////// Post processing of validate message captcha
        /////// </summary>
        /////// <param name="input"></param>
        /////// <returns></returns>
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////[Route("account/validate-captcha")]
        ////public async Task<IActionResult> ValidateCaptcha(ValidateCaptchaInput input)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        var vm = await BuildValidateCaptchaViewModelAsync(input);
        ////        await ModelStateCleanAsync(vm);
        ////        return View(vm);
        ////    }
        ////    var captchaMessageCacheData = await userApplicationService.GetCaptchaMessageCacheDataAsync(new SDK.User.GetCaptchaMessageCacheDataAsyncRequest
        ////    {
        ////        Captcha = input.Captcha,
        ////        CaptchaType = input.CaptchaType,
        ////        Mobile = input.Mobile
        ////    });

        ////    if (captchaMessageCacheData == null)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Captcha, " message captcha expired"));
        ////        ModelState.AddModelError("", AccountOptions.MessageCaptchaExpired);

        ////        var vm = await BuildValidateCaptchaViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    if (captchaMessageCacheData.Captcha != input.Captcha)
        ////    {

        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Captcha, " invalid captcha expired"));
        ////        ModelState.AddModelError("", AccountOptions.InvalidMessageCaptcha);

        ////        var vm = await BuildValidateCaptchaViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    TempData["Mobile"] = input.Mobile;
        ////    TempData["CaptchaType"] = input.CaptchaType.ToString();
        ////    TempData["Captcha"] = input.Captcha;
        ////    return RedirectToAction("password", "account", new { returnUrl = input.ReturnUrl });
        ////}

        /////// <summary>
        /////// Render the password page
        /////// </summary>
        /////// <returns></returns>
        ////[Route("account/password")]
        ////public async Task<IActionResult> Password(string returnUrl)
        ////{
        ////    var vm = await BuildPasswordViewModelAsync(returnUrl, TempData["Mobile"] as string, TempData["CaptchaType"] as string, TempData["Captcha"] as string);
        ////    return View(vm);
        ////}
        /////// <summary>
        ///////  Post processing of set password
        /////// </summary>
        /////// <param name="input"></param>
        /////// <returns></returns>
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////[Route("account/password")]
        ////public async Task<IActionResult> Password(PasswordInput input)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        var vm = await BuildPasswordViewModelAsync(input); ;
        ////        await ModelStateCleanAsync(vm);
        ////        return View(vm);
        ////    }

        ////    if (input.Password != input.ConfirmPassword)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Captcha, "two input password must be consistent"));
        ////        ModelState.AddModelError("", AccountOptions.MessageCaptchaExpired);
        ////        var vm = await BuildPasswordViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    var captchaMessageCacheData = await userApplicationService.GetCaptchaMessageCacheDataAsync(new SDK.User.GetCaptchaMessageCacheDataAsyncRequest
        ////    {
        ////        Captcha = input.Captcha,
        ////        CaptchaType = input.CaptchaType,
        ////        Mobile = input.Mobile
        ////    });

        ////    if (captchaMessageCacheData == null)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Captcha, " message captcha expired"));
        ////        ModelState.AddModelError("", AccountOptions.MessageCaptchaExpired);

        ////        var vm = await BuildPasswordViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    if (captchaMessageCacheData.Captcha != input.Captcha)
        ////    {
        ////        await _eventService.RaiseAsync(new UserLoginFailureEvent(input.Captcha, " invalid captcha expired"));
        ////        ModelState.AddModelError("", AccountOptions.InvalidMessageCaptcha);

        ////        var vm = await BuildPasswordViewModelAsync(input);
        ////        return View(vm);
        ////    }

        ////    await userApplicationService.SetPasswordAsync(new SDK.User.SetPasswordAsyncRequest
        ////    {
        ////        Mobile = input.Mobile,
        ////        Password = input.Password,
        ////    });

        ////    return RedirectToAction("login", "account", new { returnUrl = input.ReturnUrl });
        ////}

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("ExternalLoginCallback"),
                Items =
                {
                    { "returnUrl", returnUrl }
                }
            };

            // windows authentication needs special handling
            // since they don't support the redirect uri, 
            // so this URL is re-triggered when we call challenge
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // see if windows auth has already been requested and succeeded
                var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
                if (result?.Principal is WindowsPrincipal wp)
                {
                    props.Items.Add("scheme", AccountOptions.WindowsAuthenticationSchemeName);

                    var id = new ClaimsIdentity(provider);
                    id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                    id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                    // add the groups as claims -- be careful if the number of groups is too large
                    if (AccountOptions.IncludeWindowsGroups)
                    {
                        var wi = wp.Identity as WindowsIdentity;
                        var groups = wi.Groups.Translate(typeof(NTAccount));
                        var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                        id.AddClaims(roles);
                    }

                    await HttpContext.SignInAsync(
                        IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                        new ClaimsPrincipal(id),
                        props);
                    return Redirect(props.RedirectUri);
                }
                else
                {
                    // challenge/trigger windows auth
                    return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
                }
            }
            else
            {
                // start challenge and roundtrip the return URL
                props.Items.Add("scheme", provider);
                return Challenge(props, provider);
            }
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var externalUser = result.Principal;
            var claims = externalUser.Claims.ToList();

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("Unknown userid");
            }

            // remove the user id claim from the claims collection and move to the userId property
            // also set the name of the external authentication provider
            claims.Remove(userIdClaim);
            var provider = result.Properties.Items["scheme"];
            var userId = userIdClaim.Value;

            // this is where custom logic would most likely be needed to match your users from the
            // external provider's authentication result, and provision the user as you see fit.
            // 
            // check if the external user is already provisioned
            var user = _store.FindByExternalProvider(provider, userId);
            if (user == null)
            {
                // this sample simply auto-provisions new external user
                // another common approach is to start a registrations workflow first
                user = _store.AutoProvisionUser(provider, userId, claims);
            }

            var additionalClaims = new List<Claim>();

            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            AuthenticationProperties props = null;
            var id_token = result.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                props = new AuthenticationProperties();
                props.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }

            // issue authentication cookie for user
            await _eventService.RaiseAsync(new UserLoginSuccessEvent(provider, userId, user.SubjectId, user.Username));
            await HttpContext.SignInAsync(user.SubjectId, user.Username, provider, props, additionalClaims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // validate return URL and redirect back to authorization endpoint or a local page
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        ///// <summary>
        ///// Render the page for logout
        ///// </summary>
        //[HttpGet]
        //public async Task<IActionResult> Logout(string logoutId)
        //{
        //    // build a model so the logout page knows what to display
        //    var vm = await BuildLogoutViewModelAsync(logoutId);

        //    if (vm.ShowLogoutPrompt == false)
        //    {
        //        // if the request for logout was properly authenticated from IdentityServer, then
        //        // we don't need to show the prompt and can just log the user out directly.
        //        return await Logout(vm);
        //    }

        //    return View(vm);
        //}
        ///// <summary>
        ///// Handle logout page postback
        ///// </summary>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout(LogoutInputModel model)
        //{
        //    // build a model so the logged out page knows what to display
        //    var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        //    var user = HttpContext.User;
        //    if (user?.Identity.IsAuthenticated == true)
        //    {
        //        // delete local authentication cookie
        //        await HttpContext.SignOutAsync();

        //        // raise the logout event
        //        await _eventService.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
        //    }

        //    // check if we need to trigger sign-out at an upstream identity provider
        //    if (vm.TriggerExternalSignout)
        //    {
        //        // build a return URL so the upstream provider will redirect back
        //        // to us after the user has logged out. this allows us to then
        //        // complete our single sign-out processing.
        //        string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

        //        // this triggers a redirect to the external provider for sign-out
        //        return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
        //    }

        //    return View("LoggedOut", vm);
        //}



        private async Task ModelStateCleanAsync(object model)
        {
            var properties = model.GetType().GetProperties().Select(x => x.Name).ToList();

            var error = ModelState
                    .Where(x => x.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    .Select(m => new { Order = properties.IndexOf(m.Key), Error = m.Value, Key = m.Key })
                    .OrderBy(m => m.Order)
                    .FirstOrDefault();

            if (error != null)
            {
                ModelState.Clear();
                ModelState.AddModelError(error.Key, string.Join(",", error.Error.Errors.Select(x => x.ErrorMessage)));
            }

            await Task.FromResult(0);
        }


        //public async Task<ValidateIdViewModel> BuildValidateIdViewModelAsync(string returnUrl, string captchaType)
        //{
        //    return await Task.FromResult(new ValidateIdViewModel
        //    {
        //        Agreement = "true",
        //        CaptchaType = Enum.Parse<MESSAGE_CAPTCHA_TYPE>(captchaType),
        //        ReturnUrl = returnUrl
        //    });
        //}
        //public async Task<ValidateIdViewModel> BuildValidateIdViewModelAsync(ValidateIdInput input)
        //{
        //    return await Task.FromResult(new ValidateIdViewModel
        //    {
        //        Agreement = input.Agreement,
        //        GraphicCaptcha = input.GraphicCaptcha,
        //        CaptchaType = input.CaptchaType,
        //        Mobile = input.Mobile,
        //        ReturnUrl = input.ReturnUrl
        //    });
        //}
        //public async Task<ValidateCaptchaViewModel> BuildValidateCaptchaViewModelAsync(string returnUrl, string mobile, string captchaType, string graphicCaptcha)
        //{
        //    return await Task.FromResult(new ValidateCaptchaViewModel
        //    {
        //        GraphicCaptcha = graphicCaptcha,
        //        CaptchaType = Enum.Parse<MESSAGE_CAPTCHA_TYPE>(captchaType),
        //        Agreement = "true",
        //        Mobile = mobile,
        //        ReturnUrl = returnUrl
        //    });
        //}
        //public async Task<ValidateCaptchaViewModel> BuildValidateCaptchaViewModelAsync(ValidateCaptchaInput input)
        //{
        //    return await Task.FromResult(new ValidateCaptchaViewModel
        //    {
        //        GraphicCaptcha = input.GraphicCaptcha,
        //        Agreement = "true",
        //        Mobile = input.Mobile,
        //        CaptchaType = input.CaptchaType,
        //        ReturnUrl = input.ReturnUrl,
        //        Captcha = input.Captcha
        //    });
        //}
        //public async Task<PasswordViewModel> BuildPasswordViewModelAsync(string returnUrl, string mobile, string captchaType, string captcha)
        //{
        //    return await Task.FromResult(new PasswordViewModel
        //    {
        //        Mobile = mobile,
        //        Captcha = captcha,
        //        CaptchaType = Enum.Parse<MESSAGE_CAPTCHA_TYPE>(captchaType),
        //        Agreement = "true",
        //        ReturnUrl = returnUrl
        //    });
        //}
        //public async Task<PasswordViewModel> BuildPasswordViewModelAsync(PasswordInput input)
        //{
        //    return await Task.FromResult(new PasswordViewModel
        //    {
        //        Mobile = input.Mobile,
        //        Captcha = input.Captcha,
        //        Agreement = "true",
        //        ReturnUrl = input.ReturnUrl,
        //        CaptchaType = input.CaptchaType,
        //        Password = input.Password,
        //        ConfirmPassword = input.ConfirmPassword
        //    });
        //}
        public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = context.IdP } }
                };
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }
        public async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
        //public async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        //{
        //    var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        //    var user = _httpContextAccessor.HttpContext.User;
        //    if (user?.Identity.IsAuthenticated != true)
        //    {
        //        // if the user is not authenticated, then just show logged out page
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }

        //    var context = await _interaction.GetLogoutContextAsync(logoutId);
        //    if (context?.ShowSignoutPrompt == false)
        //    {
        //        // it's safe to automatically sign-out
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }

        //    // show the logout prompt. this prevents attacks where the user
        //    // is automatically signed out by another malicious web page.
        //    return vm;
        //}
        //public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        //{
        //    // get context information (client name, post logout redirect URI and iframe for federated signout)
        //    var logout = await _interaction.GetLogoutContextAsync(logoutId);

        //    var vm = new LoggedOutViewModel
        //    {
        //        AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
        //        PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
        //        ClientName = logout?.ClientId,
        //        SignOutIframeUrl = logout?.SignOutIFrameUrl,
        //        LogoutId = logoutId
        //    };

        //    var user = _httpContextAccessor.HttpContext.User;
        //    if (user?.Identity.IsAuthenticated == true)
        //    {
        //        var idp = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        //        if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
        //        {
        //            var providerSupportsSignout = await _httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(idp);
        //            if (providerSupportsSignout)
        //            {
        //                if (vm.LogoutId == null)
        //                {
        //                    // if there's no current logout context, we need to create one
        //                    // this captures necessary info from the current logged in user
        //                    // before we signout and redirect away to the external IdP for signout
        //                    vm.LogoutId = await _interaction.CreateLogoutContextAsync();
        //                }

        //                vm.ExternalAuthenticationScheme = idp;
        //            }
        //        }
        //    }

        //    return vm;
        //}
    }



}
