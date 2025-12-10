using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;


namespace WebTIGA.Controllers
{
    public class WebTecController : Controller
    {
        
        MultiserviciosEntities db = new MultiserviciosEntities();
        ContenedorModelos modelDB = new ContenedorModelos();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        public ActionResult Reporte_Hitos()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["fecha"] = DateTime.Today.Year;
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.fecha = fecha;

            ContenedorModelos mymodel = new ContenedorModelos();

            mymodel.SP_TT_DIAS_CONTROL = db2.SP_TT_DIAS_CONTROL(fecha);
            mymodel.SP_TT_DETALLE_FILTRAR_PROYECTO = db2.SP_TT_DETALLE_FILTRAR_PROYECTO(fecha);
            return View(mymodel);  
        }
        [HttpPost]
        public ActionResult Reporte_Hitos(int? fecha,string Codigo )
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            ContenedorModelos mymodel = new ContenedorModelos();
            Session["fecha"] = fecha;
            int fecha1 = Convert.ToInt32(Session["fecha"]);
            
            ViewBag.fecha = fecha;
            if (string.IsNullOrEmpty(Codigo))
            {
                Codigo = "";
            }
            var query2 = from s in db2.SP_TT_DETALLE_FILTRAR_PROYECTO(fecha1) select s;
            var query3 = from s in db2.SP_TT_DETALLE_FILTRAR_PROYECTO(fecha1) select s;
            var query4 = from s in db2.SP_TT_DIAS_CONTROL(fecha1) select s;
            var query5 = from s in db2.SP_TT_DIAS_CONTROL(fecha1) select s;

            if (!string.IsNullOrEmpty(Codigo))
            {
                query2 = query3
                .Where(k => k.Codigo.Contains(Codigo));
                query4 = query5;

            }
            mymodel.SP_TT_DETALLE_FILTRAR_PROYECTO = query2.ToList();
            mymodel.SP_TT_DIAS_CONTROL = db2.SP_TT_DIAS_CONTROL(fecha);

