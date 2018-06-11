using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Application
{
    public interface ILoginApplicationService<T>
    {
        Task<bool> ValidateCredentialsAsync(T user, string password);
        Task<T> FindByUsernameAsync(string user);
        Task SignInAsync(T user);
    }
}
