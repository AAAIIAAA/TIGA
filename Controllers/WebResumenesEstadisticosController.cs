using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;

namespace WebTIGA.Controllers
{
    public class WebResumenesEstadisticosController : Controller
    {
        SesionData session = new SesionData();
        MultiserviciosEntities1 db = new MultiserviciosEntities1();
        ContenedorModelos modelDB = new ContenedorModelos();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        // GET: WebResumenesEstadisticos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Principal()
        {
            return View();
        }
        public ActionResult Dashboard_Seguimiento()
        {
            Session["año"] = DateTime.Today.Year;
            Session["mes"] = DateTime.Today.Month;
            int año = Convert.ToInt32(Session["año"]);
            int mes = Convert.ToInt32(Session["mes"]);
            ViewBag.año = año;
            ViewBag.mes = mes;

            Session["auditor"] = "Cindy";
            string aud = Convert.ToString(Session["auditor"]);
            Session["equipo"] = "Proceso de TI";
            string equ = Convert.ToString(Session["equipo"]);
            ViewBag.equipo = equ;
            ViewBag.auditor = aud;

            var PersonaVista = from c in db2.Persona where c.Activo == 1 select new { c.IdPersona, Nombre_Ape = c.Nombres + " " + c.Apellidos };

            ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape");
            modelDB.VIEW_WT_USUARIOS = db2.VIEW_WT_USUARIOS;
            modelDB.SP_RE_EVOLUTIVO_VENCIDAS2 = db2.SP_RE_EVOLUTIVO_VENCIDAS2(año, mes);
            modelDB.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE = db2.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE(aud,"");
            modelDB.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE = db2.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE(año,mes,equ);
            modelDB.SP_RE_EVOLUTIVO_TOP = db2.SP_RE_EVOLUTIVO_TOP(año, mes);
            return View(modelDB);
        }
        [HttpPost]
        public ActionResult Dashboard_Seguimiento(int? año,int? mes,string auditor , string equipo)
        {
            if (año == null || mes == null)
            {
                año = DateTime.Today.Year;
                mes = DateTime.Today.Month;

            }
            if (auditor == null)
            {
                auditor = "Cindy Rengifo";
               

            } if (equipo == null) {
                equipo = "Proceso de TI";
            }

           

            Session["año"] = año;
            int anio = Convert.ToInt32(Session["año"]);
            Session["mes"] = mes;
            int mess = Convert.ToInt32(Session["mes"]);
            ViewBag.año = año;
            ViewBag.mes = mes;

            Session["auditor"] = auditor;
            string aud = Convert.ToString(Session["auditor"]);
            Session["equipo"] = equipo;
            string equ = Convert.ToString(Session["equipo"]);
            ViewBag.equipo = equ;
            ViewBag.auditor = aud;
            modelDB.VIEW_WT_USUARIOS = db2.VIEW_WT_USUARIOS;
            modelDB.SP_RE_EVOLUTIVO_VENCIDAS2 = db2.SP_RE_EVOLUTIVO_VENCIDAS2(anio, mess);
            modelDB.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE = db2.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE(aud, "");
            modelDB.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE = db2.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE(año, mes, equ);
            modelDB.SP_RE_EVOLUTIVO_TOP = db2.SP_RE_EVOLUTIVO_TOP(año, mes);
            return View(modelDB);
        }


        public JsonResult JsonGRAF_Evolutivo_Vencidas()
        {
            int año = Convert.ToInt32(Session["año"]);
            int mes = Convert.ToInt32(Session["mes"]);

            List<SP_RE_EVOLUTIVO_VENCIDAS2_Result> items = new List<SP_RE_EVOLUTIVO_VENCIDAS2_Result>();
            foreach (var item in (db2.SP_RE_EVOLUTIVO_VENCIDAS2(año,mes)))
            {
                items.Add(new SP_RE_EVOLUTIVO_VENCIDAS2_Result()
                {
                    Mes = item.Mes,
                    Fecha =item.Fecha,
                    EnFecha=item.EnFecha,
                    Vencido=item.Vencido
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAF_Evolutivo_Vencidas_Auditor()
        {
            
            string aud = Convert.ToString(Session["auditor"]);
            List<SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE_Result> items = new List<SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE_Result>();
            foreach (var item in (db2.SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE(aud, "")))
            {
                items.Add(new SP_RE_EVOLUTIVO_AUDIT_VENCIDAS_BASE_Result()
                {
                    Mes = item.Mes,
                    Fecha = item.Fecha,
                    EnFecha = item.EnFecha,
                    Vencido = item.Vencido
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAF_Evolutivo_Vencidas_Equipo()
        {

            int año = Convert.ToInt32(Session["año"]);
            int mes = Convert.ToInt32(Session["mes"]);

            string equ = Convert.ToString(Session["equipo"]);
            List<SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE_Result> items = new List<SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE_Result>();
            foreach (var item in (db2.SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE(año,mes,equ)))
            {
                items.Add(new SP_RE_EVOLUTIVO_VENCIDAS_EQUIPO_BASE_Result()
                {
                    Mes = item.Mes,
                    Fecha = item.Fecha,
                    EnFecha = item.EnFecha,
                    Vencido = item.Vencido
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
    }
}