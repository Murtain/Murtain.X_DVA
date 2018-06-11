using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Murtain.Resource.Story.Controllers
{
    [Produces("application/json")]
    public class LinesController : Controller
    {
        [HttpGet]
        [Route("api/lines")]
        public IEnumerable<string> Get(int index = 1, int size = 10)
        {
            return new string[] { "line-1", "line-2" };
        }

        [HttpGet]
        [Route("api/lines/{id}")]
        public IEnumerable<string> Get2(int id)
        {
            return new string[] { "line-1", "line-2" };
        }
    }
}