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
    
public partial class ECustomers
{

    public ECustomers()
    {

        this.EmailDispatchRule = new HashSet<EEmailDispatchRule>();

        this.Records = new HashSet<ERecords>();

    }


    public System.Guid ID { get; set; }

    public string CName { get; set; }

    public string EName { get; set; }

    public Nullable<int> Category { get; set; }

    public string ID2 { get; set; }

    public string Region { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Comment { get; set; }

    public string Marketer { get; set; }

    public string AgentID1 { get; set; }

    public string AgentID2 { get; set; }

    public string Contact1_Name { get; set; }

    public string Contact1_Title { get; set; }

    public string Contact1_Tel1 { get; set; }

    public string Contact1_Tel2 { get; set; }

    public string Contact1_Email1 { get; set; }

    public string Contact1_Email2 { get; set; }

    public string Contact1_DoSend { get; set; }

    public string Contact2_Name { get; set; }

    public string Contact2_Title { get; set; }

    public string Contact2_Tel1 { get; set; }

    public string Contact2_Tel2 { get; set; }

    public string Contact2_Email1 { get; set; }

    public string Contact2_Email2 { get; set; }

    public string Contact2_DoSend { get; set; }

    public string Contact3_Name { get; set; }

    public string Contact3_Title { get; set; }

    public string Contact3_Tel1 { get; set; }

    public string Contact3_Tel2 { get; set; }

    public string Contact3_Email1 { get; set; }

    public string Contact3_Email2 { get; set; }

    public string Contact3_DoSend { get; set; }

    public string Modifier { get; set; }

    public Nullable<System.DateTime> ModifiedDT { get; set; }

    public string GroupID { get; set; }



    public virtual ICollection<EEmailDispatchRule> EmailDispatchRule { get; set; }

    public virtual ICollection<ERecords> Records { get; set; }

    public virtual ECustomerCategory CustomerCategory { get; set; }

}

}
