using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace TTCS.Helpers
{
    public static class OptSelectListHelper
    {
        public static MvcHtmlString OptSelectList(this HtmlHelper html, string expression, List<SelectListItem> selectList, string optionLabel, bool disabled)
        {
            var result = "";//String.Format("<select id='{0}'>{1}</select>", expression, "{0}");
            foreach(var item in selectList)
            {
                string[] arrOption = item.Text.Split('|');
                var optionlist = "";
                foreach(var option in arrOption)
                {
                    string[] arrKeyValue = option.Split(':');
                    optionlist += String.Format("<option value='{0}'>{1}</option>", arrKeyValue[0], arrKeyValue[1]);
                }
                result += String.Format("<optgroup label='{0}'>{1}</optgroup>", item.Value, optionlist);
            }
            result = String.Format("<select id='{0}' name='{0}' {1}><option value=''>{2}</option>{3}</select>", expression, (disabled == true)?"disabled='disabled'":"", optionLabel,result);

            return MvcHtmlString.Create(result);
        }
    }
}