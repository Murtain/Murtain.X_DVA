using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.Resource.Story.Controllers
{
    [Route("api/[controller]")]
    public class ScencesController
    {
        /// <summary>
        /// list of scences
        /// </summary>
        /// <param name="index">page index</param>
        /// <param name="size">page size</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get(int index = 1, int size = 10)
        {
            return new string[] { "scence-1", "scence-2" };
        }
    }
}
