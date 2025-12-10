using System.Web;
using System.Web.Optimization;

namespace TeamTEC
{
    public class BundleConfig
    {

       

        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //,"~/Content/vendor/datatables/dataTables.bootstrap4.min.css"



            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/css/sb-admin-2.min.css",
                     "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/css/agency.min.css"
                      ));



            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/vendor/jquery/jquery.min.js",
                "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                "~/Content/js/sb-admin-2.min.js",
                //"~/Content/vendor/chart.js/Chart.min.js",
                //"~/Content/js/demo/chart-area-demo.js",
                //"~/Content/js/demo/chart-pie-demo.js",
                "~/Content/js/jqBootstrapValidation.js", "~/Content/js/contact_me.js", "~/Content/js/agency.min.js", "~/Content/js/main.js", "~/Content/js/smtp.js"

                 ));

            


        }
    }
}
