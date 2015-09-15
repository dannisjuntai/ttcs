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
    
    public partial class Records
    {
        public Records()
        {
            this.EmailRecord = new HashSet<EmailRecord>();
            this.RecordServiceMap = new HashSet<RecordServiceMap>();
        }
    
        public string RID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string AgentID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ServiceGroupID { get; set; }
        public Nullable<int> ServiceItemID { get; set; }
        public Nullable<int> CloseApproachID { get; set; }
        public string GroupID { get; set; }
        public Nullable<System.DateTime> IncomingDT { get; set; }
        public Nullable<System.DateTime> FinishDT { get; set; }
        public string Uniqueid { get; set; }
        public string QueueName { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Customers Customers { get; set; }
        public virtual ICollection<EmailRecord> EmailRecord { get; set; }
        public virtual MailGroup MailGroup { get; set; }
        public virtual PhoneRecord PhoneRecord { get; set; }
        public virtual RecordCloseApproach RecordCloseApproach { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual RecordType RecordType { get; set; }
        public virtual ServiceItem ServiceItem { get; set; }
        public virtual ICollection<RecordServiceMap> RecordServiceMap { get; set; }
    }
}