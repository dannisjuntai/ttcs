using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCS.Areas.EmailSrv.Models
{
    public class CustomersView
    {
        public ECustomers Customers { get; set; }

        public EEmailDispatchRule EmailDispatchRule { get; set; }
    }
}