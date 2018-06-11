using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.Gemini.Builder
{
    public interface IGeminiApplicationBuilder
    {
        IApplicationBuilder Builder { get; set; }
    }
}
