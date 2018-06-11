using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Text;


namespace Murtain.Domain.UnitOfWork.Interception
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UnitOfWorkInceptorAttribute : InterceptorAttribute
    {
        public UnitOfWorkInceptorAttribute()
        {
        }
        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<UnitOfWorkInterceptor>(this.Order);
        }
    }
}
