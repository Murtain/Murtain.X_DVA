using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Controllers
{
    public class GraphicCaptchaController : Controller
    {
        public GraphicCaptchaController()
        {
        }

        [HttpGet]
        [Route("captcha/graphic")]
        public async Task<ActionResult> GetGraphicCaptchaAsync()
        {
            return Ok();
            //return File(await userAccountService.GetGraphicCaptchaAsync(), @"image/jpeg");
        }
    }
}
