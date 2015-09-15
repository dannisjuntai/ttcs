using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailRejectReasonMetaData))]
    public partial class EEmailRejectReason
    {
        private class EEmailRejectReasonMetaData
        {
            public int Id { get; set; }

            [Display(Name = "退回原因")]
            [Required(ErrorMessage="請輸入原因")]
            public string Reason { get; set; }
        }
    }
}