            return View(mymodel);
            
        }

        
        public ActionResult Dashboard()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["start"] = DateTime.Now.ToString("2020-01-01");
            Session["end"] = DateTime.Now;
            DateTime start = Convert.ToDateTime(Session["start"]);
            DateTime end = Convert.ToDateTime(Session["end"]);
            
            ViewBag.start = start;
            ViewBag.end = end;

            modelDB.SP_TT_ERROR_FASES_DISTRIBUCION = db2.SP_TT_ERROR_FASES_DISTRIBUCION(start, end);
            ViewBag.cont= modelDB.SP_TT_ERROR_FASES_DISTRIBUCION.Count();
            modelDB.SP_GRAF_TABLA_HORAS2 = db2.SP_GRAF_TABLA_HORAS2(start, end);
            modelDB.SP_TBL_DETALLE_GENERAL_TEC = db2.SP_TBL_DETALLE_GENERAL_TEC(start, end);

            return View(modelDB);            
        }

        [HttpPost]
        public ActionResult Dashboard(DateTime? start, DateTime? end)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            try
            {
                if (start == null && end == null)
                {
                    Session["start"] = DateTime.Now.ToString("2020-01-01");
                    Session["end"] = DateTime.Now;
                }
                else
                {
                    Session["start"] = start;
                    Session["end"] = end;
                }

                start = Convert.ToDateTime(Session["start"]);
                end = Convert.ToDateTime(Session["end"]);

                ViewBag.start = start;
                ViewBag.end = end;

                modelDB.SP_TT_ERROR_FASES_DISTRIBUCION = db2.SP_TT_ERROR_FASES_DISTRIBUCION(start, end);
                ViewBag.cont = modelDB.SP_TT_ERROR_FASES_DISTRIBUCION.Count();

                modelDB.SP_GRAF_TABLA_HORAS2 = db2.SP_GRAF_TABLA_HORAS2(start, end);
                modelDB.SP_TBL_DETALLE_GENERAL_TEC = db2.SP_TBL_DETALLE_GENERAL_TEC(start, end);

                return View(modelDB);

            }
            catch
            {
                return RedirectToAction("Error", "WebTec");
            }

        }


        public ActionResult Detalle_General()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            DateTime start = Convert.ToDateTime(Session["start"]);
            DateTime end = Convert.ToDateTime(Session["end"]);

            ViewBag.start = start;
            ViewBag.end = end;

            modelDB.SP_TBL_DETALLE_GENERAL_TEC = db2.SP_TBL_DETALLE_GENERAL_TEC(start, end);
            return View(modelDB);
           
        }

        //hice un cambio para que pueda filtrar en detalle general
            [HttpPost]
            public ActionResult Detalle_General(string Auditor, string Equipo, DateTime? start, DateTime? end)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            try
            {
                ViewBag.start = start;
                ViewBag.end = end;

                if (string.IsNullOrEmpty(Auditor))
                {
                    Auditor = "";
                }
                if (string.IsNullOrEmpty(Equipo))
                {
                    Equipo = "";
                }
                ContenedorModelos mymodel = new ContenedorModelos();
                var query2 = from s in db2.SP_TBL_DETALLE_GENERAL_TEC(start, end) select s;
                var query3 = from s in db2.SP_TBL_DETALLE_GENERAL_TEC(start, end) select s;
                if (!string.IsNullOrEmpty(Auditor) || !string.IsNullOrEmpty(Equipo))
                {
                    query2 = query3
                    .Where(k => k.Nombre_Ape.Contains(Auditor))
                    .Where(k => k.Equipo.Contains(Equipo));
                }
                mymodel.SP_TBL_DETALLE_GENERAL_TEC = query2.ToList();
                return View(mymodel);
            }
            catch
            {
                return RedirectToAction("Error", "WebTec");
            }
        }

      


        public JsonResult JsonGRAFTiempos(string model)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            DateTime start = Convert.ToDateTime(Session["start"]);
            DateTime end = Convert.ToDateTime(Session["end"]);
            ViewBag.start = start;
            ViewBag.end = end;


            List<JsonGRAFTiempos> items2 = new List<JsonGRAFTiempos>();
            foreach (var item2 in (db.SP_GRAF_MEDIDA_DE_TIEMPO2(start, end)))
            {
                items2.Add(new JsonGRAFTiempos() {  Porcentaje = item2.Porcentaje });
            }
            return (Json(items2, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFTiemposTotales(string model)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            DateTime start = Convert.ToDateTime(Session["start"]);
            DateTime end = Convert.ToDateTime(Session["end"]);
            ViewBag.start = start;
            ViewBag.end = end;

            List<JsonGRAFTiempos> items2 = new List<JsonGRAFTiempos>();
            foreach (var item2 in (db.SP_GRAF_HORAS_TOTAL(start, end)))
            {
                items2.Add(new JsonGRAFTiempos() { Porcentaje = item2.Porcentaje });
            }
            return (Json(items2, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFDiasxFase(string model)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            int fecha = Convert.ToInt32(Session["fecha"]);
            List<SP_TT_DISTR_DIAS_AUDITOR_POR_FASE_Result> items = new List<SP_TT_DISTR_DIAS_AUDITOR_POR_FASE_Result>();
            foreach (var item in (db2.SP_TT_DISTR_DIAS_AUDITOR_POR_FASE(fecha)))
            {
                items.Add(new SP_TT_DISTR_DIAS_AUDITOR_POR_FASE_Result() { Equipo = item.Equipo , Otros = item.Otros,PLANIFICACIÓN=item.PLANIFICACIÓN,EJECUCIÓN=item.EJECUCIÓN,INFORME =item.INFORME,PLAN_ANUAL=item.PLAN_ANUAL,PAMC=item.PAMC });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFDiasxControlxEquipo(string model)
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            int fecha = Convert.ToInt32(Session["fecha"]);
            List<SP_TT_DISTR_DIAS_AUDITOR_POR_CONTROL_EQUIPO_Result> items = new List<SP_TT_DISTR_DIAS_AUDITOR_POR_CONTROL_EQUIPO_Result>();
            foreach (var item in (db2.SP_TT_DISTR_DIAS_AUDITOR_POR_CONTROL_EQUIPO(fecha)))
            {
                items.Add(new SP_TT_DISTR_DIAS_AUDITOR_POR_CONTROL_EQUIPO_Result() { Equipo = item.Equipo, Dias_Auditor = item.Dias_Auditor });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        /*se creo vistas parciales para detalles */
        public ActionResult Detalle_Proyecto_Y_Actividad()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            int fecha1 = Convert.ToInt32(Session["fecha"]);

            modelDB.SP_TT_DETALLE_PROYECTO_ACTIVIDAD = db2.SP_TT_DETALLE_PROYECTO_ACTIVIDAD(fecha1);
            return PartialView(modelDB);
            
        }
        public ActionResult Detalle_Etapas_Fases_Actividades()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            int fecha2 = Convert.ToInt32(Session["fecha"]);
            modelDB.SP_TT_DETALLE_ETAPAS_FASES_ACTIVIDADES = db2.SP_TT_DETALLE_ETAPAS_FASES_ACTIVIDADES(fecha2);
            return PartialView(modelDB);
        }
        public ActionResult Detalle_Equipo_Etapas()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            int fecha3 = Convert.ToInt32(Session["fecha"]);
            modelDB.SP_TT_EQUIPO_ETAPAS = db2.SP_TT_EQUIPO_ETAPAS(fecha3);
            return PartialView(modelDB);
        }

        public ActionResult Detalle_error_fases_distribucion()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            DateTime start = Convert.ToDateTime(Session["start"]);
            DateTime end = Convert.ToDateTime(Session["end"]);
            ViewBag.start = start;
            ViewBag.end = end;
          
            modelDB.SP_TT_ERROR_FASES_DISTRIBUCION = db2.SP_TT_ERROR_FASES_DISTRIBUCION(start,end);
            return PartialView(modelDB);
        }




    }
}