using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.Settings.GlobalSetting.Provider
{
    public abstract class GlobalSettingProvider
    {
        /// <summary>
        /// Gets all setting definitions provided by this provider.
        /// </summary>
        /// <returns>List of settings</returns>
        public abstract IEnumerable<Models.GlobalSetting> GetSettings();
    }
}
