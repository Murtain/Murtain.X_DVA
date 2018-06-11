using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Admin.Controllers
{
    public class AssetManager
    {
        static ConcurrentDictionary<string, string> ResourceStrings = new ConcurrentDictionary<string, string>();
        internal static string LoadResourceString(string name)
        {
            string value;
            if (!ResourceStrings.TryGetValue(name, out value))
            {
                var assembly = typeof(AssetManager).Assembly;
                using (var sr = new StreamReader(assembly.GetManifestResourceStream(name)))
                {
                    ResourceStrings[name] = value = sr.ReadToEnd();
                }
            }
            return value;
        }

        internal static string LoadResourceString(string name, IDictionary<string, object> values)
        {
            string value = LoadResourceString(name);
            foreach (var key in values.Keys)
            {
                var val = values[key];
                value = value.Replace("{" + key + "}", val != null ? val.ToString() : "");
            }
            return value;
        }

        internal static string LoadResourceString(string name, object values)
        {
            return LoadResourceString(name, Map(values));
        }

        static IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }
    }
}
