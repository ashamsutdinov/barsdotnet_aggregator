using System.Web.Optimization;

namespace UI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/js/extjs").Include("~/Scripts/extjs/ext-all-debug-w-comments.js"));

            bundles.Add(new StyleBundle("~/css/site").Include("~/Content/Site.css"));
        }
    }
}