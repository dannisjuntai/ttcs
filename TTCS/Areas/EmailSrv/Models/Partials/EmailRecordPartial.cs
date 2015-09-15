using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailRecordMetaData))]
    public partial class EEmailRecord
    {
        public string MailGroupID { get; set; }

        private class EEmailRecordMetaData
        {
            [Display(Name = "案件編號")]
            public string RID { get; set; }

            public int RawHeaderId { get; set; }

            [Display(Name = "處理期限")]
            public System.DateTime ExpireOn { get; set; }
            public System.DateTime ReplyOn { get; set; }

            [Display(Name = "進件時間")]
            public Nullable<System.DateTime> IncomeOn { get; set; }

            [Display(Name = "指派時間")]
            public Nullable<System.DateTime> AssignOn { get; set; }
            public string InitAgentId { get; set; }

            [Display(Name = "最後處理人員")]
            public string CurrAgentId { get; set; }

            [Display(Name = "轉派次數")]            
            public Nullable<int> AssignCount { get; set; }

            [Display(Name = "垃圾?")] 
            public Nullable<int> Garbage { get; set; }
            
            public int Id { get; set; }

            [Display(Name = "退回原因")]
            public string RejectReason { get; set; }

            [Display(Name = "處理狀況")]
            public string ProcessStatus { get; set; }
            public Nullable<int> OrderNo { get; set; }
            public string SaveCntIn { get; set; }

            public Nullable<int> UnRead { get; set; }
        }
    }
}