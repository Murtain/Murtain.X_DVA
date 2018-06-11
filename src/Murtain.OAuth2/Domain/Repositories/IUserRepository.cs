using Murtain.Extensions.Domain;
using Murtain.OAuth2.Domain.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Domain.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetAsync(long id);
    }
}
