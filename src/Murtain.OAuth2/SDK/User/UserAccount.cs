using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.OAuth2.SDK.User
{
    public class User
    {
        public User()
        {
            //Claims = new List<Claim>();
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 所在省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        public string SubjectId { get; set; }
        /// <summary>
        /// 登录提供程序
        /// </summary>
        public string LoginProvider { get; set; }
        /// <summary>
        /// 登录提供程序Id
        /// </summary>
        public string LoginProviderId { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        ///// <summary>
        ///// 声明
        ///// </summary>
        //public IList<Claim> Claims { get; set; }
    }
}
