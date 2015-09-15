using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models.Partials
{
    [MetadataType(typeof(ECustomerCategoryMetaData))]
    public partial class ECustomerCategory
    {
        private class ECustomerCategoryMetaData
        {
            [Display(Name = "編號 ")]
            public int ID { get; set; }

            [Display(Name = "敘述")]
            public string Desc { get; set; }
        }
    }
}