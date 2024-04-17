using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YAPET.Models
{
    public class PwdForget
    {
        [DisplayName("信箱")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "信箱格式錯誤")]
        public string EMail { get; set; }
    }

    public class PwdChange
    {
        [DisplayName("密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string OldPwd { get; set; }

        [DisplayName("新密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string NewPwd { get; set; }

        [DisplayName("確認新密碼")]
        [Required(ErrorMessage = "此欄位為必填")]
        [Compare("NewPwd", ErrorMessage = "新密碼不相符。")]
        public string ConfirmPwd { get; set; }
    }

}