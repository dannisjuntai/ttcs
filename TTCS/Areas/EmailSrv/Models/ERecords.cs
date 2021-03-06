
//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------


namespace TTCS.Areas.EmailSrv.Models
{

using System;
    using System.Collections.Generic;
    
public partial class ERecords
{

    public ERecords()
    {

        this.EmailRecord = new HashSet<EEmailRecord>();

        this.RecordServiceMap = new HashSet<ERecordServiceMap>();

    }


    public string RID { get; set; }

    public Nullable<System.Guid> CustomerID { get; set; }

    public Nullable<int> TypeID { get; set; }

    public string AgentID { get; set; }

    public Nullable<int> StatusID { get; set; }

    public string Comment { get; set; }

    public Nullable<int> ServiceGroupID { get; set; }

    public Nullable<int> ServiceItemID { get; set; }

    public Nullable<System.DateTime> IncomingDT { get; set; }

    public Nullable<System.DateTime> FinishDT { get; set; }

    public Nullable<int> CloseApproachID { get; set; }

    public string GroupID { get; set; }

    public string Uniqueid { get; set; }

    public string QueueName { get; set; }



    public virtual ECustomers Customers { get; set; }

    public virtual ERecordStatus RecordStatus { get; set; }

    public virtual ERecordType RecordType { get; set; }

    public virtual EServiceItem ServiceItem { get; set; }

    public virtual ICollection<EEmailRecord> EmailRecord { get; set; }

    public virtual EAgent EAgent { get; set; }

    public virtual ICollection<ERecordServiceMap> RecordServiceMap { get; set; }

    public virtual EPhoneRecord PhoneRecord { get; set; }

    public virtual ERecordCloseApproach RecordCloseApproach { get; set; }

    public virtual EMailGroup MailGroup { get; set; }

}

}
