using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Models
{
    [MetadataType(typeof(EmailReplyCanMetaData))]
    public partial class EmailReplyCan
    {
        public string TmpContent { get; set; }

        private class EmailReplyCanMetaData
        {
            public int Id { get; set; }

            [Display(Name = "範本名稱")]
            public string Name { get; set; }

            [Display(Name = "範本內容")]
            public byte[] TempCnt { get; set; }

            [Display(Name = "啟用")]
            public bool Active { get; set; }            
            
        }
    }
}