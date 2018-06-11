using Murtain.Extensions.Settings.GlobalSetting.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Murtain.Extensions.Settings.GlobalSetting.Models;

namespace Murtain.OAuth2.Providers
{
    public class EmailSettingProvider : GlobalSettingProvider
    {
        public override IEnumerable<GlobalSetting> GetSettings()
        {
            return new GlobalSetting[] { };
        }
    }
}
