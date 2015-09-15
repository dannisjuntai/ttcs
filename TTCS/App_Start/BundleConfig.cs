using System.Web;
using System.Web.Optimization;

namespace TTCS
{
    public class BundleConfig
    {
        // 如需 Bundling 的詳細資訊，請造訪 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                        "~/Scripts/jquery.dynatree.js",
                        "~/Scripts/jquery.cleditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            /*
            bundles.Add(new ScriptBundle("~/bundles/TTCS").Include(
                        "~/Scripts/TTCS_Init.js",
                        "~/Scripts/TTCS_Login.js",
                        "~/Scripts/TTCS_RecordList.js",
                        "~/Scripts/TTCS_Record.js",
                        "~/Scripts/TTCS_Customer.js",
                        "~/Scripts/TTCS_TelphoneRecord.js",
                        "~/Scripts/TTCS_EmailRecord.js",
                        "~/Scripts/TTCS_Utils.js",
                        "~/Scripts/TTCS_API.js"));
             */

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/main_themes").Include(
                        //"~/Content/yuicssreset.css",
                        "~/Content/main_themes.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/dynatree-skin/ui.dynatree.css",
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/jquery.cleditor.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
             "~/Scripts/jquery-file-upload-jquery-ui/js/vendor/jquery.ui.widget.js",
             "~/Scripts/jquery-file-upload-jquery-ui/js/jquery.iframe-transport.js",
             "~/Scripts/jquery-file-upload-jquery-ui/js/jquery.fileupload.js"
            )); 

        }
    }
}