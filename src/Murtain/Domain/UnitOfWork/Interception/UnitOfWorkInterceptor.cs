using Dora.DynamicProxy;
using Dora.Interception;
using Murtain.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.Domain.UnitOfWork.Interception
{
    public class UnitOfWorkInterceptor
    {
        private readonly InterceptDelegate next;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public UnitOfWorkInterceptor(InterceptDelegate next, IUnitOfWorkManager unitOfWorkManager)
        {
            this.next = next;
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(InvocationContext context)
        {
            try
            {
                using (var uow = unitOfWorkManager.Begin())
                {
                    await next(context);
                    await uow.CompleteAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
