using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

using WebTIGA.Autorizacion;

namespace WebTIGA.Controllers
{
    [Logueado]
    public class ResultadoAuditoriaController : Controller
    {
        MultiserviciosEntities1 db = new MultiserviciosEntities1();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos modelDB = new ContenedorModelos();

        private readonly List<string> listaEquipos = new List<string> { "Todos", "Seguros Procesos", "Seguros TI", "Salud Procesos", "Salud TI", "Crediseguro"};


        // GET: ResultadoAuditoria
        public ActionResult Inicio()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            return View();
        }

        public ActionResult ReporteGeneral()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            string emisionInicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy");
            string emisionFin = DateTime.Today.ToString("dd/MM/yyyy");

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;

            modelDB.SP_RA_REPORTE_GENERAL = db2.SP_RA_REPORTE_GENERAL(emisionInicio, emisionFin).Take(100);

            return View(modelDB);
        }

        [HttpPost]
        public ActionResult ReporteGeneral(string emisionInicio, string emisionFin)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;

            modelDB.SP_RA_REPORTE_GENERAL = db2.SP_RA_REPORTE_GENERAL(emisionInicio, emisionFin).Take(100);

            return View(modelDB);
        }


        public ActionResult ReporteClave()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            string emisionInicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy");
            string emisionFin = DateTime.Today.ToString("dd/MM/yyyy");

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;

            modelDB.SP_RA_REPORTE_CLAVE = db2.SP_RA_REPORTE_CLAVE(emisionInicio, emisionFin, 3).Take(100);

            return View(modelDB);
        }

        [HttpPost]
        public ActionResult ReporteClave(string emisionInicio, string emisionFin, int eleccion_clave)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;
            ViewBag.eleccion_clave = eleccion_clave;


            modelDB.SP_RA_REPORTE_CLAVE = db2.SP_RA_REPORTE_CLAVE(emisionInicio, emisionFin, eleccion_clave).Take(100);

            return View(modelDB);
        }
        public ActionResult ReporteSox()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            string emisionInicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy");
            string emisionFin = DateTime.Today.ToString("dd/MM/yyyy");

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;

            modelDB.SP_RA_REPORTE_SOX = db2.SP_RA_REPORTE_SOX(emisionInicio, emisionFin).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteSox(string emisionInicio, string emisionFin)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.EmisionInicio = emisionInicio;
            ViewBag.EmisionFin = emisionFin;

            modelDB.SP_RA_REPORTE_SOX = db2.SP_RA_REPORTE_SOX(emisionInicio, emisionFin).Take(100);

            return View(modelDB);
        }

        public ActionResult ReporteObsCerradas()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            string fechaInicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy");
            string fechaFin = DateTime.Today.ToString("dd/MM/yyyy");

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            modelDB.SP_RA_REPORTE_OBS_CERRADAS = db2.SP_RA_REPORTE_OBS_CERRADAS(fechaInicio, fechaFin).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteObsCerradas(string fechaInicio, string fechaFin)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            modelDB.SP_RA_REPORTE_OBS_CERRADAS = db2.SP_RA_REPORTE_OBS_CERRADAS(fechaInicio, fechaFin).Take(100);

            return View(modelDB);
        }

        public ActionResult ReporteEfectControl(int? anio)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            int anioActual = DateTime.Today.Year;
            int anioSeleccionado = anioActual;
            if (anio.HasValue)
            {
                anioSeleccionado = anio.Value;
            }
            ViewBag.AnioSeleccionado = anioSeleccionado;

            modelDB.SP_RA_REPORTE_EFECT_CONTROL = db2.SP_RA_REPORTE_EFECT_CONTROL(anioSeleccionado).Take(100);

            List<int> ListaAnio = new List<int>();
            for(int i = anioActual; i >= 2017; i--)
            {
                ListaAnio.Add(i);
            }

            ViewBag.ListaAnio = ListaAnio;

            return View(modelDB);
        }

        public ActionResult ReporteStatusActividades()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var planAnual = (from p in db2.DD_PlanAnual.Where(i => i.IdPlanAnual >= 8) select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio.Where(i => i.ID_Negocio == 1 || i.ID_Negocio == 2 || i.ID_Negocio == 4) select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();       
            
            int idPlanAnualSeleccionado = planAnual.First().IdPlanAnual;
            int idNegocioSeleccionado = negocios.First().ID_Negocio;

            ViewBag.idPlanAnualSeleccionado = idPlanAnualSeleccionado;
            ViewBag.idNegocioSeleccionado = idNegocioSeleccionado;

            modelDB.SP_RA_REPORTE_STATUS_ACTIVIDADES = db2.SP_RA_REPORTE_STATUS_ACTIVIDADES(idPlanAnualSeleccionado, idNegocioSeleccionado).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteStatusActividades(int idPlanAnual, int idNegocio)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var planAnual = (from p in db2.DD_PlanAnual.Where(i => i.IdPlanAnual >= 8) select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio.Where(i => i.ID_Negocio == 1 || i.ID_Negocio == 2 || i.ID_Negocio == 4) select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            int idPlanAnualSeleccionado = idPlanAnual;
            int idNegocioSeleccionado = idNegocio;

            ViewBag.idPlanAnualSeleccionado = idPlanAnualSeleccionado;
            ViewBag.idNegocioSeleccionado = idNegocioSeleccionado;

            modelDB.SP_RA_REPORTE_STATUS_ACTIVIDADES = db2.SP_RA_REPORTE_STATUS_ACTIVIDADES(idPlanAnualSeleccionado, idNegocioSeleccionado).Take(100);

            return View(modelDB);
        }

        public ActionResult ReporteMensualCredicorp()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            string fechaCorte = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToString("dd/MM/yyyy");
            string fechaCorteComentario = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToString("dd/MM/yyyy");
            var equipoSeleccionado = listaEquipos[0];

            ViewBag.FechaCorte = fechaCorte;
            ViewBag.FechaCorteComentario = fechaCorteComentario;
            ViewBag.EquipoSeleccionado = equipoSeleccionado;
            ViewBag.ListaEquipos = listaEquipos;

            modelDB.SP_RA_REPORTE_MENSUAL_CREDICORP = db2.SP_RA_REPORTE_MENSUAL_CREDICORP(fechaCorte, fechaCorteComentario, equipoSeleccionado).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteMensualCredicorp(string fechaCorte, string fechaCorteComentario, string equipoSeleccionado)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.FechaCorte = fechaCorte;
            ViewBag.FechaCorteComentario = fechaCorteComentario;
            ViewBag.EquipoSeleccionado = equipoSeleccionado;
            ViewBag.ListaEquipos = listaEquipos;

            modelDB.SP_RA_REPORTE_MENSUAL_CREDICORP = db2.SP_RA_REPORTE_MENSUAL_CREDICORP(fechaCorte, fechaCorteComentario, equipoSeleccionado).Take(100);

            return View(modelDB);
        }

        public ActionResult ReporteProyectosRiesgoAlto()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            
            var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            string fechaInicio = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
            string fechaFin = DateTime.Today.ToString("dd/MM/yyyy");
            int idPlanAnualSeleccionado = planAnual.First().IdPlanAnual;

            ViewBag.IdPlanAnualSeleccionado = idPlanAnualSeleccionado;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            modelDB.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO = db2.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO(fechaInicio, fechaFin, idPlanAnualSeleccionado).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteProyectosRiesgoAlto(string fechaInicio, string fechaFin, int idPlanAnual)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            ViewBag.IdPlanAnualSeleccionado = idPlanAnual;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            modelDB.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO = db2.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO(fechaInicio, fechaFin, idPlanAnual).Take(100);

            return View(modelDB);
        }

        public ActionResult ReporteObsRiesgoAlto()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            string fechaCorte= DateTime.Today.ToString("dd/MM/yyyy");
            int idPlanAnualSeleccionado = planAnual.First().IdPlanAnual;

            ViewBag.IdPlanAnualSeleccionado = idPlanAnualSeleccionado;
            ViewBag.FechaCorte = fechaCorte;

            modelDB.SP_RA_REPORTE_OBS_RIESGO_ALTO = db2.SP_RA_REPORTE_OBS_RIESGO_ALTO(fechaCorte, idPlanAnualSeleccionado).Take(100);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult ReporteObsRiesgoAlto(string fechaCorte, int idPlanAnual)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);
            modelDB.DD_PlanAnual = planAnual.ToList();

            ViewBag.IdPlanAnualSeleccionado = idPlanAnual;
            ViewBag.FechaCorte = fechaCorte;

            modelDB.SP_RA_REPORTE_OBS_RIESGO_ALTO = db2.SP_RA_REPORTE_OBS_RIESGO_ALTO(fechaCorte, idPlanAnual).Take(100);

            return View(modelDB);
        }


        public void DescargarReporteGeneral(string emisionInicio, string emisionFin)
        {
            var dia = emisionFin.Substring(0, 2);
            var mes = emisionFin.Substring(3, 2);
            var anio = emisionFin.Substring(6, 4);

            List<SP_RA_REPORTE_GENERAL_Result> reporteGeneral = db2.SP_RA_REPORTE_GENERAL(emisionInicio, emisionFin).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteGeneral");
                   

            ws.Cells["A2"].Value = "Referencia del Proceso";
            ws.Cells["B2"].Value = "Año";
            ws.Cells["C2"].Value = "Descripción del Proceso";
            ws.Cells["D2"].Value = "N° Riesgo";
            ws.Cells["E2"].Value = "Descripción del Riesgo";
            ws.Cells["H2"].Value = "Categoría del Riesgo";
            ws.Cells["I2"].Value = "Tipo de Riesgo";
            ws.Cells["J2"].Value = "Riesgo Inherente";
            ws.Cells["P2"].Value = "N° Control";
            ws.Cells["Q2"].Value = "Descipción del Control";
            ws.Cells["R2"].Value = "Control Clave";
            ws.Cells["S2"].Value = "Control SOX";
            ws.Cells["T2"].Value = "Tipo de Controles";
            ws.Cells["U2"].Value = "Naturaleza del Control";
            ws.Cells["V2"].Value = "Evaluación del Control";
            ws.Cells["W2"].Value = "Riesgo Residual";
            ws.Cells["AC2"].Value = "Referencia del Proceso Anterior";
            ws.Cells["AD2"].Value = "Clasificación del Riesgo del Trabajo";
            ws.Cells["AE2"].Value = "Subgerencia";
            ws.Cells["AF2"].Value = "Finalizado?";

            ws.Cells["E3"].Value = "Causa";
            ws.Cells["F3"].Value = "Evento";
            ws.Cells["G3"].Value = "Consecuencia";
            ws.Cells["J3"].Value = "Impacto (en US$ miles)";
            ws.Cells["L3"].Value = "Frecuencia (veces por año)";
            ws.Cells["N3"].Value = "IFC";
            ws.Cells["T3"].Value = "A";
            ws.Cells["U3"].Value = "P";
            ws.Cells["W3"].Value = "Impacto";
            ws.Cells["Y3"].Value = "Frecuencia";
            ws.Cells["AA3"].Value = "IFC";
                       
            ws.Cells["A2:A3"].Merge = true;
            ws.Cells["B2:B3"].Merge = true;
            ws.Cells["C2:C3"].Merge = true;
            ws.Cells["D2:D3"].Merge = true;
            ws.Cells["E2:G2"].Merge = true;
            ws.Cells["H2:H3"].Merge = true;
            ws.Cells["I2:I3"].Merge = true;
            ws.Cells["J2:O2"].Merge = true;
            ws.Cells["J3:K3"].Merge = true;
            ws.Cells["L3:M3"].Merge = true;
            ws.Cells["N3:O3"].Merge = true;
            ws.Cells["P2:P3"].Merge = true;
            ws.Cells["Q2:Q3"].Merge = true;
            ws.Cells["R2:R3"].Merge = true;
            ws.Cells["S2:S3"].Merge = true;
            ws.Cells["V2:V3"].Merge = true;
            ws.Cells["W2:AB2"].Merge = true;
            ws.Cells["W3:X3"].Merge = true;
            ws.Cells["Y3:Z3"].Merge = true;
            ws.Cells["AA3:AB3"].Merge = true;
            ws.Cells["AC2:AC3"].Merge = true;
            ws.Cells["AD2:AD3"].Merge = true;           
            ws.Cells["AE2:AE3"].Merge = true;
            ws.Cells["AF2:AF3"].Merge = true;

            ws.Cells["A2:AF3"].Style.WrapText = true;
            ws.Cells["A2:AF3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A2:AF3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A2:AF3"].Style.Font.Size = 8;
            ws.Cells["A2:AF3"].Style.Font.Name = "Arial";
            ws.Cells["A2:AF3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A2:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A2:I3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["A2:I3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["P2:Q3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["P2:Q3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["P2:Q3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["T2:U3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["T2:U3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["T2:U3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AC2:AC3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AC2:AC3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);           
            ws.Cells["AC2:AC3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AE2:AF3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AE2:AF3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AE2:AF3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            ws.Cells["J2:O3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["J2:O3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["J2:O3"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            ws.Cells["W2:AB3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["W2:AB3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["W2:AB3"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            ws.Cells["R2:S3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["R2:S3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(214, 220, 228));
            ws.Cells["R2:S3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            ws.Cells["V2:V3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["V2:V3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["V2:V3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AD2:AD3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AD2:AD3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["AD2:AD3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            int rowStart = 4;
            foreach (var item in reporteGeneral)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Referencia_del_Proceso;
                ws.Cells[string.Format("B{0}", rowStart)].Value = Convert.ToInt32(item.Año);
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Descripción_del_Proceso;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.N__Riesgo;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Causa;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Evento;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Consecuencia;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Categoría_del_Riesgo;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.Tipo_de_Riesgo;
                ws.Cells[string.Format("J{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_);
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Impacto;
                ws.Cells[string.Format("L{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_);
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.Frecuencia;
                ws.Cells[string.Format("N{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_);
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.IFC;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.N__Control;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.Descripción_del_Control;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.Control_Clave;
                ws.Cells[string.Format("S{0}", rowStart)].Value = Convert.ToInt32(item.Control_Sox);
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Tipo_de_Controles;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Naturaleza_del_Control;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.Evaluacion_del_Control;
                ws.Cells[string.Format("W{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_2);
                ws.Cells[string.Format("X{0}", rowStart)].Value = item.Impacto2;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_2);
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.Frecuencia2;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_2);
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.IFC2;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = item.Referencia_del_Proceso_anterior;
                ws.Cells[string.Format("AD{0}", rowStart)].Value = item.Clasificación_del_Riesgo_de_Trabajo;
                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.SubGerencia;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.Finalizado_;

                rowStart++;
            }

            ws.Cells["A4:D4"].AutoFitColumns();
            ws.Cells["H4:P4"].AutoFitColumns();
            ws.Cells["R4:AF4"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte General al " + dia + "." + mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteClave(string emisionInicio, string emisionFin, int? eleccion_clave)
        {
            int valorEleccionClave = eleccion_clave ?? 1;
            var dia = emisionFin.Substring(0, 2);
            var mes = emisionFin.Substring(3, 2);
            var anio = emisionFin.Substring(6, 4);

            List<SP_RA_REPORTE_CLAVE_Result> reporteClave = db2.SP_RA_REPORTE_CLAVE(emisionInicio, emisionFin, valorEleccionClave).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteClave");

            ws.Cells["AE1"].Value = "Observaciones";

            ws.Cells["A2"].Value = "Referencia del Proceso";
            ws.Cells["B2"].Value = "Año";
            ws.Cells["C2"].Value = "Descripción del Proceso";
            ws.Cells["D2"].Value = "N° Riesgo";
            ws.Cells["E2"].Value = "Descripción del Riesgo";
            ws.Cells["H2"].Value = "Categoría del Riesgo";
            ws.Cells["I2"].Value = "Tipo de Riesgo";
            ws.Cells["J2"].Value = "Riesgo Inherente";
            ws.Cells["P2"].Value = "N° Control";
            ws.Cells["Q2"].Value = "Descipción del Control";
            ws.Cells["R2"].Value = "Control Clave";
            ws.Cells["S2"].Value = "Control SOX";
            ws.Cells["T2"].Value = "Tipo de Controles";
            ws.Cells["U2"].Value = "Naturaleza del Control";
            ws.Cells["V2"].Value = "Evaluación del Control";
            ws.Cells["W2"].Value = "Riesgo Residual";
            ws.Cells["AC2"].Value = "Referencia del Proceso Anterior";
            ws.Cells["AD2"].Value = "Tipo de Riesgo del Trabajo";
            ws.Cells["AE2"].Value = "Código";
            ws.Cells["AF2"].Value = "Título";
            ws.Cells["AG2"].Value = "Status";
            ws.Cells["AH2"].Value = "Fecha de Último Cambio de Status";
            ws.Cells["AI2"].Value = "Fecha de Corte (Reporte)";
            ws.Cells["AJ2"].Value = "Criticidad de la Observación";
            ws.Cells["AK2"].Value = "Subgerencia";
            ws.Cells["AL2"].Value = "Finalizado?";

            ws.Cells["E3"].Value = "Causa";
            ws.Cells["F3"].Value = "Evento";
            ws.Cells["G3"].Value = "Consecuencia";
            ws.Cells["J3"].Value = "Impacto (en US$ miles)";
            ws.Cells["L3"].Value = "Frecuencia (veces por año)";
            ws.Cells["N3"].Value = "IFC";
            ws.Cells["T3"].Value = "A";
            ws.Cells["U3"].Value = "P";
            ws.Cells["W3"].Value = "Impacto";
            ws.Cells["Y3"].Value = "Frecuencia";
            ws.Cells["AA3"].Value = "IFC";

            ws.Cells["A2:A3"].Merge = true;
            ws.Cells["B2:B3"].Merge = true;
            ws.Cells["C2:C3"].Merge = true;
            ws.Cells["D2:D3"].Merge = true;
            ws.Cells["H2:H3"].Merge = true;
            ws.Cells["I2:I3"].Merge = true;
            ws.Cells["P2:P3"].Merge = true;
            ws.Cells["Q2:Q3"].Merge = true;
            ws.Cells["R2:R3"].Merge = true;
            ws.Cells["S2:S3"].Merge = true;
            ws.Cells["V2:V3"].Merge = true;
            ws.Cells["AC2:AC3"].Merge = true;
            ws.Cells["AD2:AD3"].Merge = true;
            ws.Cells["AE2:AE3"].Merge = true;
            ws.Cells["AF2:AF3"].Merge = true;
            ws.Cells["AG2:AG3"].Merge = true;
            ws.Cells["AH2:AH3"].Merge = true;
            ws.Cells["AI2:AI3"].Merge = true;
            ws.Cells["AJ2:AJ3"].Merge = true;
            ws.Cells["AK2:AK3"].Merge = true;
            ws.Cells["AL2:AL3"].Merge = true;
            ws.Cells["E2:G2"].Merge = true;
            ws.Cells["J2:O2"].Merge = true;
            ws.Cells["W2:AB2"].Merge = true;
            ws.Cells["J3:K3"].Merge = true;
            ws.Cells["L3:M3"].Merge = true;
            ws.Cells["N3:O3"].Merge = true;
            ws.Cells["W3:X3"].Merge = true;
            ws.Cells["Y3:Z3"].Merge = true;
            ws.Cells["AA3:AB3"].Merge = true;
            ws.Cells["AE1:AJ1"].Merge = true;

            ws.Cells["A2:AL3"].Style.WrapText = true;
            ws.Cells["A1:AL3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:AL3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:AL3"].Style.Font.Size = 8;
            ws.Cells["A1:AL3"].Style.Font.Name = "Arial";

            ws.Cells["A2:AL3,AE1:AJ1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3,AE1:AJ1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3,AE1:AJ1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3,AE1:AJ1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;            

            ws.Cells["A2:I3,P2:Q3,T2:V3,AC2:AD3,AE1:AJ3,AK2:AL3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A2:I3,P2:Q3,T2:V3,AC2:AD3,AE1:AJ3,AK2:AL3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["A2:I3,P2:Q3,T2:V3,AC2:AD3,AE1:AJ3,AK2:AL3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["J2:O3,W2:AB3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["J2:O3,W2:AB3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["J2:O3,W2:AB3"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            ws.Cells["R2:S3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["R2:S3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(197, 217, 241));
            ws.Cells["R2:S3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);           

            string DateCellFormat = "mm/dd/yyyy";

            int rowStart = 4;
            foreach (var item in reporteClave)
            {
                ws.Cells[string.Format("A{0}:AL{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AL{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AL{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AL{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[string.Format("N{0}:O{0},AA{0}:AB{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[string.Format("N{0}:O{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
                ws.Cells[string.Format("AA{0}:AB{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 153));

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Referencia_del_Proceso;
                ws.Cells[string.Format("B{0}", rowStart)].Value = Convert.ToInt32(item.Año);
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Descripción_del_Proceso;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.N_Riesgo;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Causa;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Evento;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Consecuencia;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Categoría_del_Riesgo;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.Tipo_de_Riesgo;
                ws.Cells[string.Format("J{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto_en_US__miles);
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Impacto;
                ws.Cells[string.Format("L{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia_veces_por_año);
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.Frecuencia;
                ws.Cells[string.Format("N{0}", rowStart)].Value = Convert.ToDecimal(item.IFC_en_US__miles);
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.IFC;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.N_Control;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.Descripción_del_Control;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.Control_Clave;
                ws.Cells[string.Format("S{0}", rowStart)].Value = Convert.ToInt32(item.Control_Sox);
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Tipo_de_Controles;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Naturaleza_del_Control;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.Evaluacion_del_Control;
                ws.Cells[string.Format("W{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_2);
                ws.Cells[string.Format("X{0}", rowStart)].Value = item.Impacto2;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia_veces_por_año2);
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.Frecuencia2;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = Convert.ToDecimal(item.IFC_en_US__miles2);
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.IFC2;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = item.Referencia_del_Proceso_anterior;
                ws.Cells[string.Format("AD{0}", rowStart)].Value = item.Clasificación_del_Riesgo_de_Trabajo;
                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.Codigo;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.Titulo;
                ws.Cells[string.Format("AG{0}", rowStart)].Value = item.Status;
                ws.Cells[string.Format("AH{0}", rowStart)].Style.Numberformat.Format = DateCellFormat;
                ws.Cells[string.Format("AH{0}", rowStart)].Value = item.Fecha_Cambio_Status;
                ws.Cells[string.Format("AI{0}", rowStart)].Style.Numberformat.Format = DateCellFormat;
                ws.Cells[string.Format("AI{0}", rowStart)].Value = item.Fecha_de_Corte;
                ws.Cells[string.Format("AJ{0}", rowStart)].Value = item.Criticidad_Observacion;
                ws.Cells[string.Format("AK{0}", rowStart)].Value = item.SubGerencia;
                ws.Cells[string.Format("AL{0}", rowStart)].Value = item.Finalizado_;

                rowStart++;
            }

            ws.Cells["A4:D4,H4:P4,R4:AE4,AG4:AL4"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte Clave al " + dia + "."+ mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteSOX(string emisionInicio, string emisionFin)
        {
            var dia = emisionFin.Substring(0, 2);
            var mes = emisionFin.Substring(3, 2);
            var anio = emisionFin.Substring(6, 4);

            List<SP_RA_REPORTE_SOX_Result> reporteSOX = db2.SP_RA_REPORTE_SOX(emisionInicio, emisionFin).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteSOX");


            ws.Cells["A2"].Value = "Referencia del Proceso";
            ws.Cells["B2"].Value = "Año";
            ws.Cells["C2"].Value = "Descripción del Proceso";
            ws.Cells["D2"].Value = "N° Riesgo";
            ws.Cells["E2"].Value = "Descripción del Riesgo";
            ws.Cells["H2"].Value = "Categoría del Riesgo";
            ws.Cells["I2"].Value = "Tipo de Riesgo";
            ws.Cells["J2"].Value = "Riesgo Inherente";
            ws.Cells["P2"].Value = "N° Control";
            ws.Cells["Q2"].Value = "Descipción del Control";
            ws.Cells["R2"].Value = "Control Clave";
            ws.Cells["S2"].Value = "Control SOX";
            ws.Cells["T2"].Value = "Tipo de Controles";
            ws.Cells["U2"].Value = "Naturaleza del Control";
            ws.Cells["V2"].Value = "Evaluación del Control";
            ws.Cells["W2"].Value = "Riesgo Residual";
            ws.Cells["AC2"].Value = "Referencia del Proceso Anterior";
            ws.Cells["AD2"].Value = "Clasificación del Riesgo del Trabajo";
            ws.Cells["AE2"].Value = "Subgerencia";
            ws.Cells["AF2"].Value = "Finalizado?";


            ws.Cells["E3"].Value = "Causa";
            ws.Cells["F3"].Value = "Evento";
            ws.Cells["G3"].Value = "Consecuencia";
            ws.Cells["J3"].Value = "Impacto (en US$ miles)";
            ws.Cells["L3"].Value = "Frecuencia (veces por año)";
            ws.Cells["N3"].Value = "IFC";
            ws.Cells["T3"].Value = "A";
            ws.Cells["U3"].Value = "P";
            ws.Cells["W3"].Value = "Impacto";
            ws.Cells["Y3"].Value = "Frecuencia";
            ws.Cells["AA3"].Value = "IFC";

            ws.Cells["A2:A3"].Merge = true;
            ws.Cells["B2:B3"].Merge = true;
            ws.Cells["C2:C3"].Merge = true;
            ws.Cells["D2:D3"].Merge = true;
            ws.Cells["E2:G2"].Merge = true;
            ws.Cells["H2:H3"].Merge = true;
            ws.Cells["I2:I3"].Merge = true;
            ws.Cells["J2:O2"].Merge = true;
            ws.Cells["J3:K3"].Merge = true;
            ws.Cells["L3:M3"].Merge = true;
            ws.Cells["N3:O3"].Merge = true;
            ws.Cells["P2:P3"].Merge = true;
            ws.Cells["Q2:Q3"].Merge = true;
            ws.Cells["R2:R3"].Merge = true;
            ws.Cells["S2:S3"].Merge = true;
            ws.Cells["V2:V3"].Merge = true;
            ws.Cells["W2:AB2"].Merge = true;
            ws.Cells["W3:X3"].Merge = true;
            ws.Cells["Y3:Z3"].Merge = true;
            ws.Cells["AA3:AB3"].Merge = true;
            ws.Cells["AC2:AC3"].Merge = true;
            ws.Cells["AD2:AD3"].Merge = true;
            ws.Cells["AE2:AE3"].Merge = true;
            ws.Cells["AF2:AF3"].Merge = true;


            ws.Cells["A2:AF3"].Style.WrapText = true;
            ws.Cells["A2:AF3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A2:AF3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A2:AF3"].Style.Font.Size = 8;
            ws.Cells["A2:AF3"].Style.Font.Name = "Arial";
            ws.Cells["A2:AF3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AF3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A2:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A2:I3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["A2:I3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["P2:Q3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["P2:Q3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["P2:Q3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["T2:U3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["T2:U3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["T2:U3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AC2:AC3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AC2:AC3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AC2:AC3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AE2:AF3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AE2:AF3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AE2:AF3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            ws.Cells["J2:O3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["J2:O3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["J2:O3"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            ws.Cells["W2:AB3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["W2:AB3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["W2:AB3"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            ws.Cells["R2:S3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["R2:S3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(214, 220, 228));
            ws.Cells["R2:S3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            ws.Cells["V2:V3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["V2:V3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["V2:V3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AD2:AD3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AD2:AD3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["AD2:AD3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            int rowStart = 4;
            foreach (var item in reporteSOX)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Referencia_del_Proceso;
                ws.Cells[string.Format("B{0}", rowStart)].Value = Convert.ToInt32(item.Año);
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Descripción_del_Proceso;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.N__Riesgo;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Causa;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Evento;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Consecuencia;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Categoría_del_Riesgo;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.Tipo_de_Riesgo;
                ws.Cells[string.Format("J{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_);
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Impacto;
                ws.Cells[string.Format("L{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_);
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.Frecuencia;
                ws.Cells[string.Format("N{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_);
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.IFC;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.N__Control;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.Descripción_del_Control;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.Control_Clave;
                ws.Cells[string.Format("S{0}", rowStart)].Value = Convert.ToInt32(item.Control_Sox);
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Tipo_de_Controles;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Naturaleza_del_Control;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.Evaluacion_del_Control;
                ws.Cells[string.Format("W{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_2);
                ws.Cells[string.Format("X{0}", rowStart)].Value = item.Impacto2;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_2);
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.Frecuencia2;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_2);
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.IFC2;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = item.Referencia_del_Proceso_anterior;
                ws.Cells[string.Format("AD{0}", rowStart)].Value = item.Clasificación_del_Riesgo_de_Trabajo;
                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.SubGerencia;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.Finalizado_;

                rowStart++;
            }

            ws.Cells["A4:D4"].AutoFitColumns();
            ws.Cells["H4:P4"].AutoFitColumns();
            ws.Cells["R4:AF4"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte SOX al " + dia + "." + mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteObsCerradas(string fechaInicio, string fechaFin)
        {
            var dia = fechaFin.Substring(0, 2);
            var mes = fechaFin.Substring(3, 2);
            var anio = fechaFin.Substring(6, 4);

            List<SP_RA_REPORTE_OBS_CERRADAS_Result> reporteObsCerradas = db2.SP_RA_REPORTE_OBS_CERRADAS(fechaInicio, fechaFin).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteObsCerradas");

            ws.Cells["AE1"].Value = "Observaciones";

            ws.Cells["A2"].Value = "Referencia del Proceso";
            ws.Cells["B2"].Value = "Año";
            ws.Cells["C2"].Value = "Descripción del Proceso";
            ws.Cells["D2"].Value = "N° Riesgo";
            ws.Cells["E2"].Value = "Descripción del Riesgo";
            ws.Cells["H2"].Value = "Categoría del Riesgo";
            ws.Cells["I2"].Value = "Tipo de Riesgo";
            ws.Cells["J2"].Value = "Riesgo Inherente";
            ws.Cells["P2"].Value = "N° Control";
            ws.Cells["Q2"].Value = "Descipción del Control";
            ws.Cells["R2"].Value = "Control Clave";
            ws.Cells["S2"].Value = "Control SOX";
            ws.Cells["T2"].Value = "Tipo de Controles";
            ws.Cells["U2"].Value = "Naturaleza del Control";
            ws.Cells["V2"].Value = "Evaluación del Control";
            ws.Cells["W2"].Value = "Riesgo Residual";
            ws.Cells["AC2"].Value = "Referencia del Proceso Anterior";
            ws.Cells["AD2"].Value = "Clasificación del Riesgo del Trabajo";
            ws.Cells["AE2"].Value = "Código";
            ws.Cells["AF2"].Value = "Título";
            ws.Cells["AG2"].Value = "Status";
            ws.Cells["AH2"].Value = "Fecha de Último Cambio de Status";
            ws.Cells["AI2"].Value = "Fecha de Corte (Reporte)";
            ws.Cells["AJ2"].Value = "Criticidad de la Observación";
            ws.Cells["AK2"].Value = "Subgerencia";
            ws.Cells["AL2"].Value = "Finalizado?";

            ws.Cells["E3"].Value = "Causa";
            ws.Cells["F3"].Value = "Evento";
            ws.Cells["G3"].Value = "Consecuencia";
            ws.Cells["J3"].Value = "Impacto (en US$ miles)";
            ws.Cells["L3"].Value = "Frecuencia (veces por año)";
            ws.Cells["N3"].Value = "IFC";
            ws.Cells["T3"].Value = "A";
            ws.Cells["U3"].Value = "P";
            ws.Cells["W3"].Value = "Impacto";
            ws.Cells["Y3"].Value = "Frecuencia";
            ws.Cells["AA3"].Value = "IFC";

            ws.Cells["AE1:AJ1"].Merge = true;
            ws.Cells["A2:A3"].Merge = true;
            ws.Cells["B2:B3"].Merge = true;
            ws.Cells["C2:C3"].Merge = true;
            ws.Cells["D2:D3"].Merge = true;
            ws.Cells["E2:G2"].Merge = true;
            ws.Cells["H2:H3"].Merge = true;
            ws.Cells["I2:I3"].Merge = true;
            ws.Cells["J2:O2"].Merge = true;
            ws.Cells["J3:K3"].Merge = true;
            ws.Cells["L3:M3"].Merge = true;
            ws.Cells["N3:O3"].Merge = true;
            ws.Cells["P2:P3"].Merge = true;
            ws.Cells["Q2:Q3"].Merge = true;
            ws.Cells["R2:R3"].Merge = true;
            ws.Cells["S2:S3"].Merge = true;
            ws.Cells["V2:V3"].Merge = true;
            ws.Cells["W2:AB2"].Merge = true;
            ws.Cells["W3:X3"].Merge = true;
            ws.Cells["Y3:Z3"].Merge = true;
            ws.Cells["AA3:AB3"].Merge = true;
            ws.Cells["AC2:AC3"].Merge = true;
            ws.Cells["AD2:AD3"].Merge = true;
            ws.Cells["AE2:AE3"].Merge = true;
            ws.Cells["AF2:AF3"].Merge = true;
            ws.Cells["AG2:AG3"].Merge = true;
            ws.Cells["AH2:AH3"].Merge = true;
            ws.Cells["AI2:AI3"].Merge = true;
            ws.Cells["AJ2:AJ3"].Merge = true;
            ws.Cells["AK2:AK3"].Merge = true;
            ws.Cells["AL2:AL3"].Merge = true;

            ws.Cells["A2:AL3"].Style.WrapText = true;
            ws.Cells["A1:AL3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:AL3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:AL3"].Style.Font.Size = 8;
            ws.Cells["A1:AL3"].Style.Font.Name = "Arial";
            ws.Cells["A2:AL3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:AL3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["AE1:AJ1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["AE1:AJ1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["AE1:AJ1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["AE1:AJ1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A2:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A2:I3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["A2:I3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["P2:Q3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["P2:Q3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["P2:Q3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["T2:U3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["T2:U3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["T2:U3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AC2:AC3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AC2:AC3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AC2:AC3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AE1:AJ3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AE1:AJ3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AE1:AJ3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AK2:AL3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AK2:AL3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            ws.Cells["AK2:AL3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            ws.Cells["J2:O3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["J2:O3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["J2:O3"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            ws.Cells["W2:AB3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["W2:AB3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["W2:AB3"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            ws.Cells["R2:S3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["R2:S3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(214, 220, 228));
            ws.Cells["R2:S3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            ws.Cells["V2:V3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["V2:V3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["V2:V3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));
            ws.Cells["AD2:AD3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["AD2:AD3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
            ws.Cells["AD2:AD3"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 255));

            string DateCellFormat = "mm/dd/yyyy";

            int rowStart = 4;
            foreach (var item in reporteObsCerradas)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Referencia_del_Proceso;
                ws.Cells[string.Format("B{0}", rowStart)].Value = Convert.ToInt32(item.Año);
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Descripción_del_Proceso;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.N__Riesgo;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Causa;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Evento;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Consecuencia;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Categoría_del_Riesgo;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.Tipo_de_Riesgo;
                ws.Cells[string.Format("J{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_);
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Impacto;
                ws.Cells[string.Format("L{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_);
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.Frecuencia;
                ws.Cells[string.Format("N{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_);
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.IFC;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.N__Control;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.Descripción_del_Control;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.Control_Clave;
                ws.Cells[string.Format("S{0}", rowStart)].Value = Convert.ToInt32(item.Control_Sox);
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Tipo_de_Controles;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Naturaleza_del_Control;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.Evaluacion_del_Control;
                ws.Cells[string.Format("W{0}", rowStart)].Value = Convert.ToDecimal(item.Impacto__en_US__miles_2);
                ws.Cells[string.Format("X{0}", rowStart)].Value = item.Impacto2;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = Convert.ToDecimal(item.Frecuencia__veces_por_año_2);
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.Frecuencia2;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = Convert.ToDecimal(item.IFC__en_US__miles_2);
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.IFC2;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = item.Referencia_del_Proceso_anterior;
                ws.Cells[string.Format("AD{0}", rowStart)].Value = item.Clasificación_del_Riesgo_de_Trabajo;
                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.Codigo;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.Titulo;
                ws.Cells[string.Format("AG{0}", rowStart)].Value = item.Status;
                ws.Cells[string.Format("AH{0}", rowStart)].Style.Numberformat.Format = DateCellFormat;
                ws.Cells[string.Format("AH{0}", rowStart)].Value = item.Fecha_Cambio_Status;
                ws.Cells[string.Format("AI{0}", rowStart)].Style.Numberformat.Format = DateCellFormat;
                ws.Cells[string.Format("AI{0}", rowStart)].Value = item.Fecha_de_Corte;
                ws.Cells[string.Format("AJ{0}", rowStart)].Value = item.Criticidad_Observacion;
                ws.Cells[string.Format("AK{0}", rowStart)].Value = item.SubGerencia;
                ws.Cells[string.Format("AL{0}", rowStart)].Value = item.Finalizado_;

                rowStart++;
            }

            ws.Cells["A4:D4"].AutoFitColumns();
            ws.Cells["H4:P4"].AutoFitColumns();
            ws.Cells["R4:AE4"].AutoFitColumns();
            ws.Cells["AG4:AL4"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte Observaciones Cerradas al " + dia + "." + mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteEfectControl(int anio)
        {      
            List<SP_RA_REPORTE_EFECT_CONTROL_Result> reporteEfectControl = db2.SP_RA_REPORTE_EFECT_CONTROL(anio).ToList();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteEfectividadControl");


            ws.Cells["A1"].Value = "CODIGO";
            ws.Cells["B1"].Value = "PROYECTOS";
            ws.Cells["C1"].Value = "IFCR Inherente";
            ws.Cells["D1"].Value = "Cobertura del IFCR Residual";
            ws.Cells["E1"].Value = "EFECTIVOS";
            ws.Cells["F1"].Value = "EFECTIVO NO FORMALIZADO";
            ws.Cells["G1"].Value = "INEFECTIVO PRUEBA";
            ws.Cells["H1"].Value = "INEFECTIVO DISEÑO";
            ws.Cells["I1"].Value = "CONTROL INEXISTENTE";
            ws.Cells["J1"].Value = "TOTAL";
            ws.Cells["K1"].Value = "Tipo";
            ws.Cells["L1"].Value = "Estado";
            ws.Cells["M1"].Value = "Evaluación";
            ws.Cells["N1"].Value = "Calificativo %";
            ws.Cells["O1"].Value = "Tipo de Riesgo";
            ws.Cells["P1"].Value = "# Observaciones Riesgo Crítico / Alto";
            ws.Cells["Q1"].Value = "Negocio";

            ws.Cells["A1:Q1"].Style.WrapText = true;
            ws.Cells["A1:Q1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:Q1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:Q1"].Style.Font.Size = 10;
            ws.Cells["A1:Q1"].Style.Font.Bold = true;
            ws.Cells["A1:Q1"].Style.Font.Name = "Calibri";
            ws.Cells["A1:Q1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:Q1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:Q1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:Q1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            
            ws.Cells["A1:Q1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A1:Q1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(34, 43, 53));
            ws.Cells["A1:Q1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            int rowStart = 2;
            foreach (var item in reporteEfectControl)
            {
                ws.Cells[string.Format("A{0}:Q{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:Q{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:Q{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:Q{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[string.Format("C{0}:D{0},N{0},P{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[string.Format("C{0}:D{0},N{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
                ws.Cells[string.Format("P{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.CODIGO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.PROYECTOS;
                ws.Cells[string.Format("C{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("C{0}", rowStart)].Value = Convert.ToDecimal(item.IFC_Inherente);
                ws.Cells[string.Format("D{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("D{0}", rowStart)].Value = Convert.ToDecimal(item.Cobertura_IFC_Residual);
                ws.Cells[string.Format("E{0}", rowStart)].Value = Convert.ToInt32(item.EFECTIVOS);
                ws.Cells[string.Format("F{0}", rowStart)].Value = Convert.ToInt32(item.EFECTIVO_NO_FORMALIZADO);
                ws.Cells[string.Format("G{0}", rowStart)].Value = Convert.ToInt32(item.INEFECTIVO_PRUEBA);
                ws.Cells[string.Format("H{0}", rowStart)].Value = Convert.ToInt32(item.INEFECTIVO_DISEÑO);
                ws.Cells[string.Format("I{0}", rowStart)].Value = Convert.ToInt32(item.CONTROL_INEXISTENTE);
                ws.Cells[string.Format("J{0}", rowStart)].Value = Convert.ToInt32(item.TOTAL_CONTROLES);
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Tipo;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.Estado;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.Calificativo;
                ws.Cells[string.Format("N{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("N{0}", rowStart)].Value = Convert.ToDecimal(item.C__Calificativo);
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.Tipo_de_Riesgo;
                ws.Cells[string.Format("P{0}", rowStart)].Value = Convert.ToInt32(item.Obs_Riesgo_Alto);
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.NEGOCIO;

                rowStart++;
            }

            ws.Cells["A1:Q1"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte_Efectividad_Controles_" + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteStatusActividades(int idPlanAnual, int idNegocio)
        {
            List<SP_RA_REPORTE_STATUS_ACTIVIDADES_Result> reporteStatusActividades = db2.SP_RA_REPORTE_STATUS_ACTIVIDADES(idPlanAnual, idNegocio).ToList();
            DD_PlanAnual planAnual = db2.DD_PlanAnual.Find(idPlanAnual);
            DPA_Negocio negocio = db2.DPA_Negocio.Find(idNegocio);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteStatusActividades");

            ws.Cells["A2"].Value = "Informe";
            ws.Cells["B2"].Value = "Código";
            ws.Cells["C2"].Value = "Evaluación";
            ws.Cells["D2"].Value = "Estado";
            ws.Cells["E2"].Value = "Tipo";
            ws.Cells["F2"].Value = "Fecha de Inicio del Plan";
            ws.Cells["G2"].Value = "Fecha de Fin del Plan";
            ws.Cells["H2"].Value = "Actividad";
            ws.Cells["I2"].Value = "Fecha de Emisión";
            ws.Cells["J2"].Value = "Último Día";
            ws.Cells["K2"].Value = "Fecha de Ejecución Real";
            ws.Cells["L2"].Value = "Fecha Revisado";
            ws.Cells["M2"].Value = "Cantidad de Sprints";

            ws.Cells["A2:M2"].Style.WrapText = true;
            ws.Cells["A2:M2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A2:M2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A2:M2"].Style.Font.Size = 8;
            ws.Cells["A2:M2"].Style.Font.Name = "Arial";
            ws.Cells["A2:M2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:M2"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:M2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A2:M2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A2:M2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A2:M2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 0, 0));
            ws.Cells["A2:M2"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            int rowStart = 3;
            foreach (var item in reporteStatusActividades)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Informe;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Codigo;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Evaluacion;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Estado;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Tipo;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Fecha_Inicio_Plan;
                ws.Cells[string.Format("F{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Fecha_Fin_Plan;
                ws.Cells[string.Format("G{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Actividad;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.Fecha_Emision;
                ws.Cells[string.Format("I{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.Ultimo_Dia;
                ws.Cells[string.Format("J{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Fecha_Ejecucion_Real;
                ws.Cells[string.Format("K{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.Fecha_Revisado;
                ws.Cells[string.Format("L{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("M{0}", rowStart)].Value = Convert.ToInt32(item.Cantidad_Sprints);

                rowStart++;
            }

            ws.Cells["A2:M2"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte_Status_Actividades_" + negocio.Abreviacion + "_" + planAnual.Nombre + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void DescargarReporteMensualCredicorp(string fechaCorte, string fechaCorteComentario, string equipoSeleccionado)
        {
            var dia = fechaCorte.Substring(0, 2);
            var mes = fechaCorte.Substring(3, 2);
            var anio = fechaCorte.Substring(6, 4);

            List<SP_RA_REPORTE_MENSUAL_CREDICORP_Result> reporteMensualCredicorp = db2.SP_RA_REPORTE_MENSUAL_CREDICORP(fechaCorte, fechaCorteComentario, equipoSeleccionado).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteMensualCredicorp");

            ws.Cells["A1"].Value = "ID";
            ws.Cells["B1"].Value = "PROYECTO";
            ws.Cells["C1"].Value = "NOMBRE PROYECTO";
            ws.Cells["D1"].Value = "TITULO OBSERVACION";
            ws.Cells["E1"].Value = "OBSERVACION";
            ws.Cells["F1"].Value = "RECOMENDACION";
            ws.Cells["G1"].Value = "RESPUESTA";
            ws.Cells["H1"].Value = "UNIDAD RESPONSABLE";
            ws.Cells["I1"].Value = "CONTACTO";
            ws.Cells["J1"].Value = "RESPONSABLE";
            ws.Cells["K1"].Value = "FECHA EMISION";
            ws.Cells["L1"].Value = "RIESGO";
            ws.Cells["M1"].Value = "ULTIMA ACTUALIZACION";
            ws.Cells["N1"].Value = "FECHA ACTUALIZACION";
            ws.Cells["O1"].Value = "FECHA DE VENCIMIENTO";
            ws.Cells["P1"].Value = "ESTADO";
            ws.Cells["Q1"].Value = "FECHA DE CARGA TC";
            ws.Cells["R1"].Value = "REGULADOR";
            ws.Cells["S1"].Value = "REGULADO";
            ws.Cells["T1"].Value = "EMAIL RESPONSABLE";
            ws.Cells["U1"].Value = "EMAIL CONTACTO";
            ws.Cells["V1"].Value = "COORDINADOR";
            ws.Cells["W1"].Value = "COMENTARIO TEAM CENTRAL";
            ws.Cells["X1"].Value = "COMENTARIO";
            ws.Cells["Y1"].Value = "RIESGO MITIGADO";
            ws.Cells["Z1"].Value = "GENERA PREOCUPACION";
            ws.Cells["AA1"].Value = "SEMÁFORO";

            ws.Cells["A1:AA1"].Style.WrapText = true;
            ws.Cells["A1:AA1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:AA1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:AA1"].Style.Font.Size = 10;
            ws.Cells["A1:AA1"].Style.Font.Bold = true;
            ws.Cells["A1:AA1"].Style.Font.Name = "Calibri";
            ws.Cells["A1:AA1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:AA1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:AA1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:AA1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A1:V1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A1:V1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(34, 43, 53));
            ws.Cells["A1:V1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            ws.Cells["W1:AA1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["W1:AA1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 0));
            ws.Cells["W1:AA1"].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            int rowStart = 2;
            foreach (var item in reporteMensualCredicorp)
            {
                ws.Cells[string.Format("A{0}:AA{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AA{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AA{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AA{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        
                if (item.SEMAFORO == "Rojo"){
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 0, 0));
                }
                else if (item.SEMAFORO == "Ámbar")
                {
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));
                }
                else if (item.SEMAFORO == "Verde")
                {
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells[string.Format("AA{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 176, 80));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = Convert.ToInt32(item.ID);
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.PROYECTO;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.NOMBRE_PROYECTO;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.TITULO_OBSERVACION;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.OBSERVACION;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.RECOMENDACION;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.PRIMERA_RESPUESTA;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.UNIDAD_RESPONSABLE;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.CONTACTO;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.FECHA_EMISIÓN;
                ws.Cells[string.Format("K{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.RIESGO;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.ÚLTIMA_RESPUESTA;
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.FECHA_DE_VENCIMIENTO;
                ws.Cells[string.Format("O{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.ESTADO;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.EMISOR;
                ws.Cells[string.Format("S{0}", rowStart)].Value = item.INFORME;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.EMAIL_CONTACTO;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.COORDINADOR;
                ws.Cells[string.Format("W{0}", rowStart)].Value = item.REQUIERE_COMENTARIO;
                ws.Cells[string.Format("X{0}", rowStart)].Value = item.ÚLTIMO_COMENTARIO_BCP;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = item.RIESGO_MITIGADO;
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.GENERA_PREOCUPACION;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = item.SEMAFORO;

                rowStart++;
            }

            ws.Cells["A1:Z1"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= ReporteMensualCredicorp al " + dia + "." + mes + "." + anio + "_" + equipoSeleccionado + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        public void DescargarReporteProyectosRiesgoAlto(string fechaInicio, string fechaFin, int idPlanAnual)
        {
            var dia = fechaFin.Substring(0, 2);
            var mes = fechaFin.Substring(3, 2);
            var anio = fechaFin.Substring(6, 4);

            List<SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO_Result> reporteProyectosRiesgoAlto = db2.SP_RA_REPORTE_PROYECTOS_RIESGO_ALTO(fechaInicio, fechaFin, idPlanAnual).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProyectosRiesgoAlto");

            ws.Cells["A1"].Value = "Código";
            ws.Cells["B1"].Value = "Nombre Proyecto";
            ws.Cells["C1"].Value = "Fecha Inicio Ejecución";
            ws.Cells["D1"].Value = "Fecha Fin Ejecución";
            ws.Cells["E1"].Value = "Estado";
            ws.Cells["F1"].Value = "Calificativo";
            ws.Cells["G1"].Value = "Emisión de Informe (Último Día)";
            ws.Cells["H1"].Value = "Emisión de Informe (Ejecución Real)";

            ws.Cells["A1:H1"].Style.WrapText = true;
            ws.Cells["A1:H1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:H1"].Style.Font.Size = 10;
            ws.Cells["A1:H1"].Style.Font.Bold = true;
            ws.Cells["A1:H1"].Style.Font.Name = "Calibri";
            ws.Cells["A1:H1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:H1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:H1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(34, 43, 53));
            ws.Cells["A1:H1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            int rowStart = 2;
            foreach (var item in reporteProyectosRiesgoAlto)
            {
                ws.Cells[string.Format("A{0}:H{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:H{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:H{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:H{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Codigo;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Nombre_Proyecto;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.FechaInicioEjecución;
                ws.Cells[string.Format("C{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.FechaFinEjecución;
                ws.Cells[string.Format("D{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Estado;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Calificativo;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Emisión_de__Informe__Último_Día_;
                ws.Cells[string.Format("G{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Emisión_de__Informe__Ejecución_Real_;
                ws.Cells[string.Format("H{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                rowStart++;
            }

            ws.Cells["A1:H1"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= ReporteProyectosRiesgoAlto al " + dia + "." + mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        public void DescargarReporteObsRiesgoAlto(string fechaCorte, int idPlanAnual)
        {
            var dia = fechaCorte.Substring(0, 2);
            var mes = fechaCorte.Substring(3, 2);
            var anio = fechaCorte.Substring(6, 4);

            List<SP_RA_REPORTE_OBS_RIESGO_ALTO_Result> reporteObsRiesgoAlto = db2.SP_RA_REPORTE_OBS_RIESGO_ALTO(fechaCorte, idPlanAnual).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Detalle de Obs Riesgo C-A");

            ws.Cells["A1"].Value = "Código del Proyecto";
            ws.Cells["B1"].Value = "Nombre del Proyecto";
            ws.Cells["C1"].Value = "Título de Observación";
            ws.Cells["D1"].Value = "Observación";
            ws.Cells["E1"].Value = "Recomendación";
            ws.Cells["F1"].Value = "Plan de Acción";
            ws.Cells["G1"].Value = "Estado";
            ws.Cells["H1"].Value = "Riesgo";
            ws.Cells["I1"].Value = "Fecha Vencimiento";
            ws.Cells["J1"].Value = "Unidad Responsable";
            ws.Cells["K1"].Value = "Estado Actualizado";
            ws.Cells["L1"].Value = "ID";
            ws.Cells["M1"].Value = "Negocio";

            ws.Cells["A1:M1"].Style.WrapText = true;
            ws.Cells["A1:M1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A1:M1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A1:M1"].Style.Font.Size = 10;
            ws.Cells["A1:M1"].Style.Font.Bold = true;
            ws.Cells["A1:M1"].Style.Font.Name = "Calibri";
            ws.Cells["A1:M1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:M1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:M1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A1:M1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells["A1:M1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A1:M1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(34, 43, 53));
            ws.Cells["A1:M1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

            int rowStart = 2;
            foreach (var item in reporteObsRiesgoAlto)
            {
                ws.Cells[string.Format("A{0}:M{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:M{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:M{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:M{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.PROYECTO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.NOMBRE_PROYECTO;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.TITULO_OBSERVACION;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.OBSERVACION;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.RECOMENDACION;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.PRIMERA_RESPUESTA;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.ESTADO;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.RIESGO;
                ws.Cells[string.Format("I{0}", rowStart)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.FECHA_DE_VENCIMIENTO;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.UNIDAD_RESPONSABLE;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.SITUACION;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.NEGOCIO;

                rowStart++;
            }

            ws.Cells["A1:M1"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= ReporteObsRiesgoAlto al " + dia + "." + mes + "." + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }
}