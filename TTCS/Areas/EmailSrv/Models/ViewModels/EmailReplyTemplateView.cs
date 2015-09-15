using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;

namespace TTCS.Areas.EmailSrv.Models
{
    public class TemplateVariables
    {
        [Display(Name="客服人員變數")]
        public string AgentID { get; set; }
    }

    public class EmailReplyTemplate
    {
        public IEnumerable<EEmailReplyCan> EmailReplyCan { get; set; }
    }
}