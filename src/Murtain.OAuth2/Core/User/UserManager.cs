using Murtain.Extensions.AutoMapper;
using Murtain.OAuth2.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Murtain.OAuth2.SDK.User;
using System.Linq.Expressions;
using Murtain.OAuth2.Domain.Aggregates.User;

namespace Murtain.OAuth2.Core.User
{

    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;


        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<IdentityUser> CreateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> CreateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        //public async Task SetPasswordAsync(string mobile, string password)
        //{
        //    var user = await userRepository.FirstOrDefaultAsync(x => x.Mobile == mobile);

        //    if (user == null)
        //    {

        //        var subId = await GeneratorSubjectIdAsync();
        //        var entity = new Domain.Entities.User
        //        {
        //            Mobile = mobile,
        //            Password = await EncryptPasswordAsync(password),
        //            SubjectId = subId,
        //            ChangeTime = DateTime.Now,
        //            Name = mobile,
        //            NickName = "X_" + subId.Substring(0, 16),
        //            IsActived = true,
        //            IsDeleted = false,
        //        };

        //        await userRepository.AddAsync(entity);
        //    }
        //    else
        //    {
        //        user.Password = await EncryptPasswordAsync(password);
        //        await userRepository.UpdatePropertyAsync(user, x => new { x.Password });
        //    }
        //}
        //public async Task<Domain.Entities.User> AuthenticateLocalAsync(string username, string password)
        //{
        //    var user = await userRepository.FirstOrDefaultAsync(x => x.Mobile == username || x.Email == username);

        //    if (user == null || user.Password != await EncryptPasswordAsync(password))
        //    {
        //        return null;
        //    }

        //    return user;
        //}
        //public async Task<Domain.Entities.User> AuthenticateExternalAsync(AuthenticateExternalRequest input)
        //{
        //    // try find user with login provider and provider id
        //    var user = await userRepository.FirstOrDefaultAsync(x => x.LoginProvider == input.LoginProvider && x.LoginProviderId == input.LoginProviderId);

        //    // if not found create an new user
        //    if (user == null)
        //    {
        //        var entity = input.MapTo<Domain.Entities.User>();
        //        entity.SubjectId = await GeneratorSubjectIdAsync();

        //        return await userRepository.AddAsync(entity);
        //    }
        //    return user;
        //}
        //public async Task<Domain.Entities.User> GetProfileDataAsync(string subId)
        //{
        //    return await userRepository.FirstOrDefaultAsync(x => x.SubjectId == subId);
        //}
        //public async Task<Domain.Entities.User> FindAsync(string mobile)
        //{
        //    return await userRepository.FirstOrDefaultAsync(x => x.Mobile == mobile);
        //}
        //public async Task SaveEmailAsync(string mobile, string email)
        //{
        //    var user = await userRepository.FirstOrDefaultAsync(x => x.Mobile == mobile);

        //    if (user != null)
        //    {
        //        user.Email = email;
        //        await userRepository.UpdatePropertyAsync(user, x => new { x.Password });
        //    }
        //}

        //private Task<string> EncryptPasswordAsync(string password)
        //{
        //    return Task.FromResult(CryptoManager.EncryptMD5(password).ToUpper());
        //}
        //private Task<string> GeneratorSubjectIdAsync()
        //{
        //    return Task.FromResult(Guid.NewGuid().ToString("N").ToUpper());
        //}

        //public async Task UpdatePropertyAsync(Domain.Entities.User input)
        //{
        //    var entity = await userRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
        //    await userRepository.UpdateCompareAsync(input, entity);
        //}
    }
}
