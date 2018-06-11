using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Extensions.Settings.Collections;
using Murtain.Extensions.Settings.GlobalSetting.Provider;
using Murtain.Extensions.Settings.Caching.Provider;

namespace Murtain.Extensions.Settings.Configuration
{
    public class SettingsConfiguration : ISettingsConfiguration
    {

        public SettingsConfiguration()
        {
            this.GlobalSettingCacheName = "GlobalSettings";
            this.CacheSettingName = "CacheSettings";
            this.GlobalSettingProviders = new TypeList<GlobalSettingProvider>();
            this.CacheSettingProviders = new TypeList<CacheSettingProvider>();
        }

        public string AssemblyLoaderParttern { get; set; }
        public string GlobalSettingCacheName { get; set; }
        public string CacheSettingName { get; set; }
        public ITypeList<GlobalSettingProvider> GlobalSettingProviders { get; }
        public ITypeList<CacheSettingProvider> CacheSettingProviders { get; set; }

    }
}
