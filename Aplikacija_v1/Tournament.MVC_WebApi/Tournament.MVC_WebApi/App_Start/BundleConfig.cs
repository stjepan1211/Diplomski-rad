using System.Web;
using System.Web.Optimization;

namespace Tournament.MVC_WebApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/angular-datepicker.css"));

            bundles.Add(new ScriptBundle("~/app/js").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-ui-router.js",
                    "~/app/js/md5.js",
                    "~/Scripts/loading-bar.js",
                    "~/app/js/ngStorage.js",
                    "~/Scripts/angular-local-storage.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-messages.js",
                    "~/app/js/ngDatepicker.js",
                    "~/Scripts/lodash.js",
                    "~/app/js/angular-simple-loger.js",
                    "~/app/js/angular-google-maps.js"
                ));

            bundles.Add(new ScriptBundle("~/app").Include(
                    "~/app/app.js",
                    "~/app/services/AuthenticationService.js",
                    "~/app/controllers/*Controller.js",
                    "~/app/directives/*Directive.js"
                ));
        }
    }
}
