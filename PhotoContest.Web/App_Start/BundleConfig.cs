using System.Web;
using System.Web.Optimization;

namespace PhotoContest.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/additional").Include(
                        "~/Scripts/additional.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.datetimepicker").Include(
                        "~/Scripts/jquery.datetimepicker.full*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                      "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                       "~/Content/jquery.datetimepicker.css",
                       "~/Content/jquery-ui.css",
                       "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/Images/css").Include(
            "~/Content/Images/PagedImage.css").Include(
            "~/Content/Images/UploadSection.css"));

            bundles.Add(new StyleBundle("~/Content/ViewImage/css").Include(
                      "~/Content/Images/ViewImage.css"));
        }
    }
}
