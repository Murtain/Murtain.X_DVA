using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace Murtain.Extensions.AutoMapper.Configuration
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public IEnumerable<Action<IMapperConfigurationExpression>> Configurators { get; }

        public string AssemblyLoaderParttern { get; set; }

        public AutoMapperConfiguration()
        {
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}
