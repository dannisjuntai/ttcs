//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTCS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailRawHeader
    {
        public EmailRawHeader()
        {
            this.EmailRawBody = new HashSet<EmailRawBody>();
            this.EmailRecord = new HashSet<EmailRecord>();
        }
    
        public int Id { get; set; }
        public string MsgId { get; set; }
        public string MsgFrom { get; set; }
        public string MsgTo { get; set; }
        public string MsgCc { get; set; }
        public string MsgBcc { get; set; }
        public string MsgReplyTo { get; set; }
        public string MsgReceivedBy { get; set; }
        public string MsgInReplyTo { get; set; }
        public string MsgSubject { get; set; }
        public System.DateTime MsgSentOn { get; set; }
        public int Completed { get; set; }
    
        public virtual ICollection<EmailRawBody> EmailRawBody { get; set; }
        public virtual ICollection<EmailRecord> EmailRecord { get; set; }
    }
}
