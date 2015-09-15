using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TTCS.Areas.EmailSrv.Models
{
    public class EmailAdmin_SearchCondition
    {
        public string type { get; set; }

        public string condition { get; set; }

        public string Filter_Record_TypeID { get; set; }       // 案件類型

        public string Filter_Record_StatusID { get; set; }     // 案件狀態

        public static string Serialize(EmailAdmin_SearchCondition xyz)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(xyz);
        }

        public static EmailAdmin_SearchCondition Deserialize(string data)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<EmailAdmin_SearchCondition>(data);
        }

    }
}