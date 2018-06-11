using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Murtain.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Murtain.Extensions.Settings.Caching.Models;
using Murtain.Extensions.Settings.Caching.Provider;
using Murtain.Extensions.Settings.Configuration;

namespace Murtain.Extensions.Settings.Caching
{
    public class CacheSettingManager : ICacheSettingManager
    {
        private readonly IMemoryCache memoryCache;
        private readonly IServiceProvider serviceProvider;
        private readonly ISettingsConfiguration settingsConfiguration;

        public CacheSettingManager(IServiceProvider serviceProvider, IMemoryCache memoryCache, ISettingsConfiguration settingsConfiguration)
        {
            this.memoryCache = memoryCache;
            this.serviceProvider = serviceProvider;
            this.settingsConfiguration = settingsConfiguration;
        }

        private IEnumerable<CacheSetting> GlobalSettings
        {
            get
            {
                return memoryCache.GetOrCreate<IEnumerable<CacheSetting>>(settingsConfiguration.CacheSettingName, (entry) =>
                {

                    var temp = new List<CacheSetting>();

                    foreach (var providerType in settingsConfiguration.CacheSettingProviders)
                    {
                        var provider = CreateCacheSettingProvider(providerType);
                        foreach (var s in provider.GetCacheSettings())
                        {
                            temp.Add(s);
                        }
                    }

                    return temp;
                });
            }
        }
        public Task<IEnumerable<CacheSetting>> GetAsync()
        {
            return Task.FromResult(GlobalSettings);
        }

        public Task<CacheSetting> GetAsync(string name)
        {
            return Task.FromResult(GlobalSettings.FirstOrDefault(x => x.Name == name));
        }

        public Task<TimeSpan?> GetTimeSpanAsync(string name)
        {
            return Task.FromResult(GlobalSettings.FirstOrDefault(x => x.Name == name)?.ExpiredTime);
        }

        public Task<string> GetValueAsync(string name)
        {
            return Task.FromResult(GlobalSettings.FirstOrDefault(x => x.Name == name)?.Value);
        }

        private CacheSettingProvider CreateCacheSettingProvider(Type providerType)
        {
            return serviceProvider.GetService(providerType).TryAs<CacheSettingProvider>();
        }
    }
}
