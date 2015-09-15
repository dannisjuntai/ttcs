using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailFilterRuleMetaData))]
    public partial class EEmailFilterRule
    {

        private class EEmailFilterRuleMetaData
        {
            public int Id { get; set; }

            [Display(Name = "主旨")]
            public string MsgSubject { get; set; }

            [Display(Name = "信件內容")]
            public string MsgBody { get; set; }

            [Display(Name = "寄件者")]
            public string MsgFrom { get; set; }

            [Display(Name = "寄件者IP")]
            public string MsgReceivedBy { get; set; }

            [Display(Name = "啟用")]
            public bool Active { get; set; }
        }
    }
}