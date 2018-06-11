using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Extensions.Settings.Caching.Models;

namespace Murtain.Extensions.Settings.Caching.Provider
{
    public abstract class CacheSettingProvider
    {
        public abstract IEnumerable<Murtain.Extensions.Settings.Caching.Models.CacheSetting> GetCacheSettings();
    }
}
