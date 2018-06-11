using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Murtain.Extensions.Settings.GlobalSetting.Models;
using Murtain.Extensions.Settings.GlobalSetting.Provider;
using Murtain.Extensions.Settings.GlobalSetting.Store;
using Microsoft.Extensions.Caching.Memory;
using Murtain.Extensions.Settings.Configuration;

namespace Murtain.Extensions.Settings.GlobalSetting
{
    public class GlobalSettingManager : IGlobalSettingManager
    {
        private readonly IGlobalSettingStore globalSettingStore;
        private readonly ISettingsConfiguration globalSettingConfiguration;
        private readonly IMemoryCache memoryCache;
        private readonly IServiceProvider serviceProvider;


        public GlobalSettingManager(IGlobalSettingStore globalSettingStore, IServiceProvider serviceProvider, ISettingsConfiguration globalSettingConfiguration, IMemoryCache memoryCache)
        {
            this.globalSettingConfiguration = globalSettingConfiguration;
            this.memoryCache = memoryCache;
            this.serviceProvider = serviceProvider;
            this.globalSettingStore = globalSettingStore;

            ConfigurationLoadAsync();
        }


        private void ConfigurationLoadAsync()
        {
            globalSettingStore.MigrationAsync(GlobalSettings);
        }

        private IEnumerable<Models.GlobalSetting> GlobalSettings
        {
            get
            {
                return memoryCache.GetOrCreate<IEnumerable<Models.GlobalSetting>>(globalSettingConfiguration.GlobalSettingCacheName, (entry) =>
                {
                    var temp = new List<Models.GlobalSetting>();

                    foreach (var providerType in globalSettingConfiguration.CacheSettingProviders)
                    {
                        var provider = CreateGlobalSettingProvider(providerType);
                        foreach (var s in provider.GetSettings())
                        {
                            temp.Add(globalSettingStore.GetAsync(s.Name).Result);
                        }
                    }
                    return temp;
                });
            }
        }

        private GlobalSettingProvider CreateGlobalSettingProvider(Type providerType)
        {
            return serviceProvider.GetService(providerType).TryAs<GlobalSettingProvider>();
        }
        public async Task AddOrUpdateAsync(Models.GlobalSetting data)
        {
            await globalSettingStore.AddOrUpdateAsync(data);
        }

        public async Task ClearGlobalSettingCacheAsync()
        {
            memoryCache.Remove(globalSettingConfiguration.GlobalSettingCacheName);
            await Task.FromResult(0);
        }

        public async Task DeleteAsync(string name)
        {
            await globalSettingStore.DeleteAsync(name);
        }

        public async Task<IEnumerable<Models.GlobalSetting>> GetAsync()
        {
            return await Task.FromResult(GlobalSettings);
        }

        public async Task<Models.GlobalSetting> GetValueAsync(string name)
        {
            return await Task.FromResult(GlobalSettings.FirstOrDefault(x => x.Name == name));
        }

        public async Task<string> GetValueAsync(string name, GlobalSettingScope scope = GlobalSettingScope.Application)
        {
            return await Task.FromResult(GlobalSettings.FirstOrDefault(x => x.Name == name && x.Scope == scope)?.Value);
        }

    }
}
