using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Murtain.Extensions.Gemini.Builder
{
    public interface IGeminiApplicationServiceCollection
    {
        IServiceCollection Services { get; }
    }
}
