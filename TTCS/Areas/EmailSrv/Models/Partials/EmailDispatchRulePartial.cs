using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailDispatchRuleMetaData))]
    public partial class EEmailDispatchRule
    {   
        [Display(Name = "分派條件")]
        public string DispatchCondition { get; set; }

        private class EEmailDispatchRuleMetaData
        {
            public int Id { get; set; }
            
            [Display(Name = "修改時間")]
            public Nullable<System.DateTime> ModifyOn { get; set; }

            [Display(Name = "修改人員")]
            public string ModifyBy { get; set; }
            
            [Display(Name = "客戶")]
            public Nullable<System.Guid> CustomerId { get; set; }
                        
            [Display(Name = "客服順位1")]
            [Required(ErrorMessage = "請選擇客服順位1")]
            public string AgentId1 { get; set; }

            [Display(Name = "客服順位2")]
            public string AgentId2 { get; set; }

            [Display(Name = "客服順位3")]
            public string AgentId3 { get; set; }

            [Display(Name = "分派依據")]
            public Nullable<int> ConditionType { get; set; }

            public string Condition { get; set; }

            [Display(Name = "全部客服順位未值機, 是否啟用自動派送")]
            public bool AutoDispatch { get; set; }
    
        }
    }
}