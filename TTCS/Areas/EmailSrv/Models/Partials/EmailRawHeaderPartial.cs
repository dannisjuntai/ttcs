using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailRawHeaderMetaData))]
    public partial class EEmailRawHeader
    {
        [Display(Name = "回覆內容")]
        public string MsgReplyBody { get; set; }
                
        private class EEmailRawHeaderMetaData
        {
            public int Id { get; set; }
            public string MsgId { get; set; }

            [AllowHtml]
            [Display(Name = "寄件者")]
            public string MsgFrom { get; set; }
            public string MsgCc { get; set; }
            public string MsgBcc { get; set; }
            public string MsgReplyTo { get; set; }
            public string MsgReceivedBy { get; set; }
            public string MsgInReplyTo { get; set; }

            [Display(Name = "寄件主旨")]
            public string MsgSubject { get; set; }

            [Display(Name = "寄件日期")]
            public System.DateTime MsgSentOn { get; set; }
            public int Completed { get; set; }
        }
    }
}