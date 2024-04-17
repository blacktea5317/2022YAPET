using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YAPET.Models
{
    public class Login
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "帳號為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "密碼為必填")]
        [RegularExpression("[a-zA-Z0-9_]{4,30}", ErrorMessage = "請填寫4~30個英文或數字")]
        public string Password { get; set; }
    }
}