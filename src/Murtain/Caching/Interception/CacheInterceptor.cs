using Dora.DynamicProxy;
using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.Caching.Interception
{
    public class CacheInterceptor
    {
        private readonly InterceptDelegate _next;
        private readonly ICacheManager _cacheManager;
        public CacheInterceptor(InterceptDelegate next, ICacheManager cacheManager)
        {
            _next = next;
            _cacheManager = cacheManager;
        }

        public async Task InvokeAsync(InvocationContext context)
        {
            if (!context.Method.GetParameters().All(it => it.IsIn))
            {
                await _next(context);
            }

            var value = await _cacheManager.Retrive<Task<object>>(context.Method.Name + Guid.NewGuid(), async () =>
            {
                await _next(context);
                return context.ReturnValue;
            });

            context.ReturnValue = value;
        }
    }
}
