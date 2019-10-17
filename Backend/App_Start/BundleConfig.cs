using System.Web;
using System.Web.Optimization;

namespace Backend
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/popper.min.js",
                        "~/Scripts/bootstrap.bundle.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/hfplay").Include(
                      "~/Scripts/respond.js",
                      "~/Scripts/hfplay.js",
                      "~/Scripts/Menu.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/cards.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cssbootstrap").Include(
                        "~/Content/bootstrap.css"));

        }
    }
}
