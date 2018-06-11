using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Murtain.Resource.Story.Controllers
{
    [Route("api/[controller]")]
    public class StoriesController : Controller
    {
        /// <summary>
        /// list of stories
        /// </summary>
        /// <param name="index">page index</param>
        /// <param name="size">page size</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get(int index = 1, int size = 10)
        {
            return new string[] { "story-1", "story-2" };
        }

    }
}
