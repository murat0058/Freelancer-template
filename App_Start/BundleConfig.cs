using System.Web;
using System.Web.Optimization;

namespace HafifCMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/freelancer.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/plugins/metisMenu/metisMenu.min.css",
                      "~/Content/sb-admin-2.css",
                      "~/fonts/font-awesome-4.1.0/css/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/js/plugins/metisMenu/metisMenu.min.js",
                      "~/js/sb-admin-2.js"));
        
        }
    }
}
