using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TTCS.Models;

namespace TTCS.App_Start
{
    public class Helper
    {
        static public dynamic GetAjaxRet(int result_code, string message)
        {
            dynamic ret = new System.Dynamic.ExpandoObject();
            ret.result = result_code;
            ret.message = message;
            return ret;
        }

        static public string FetchExceptionMessage(Exception ex)
        {
            string msg = ex.Message;
            Exception curr_ex = ex;

            while (curr_ex.InnerException != null)
            {
                curr_ex = curr_ex.InnerException;
                msg += "\n" + curr_ex.Message;
            }
            return msg;
        }
    }
}