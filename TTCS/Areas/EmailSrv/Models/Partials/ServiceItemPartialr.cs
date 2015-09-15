using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EServiceItemMetaData))]
    public partial class EServiceItem
    {
        public int Checked { get; set; }
        public string GroupName { get; set; }
        private class EServiceItemMetaData
        {
            public int GroupID { get; set; }
            public int ItemID { get; set; }
            public string ItemDesc { get; set; }
            public Nullable<int> DispNo { get; set; }        
        }
    }
}
