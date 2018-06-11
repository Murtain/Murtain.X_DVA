using Murtain.Extensions.Settings.Caching.Provider;
using Murtain.Extensions.Settings.Collections;
using Murtain.Extensions.Settings.GlobalSetting.Provider;
using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.Settings.Configuration
{
    public interface ISettingsConfiguration
    {
        string AssemblyLoaderParttern { get; set; }
        /// <summary>
        /// Settings cache key , default `CacheSettings`
        /// </summary>
        string CacheSettingName { get; set; }
        /// <summary>
        /// Settings cache key , default `GlobalSettings`
        /// </summary>
        string GlobalSettingCacheName { get; set; }
        /// <summary>
        /// List of settings providers.
        /// </summary>
        ITypeList<CacheSettingProvider> CacheSettingProviders { get; }
        /// <summary>
        /// List of settings providers.
        /// </summary>
        ITypeList<GlobalSettingProvider> GlobalSettingProviders { get; }
    }
}
