using System.Web.Optimization;

namespace SG2.CORE.WEB.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            var fontAwsomCdn = "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css";
            bundles.Add(new StyleBundle("~/socialgrowthengine/css", fontAwsomCdn).Include(
                      "~/socialgrowthengine/assets/bootstrap7d04.css",
                      "~/socialgrowthengine/assets/kk-style7d04.css",
                      "~/socialgrowthengine/assets/slick7d04.css",
                      "~/socialgrowthengine/assets/home.css",
                      "~/socialgrowthengine/assets/flickity.min.css"));

            
            bundles.Add(new ScriptBundle("~/scripts/js").Include(
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js",
                      "~/Scripts/Intercom1.1.js"
                      )
                     );
            bundles.Add(new ScriptBundle("~/socialgrowthengine/js").Include(
                      "~/socialgrowthengine/assets/jquery.min7d04.js",
                      "~/socialgrowthengine/assets/bootstrap.min7d04.js",
                      "~/socialgrowthengine/assets/flickity.pkgd.min.js",
                      "~/socialgrowthengine/assets/slick.min7d04.js",
                      "~/socialgrowthengine/assets/custom-scripts7d04.js",
                      "~/socialgrowthengine/assets/custom-script.js"
                      )
                     );

            BundleTable.EnableOptimizations = true;
        }
    }
}
