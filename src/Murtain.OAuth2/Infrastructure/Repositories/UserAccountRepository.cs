
using Murtain.OAuth2.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Extensions.Domain;
using Microsoft.EntityFrameworkCore;
using Murtain.OAuth2.Domain.Aggregates.User;
using System.Threading.Tasks;
using System.Linq;

namespace Murtain.OAuth2.Infrastructure.Repositories
{
    public class UserAccountRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserAccountRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return dbContext;
            }
        }

        public async Task<IdentityUser> GetAsync(long id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                await dbContext.Entry(user).Reference(x => x.UserProperty).LoadAsync();
            }
            return user;
        }

        public async Task<IdentityUser> AddAsync(IdentityUser user)
        {
            return await Task.FromResult(dbContext.Users.Add(user).Entity);
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            dbContext.Entry(user).State = EntityState.Modified;
            await Task.FromResult(0);
        }
    }
}
