using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(ECustomersMetaData))]
    public partial class ECustomers
    {
        [Display(Name = "已指派")]
        public int IsAssignRule { get; set; }

        private class ECustomersMetaData
        {
            public System.Guid ID { get; set; }

            [Display(Name = "中文名稱")]
            public string CName { get; set; }

            [Display(Name = "英文名稱")]
            public string EName { get; set; }

            [Display(Name = "客戶分類")]
            public string Category { get; set; }
            [Display(Name = "身分證字號 / 營利事業登記證")]
            public string ID2 { get; set; }
            [Display(Name = "客戶區域")]
            public string Region { get; set; }
            [Display(Name = "客戶所在城市")]
            public string City { get; set; }
            [Display(Name = "客戶地址")]
            public string Address { get; set; }
            [Display(Name = "備註")]
            public string Comment { get; set; }
            [Display(Name = "業務員")]
            public string Marketer { get; set; }
            [Display(Name = "負責客服人員1")]
            public string AgentID1 { get; set; }
            [Display(Name = "負責客服人員2")]
            public string AgentID2 { get; set; }
            [Display(Name = "客戶群組")]
            public string GroupID { get; set; }
 
            [Display(Name = "聯絡人1姓名")]
            public string Contact1_Name { get; set; }
            [Display(Name = "聯絡人1稱謂")]
            public string Contact1_Title { get; set; }
            [Display(Name = "聯絡人1電話1")]
            public string Contact1_Tel1 { get; set; }
            [Display(Name = "聯絡人1電話2")]
            public string Contact1_Tel2 { get; set; }
            [Display(Name = "聯絡人1郵件1")]
            public string Contact1_Email1 { get; set; }
            [Display(Name = "聯絡人1郵件2")]
            public string Contact1_Email2 { get; set; }            
            public string Contact1_DoSend { get; set; }

            [Display(Name = "聯絡人2姓名")]
            public string Contact2_Name { get; set; }
            [Display(Name = "聯絡人2稱謂")]
            public string Contact2_Title { get; set; }
            [Display(Name = "聯絡人2電話1")]
            public string Contact2_Tel1 { get; set; }
            [Display(Name = "聯絡人2電話2")]
            public string Contact2_Tel2 { get; set; }
            [Display(Name = "聯絡人2郵件1")]
            public string Contact2_Email1 { get; set; }
            [Display(Name = "聯絡人2郵件2")]
            public string Contact2_Email2 { get; set; }            
            public string Contact2_DoSend { get; set; }

            [Display(Name = "聯絡人3姓名")]
            public string Contact3_Name { get; set; }
            [Display(Name = "聯絡人3稱謂")]
            public string Contact3_Title { get; set; }
            [Display(Name = "聯絡人3電話1")]
            public string Contact3_Tel1 { get; set; }
            [Display(Name = "聯絡人3電話2")]
            public string Contact3_Tel2 { get; set; }
            [Display(Name = "聯絡人3郵件1")]
            public string Contact3_Email1 { get; set; }
            [Display(Name = "聯絡人3郵件2")]
            public string Contact3_Email2 { get; set; }            
            public string Contact3_DoSend { get; set; }

            [Display(Name = "最後更新者")]
            public string Modifier { get; set; }
            [Display(Name = "最後更新時間")]
            public Nullable<System.DateTime> ModifiedDT { get; set; }
        }
    }
}