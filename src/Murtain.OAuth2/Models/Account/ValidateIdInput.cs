using Murtain.Extensions.AutoMapper;
using Murtain.OAuth2.Models.Account;
using Murtain.OAuth2.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Models
{
    [AutoMap(typeof(ValidateIdViewModel))]
    public class ValidateIdInput : AccountViewModel
    {

        public MESSAGE_CAPTCHA_TYPE CaptchaType { get; set; }

        [Required(ErrorMessage = "请输入有效的手机号码")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "请输入图片中的验证码")]
        public string GraphicCaptcha { get; set; }

        [RegularExpression("true", ErrorMessage = "请阅读《法律声明》及《隐私条款》")]
        public string Agreement { get; set; } = "false";
    }


}
