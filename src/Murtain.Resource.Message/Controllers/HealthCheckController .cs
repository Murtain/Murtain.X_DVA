﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Murtain.Resource.Message.Controllers
{
    [Route("[Controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet()]
        public IActionResult Ping()
        {
            return Ok("I'm fine");
        }
    }
}
