
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
    
public partial class EEmailSavedBody
{

    public EEmailSavedBody()
    {

        this.EmailRecord = new HashSet<EEmailRecord>();

    }


    public string Id { get; set; }

    public string FileName { get; set; }

    public Nullable<int> CodePage { get; set; }

    public byte[] BodyCnt { get; set; }

    public string SendTo { get; set; }

    public string Cc { get; set; }

    public string Bcc { get; set; }

    public Nullable<int> MailSignature { get; set; }



    public virtual ICollection<EEmailRecord> EmailRecord { get; set; }

}

}
