using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace TTCS.Areas.EmailSrv.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "請輸入帳號!")]
        [Display(Name = "帳號")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入密碼!")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}
