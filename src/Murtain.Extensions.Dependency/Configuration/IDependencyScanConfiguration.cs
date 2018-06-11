using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.Extensions.Dependency.Configuration
{
    public interface IDependencyScanConfiguration
    {
        string AssemblyLoaderParttern { get; set; }
    }
}
