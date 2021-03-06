
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
    
public partial class EEmailRecord
{

    public string RID { get; set; }

    public Nullable<int> RawHeaderId { get; set; }

    public Nullable<System.DateTime> ExpireOn { get; set; }

    public Nullable<System.DateTime> ReplyOn { get; set; }

    public Nullable<System.DateTime> IncomeOn { get; set; }

    public Nullable<System.DateTime> AssignOn { get; set; }

    public string InitAgentId { get; set; }

    public string CurrAgentId { get; set; }

    public Nullable<int> AssignCount { get; set; }

    public Nullable<int> Garbage { get; set; }

    public int Id { get; set; }

    public string RejectReason { get; set; }

    public string ProcessStatus { get; set; }

    public Nullable<int> OrderNo { get; set; }

    public string SaveCntIn { get; set; }

    public Nullable<int> ForwardCount { get; set; }

    public Nullable<int> UnRead { get; set; }

    public string MsgSubjectModifiedBy { get; set; }



    public virtual EAgent Agent { get; set; }

    public virtual EAgent Agent1 { get; set; }

    public virtual EEmailRawHeader EmailRawHeader { get; set; }

    public virtual EEmailSavedBody EmailSavedBody { get; set; }

    public virtual ERecords Records { get; set; }

}

}
