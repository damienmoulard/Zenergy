using System.Web;
using System.Web.Optimization;

namespace Zenergy
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Lib/jquery-{version}.js"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            "~/Scripts/Lib/angular.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-resource").Include(
            "~/Scripts/Lib/angular-resource.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/zenergyApp")
            .Include("~/Scripts/zenergyApp.js")
            .IncludeDirectory("~/Scripts/Controllers", "*.js")
            .IncludeDirectory("~/Scripts/Factories", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Lib/bootstrap.js",
                      "~/Scripts/Lib/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
