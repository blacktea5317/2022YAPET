using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YAPET.Models
{
    public class Register
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string UserAccount { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string UserPwd { get; set; }
        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [Compare("UserPwd", ErrorMessage = "密碼不相符。")]
        public string UserPwdConfirm { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string UserName { get; set; }
        [DisplayName("信箱")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "信箱格式錯誤")]
        public string UserEmail { get; set; }
    }
}