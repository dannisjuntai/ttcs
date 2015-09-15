using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(ERecordCloseApproachMetaData))]
    public partial class ERecordCloseApproach
    {
        private class ERecordCloseApproachMetaData
        {
            public int ApproachID { get; set; }

            [Display(Name = "結案處理方式")]
            [Required(ErrorMessage = "請輸入處理方式")]
            public string ApproachName { get; set; }
        }
    }
}