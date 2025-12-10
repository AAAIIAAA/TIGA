using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;

namespace WebTIGA.Controllers
{
    public class WebResultadoAuditoriaController : Controller
    {
        MultiserviciosEntities1 db = new MultiserviciosEntities1();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos modelDB = new ContenedorModelos();
        // GET: WebResultadoAuditoria
        public ActionResult Index()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            return View();
        }
        [HttpGet]
        public ActionResult Dashboard_WRA()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["negocio"] = "Negocio de Seguros";
            Session["gerencia"] = "%%";
            Session["fecha"] = DateTime.Today.Year;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;
            Session["date"] =  DateTime.Today;

            DateTime date = Convert.ToDateTime(Session["date"]);

            var query = (from u in db2.TS_Estructura
                        where u.GERENCIA_NIVEL_1 != " "
                        select  u.GERENCIA_NIVEL_1).Distinct();

            var datos = query.ToList();
            var query2 = (from u in db2.VIEW_NEGOCIOS
                         select u.Neg).Distinct();

            var datos2 = query2.ToList();
            ViewBag.GERENCIA = new SelectList(datos,"GERENCIA NIVEL 1");
            ViewBag.NEGOCIO = new SelectList(datos2, "Neg");

            modelDB.SP_CONTROLES_EVALUADOS = db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio, gerencia, date);
            modelDB.SP_CONTROLES_EVALUADOS = db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio, gerencia, DateTime.Now.AddYears(-1));
            modelDB.SP_WT_RESULTADO_AUDITORIA = db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia,date);
            modelDB.SP_WT_RESULTADO_AUDITORIA = db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia, DateTime.Now.AddYears(-1));
            modelDB.SP_WT_SEGUIMIENTO_OBSERVACIONES = db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES(negocio, gerencia);
            modelDB.SP_WT_SEGUIMIENTO_OBSERVACIONES_2 = db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES_2(negocio, gerencia);
            return View(modelDB);
        }
        [HttpPost]
        public ActionResult Dashboard_WRA(string NEGOCIO,string GERENCIA)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["negocio"] = NEGOCIO;
            Session["gerencia"] = GERENCIA;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            Session["fecha"] = DateTime.Today.Year; 
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.fecha = fecha;
            Session["date"] = DateTime.Today;
            DateTime date = Convert.ToDateTime(Session["date"]);
            var query = (from u in db2.TS_Estructura
                         where u.GERENCIA_NIVEL_1 != " "
                         select u.GERENCIA_NIVEL_1).Distinct();

            var datos = query.ToList();
            var query2 = (from u in db2.VIEW_NEGOCIOS
                          select u.Neg).Distinct();

            var datos2 = query2.ToList();
            ViewBag.GERENCIA = new SelectList(datos, "GERENCIA NIVEL 1");
            ViewBag.NEGOCIO = new SelectList(datos2, "Neg");
            modelDB.SP_CONTROLES_EVALUADOS = db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio, gerencia, date);
            modelDB.SP_CONTROLES_EVALUADOS = db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio, gerencia, DateTime.Now.AddYears(-1));
            modelDB.SP_WT_RESULTADO_AUDITORIA = db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia, date);
            modelDB.SP_WT_RESULTADO_AUDITORIA = db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia, DateTime.Now.AddYears(-1));
            modelDB.SP_WT_SEGUIMIENTO_OBSERVACIONES = db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES(negocio, gerencia);
            modelDB.SP_WT_SEGUIMIENTO_OBSERVACIONES_2 = db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES_2(negocio, gerencia);
            return View(modelDB);
        }



        public ActionResult Reportes()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            modelDB.SP_WT_REPORTES = db2.SP_WT_REPORTES();

            return View(modelDB);
        }
        public ActionResult Reporte_1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            //modelDB.SP_WT_REPORTE_GENERAL = db2.SP_WT_REPORTE_GENERAL();


            return View(modelDB);
        }
        public ActionResult Reporte_2()
        {


            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            modelDB.SP_WT_REPORTE_CLAVE1 = db2.SP_WT_REPORTE_CLAVE();
            return View(modelDB);
        }
        public ActionResult Reporte_3()
        {

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            modelDB.SP_WT_REPORTE_SOX = db.SP_WT_REPORTE_SOX();

            return View(modelDB);
        }

        public JsonResult JsonGRAFControles_Evaluados1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            DateTime date = Convert.ToDateTime(Session["date"]);
            List<SP_CONTROLES_EVALUADOS_Result1> items = new List<SP_CONTROLES_EVALUADOS_Result1>();
            foreach (var item in (db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio,gerencia,date)))
            {
                items.Add(new SP_CONTROLES_EVALUADOS_Result1() { Efectividad = item.Efectividad, Total = item.Total });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAFControles_Evaluados2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            DateTime date = Convert.ToDateTime(Session["date"]);
            List<SP_CONTROLES_EVALUADOS_Result1> items = new List<SP_CONTROLES_EVALUADOS_Result1>();
            foreach (var item in (db2.SP_WT_RA_CONTROLES_EVALUADOS(negocio, gerencia, DateTime.Now.AddYears(-1))))
            {
                items.Add(new SP_CONTROLES_EVALUADOS_Result1() { Efectividad = item.Efectividad, Total = item.Total });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFResultado_Auditoria1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            DateTime date = Convert.ToDateTime(Session["date"]);
            List<SP_WT_RESULTADO_AUDITORIA_Result> items = new List<SP_WT_RESULTADO_AUDITORIA_Result>();
            foreach (var item in (db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia, date)))
            {
                items.Add(new SP_WT_RESULTADO_AUDITORIA_Result() { Calificativo = item.Calificativo, Total = item.Total });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAFResultado_Auditoria2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            DateTime date = Convert.ToDateTime(Session["date"]);
            List<SP_WT_RESULTADO_AUDITORIA_Result> items = new List<SP_WT_RESULTADO_AUDITORIA_Result>();
            foreach (var item in (db2.SP_WT_RA_RESULTADO_AUDITORIA(negocio, gerencia, DateTime.Now.AddYears(-1))))
            {
                items.Add(new SP_WT_RESULTADO_AUDITORIA_Result() { Calificativo = item.Calificativo, Total = item.Total });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFSeguimiento_Observaciones1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;

            List<SP_WT_SEGUIMIENTO_OBSERVACIONES_Result> items = new List<SP_WT_SEGUIMIENTO_OBSERVACIONES_Result>();
            foreach (var item in (db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES(negocio, gerencia)))
            {
                items.Add(new SP_WT_SEGUIMIENTO_OBSERVACIONES_Result() { Color = item.Color, Total = item.Total ,Descripcion=item.Descripcion});
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAFSeguimiento_Observaciones2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;

            List<SP_WT_SEGUIMIENTO_OBSERVACIONES_2_Result> items = new List<SP_WT_SEGUIMIENTO_OBSERVACIONES_2_Result>();
            foreach (var item in (db2.SP_WT_RA_SEGUIMIENTO_OBSERVACIONES_2(negocio, gerencia)))
            {
                items.Add(new SP_WT_SEGUIMIENTO_OBSERVACIONES_2_Result() { GERENCIA_NIVEL_2 = item.GERENCIA_NIVEL_2, Auditoría_Externa = item.Auditoría_Externa,
                    Auditoría_Interna = item.Auditoría_Interna, SBS = item.SBS, SUSALUD = item.SUSALUD,Banmedica = item.Banmedica
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

       
        public ActionResult Detalle_Resultados_Auditoria()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            Session["negocio"] = "Negocio de Seguros";
            Session["gerencia"] = "%%";
            Session["fecha"] = DateTime.Today.Year;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);

            //Session["date"] = DateTime.Now.ToString("2020-09-11");
            Session["date"] = DateTime.Today;
            DateTime date = Convert.ToDateTime(Session["date"]);

            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;

            modelDB.SP_WT_DETALLE_RESULTADOS_AUDITORIA_1 = db2.SP_WT_RA_DETALLE_RESULTADOS_AUDITORIA_1(negocio, gerencia, date);
            modelDB.SP_WT_DETALLE_RESULTADOS_AUDITORIA_2 = db2.SP_WT_RA_DETALLE_RESULTADOS_AUDITORIA_2(negocio, gerencia, date);
            return View(modelDB);
        }

       
        public ActionResult Detalle_Seguimiento_Observaciones()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["negocio"] = "Negocio de Seguros";
            Session["gerencia"] = "%%";
            Session["fecha"] = DateTime.Today.Year;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);

            DateTime date = DateTime.Today;
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_1 = db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_1(negocio, gerencia,date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_2 = db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_2(negocio, gerencia, date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2 = db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2(negocio, gerencia,date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO= db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO(negocio, gerencia, date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISION = db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISION(negocio, gerencia, date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_CRITICIDAD = db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_CRITICIDAD(negocio, gerencia, date);
            modelDB.SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2=db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2(negocio, gerencia, date);
            return View(modelDB);
        }
        public ActionResult Detalle_Controles_Evaluados()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            Session["negocio"] = "Negocio de Seguros";
            Session["gerencia"] = "%%";
            Session["fecha"] = DateTime.Today.Year;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;

            //Session["date"] = DateTime.Now.ToString("2020-09-11");
            Session["date"] = DateTime.Today;
            DateTime date = Convert.ToDateTime(Session["date"]);

            modelDB.SP_DETALLE_CONTROLES_EVALUADOS_1 = db2.SP_WT_RA_DETALLE_CONTROLES_EVALUADOS_1(negocio, gerencia, date);
            modelDB.SP_DETALLE_CONTROLES_EVALUADOS_2 = db2.SP_WT_RA_DETALLE_CONTROLES_EVALUADOS_2(negocio, gerencia, date);
            modelDB.SP_DETALLE_CONTROLES_EVALUADOS_3 = db2.SP_WT_RA_DETALLE_CONTROLES_EVALUADOS_3(negocio, gerencia, date);
            return View(modelDB);
        }

        public JsonResult JsonGRAFSeguimiento_Observaciones_Detalle_s_a_e_2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            DateTime fecha = DateTime.Today;

            List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2_Result> items = new List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2(negocio, gerencia,fecha)))
            {
                items.Add(new SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_STOCK_ANTIGUEDAD_EMISOR_2_Result()
                {
                    GERENCIA_1 = item.GERENCIA_1,
                    En_Fecha = item.En_Fecha,
                    Vencido_SBS = item.Vencido_SBS,
                    Vencido_Auditoría_Externa  = item.Vencido_Auditoría_Externa,
                    Vencido_Auditoría_Interna = item.Vencido_Auditoría_Interna,
                    Vencido_SUSALUD = item.Vencido_SUSALUD
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }



        public JsonResult JsonGRAFDetalle_Seguimiento_Observaciones_2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;

            DateTime fecha = DateTime.Today;

            List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_2_Result> items = new List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_2_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_2(negocio, gerencia,fecha)))
            {
                items.Add(new SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_2_Result()
                {
                    EMISOR = item.EMISOR,
                    Cantidad = item.Cantidad
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        public JsonResult JsonGRAFDetalle_Seguimiento_Observaciones_Distribucion_emisor_2()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;



            DateTime fecha = DateTime.Today;


            List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2_Result> items = new List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2(negocio, gerencia,fecha)))
            {
                items.Add(new SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_EMISOR_2_Result()
                {
                    EstadoTM = item.EstadoTM,
                    Auditoría_Externa = item.Auditoría_Externa,
                    Auditoría_Interna = item.Auditoría_Interna,
                    Banmedica = item.Banmedica,
                    SBS = item.SBS,
                    SUSALUD = item.SUSALUD
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }


        public JsonResult JsonGRAFDetalle_Seguimiento_Observaciones_Distribucion_Estado_Riesgo()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;



            DateTime fecha = DateTime.Today;


            List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO_Result> items = new List<SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO(negocio, gerencia, fecha)))
            {
                items.Add(new SP_WT_DETALLE_SEGUIMIENTO_OBSERVACIONES_DISTRIBUCION_ESTADO_RIESGO_Result()
                {
                    RIESGO = item.RIESGO,
                    En_Fecha = item.En_Fecha,
                    Vencido = item.Vencido

                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }





        public JsonResult JsonGRAFDetalle_Controles_Evaluados_1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;


            DateTime date = Convert.ToDateTime(Session["date"]);

            List<SP_DETALLE_CONTROLES_EVALUADOS_1_Result> items = new List<SP_DETALLE_CONTROLES_EVALUADOS_1_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_CONTROLES_EVALUADOS_1(negocio, gerencia, date)))
            {
                items.Add(new SP_DETALLE_CONTROLES_EVALUADOS_1_Result()
                {
                    Año = item.Año,
                    Porcentaje = item.Porcentaje
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
        public JsonResult JsonGRAFDetalle_Resultado_Auditoria_1()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            string negocio = Convert.ToString(Session["negocio"]);
            string gerencia = Convert.ToString(Session["gerencia"]);
            int fecha = Convert.ToInt32(Session["fecha"]);
            ViewBag.negocio = negocio;
            ViewBag.gerencia = gerencia;
            ViewBag.fecha = fecha;

            //Session["date"] = DateTime.Now.ToString("2020-09-11");
            Session["date"] = DateTime.Today;
            DateTime date = Convert.ToDateTime(Session["date"]);

            List<SP_WT_DETALLE_RESULTADOS_AUDITORIA_1_Result> items = new List<SP_WT_DETALLE_RESULTADOS_AUDITORIA_1_Result>();
            foreach (var item in (db2.SP_WT_RA_DETALLE_RESULTADOS_AUDITORIA_1(negocio, gerencia,date)))
            {
                items.Add(new SP_WT_DETALLE_RESULTADOS_AUDITORIA_1_Result()
                {
                    Macroproceso = item.Macroproceso,
                    Aceptable = item.Aceptable,
                    Satisfactorio = item.Satisfactorio,
                    Sin_Calificativo = item.Sin_Calificativo,
                    Regular = item.Regular,
                    Deficiente = item.Deficiente
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }
    }
}