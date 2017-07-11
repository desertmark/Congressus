﻿using System.Web;
using System.Web.Optimization;

namespace Congressus.Web
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //APP Bundles
            bundles.Add(new ScriptBundle("~/bundles/app/becas").Include(
                        "~/Scripts/app/becas/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/app/charlas").Include(
                        "~/Scripts/app/charlas/*.js"));
            
            //Lib Bundles
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            //extensiones Mias
            bundles.Add(new ScriptBundle("~/bundles/jquery.extensions").Include(
                "~/Scripts/jquery.ajax.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUnobtrusive").Include(
                "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/bonnet").Include(
                      "~/Scripts/jquery.bonnet*"));
        }
    }
}
