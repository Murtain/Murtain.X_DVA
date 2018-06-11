using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Core.User
{
    public interface IUserManager
    {
        Task<Domain.Aggregates.User.IdentityUser> FindByUsernameAsync(string username);
        Task<Domain.Aggregates.User.IdentityUser> CreateAsync(Domain.Aggregates.User.IdentityUser user);
        Task<Domain.Aggregates.User.IdentityUser> CreateAsync(string username, string password);

    }
}
