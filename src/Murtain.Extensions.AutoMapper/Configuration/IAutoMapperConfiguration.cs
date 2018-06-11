using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace Murtain.Extensions.AutoMapper.Configuration
{
    public interface IAutoMapperConfiguration
    {
        string AssemblyLoaderParttern { get; set; }
        IEnumerable<Action<IMapperConfigurationExpression>> Configurators { get; }
    }
}
