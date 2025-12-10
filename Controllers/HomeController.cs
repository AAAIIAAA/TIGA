using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Autorizacion;
using WebTIGA.Models;

namespace WebTIGA.Controllers
{
    [Logueado]
    public class HomeController : Controller
    {
        
        ContenedorModelos modelDB = new ContenedorModelos();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        RecoveryViewModel model = new RecoveryViewModel();

        SesionData sesion = new SesionData();

        public ActionResult Index()
        {

            var usuario = Convert.ToInt32(Session["IdUser"]);
    
            var nombre = Convert.ToString(Session["usuario"]);

            ViewBag.Bienvenido = "Bienvenid@";
          
            ViewBag.nombre = nombre;
            modelDB.SP_MODULOS_USUARIOS_Result = db2.SP_MODULOS_USUARIOS(usuario);

            var usuarioN = from a in db2.Persona
                           where a.IdPersona == usuario
                           select a;
            modelDB.Persona = usuarioN.ToList();
            return View(modelDB);
        }
        public ActionResult CerrarSesion()
        {
            sesion.destroySession();
            return RedirectToAction("Login_", "Login");
        }

    }
}