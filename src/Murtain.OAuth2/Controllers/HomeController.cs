using IdentityModel.Client;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Murtain.OAuth2.Attributes;
using Murtain.OAuth2.Configuration;
using Murtain.OAuth2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Web.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService identityServerInteractionService;

        private readonly ILogger logger;

        public HomeController(IIdentityServerInteractionService identityServerInteractionService, ILogger<HomeController> logger)
        {
            this.identityServerInteractionService = identityServerInteractionService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogInformation("home Index ...");
            return View();
        }

        /// <summary>
        /// Render the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await identityServerInteractionService.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }

}