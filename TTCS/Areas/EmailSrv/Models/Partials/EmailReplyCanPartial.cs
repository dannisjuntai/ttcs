using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailReplyCanMetaData))]
    public partial class EEmailReplyCan
    {
        public string TmpContent { get; set; }

        private class EEmailReplyCanMetaData
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "請輸入範本名稱")]
            [Remote("_VerifyReplyTemplateName", "EmailReplyTemplate", AdditionalFields="Id",ErrorMessage="系統已存在相同名稱範本")]
            [Display(Name = "範本名稱")]
            public string Name { get; set; }

            [Display(Name = "範本內容")]
            public byte[] TempCnt { get; set; }

            [Display(Name = "啟用")]
            public bool Active { get; set; }            
            
        }
    }
}