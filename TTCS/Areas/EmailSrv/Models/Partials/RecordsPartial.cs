using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(ERecordsMetaData))]
    public partial class ERecords
    {
        public string ServiceItemNames { get; set; }
        public string MailGroupID { get; set; }

        private class ERecordsMetaData
        {
            [Display(Name = "案件編號")]
            public string RID { get; set; }
            public Nullable<System.Guid> CustomerID { get; set; }

            [Display(Name = "案件類型")]
            public Nullable<int> TypeID { get; set; }
                        
            [Display(Name = "客服人員")]
            public string AgentID { get; set; }

            [Display(Name = "案件狀態")]
            public Nullable<int> StatusID { get; set; }
            public string Comment { get; set; }
            public Nullable<int> ServiceGroupID { get; set; }

            [Display(Name = "小結")]
            public Nullable<int> ServiceItemID { get; set; }

            [Display(Name = "進件時間")]
            public Nullable<System.DateTime> IncomingDT { get; set; }

            [Display(Name = "結案時間")]
            public Nullable<System.DateTime> FinishDT { get; set; }

            public string GroupID { get; set; }
        }
    }
}