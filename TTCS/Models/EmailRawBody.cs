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
    
    public partial class EmailRawBody
    {
        public int Id { get; set; }
        public int RawHeaderId { get; set; }
        public string PlaceHolder { get; set; }
        public string CntMedia { get; set; }
        public int CodePage { get; set; }
        public string FileName { get; set; }
        public byte[] BinaryCnt { get; set; }
    
        public virtual EmailRawHeader EmailRawHeader { get; set; }
    }
}
