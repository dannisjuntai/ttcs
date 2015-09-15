using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailScheduleSettingMetaData))]
    public partial class EEmailScheduleSetting
    {
        [Display(Name = "　　　　派送週期")]
        [Required(ErrorMessage = "請輸入數字")]
        [Range(1, 60, ErrorMessage="必需介於1和60")]
        public int Period { get; set; }
                
        public int Frequency { get; set; }

        private class EEmailScheduleSettingMetaData
        {
            public int Id { get; set; }

            public string Name { get; set; }
            public string Value { get; set; }
                        
            [Display(Name = "　　　　　　啟用")]
            public bool Active { get; set; }
        }
    }
}