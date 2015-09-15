using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace TTCS.Helpers
{
    public static class Ajax
    {
        static public dynamic GetAjaxRet(int result_code, string message)
        {
            dynamic ret = new System.Dynamic.ExpandoObject();
            ret.result = result_code;
            ret.message = message;
            return ret;
        }
    }


    public static class Error
    {
        static public string GetModelCheckString(ICollection<ModelState> ModelStateValues)
        {
            string msg = "";
            foreach (ModelState modelState in ModelStateValues)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    msg += error.ErrorMessage + "\n";
                }
            }
            return msg;
        }


        static public string ValidationExceptionToString(DbEntityValidationException ex)
        {
            string msg = "";
            foreach (DbEntityValidationResult vr in ex.EntityValidationErrors)
            {
                foreach (DbValidationError ve in vr.ValidationErrors)
                {
                    msg += String.Format("'{0}': {1}\n", ve.PropertyName, ve.ErrorMessage);
                }
            }

            return msg;
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