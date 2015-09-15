using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailContactsMetaData))]
    public partial class EEmailContacts
    {
        private class EEmailContactsMetaData
        {
            public int Id { get; set; }

            [Display(Name = "群組")]
            public string ContactGroup { get; set; }

            [Display(Name = "名稱")]
            public string ContactName { get; set; }

            [Display(Name = "信箱")]
            [Required(ErrorMessage="請輸入郵件信箱")]
            [EmailAddress(ErrorMessage="請輸入正確郵件信箱格式")]
            public string ContactEmail { get; set; }
        }
    }
}