using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Extensions.Settings.GlobalSetting.Provider;
using Murtain.Extensions.Settings.GlobalSetting.Models;

namespace Murtain.OAuth2.Core.Providers
{
    public class EmailSettingProvider : GlobalSettingProvider
    {
        public override IEnumerable<GlobalSetting> GetSettings()
        {
            return new GlobalSetting[] { };
        }
    }
}
