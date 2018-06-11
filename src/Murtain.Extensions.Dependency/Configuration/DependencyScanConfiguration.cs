using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Murtain.Extensions.Dependency.Configuration
{
    public class DependencyScanConfiguration : IDependencyScanConfiguration
    {

        public string AssemblyLoaderParttern { get; set; }

        public DependencyScanConfiguration()
        {
        }
    }
}
