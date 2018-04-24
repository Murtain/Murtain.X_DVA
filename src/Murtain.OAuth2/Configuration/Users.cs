using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Configuration
{
    public class Users
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                 new TestUser(){
                     SubjectId = "sub-1061306002",
                     Username = "alice@qq.com",
                     Password = "alice"
                 }
            };
        }
    }
}
