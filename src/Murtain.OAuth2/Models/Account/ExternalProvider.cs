using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Models.Account
{
    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
