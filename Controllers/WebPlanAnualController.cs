using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;
using System.Globalization;
using WebTIGA.Autorizacion;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using OfficeOpenXml;
using System.Transactions;
using OfficeOpenXml.Style;
using Newtonsoft.Json;
namespace WebTIGA.Controllers
{
    [Logueado]
    public class WebPlanAnualController : Controller
    {

        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos modelDB = new ContenedorModelos();

        private readonly List<string> listaSN = new List<string> { "SI", "NO" };
        private readonly List<string> listaTipoEvaluacion = new List<string> { "COLABORATIVO", "MESA", "TRADICIONAL" };
        private readonly List<string> listaUltimoCalificativo = new List<string> { "Sin Calificativo", "Aceptable", "Regular", "Satisfactorio" };
        private readonly List<string> listaFaseSOX = new List<string> { "No Aplica", "I", "II", "III" };
        private readonly List<string> listaEquipoResponsable = new List<string> { "Auditoría de Procesos", "Auditoría de Procesos de Seguros", "Auditoría de Prestación de Salud", "Auditoría de Procesos de Tecnología", "Calidad" };


        // GET: WebPlanAnual

        // GET: WebPlanAnual
        public ActionResult Inicio()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            return View();
        }

        public ActionResult ElaboracionPlanAnual()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            return View();
        }

        public ActionResult ElaboracionPlanDinamico()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            return View();
        }


        public ActionResult EjecucionPlanAnual(int? pa = null)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.ID_Plan_Anual >= 4) select p).OrderByDescending(p => p.ID_Plan_Anual);

            modelDB.DPA_Plan_Anual = planAnual.ToList();



            pa = pa ?? 14;

            modelDB.fn_TG_Obtener_Proyectos_v2 = db2.fn_TG_Obtener_Proyectos_v2(pa);





            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();


            ViewBag.PlanAnualEjecucion = planAnualSeleccionado;




            return View(modelDB);
        }

        public void EjecucionPlanAnualReporte(int? pa = null)
        {

            pa = pa ?? 14;




            List<fn_TG_Obtener_Proyectos_v1_Result> reporte = db2.fn_TG_Obtener_Proyectos_v1(pa).ToList();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteProyectos");



            ws.Cells["A1"].Value = "ID";
            ws.Cells["B1"].Value = "Código";
            ws.Cells["C1"].Value = "Riesgo";
            ws.Cells["D1"].Value = "Compañía";
            ws.Cells["E1"].Value = "Macroproceso";
            ws.Cells["F1"].Value = "Plan";
            ws.Cells["G1"].Value = "Evaluación";
            ws.Cells["H1"].Value = "Estado";
            ws.Cells["I1"].Value = "Proceso Evaluado";
            ws.Cells["J1"].Value = "Tipo";
            ws.Cells["K1"].Value = "Criticidad";
            ws.Cells["L1"].Value = "Criticidad_Valor";
            ws.Cells["M1"].Value = "Fecha Inicio Plan";
            ws.Cells["N1"].Value = "Fecha Fin Plan";
            ws.Cells["O1"].Value = "Fecha Inicio Ejecucion";
            ws.Cells["P1"].Value = "Fecha Fin Ejecucion";
            ws.Cells["Q1"].Value = "Días Plan GA";
            ws.Cells["R1"].Value = "Días Ejecutado GA";
            ws.Cells["S1"].Value = "Auditor 1 Plan";
            ws.Cells["T1"].Value = "Auditor 2 Plan";
            ws.Cells["U1"].Value = "Auditor 3 Plan";
            ws.Cells["V1"].Value = "Auditor 4 Plan";
            ws.Cells["W1"].Value = "Auditor 5 Plan";
            ws.Cells["X1"].Value = "SCRUM Máster Plan";
            ws.Cells["Y1"].Value = "Proyect Owner Plan";
            ws.Cells["Z1"].Value = "Auditor 1 Ejecución";
            ws.Cells["AA1"].Value = "Auditor 2 Ejecución";
            ws.Cells["AB1"].Value = "Auditor 3 Ejecución";
            ws.Cells["AC1"].Value = "Auditor 4 Ejecución";
            ws.Cells["AD1"].Value = "Auditor 5 Ejecución";
            ws.Cells["AE1"].Value = "SCRUM Máster Ejecución";
            ws.Cells["AF1"].Value = "Proyect Owner Ejecución";
            ws.Cells["AG1"].Value = "Gerente";
            ws.Cells["AH1"].Value = "Fecha de Envío";
            ws.Cells["AI1"].Value = "Enviado por";
            ws.Cells["AJ1"].Value = "PAC";
            ws.Cells["AK1"].Value = "Anexo 30";
            ws.Cells["AL1"].Value = "CAI";
            ws.Cells["AM1"].Value = "Cuatrimestral";
            ws.Cells["AN1"].Value = "Observaciones";
            ws.Cells["AO1"].Value = "Fecha de Bloqueo";
            ws.Cells["AP1"].Value = "Año";
            ws.Cells["AQ1"].Value = "Fecha de Emisíon";
            ws.Cells["AR1"].Value = "Calificativo";
            ws.Cells["AS1"].Value = "% Calificativo";
            ws.Cells["AT1"].Value = "IFC Inherente";
            ws.Cells["AU1"].Value = "IFC Residual";
            ws.Cells["AV1"].Value = "Cobertura IFC Residual ";
            ws.Cells["AW1"].Value = "Riesgo Final";
            ws.Cells["AX1"].Value = "Riesgo del trabajo";
            ws.Cells["AY1"].Value = "Riesgo Inherente";
            ws.Cells["AZ1"].Value = "Administración del Riesgo";
            ws.Cells["BA1"].Value = "Entrega Informe Gerencia segun COA Ultimo Dia";
            ws.Cells["BB1"].Value = "Entrega Informe Gerencia Ultimo Dia";
            ws.Cells["BC1"].Value = "Entrega Informe Gerencia Ejecucion Real";
            ws.Cells["BD1"].Value = "Emision Borrador Informe Ultimo Dia";
            ws.Cells["BE1"].Value = "Emision Borrador Informe Ejecucion Real";
            ws.Cells["BF1"].Value = "Emision Informe Ultimo Dia";
            ws.Cells["BG1"].Value = "Emision Informe Ejecucion Real";


            var range = ws.Cells["A1:BG1"];


            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#B0E0E6")); // Color celeste pastel

            range.Style.Font.Bold = true;


            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            range.Style.WrapText = true;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.Font.Size = 8;
            range.Style.Font.Name = "Arial";








            int rowStart = 2;
            foreach (var item in reporte)


            {

                ws.Cells[string.Format("A{0}:BG{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:BG{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:BG{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:BG{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                ws.Cells[string.Format("A{0}", rowStart)].Value = item.IdProyecto;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Codigo;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Riesgo;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Compania;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Macroproceso;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Plan;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Evaluacion;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Estado;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.ProcesoEvaluado;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.Tipo;

                ws.Cells[string.Format("K{0}", rowStart)].Value = item.Criticidad;
                ws.Cells[string.Format("L{0}", rowStart)].Value = Convert.ToInt32(item.Criticidad_num);


                ws.Cells[string.Format("M{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.FechaInicioPlan;
                ws.Cells[string.Format("N{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("N{0}", rowStart)].Value = item.FechaFinPlan;
                ws.Cells[string.Format("O{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.FechaInicioEjecucion;
                ws.Cells[string.Format("P{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.FechaFinEjecucion;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = Convert.ToInt32(item.DiasPlanGA);
                ws.Cells[string.Format("R{0}", rowStart)].Value = Convert.ToInt32(item.DiasEjecutadoGA);

                ws.Cells[string.Format("S{0}", rowStart)].Value = item.Auditor1_plan;
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Auditor2_plan;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Auditor3_plan;
                ws.Cells[string.Format("V{0}", rowStart)].Value = item.Auditor4_plan;
                ws.Cells[string.Format("W{0}", rowStart)].Value = item.Auditor5_plan;



                ws.Cells[string.Format("X{0}", rowStart)].Value = item.AuditoresSenior_plan;
                ws.Cells[string.Format("Y{0}", rowStart)].Value = item.GerenteAdjunto_plan;

                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.Auditor1_ejecucion;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = item.Auditor2_ejecucion;
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.Auditor3_ejecucion;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = item.Auditor4_ejecucion;
                ws.Cells[string.Format("AD{0}", rowStart)].Value = item.Auditor5_ejecucion;



                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.AuditoresSenior_ejecucion;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.GerenteAdjunto_ejecucion;
                ws.Cells[string.Format("AG{0}", rowStart)].Value = item.Gerente;

                ws.Cells[string.Format("AH{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";

                ws.Cells[string.Format("AH{0}", rowStart)].Value = item.FechaEnvio;
                ws.Cells[string.Format("AI{0}", rowStart)].Value = item.EnviadoPor;

                ws.Cells[string.Format("AJ{0}", rowStart)].Value = Convert.ToInt32(item.PAC);
                ws.Cells[string.Format("AK{0}", rowStart)].Value = Convert.ToInt32(item.Anexo30);
                ws.Cells[string.Format("AL{0}", rowStart)].Value = item.CAI;
                ws.Cells[string.Format("AM{0}", rowStart)].Value = item.Cuatrimestral;


                ws.Cells[string.Format("AN{0}", rowStart)].Value = item.Observaciones;
                ws.Cells[string.Format("AO{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("AO{0}", rowStart)].Value = item.FechaBloqueo;

                ws.Cells[string.Format("AP{0}", rowStart)].Value = Convert.ToInt32(item.Anio);
                ws.Cells[string.Format("AQ{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("AQ{0}", rowStart)].Value = item.FechaEmision;
                ws.Cells[string.Format("AR{0}", rowStart)].Value = item.Calificativo;

                ws.Cells[string.Format("AS{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("AS{0}", rowStart)].Value = item.Calificativo_;

                ws.Cells[string.Format("AT{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("AT{0}", rowStart)].Value = item.IFC_Inherente;
                ws.Cells[string.Format("AU{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("AU{0}", rowStart)].Value = item.IFC_Residual;
                ws.Cells[string.Format("AV{0}", rowStart)].Style.Numberformat.Format = "0.00";
                ws.Cells[string.Format("AV{0}", rowStart)].Value = item.Cobertura_IFC_Residual;

                ws.Cells[string.Format("AW{0}", rowStart)].Value = item.Riesgo_Final;
                ws.Cells[string.Format("AX{0}", rowStart)].Value = item.Riesgo_Del_Trabajo;
                ws.Cells[string.Format("AY{0}", rowStart)].Value = item.Riesgo_Inherente;
                ws.Cells[string.Format("AZ{0}", rowStart)].Value = item.Administracion_Del_Riesgo;

                ws.Cells[string.Format("BA{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BA{0}", rowStart)].Value = item.Entrega_Informe_Gerencia_segun_COA_Ultimo_Dia;

                ws.Cells[string.Format("BB{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BB{0}", rowStart)].Value = item.Entrega_Informe_Gerencia_Ultimo_Dia;

                ws.Cells[string.Format("BC{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BC{0}", rowStart)].Value = item.Entrega_Informe_Gerencia_Ejecucion_Real;

                ws.Cells[string.Format("BD{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BD{0}", rowStart)].Value = item.Emision_Borrador_Informe_Ultimo_Dia;
                ws.Cells[string.Format("BE{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BE{0}", rowStart)].Value = item.Emision_Borrador_Informe_Ejecucion_Real;

                ws.Cells[string.Format("BF{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BF{0}", rowStart)].Value = item.Emision_Informe_Ultimo_Dia;

                ws.Cells[string.Format("BG{0}", rowStart)].Style.Numberformat.Format = "dd/mm/yyyy";
                ws.Cells[string.Format("BG{0}", rowStart)].Value = item.Emision_Informe_Ejecucion_Real;




                rowStart++;
            }

            int anchoColumnas = 30;


            for (int i = 1; i <= ws.Dimension.End.Column; i++)
            {
                ws.Column(i).Width = anchoColumnas;
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte_Proyectos.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }


        public ActionResult PV_DetalleProyecto(int pa, int id)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            modelDB.SP_DD_Proyecto_Obtener_detalle_v7 = db2.SP_DD_Proyecto_Obtener_detalle_v7(id);



            var proyectos = (from p in db2.TG_Proyecto.Where(p => p.IdPlanAnual == pa) select p).OrderBy(p => p.Codigo);

            modelDB.TG_Proyecto = proyectos.ToList();


            var compañias = db2.TG_Proyecto
                                           .Select(o => o.Compañia)
                                            .Distinct()
                                           .ToList();


            ViewBag.Compañias = compañias;



            var procesoevaluado = (from p in db2.TG_Proceso_Evaluado select p).OrderBy(p => p.Nombre);

            modelDB.TG_Proceso_Evaluado = procesoevaluado.ToList();

            var persona = (from p in db2.Persona.Where(p => p.Activo == 1 && (p.Función.Contains("03") || p.Función.Contains("02") || p.Función.Contains("04"))) select p).OrderBy(p => p.NombreSharepoint);

            modelDB.Persona = persona.ToList();

            var enviadopor = (from p in db2.Persona.Where(p => p.Activo == 1 && (p.Función.Contains("01") || p.Función.Contains("03") || p.Función.Contains("02") || p.Función.Contains("04"))) select p).OrderBy(p => p.NombreSharepoint);

            ViewBag.enviadopor = enviadopor.ToList();

            var auditores = (from p in db2.Persona.Where(p => p.Activo == 1 && p.Función.Contains("01")) select p).OrderBy(p => p.NombreSharepoint);

            ViewBag.auditores = auditores.ToList();

            var auditores_2 = (from p in db2.Persona.Where(p => p.Activo == 1 && p.Función.Contains("01")) select p).OrderBy(p => p.NombreSharepoint);

            ViewBag.auditores_2 = auditores_2.ToList();

            var riesgoBCP = (from p in db2.TG_RiesgoBCP select p).OrderBy(p => p.Nombre);

            modelDB.TG_RiesgoBCP = riesgoBCP.ToList();

            var riesgoinherente = (from p in db2.TG_RiesgoInherente select p).OrderBy(p => p.Nombre);

            modelDB.TG_RiesgoInherente = riesgoinherente.ToList();

            var administraciondelriesgo = (from p in db2.TG_AdministraciónDelRiesgo select p).OrderBy(p => p.Nombre);

            modelDB.TG_AdministraciónDelRiesgo = administraciondelriesgo.ToList();

            var riesgodeltrabajo = (from p in db2.TG_Riesgo_Del_Trabajo select p).OrderBy(p => p.Nombre);

            modelDB.TG_Riesgo_Del_Trabajo = riesgodeltrabajo.ToList();

            var actividades_equipo = (from p in db2.SP_DD_Proyecto_Actividad_Informe_Obtener_v4(id).Where(p => p.Responsable == "Jefe de Auditoría" || p.Responsable == "Auditor") select p);

            ViewBag.actividades_equipo = actividades_equipo.ToList();

            var actividades_scrum = (from p in db2.SP_DD_Proyecto_Actividad_Informe_Obtener_v4(id).Where(p => p.Responsable == "SubGerente") select p);

            ViewBag.actividades_scrum = actividades_scrum.ToList();

            var actividades_po = (from p in db2.SP_DD_Proyecto_Actividad_Informe_Obtener_v4(id).Where(p => p.Responsable == "Gerente") select p);

            ViewBag.actividades_po = actividades_po.ToList();

            var justificacion = (from p in db2.TG_Actividad_Justificacion.Where(p => p.Activo == 1) select p).OrderBy(p => p.Texto);

            ViewBag.justificacion = justificacion.ToList();

            var proyecto_rc = (from p in db2.TG_Proyecto.Where(p => p.IdProyecto == id) select p.Codigo).FirstOrDefault();
            string codigo_modificado = proyecto_rc.Replace(" ", "");


            var riesgo_control = (from p in db2.TG_Proyecto_Riesgo_Control.Where(p => p.Referencia_del_proceso == codigo_modificado) select p).OrderBy(p => p.NroControl);
            modelDB.TG_Proyecto_Riesgo_Control = riesgo_control.ToList();

            modelDB.fn_TG_Get_Mapa_Aseguramiento_v3_Result = db2.fn_TG_Get_Mapa_Aseguramiento_v3(codigo_modificado);



            return PartialView(modelDB);
        }

        [HttpPost]
        public ActionResult PV_EliminarProyecto(int id)
        {
            try
            {



                int filasAfectadas = db2.SP_TG_Proyecto_del(id);


                if (filasAfectadas > 2)
                {

                    return Json(new { success = true, message = "Cambios guardados exitosamente" });
                }
                else
                {

                    return Json(new { success = false, message = "No se realizaron cambios o ocurrió un error." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }





        }


        [HttpPost]
        public ActionResult PV_Actividad_Guardar(
            int idproyacti,
           string estado,
           string fechaejecreal, string fecharevision,
           string revisor, int? justificacion

           )
        {
            try
            {

                justificacion = justificacion ?? 0;
                if (string.IsNullOrEmpty(fechaejecreal))
                {
                    fechaejecreal = null;
                }



                int filasAfectadas = db2.SP_TG_Update_Actividad_v2(idproyacti, estado, fechaejecreal, fecharevision, revisor, justificacion);


                if (filasAfectadas > 0)
                {

                    return Json(new { success = true, message = "Cambios guardados exitosamente" });
                }
                else
                {

                    return Json(new { success = false, message = "No se realizaron cambios o ocurrió un error." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult PV_Proyecto_Guardar(
          int idproyecto,
               int idplananual,
               string compania,
               int? proceso_evaluado,
               string fecha_bloqueo,
                int? cant_pruebas_real,
                int? pac,
                int? mapa_aseg,
               int? anexo30,
               string fecha_reemplazo,
                int? proyecto_reemplazo,
                string fecha_envio,
               string enviado_por,
                string cuatrimestral,
               string cai,
               int? cantidad_sprint,
                int? riesgo_trabajo,
                int? riesgo_inherente,
                int? riesgo_bcp,
                int? admin_riesgo,
                string observaciones,
                string project_owner,
                string scrum_master,
                string scrum1,
                 string scrum2,
                 string scrum3,
                 string scrum4,
                 string scrum5,
                 string inicio_ejecucion,
                 string fin_ejecucion,
                int? dias_ejecucion,
                 string fecha_emision,
                 string inicio_aprobada


          )
        {
            string nombre = Convert.ToString(Session["usuario"]);
            try
            {



                if (string.IsNullOrEmpty(fecha_bloqueo))
                {
                    fecha_bloqueo = null;
                }
                if (string.IsNullOrEmpty(fecha_reemplazo))
                {
                    fecha_reemplazo = null;
                }
                if (string.IsNullOrEmpty(fecha_envio))
                {
                    fecha_envio = null;
                }
                if (string.IsNullOrEmpty(enviado_por))
                {
                    enviado_por = null;
                }
                if (string.IsNullOrEmpty(cuatrimestral))
                {
                    cuatrimestral = null;
                }
                if (string.IsNullOrEmpty(cai))
                {
                    cai = null;
                }
                if (string.IsNullOrEmpty(observaciones))
                {
                    observaciones = null;
                }
                if (string.IsNullOrEmpty(project_owner))
                {
                    project_owner = null;
                }
                if (string.IsNullOrEmpty(scrum_master))
                {
                    scrum_master = null;
                }
                if (string.IsNullOrEmpty(scrum1))
                {
                    scrum1 = null;
                }
                if (string.IsNullOrEmpty(scrum2))
                {
                    scrum2 = null;
                }
                if (string.IsNullOrEmpty(scrum3))
                {
                    scrum3 = null;
                }
                if (string.IsNullOrEmpty(scrum4))
                {
                    scrum4 = null;
                }
                if (string.IsNullOrEmpty(scrum5))
                {
                    scrum5 = null;
                }
                if (string.IsNullOrEmpty(inicio_ejecucion))
                {
                    inicio_ejecucion = null;
                }
                if (string.IsNullOrEmpty(fin_ejecucion))
                {
                    fin_ejecucion = null;
                }
                if (string.IsNullOrEmpty(fecha_emision))
                {
                    fecha_emision = null;
                }
                if (string.IsNullOrEmpty(inicio_aprobada))
                {
                    inicio_aprobada = null;
                }

                int filasAfectadas = db2.TG_Update_Proyecto_v7(
                    idproyecto, idplananual,
                    compania, proceso_evaluado, fecha_bloqueo, cant_pruebas_real,
                    pac,mapa_aseg, anexo30, fecha_reemplazo, proyecto_reemplazo, fecha_envio,
                    enviado_por, cai, cuatrimestral, cantidad_sprint, riesgo_trabajo, riesgo_inherente,
                    riesgo_bcp, admin_riesgo, observaciones, project_owner, scrum_master,
                    scrum1, scrum2, scrum3, scrum4, scrum5, inicio_ejecucion, fin_ejecucion, dias_ejecucion, fecha_emision, nombre, inicio_aprobada
                    );




                if (filasAfectadas > 0)
                {

                    return Json(new { success = true, message = "Cambios guardados exitosamente" });
                }
                else
                {

                    return Json(new { success = false, message = "No se realizaron cambios o ocurrió un error." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult PV_Guardar_Cambios_Riesgos_Controles(List<ChangeModel> cambios)
        {
            try
            {
                if (cambios != null && cambios.Count > 0)
                {
                    foreach (var cambio in cambios)
                    {
                        var registro = db2.TG_Proyecto_Riesgo_Control.Find(cambio.id);
                        if (registro != null)
                        {
                            switch (cambio.field)
                            {
                                case "C1eraLinea":
                                    registro.C1eraLinea = cambio.value;
                                    break;
                                case "C2daLinea":
                                    registro.C2daLinea = cambio.value;
                                    break;
                                case "C3eraLinea":
                                    registro.C3eraLinea = cambio.value;
                                    break;
                                case "Riesgo_1eraLinea":
                                    registro.Riesgo_1eraLinea = cambio.value;
                                    break;
                                case "Riesgo_2daLinea":
                                    registro.Riesgo_2daLinea = cambio.value;
                                    break;
                                case "Control_1eraLinea":
                                    registro.Control_1eraLinea = cambio.value;
                                    break;
                                case "Control_2daLinea":
                                    registro.Control_2daLinea = cambio.value;
                                    break;
                            }
                            db2.Entry(registro).State = EntityState.Modified;
                        }
                    }
                    db2.SaveChanges();

                    return Json(new { success = true, message = "Cambios guardados exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "No se realizaron cambios o ocurrió un error." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }
        }
        public class ChangeModel
        {
            public int id { get; set; }
            public string field { get; set; }
            public int value { get; set; }
        }

        [HttpPost]
        public ActionResult PV_Proyecto_Crear(

             string codigo,
               string evaluacion,
                int? plan_anual,
                int? estado,
                string riesgo,
                string plan,
                int? compañia,
                 int? pruebas_plan,
                 string macroproceso,
                 string tipo,
                 int? criticidad,
                 int? proceso_evaluado,
                 int? riesgo_bcp,
                 int? riesgo_inherente,
                 int? admin_riesgo,
                 string fecha_inicio_plan,
                 string fecha_fin_plan,
                 int? dias_plan,
                 string emision_informe,
                string correo_inicio,
                string enviado_por,
                string fecha_inicio_ejec,

                string fecha_fin_ejec,
                int? dias_ejec,
                 int? pac,
                 int? mapa_aseg,
                 int? anexo30,
                 string po,
                 string scrum_master,
                 string auditor1,
                 string auditor2,
                 string auditor3,
                 string auditor4,
                 string auditor5,
                 string observaciones


          )
        {
            string nombre = Convert.ToString(Session["usuario"]);
            try
            {



                if (string.IsNullOrEmpty(codigo))
                {
                    codigo = null;
                }
                if (string.IsNullOrEmpty(evaluacion))
                {
                    evaluacion = null;
                }
                if (string.IsNullOrEmpty(riesgo))
                {
                    riesgo = null;
                }
                if (string.IsNullOrEmpty(plan))
                {
                    plan = null;
                }
                if (string.IsNullOrEmpty(macroproceso))
                {
                    macroproceso = null;
                }
                if (string.IsNullOrEmpty(tipo))
                {
                    tipo = null;
                }
                if (string.IsNullOrEmpty(fecha_inicio_plan))
                {
                    fecha_inicio_plan = null;
                }
                if (string.IsNullOrEmpty(fecha_fin_plan))
                {
                    fecha_fin_plan = null;
                }
                if (string.IsNullOrEmpty(emision_informe))
                {
                    emision_informe = null;
                }
                if (string.IsNullOrEmpty(correo_inicio))
                {
                    correo_inicio = null;
                }
                if (string.IsNullOrEmpty(enviado_por))
                {
                    enviado_por = null;
                }
                if (string.IsNullOrEmpty(fecha_inicio_ejec))
                {
                    fecha_inicio_ejec = null;
                }
                if (string.IsNullOrEmpty(fecha_fin_ejec))
                {
                    fecha_fin_ejec = null;
                }
                if (string.IsNullOrEmpty(po))
                {
                    po = null;
                }
                if (string.IsNullOrEmpty(scrum_master))
                {
                    scrum_master = null;
                }
                if (string.IsNullOrEmpty(auditor1))
                {
                    auditor1 = null;
                }
                if (string.IsNullOrEmpty(auditor2))
                {
                    auditor2 = null;
                }
                if (string.IsNullOrEmpty(auditor3))
                {
                    auditor3 = null;
                }
                if (string.IsNullOrEmpty(auditor4))
                {
                    auditor4 = null;
                }
                if (string.IsNullOrEmpty(auditor5))
                {
                    auditor5 = null;
                }
                if (string.IsNullOrEmpty(observaciones))
                {
                    observaciones = null;
                }



                int filasAfectadas1 = db2.SP_TG_Proyecto_Ins_v5(
                    codigo, evaluacion, plan_anual, estado, riesgo, plan, compañia, pruebas_plan, macroproceso, tipo,
                    criticidad, proceso_evaluado, riesgo_bcp, riesgo_inherente, admin_riesgo, fecha_inicio_plan,
                    fecha_fin_plan, dias_plan, emision_informe, correo_inicio, enviado_por, fecha_inicio_ejec,
                    fecha_fin_ejec, dias_ejec, pac,mapa_aseg, anexo30, po, scrum_master, auditor1, auditor2, auditor3, auditor4, auditor5, observaciones)
                    ;
                int filasAafectadas2 = db2.SP_TG_Proyecto_Actividad_Informe_Ins_v2(codigo);




                int totalfilas = filasAafectadas2 + filasAfectadas1;

                if (totalfilas > 1)
                {

                    return Json(new { success = true, message = "Cambios guardados exitosamente" });
                }
                else
                {

                    return Json(new { success = false, message = "No se realizaron cambios o ocurrió un error." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }
        }


        public ActionResult PV_CrearProyecto()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.ID_Plan_Anual >= 4) select p).OrderByDescending(p => p.ID_Plan_Anual);

            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var compañia = (from p in db2.TG_Informe.Where(x => x.Activo == 1) select p).OrderBy(p => p.Nombre);

            modelDB.TG_Informe = compañia.ToList();

            var procesoevaluado = (from p in db2.TG_Proceso_Evaluado select p).OrderBy(p => p.Nombre);

            modelDB.TG_Proceso_Evaluado = procesoevaluado.ToList();

            var riesgoBCP = (from p in db2.TG_RiesgoBCP select p).OrderBy(p => p.Nombre);

            modelDB.TG_RiesgoBCP = riesgoBCP.ToList();

            var riesgoinherente = (from p in db2.TG_RiesgoInherente select p).OrderBy(p => p.Nombre);

            modelDB.TG_RiesgoInherente = riesgoinherente.ToList();

            var administraciondelriesgo = (from p in db2.TG_AdministraciónDelRiesgo select p).OrderBy(p => p.Nombre);

            modelDB.TG_AdministraciónDelRiesgo = administraciondelriesgo.ToList();

            var persona = (from p in db2.Persona.Where(p => p.Activo == 1 && (p.Función.Contains("03") || p.Función.Contains("02") || p.Función.Contains("04"))) select p).OrderBy(p => p.NombreSharepoint);

            modelDB.Persona = persona.ToList();

            var auditores = (from p in db2.Persona.Where(p => p.Activo == 1 && p.Función.Contains("01")) select p).OrderBy(p => p.NombreSharepoint);

            ViewBag.auditores = auditores.ToList();

            var estados = (from p in db2.TG_Estados_Proyecto select p).OrderBy(p => p.Nombre);

            modelDB.TG_Estados_Proyecto = estados.ToList();





            return PartialView(modelDB);
        }

        public ActionResult PV_DetalleActividad(int idactividad, int idproyecto)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);



            var actividad = (from p in db2.SP_DD_Proyecto_Actividad_Informe_Obtener_v4(idproyecto).Where(p => p.IdProyectoActividadInforme == idactividad) select p);

            ViewBag.actividad = actividad.ToList();

            var justificacion = (from p in db2.TG_Actividad_Justificacion.Where(p => p.Activo == 1) select p).OrderBy(p => p.Texto);



            modelDB.TG_Actividad_Justificacion = justificacion.ToList();


            return PartialView(modelDB);
        }


        //VISTA DE CARGA DE PLAN ANUAL
        public ActionResult CargaPlanAnual(int? pa, int? ne)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.Anio_Plan >= 2022) select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();
            int idNegocioSeleccionado = ne != null ? (int)ne : negocios.First().ID_Negocio;

            ViewBag.PlanAnualSeleccionado = planAnualSeleccionado;
            ViewBag.idNegocioSeleccionado = idNegocioSeleccionado;

            modelDB.SP_DPA_Carga_Plan = db2.SP_DPA_Carga_Plan(planAnualSeleccionado.ID_Plan_Anual, idNegocioSeleccionado);

            return View(modelDB);
        }
        public ActionResult PV_CrearPlan()
        {
            var planAnual = (from p in db2.DPA_Plan_Anual where p.Anio_Plan >= 2022 select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            ViewBag.anioPlanValido = DateTime.Today.Year + 1;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearPlan(int idPlanAnterior, int anioNuevoPlan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DPA_Plan_Anual planAnualAnterior = db2.DPA_Plan_Anual.Find(idPlanAnterior);
                    DPA_Plan_Anual planAnual = db2.DPA_Plan_Anual.Where(p => p.Anio_Plan == anioNuevoPlan).FirstOrDefault();

                    if (planAnual != null)
                    {
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "Ya existe un plan cargado para el valor ingresado";
                        return RedirectToAction("CargaPlanAnual");
                    }
                    else
                    {
                        DPA_Plan_Anual nuevoPlan = new DPA_Plan_Anual
                        {
                            Anio_Plan = anioNuevoPlan,
                            Fecha_Inicio = new DateTime(anioNuevoPlan, 1, 1),
                            Fecha_Fin = new DateTime(anioNuevoPlan, 12, 31),
                            Flag_Edicion = 1
                        };

                        db2.DPA_Plan_Anual.Add(nuevoPlan);
                        db2.SaveChanges();

                        db2.SP_DPA_Crear_Nuevo_Plan(idPlanAnterior, anioNuevoPlan);
                        db2.SaveChanges();

                        TempData["Icon"] = "success";
                        TempData["Title"] = "Creación realizada";
                        TempData["Text"] = "Se ha realizado la creacion del plan anual de forma satisfactoria";
                        return RedirectToAction("CargaPlanAnual");
                    }
                }
            }
            catch (RetryLimitExceededException)
            {
                TempData["Icon"] = "error";
                TempData["Title"] = "Error";
                TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                return RedirectToAction("CargaPlanAnual");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarPlanAnual(int idPlanAnual)
        {
            DPA_Plan_Anual planAnual = db2.DPA_Plan_Anual.Find(idPlanAnual);

            if (planAnual != null)
            {
                db2.DPA_Plan_Anual.Remove(planAnual);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Eliminación realizada";
                TempData["Text"] = "Se ha eliminado el plan anual de la base de datos";
                return RedirectToAction("CargaPlanAnual");
            }
            else
            {
                TempData["Icon"] = "error";
                TempData["Title"] = "Error";
                TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
                return RedirectToAction("CargaPlanAnual");
            }
        }


        // VISTA DE GESTIÓN DEL UNIVERSO
        public ActionResult GestionEvaluaciones(int? idNegocio)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var negocios = (from n in db2.DPA_Negocio select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            DPA_Negocio negocioSeleccionado = idNegocio != null ? db2.DPA_Negocio.Find(idNegocio) : negocios.First();
            ViewBag.NegocioSeleccionado = negocioSeleccionado;

            modelDB.DPA_Evaluacion = db2.DPA_Evaluacion.Where(e => e.ID_Negocio == negocioSeleccionado.ID_Negocio);

            return View(modelDB);
        }
        public ActionResult PV_CrearEvaluacion(int idNegocio)
        {
            ViewBag.NegocioSeleccionado = db2.DPA_Negocio.Find(idNegocio);

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearEvaluacion(DPA_Evaluacion nuevaEvaluacion)
        {
            if (ModelState.IsValid)
            {
                nuevaEvaluacion.Evaluacion = nuevaEvaluacion.Evaluacion.Trim().ToUpper();
                db2.DPA_Evaluacion.Add(nuevaEvaluacion);
                db2.SaveChanges();
                AgregarLogEvaluacion(nuevaEvaluacion.ID_Evaluacion, nuevaEvaluacion.Evaluacion, nuevaEvaluacion.ID_Negocio, 1);

                TempData["Icon"] = "success";
                TempData["Title"] = "Creación realizada";
                TempData["Text"] = "Se ha realizado la creacion de la evaluacion de forma satisfactoria";
                return RedirectToAction("GestionEvaluaciones", new { idNegocio = nuevaEvaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("GestionEvaluaciones");
        }
        public ActionResult PV_EditarEvaluacion(int idEvaluacion)
        {
            DPA_Evaluacion evaluacion = db2.DPA_Evaluacion.Find(idEvaluacion);
            ViewBag.Evaluacion = evaluacion;
            ViewBag.Negocio = db2.DPA_Negocio.Find(evaluacion.ID_Negocio);

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarEvaluacion(DPA_Evaluacion nuevaEvaluacion)
        {
            if (ModelState.IsValid && nuevaEvaluacion != null)
            {
                var evaluacion = db2.DPA_Evaluacion.Find(nuevaEvaluacion.ID_Evaluacion);

                if (evaluacion.Evaluacion != nuevaEvaluacion.Evaluacion.Trim().ToUpper())
                {
                    AgregarLogCambioNombreEv(evaluacion.ID_Evaluacion, evaluacion.Evaluacion, evaluacion.ID_Negocio, nuevaEvaluacion.Evaluacion.Trim().ToUpper());
                    evaluacion.Evaluacion = nuevaEvaluacion.Evaluacion.Trim().ToUpper();
                }

                db2.Entry(evaluacion).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se ha realizado la edicion de la evaluacion de forma satisfactoria";
                return RedirectToAction("GestionEvaluaciones", new { idNegocio = evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("GestionEvaluaciones");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EliminarEvaluacion(int idEvaluacion)
        {
            DPA_Evaluacion evaluacion = db2.DPA_Evaluacion.Find(idEvaluacion);

            if (evaluacion != null)
            {
                AgregarLogEvaluacion(evaluacion.ID_Evaluacion, evaluacion.Evaluacion, evaluacion.ID_Negocio, 4);
                db2.DPA_Evaluacion.Remove(evaluacion);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Eliminación realizada";
                TempData["Text"] = "Se ha eliminado la evaluacion de la base de datos";
                return RedirectToAction("GestionEvaluaciones", new { idNegocio = evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("GestionEvaluaciones");
        }


        // VISTA DE MANTENIMIENTO DE PLAN ANUAL
        public ActionResult MantenimientoPlan(int? pa, int? ne)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.Anio_Plan >= 2022) select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();
            DPA_Negocio negocioSeleccionado = ne != null ? db2.DPA_Negocio.Find(ne) : negocios.First();

            ViewBag.PlanAnualSeleccionado = planAnualSeleccionado;
            ViewBag.NegocioSeleccionado = negocioSeleccionado;
            ViewBag.flagEdicionPlan = planAnualSeleccionado.Flag_Edicion;

            modelDB.SP_DPA_Mantenimiento_Plan = db2.SP_DPA_Mantenimiento_Plan(planAnualSeleccionado.ID_Plan_Anual, negocioSeleccionado.ID_Negocio);

            return View(modelDB);
        }
        public ActionResult PV_CrearEvaluacionPlan(int idPlanAnual, int idNegocio)
        {
            ViewBag.idRol = Session["idRol"];

            modelDB.DD_ProcesoEvaluado = db2.DD_ProcesoEvaluado.Where(p => p.IdNegocios == idNegocio).OrderBy(p => p.Nombre);
            modelDB.DPA_Equipo = db2.DPA_Equipo;
            modelDB.DPA_Riesgo_Asociado = db2.DPA_Riesgo_Asociado.Where(r => r.Estado == 1);

            var listaEvaluacionesPlan = from ep in db2.DPA_Evaluacion_Plan where ep.ID_Plan_Anual == idPlanAnual select ep;

            modelDB.DPA_Evaluacion = db2.DPA_Evaluacion.Where(e => listaEvaluacionesPlan.All(le => le.ID_Evaluacion != e.ID_Evaluacion))
                                                        .Where(e => e.ID_Negocio == idNegocio)
                                                        .OrderBy(e => e.Evaluacion);

            ViewBag.idPlanSel = idPlanAnual;
            ViewBag.idNegocioSel = idNegocio;

            ViewBag.listaSN = listaSN;
            ViewBag.listaTipoEvaluacion = listaTipoEvaluacion;
            ViewBag.listaUltimoCalificativo = listaUltimoCalificativo;
            ViewBag.listaFaseSOX = listaFaseSOX;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearEvaluacionPlan(DPA_Evaluacion nuevaEvaluacion, DPA_Evaluacion_Plan evaluacionPlan)
        {
            if (ModelState.IsValid)
            {
                var evaluacion = db2.DPA_Evaluacion.Find(nuevaEvaluacion.ID_Evaluacion);
                evaluacion.ID_Proceso_Evaluado = nuevaEvaluacion.ID_Proceso_Evaluado;
                evaluacion.Objetivo = nuevaEvaluacion.Objetivo.Trim();
                evaluacion.Centro_Costo = nuevaEvaluacion.Centro_Costo.Trim();
                evaluacion.Atributo_Universo = nuevaEvaluacion.Atributo_Universo.Trim();
                evaluacion.Tipo_Evaluacion = nuevaEvaluacion.Tipo_Evaluacion.Trim();
                evaluacion.ID_Equipo_Responsable = nuevaEvaluacion.ID_Equipo_Responsable;
                evaluacion.PAC = nuevaEvaluacion.PAC.Trim();
                evaluacion.Anexo_30 = nuevaEvaluacion.Anexo_30.Trim();
                evaluacion.Regulatorio = nuevaEvaluacion.Regulatorio.Trim();
                evaluacion.N_SBS = nuevaEvaluacion.N_SBS != null ? nuevaEvaluacion.N_SBS.Trim() : nuevaEvaluacion.N_SBS;
                evaluacion.Evaluacion_SBS = nuevaEvaluacion.Evaluacion_SBS != null ? nuevaEvaluacion.Evaluacion_SBS.Trim() : nuevaEvaluacion.Evaluacion_SBS;
                evaluacion.SOX = nuevaEvaluacion.SOX.Trim();
                evaluacion.Fase_SOX = nuevaEvaluacion.Fase_SOX.Trim();
                evaluacion.Nombre_Mesa = nuevaEvaluacion.Nombre_Mesa != null ? nuevaEvaluacion.Nombre_Mesa.Trim() : nuevaEvaluacion.Nombre_Mesa;
                evaluacion.Comentario = nuevaEvaluacion.Comentario != null ? nuevaEvaluacion.Comentario.Trim() : nuevaEvaluacion.Comentario;
                evaluacion.Squad_COE_Area = nuevaEvaluacion.Squad_COE_Area != null ? nuevaEvaluacion.Squad_COE_Area.Trim() : nuevaEvaluacion.Squad_COE_Area;
                evaluacion.Aplicaciones = nuevaEvaluacion.Aplicaciones != null ? nuevaEvaluacion.Aplicaciones.Trim() : nuevaEvaluacion.Aplicaciones;
                evaluacion.RPA = nuevaEvaluacion.RPA != null ? nuevaEvaluacion.RPA.Trim() : nuevaEvaluacion.RPA;
                evaluacion.Chatbot = nuevaEvaluacion.Chatbot;
                evaluacion.IA = nuevaEvaluacion.IA;
                evaluacion.API = nuevaEvaluacion.API != null ? nuevaEvaluacion.API.Trim() : nuevaEvaluacion.API;
                evaluacion.N_Riesgos = nuevaEvaluacion.N_Riesgos;
                evaluacion.N_Controles = nuevaEvaluacion.N_Controles;
                evaluacion.Accesos_Aplicacion = nuevaEvaluacion.Accesos_Aplicacion != null ? nuevaEvaluacion.Accesos_Aplicacion.Trim() : nuevaEvaluacion.Accesos_Aplicacion;
                evaluacion.Accesos_BD = nuevaEvaluacion.Accesos_BD != null ? nuevaEvaluacion.Accesos_BD.Trim() : nuevaEvaluacion.Accesos_BD;
                evaluacion.Ratio_Dias_Control = nuevaEvaluacion.Ratio_Dias_Control;
                evaluacion.Uso_Analitica_EWP = nuevaEvaluacion.Uso_Analitica_EWP != null ? nuevaEvaluacion.Uso_Analitica_EWP.Trim() : nuevaEvaluacion.Uso_Analitica_EWP;
                evaluacion.Uso_Analitica_Registrado = nuevaEvaluacion.Uso_Analitica_Registrado != null ? nuevaEvaluacion.Uso_Analitica_Registrado.Trim() : nuevaEvaluacion.Uso_Analitica_Registrado;
                evaluacion.Analítica_Tipo_Pruebas = nuevaEvaluacion.Analítica_Tipo_Pruebas != null ? nuevaEvaluacion.Analítica_Tipo_Pruebas.Trim() : nuevaEvaluacion.Analítica_Tipo_Pruebas;
                evaluacion.SIEMBRA = nuevaEvaluacion.SIEMBRA;
                evaluacion.RIEGA = nuevaEvaluacion.RIEGA;
                evaluacion.SIEGA = nuevaEvaluacion.SIEGA;

                AgregarLogEvaluacion(evaluacion.ID_Evaluacion, evaluacion.Evaluacion, evaluacion.ID_Negocio, 2);
                db2.Entry(evaluacion).State = EntityState.Modified;

                evaluacionPlan.Ultimo_Calificativo = evaluacionPlan.Ultimo_Calificativo.Trim();
                db2.DPA_Evaluacion_Plan.Add(evaluacionPlan);
                db2.SaveChanges();
                AgregarLogEvaluacionPlan(evaluacionPlan.ID_Evaluacion_Plan, 1, "Nuevo", 0);

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluación Agregada";
                TempData["Text"] = "Se ha guardado los cambios satisfactoriamente";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }
        public ActionResult PV_EditarEvaluacionPlan(int idEvaluacionPlan, int idNegocio)
        {
            ViewBag.idRol = Session["idRol"];

            modelDB.DD_ProcesoEvaluado = db2.DD_ProcesoEvaluado.Where(p => p.IdNegocios == idNegocio).OrderBy(p => p.Nombre);
            modelDB.DPA_Equipo = db2.DPA_Equipo;
            modelDB.DPA_Riesgo_Asociado = db2.DPA_Riesgo_Asociado.Where(r => r.Estado == 1);

            var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            ViewBag.evaluacionPlan = evaluacionPlan;
            ViewBag.evaluacion = db2.DPA_Evaluacion.Find(evaluacionPlan.ID_Evaluacion);

            ViewBag.listaSN = listaSN;
            ViewBag.listaTipoEvaluacion = listaTipoEvaluacion;
            ViewBag.listaUltimoCalificativo = listaUltimoCalificativo;
            ViewBag.listaFaseSOX = listaFaseSOX;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarEvaluacionPlan(DPA_Evaluacion evaluacionIngresada, DPA_Evaluacion_Plan evalPlanIngresado)
        {
            if (ModelState.IsValid)
            {
                // EVALUACIÓN PLAN
                var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(evalPlanIngresado.ID_Evaluacion_Plan);
                evaluacionPlan.Ultimo_Calificativo = evalPlanIngresado.Ultimo_Calificativo.Trim();
                evaluacionPlan.ID_Riesgo_Asociado = evalPlanIngresado.ID_Riesgo_Asociado;

                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();
                AgregarLogEvaluacionPlan(evaluacionPlan.ID_Evaluacion_Plan, 2, "Campos editados", 0);

                // EVALUACIÓN
                var evaluacion = db2.DPA_Evaluacion.Find(evaluacionIngresada.ID_Evaluacion);
                evaluacion.ID_Proceso_Evaluado = evaluacionIngresada.ID_Proceso_Evaluado;
                evaluacion.Objetivo = evaluacionIngresada.Objetivo.Trim();
                evaluacion.Centro_Costo = evaluacionIngresada.Centro_Costo.Trim();
                evaluacion.Atributo_Universo = evaluacionIngresada.Atributo_Universo.Trim();
                evaluacion.Tipo_Evaluacion = evaluacionIngresada.Tipo_Evaluacion.Trim();
                evaluacion.ID_Equipo_Responsable = evaluacionIngresada.ID_Equipo_Responsable;
                evaluacion.PAC = evaluacionIngresada.PAC.Trim();
                evaluacion.Anexo_30 = evaluacionIngresada.Anexo_30.Trim();
                evaluacion.Regulatorio = evaluacionIngresada.Regulatorio.Trim();
                evaluacion.N_SBS = evaluacionIngresada.N_SBS != null ? evaluacionIngresada.N_SBS.Trim() : evaluacionIngresada.N_SBS;
                evaluacion.Evaluacion_SBS = evaluacionIngresada.Evaluacion_SBS != null ? evaluacionIngresada.Evaluacion_SBS.Trim() : evaluacionIngresada.Evaluacion_SBS;
                evaluacion.SOX = evaluacionIngresada.SOX.Trim();
                evaluacion.Fase_SOX = evaluacionIngresada.Fase_SOX.Trim();
                evaluacion.Nombre_Mesa = evaluacionIngresada.Nombre_Mesa != null ? evaluacionIngresada.Nombre_Mesa.Trim() : evaluacionIngresada.Nombre_Mesa;
                evaluacion.Comentario = evaluacionIngresada.Comentario != null ? evaluacionIngresada.Comentario.Trim() : evaluacionIngresada.Comentario;
                evaluacion.Squad_COE_Area = evaluacionIngresada.Squad_COE_Area != null ? evaluacionIngresada.Squad_COE_Area.Trim() : evaluacionIngresada.Squad_COE_Area;
                evaluacion.Aplicaciones = evaluacionIngresada.Aplicaciones != null ? evaluacionIngresada.Aplicaciones.Trim() : evaluacionIngresada.Aplicaciones;
                evaluacion.RPA = evaluacionIngresada.RPA != null ? evaluacionIngresada.RPA.Trim() : evaluacionIngresada.RPA;
                evaluacion.Chatbot = evaluacionIngresada.Chatbot;
                evaluacion.IA = evaluacionIngresada.IA;
                evaluacion.API = evaluacionIngresada.API != null ? evaluacionIngresada.API.Trim() : evaluacionIngresada.API;
                evaluacion.N_Riesgos = evaluacionIngresada.N_Riesgos;
                evaluacion.N_Controles = evaluacionIngresada.N_Controles;
                evaluacion.Accesos_Aplicacion = evaluacionIngresada.Accesos_Aplicacion != null ? evaluacionIngresada.Accesos_Aplicacion.Trim() : evaluacionIngresada.Accesos_Aplicacion;
                evaluacion.Accesos_BD = evaluacionIngresada.Accesos_BD != null ? evaluacionIngresada.Accesos_BD.Trim() : evaluacionIngresada.Accesos_BD;
                evaluacion.Ratio_Dias_Control = evaluacionIngresada.Ratio_Dias_Control;
                evaluacion.Uso_Analitica_EWP = evaluacionIngresada.Uso_Analitica_EWP != null ? evaluacionIngresada.Uso_Analitica_EWP.Trim() : evaluacionIngresada.Uso_Analitica_EWP;
                evaluacion.Uso_Analitica_Registrado = evaluacionIngresada.Uso_Analitica_Registrado != null ? evaluacionIngresada.Uso_Analitica_Registrado.Trim() : evaluacionIngresada.Uso_Analitica_Registrado;
                evaluacion.Analítica_Tipo_Pruebas = evaluacionIngresada.Analítica_Tipo_Pruebas != null ? evaluacionIngresada.Analítica_Tipo_Pruebas.Trim() : evaluacionIngresada.Analítica_Tipo_Pruebas;
                evaluacion.SIEMBRA = evaluacionIngresada.SIEMBRA;
                evaluacion.RIEGA = evaluacionIngresada.RIEGA;
                evaluacion.SIEGA = evaluacionIngresada.SIEGA;

                if (evaluacion.Evaluacion != evaluacionIngresada.Evaluacion.Trim().ToUpper())
                {
                    AgregarLogCambioNombreEv(evaluacion.ID_Evaluacion, evaluacion.Evaluacion, evaluacion.ID_Negocio, evaluacionIngresada.Evaluacion.Trim().ToUpper());
                    evaluacion.Evaluacion = evaluacionIngresada.Evaluacion.Trim().ToUpper();
                }
                db2.Entry(evaluacion).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se ha guardado los cambios satisfactoriamente";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RetirarEvaluacionPlan(int idEvaluacionPlan)
        {
            var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
            if (evaluacionPlan != null)
            {
                AgregarLogEvaluacionPlan(idEvaluacionPlan, 3, "Retirado", 0);

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluación retirada";
                TempData["Text"] = "Se ha retirado la evaluacion del plan anual";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestablecerEvaluacionPlan(int idEvaluacionPlan)
        {
            DPA_Log_Evaluacion_Plan logEvaluacionPlan = db2.DPA_Log_Evaluacion_Plan.Where(l => l.ID_Evaluacion_Plan == idEvaluacionPlan)
                                                                                    .Where(l => l.ID_Tipo_Operacion == 3)
                                                                                    .First();
            if (logEvaluacionPlan != null)
            {
                var idPlanAnual = logEvaluacionPlan.DPA_Evaluacion_Plan.ID_Plan_Anual;
                var idNegocio = logEvaluacionPlan.DPA_Evaluacion_Plan.DPA_Evaluacion.ID_Negocio;
                db2.DPA_Log_Evaluacion_Plan.Remove(logEvaluacionPlan);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluación restablecida";
                TempData["Text"] = "Se ha restablecido la evaluacion del plan anual";
                return RedirectToAction("MantenimientoPlan", new { pa = idPlanAnual, ne = idNegocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }

        //REEMPLAZAR 

        public ActionResult PV_ReemplazarEvaluacionPlan(int idEvaluacionPlan)
        {
            var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            //modelDB.DD_ProcesoEvaluado = db2.DD_ProcesoEvaluado.Where(p => p.IdNegocios == evaluacionPlan.DPA_Evaluacion.ID_Negocio).OrderBy(p => p.Nombre);
            modelDB.DPA_Equipo = db2.DPA_Equipo;
            modelDB.DPA_Riesgo_Asociado = db2.DPA_Riesgo_Asociado.Where(r => r.Estado == 1);

            ViewBag.evalPlanSel = evaluacionPlan;
            ViewBag.listaEvalPlan = db2.DPA_Evaluacion_Plan.Where(e => e.ID_Plan_Anual == evaluacionPlan.ID_Plan_Anual)
                                                                .Where(e => e.DPA_Evaluacion.ID_Negocio == evaluacionPlan.DPA_Evaluacion.ID_Negocio)
                                                                .OrderBy(e => e.DPA_Evaluacion.Evaluacion).ToList();

            ViewBag.listaSN = listaSN;
            ViewBag.listaTipoEvaluacion = listaTipoEvaluacion;
            ViewBag.listaUltimoCalificativo = listaUltimoCalificativo;
            ViewBag.listaFaseSOX = listaFaseSOX;

            return PartialView(modelDB);
        }






        public ActionResult PV_FusionarEvaluacionPlan(int idEvaluacionPlan)
        {
            var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            modelDB.DD_ProcesoEvaluado = db2.DD_ProcesoEvaluado.Where(p => p.IdNegocios == evaluacionPlan.DPA_Evaluacion.ID_Negocio).OrderBy(p => p.Nombre);
            modelDB.DPA_Equipo = db2.DPA_Equipo;
            modelDB.DPA_Riesgo_Asociado = db2.DPA_Riesgo_Asociado.Where(r => r.Estado == 1);

            ViewBag.evalPlanSel = evaluacionPlan;
            ViewBag.listaEvalPlan = db2.DPA_Evaluacion_Plan.Where(e => e.ID_Plan_Anual == evaluacionPlan.ID_Plan_Anual)
                                                                .Where(e => e.DPA_Evaluacion.ID_Negocio == evaluacionPlan.DPA_Evaluacion.ID_Negocio)
                                                                .OrderBy(e => e.DPA_Evaluacion.Evaluacion).ToList();

            ViewBag.listaSN = listaSN;
            ViewBag.listaTipoEvaluacion = listaTipoEvaluacion;
            ViewBag.listaUltimoCalificativo = listaUltimoCalificativo;
            ViewBag.listaFaseSOX = listaFaseSOX;

            return PartialView(modelDB);
        }

        //REEMPLAZAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_ReemplazarEvaluacionPlan(DPA_Evaluacion_Plan evaluacionPlan, List<DPA_Evaluacion_Plan> listaEvalPlan)
        {
            if (ModelState.IsValid && evaluacionPlan != null && listaEvalPlan != null)
            {
                //CREACIÓN DE LA EVALUACIÓN RESULTANTE
                var evaluacionPadre = db2.DPA_Evaluacion_Plan.Find(listaEvalPlan[0].ID_Evaluacion_Plan).DPA_Evaluacion;

                evaluacionPlan.DPA_Evaluacion.Evaluacion = evaluacionPlan.DPA_Evaluacion.Evaluacion.Trim().ToUpper();
                evaluacionPlan.DPA_Evaluacion.ID_Negocio = evaluacionPadre.ID_Negocio;
                evaluacionPlan.DPA_Evaluacion.ID_Proceso_Evaluado = evaluacionPadre.ID_Proceso_Evaluado;
                evaluacionPlan.DPA_Evaluacion.Objetivo = evaluacionPadre.Objetivo;
                evaluacionPlan.DPA_Evaluacion.Centro_Costo = evaluacionPadre.Centro_Costo;
                evaluacionPlan.DPA_Evaluacion.Atributo_Universo = evaluacionPadre.Atributo_Universo;
                evaluacionPlan.DPA_Evaluacion.Tipo_Evaluacion = evaluacionPadre.Tipo_Evaluacion;
                evaluacionPlan.DPA_Evaluacion.ID_Equipo_Responsable = evaluacionPadre.ID_Equipo_Responsable;
                evaluacionPlan.DPA_Evaluacion.PAC = evaluacionPadre.PAC;
                evaluacionPlan.DPA_Evaluacion.Anexo_30 = evaluacionPadre.Anexo_30;
                evaluacionPlan.DPA_Evaluacion.Regulatorio = evaluacionPadre.Regulatorio;
                evaluacionPlan.DPA_Evaluacion.N_SBS = evaluacionPadre.N_SBS;
                evaluacionPlan.DPA_Evaluacion.Evaluacion_SBS = evaluacionPadre.Evaluacion_SBS;
                evaluacionPlan.DPA_Evaluacion.SOX = evaluacionPadre.SOX;
                evaluacionPlan.DPA_Evaluacion.Fase_SOX = evaluacionPadre.Fase_SOX;
                evaluacionPlan.DPA_Evaluacion.Nombre_Mesa = evaluacionPadre.Nombre_Mesa;
                evaluacionPlan.DPA_Evaluacion.Comentario = evaluacionPadre.Comentario;
                evaluacionPlan.DPA_Evaluacion.Squad_COE_Area = evaluacionPadre.Squad_COE_Area;
                evaluacionPlan.DPA_Evaluacion.Aplicaciones = evaluacionPadre.Aplicaciones;
                evaluacionPlan.DPA_Evaluacion.RPA = evaluacionPadre.RPA;
                evaluacionPlan.DPA_Evaluacion.Chatbot = evaluacionPadre.Chatbot;
                evaluacionPlan.DPA_Evaluacion.IA = evaluacionPadre.IA;
                evaluacionPlan.DPA_Evaluacion.API = evaluacionPadre.API;
                evaluacionPlan.DPA_Evaluacion.N_Riesgos = evaluacionPadre.N_Riesgos;
                evaluacionPlan.DPA_Evaluacion.N_Controles = evaluacionPadre.N_Controles;
                evaluacionPlan.DPA_Evaluacion.Accesos_Aplicacion = evaluacionPadre.Accesos_Aplicacion;
                evaluacionPlan.DPA_Evaluacion.Accesos_BD = evaluacionPadre.Accesos_BD;
                evaluacionPlan.DPA_Evaluacion.Ratio_Dias_Control = evaluacionPadre.Ratio_Dias_Control;
                evaluacionPlan.DPA_Evaluacion.Uso_Analitica_EWP = evaluacionPadre.Uso_Analitica_EWP;
                evaluacionPlan.DPA_Evaluacion.Uso_Analitica_Registrado = evaluacionPadre.Uso_Analitica_Registrado;
                evaluacionPlan.DPA_Evaluacion.Analítica_Tipo_Pruebas = evaluacionPadre.Analítica_Tipo_Pruebas;
                evaluacionPlan.DPA_Evaluacion.SIEMBRA = evaluacionPadre.SIEMBRA;
                evaluacionPlan.DPA_Evaluacion.SIEGA = evaluacionPadre.SIEGA;
                evaluacionPlan.DPA_Evaluacion.RIEGA = evaluacionPadre.RIEGA;

                //AGREGAR EVALUACIÓN RESULTANTE AL PLAN
                db2.DPA_Evaluacion_Plan.Add(evaluacionPlan);
                db2.SaveChanges();

                AgregarLogEvaluacion(evaluacionPlan.DPA_Evaluacion.ID_Evaluacion, evaluacionPlan.DPA_Evaluacion.Evaluacion, evaluacionPlan.DPA_Evaluacion.ID_Negocio, 1);
                AgregarLogEvaluacionPlan(evaluacionPlan.ID_Evaluacion_Plan, 1, "Nuevo por Reemplazo", 0);

                //RETIRO DE EVALUACIONES A REEMPLZAR
                foreach (var item in listaEvalPlan)
                    AgregarLogEvaluacionPlan(item.ID_Evaluacion_Plan, 3, "Retirado por Reemplazo", evaluacionPlan.DPA_Evaluacion.ID_Evaluacion);

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluaciones Fusionadas";
                TempData["Text"] = "Se han guardado los cambios satisfactoriamente";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_FusionarEvaluacionPlan(DPA_Evaluacion_Plan evaluacionPlan, List<DPA_Evaluacion_Plan> listaEvalPlan)
        {
            if (ModelState.IsValid && evaluacionPlan != null && listaEvalPlan != null)
            {
                //CREACIÓN DE LA EVALUACIÓN RESULTANTE
                var evaluacionPadre = db2.DPA_Evaluacion_Plan.Find(listaEvalPlan[0].ID_Evaluacion_Plan).DPA_Evaluacion;

                evaluacionPlan.DPA_Evaluacion.Evaluacion = evaluacionPlan.DPA_Evaluacion.Evaluacion.Trim().ToUpper();
                evaluacionPlan.DPA_Evaluacion.ID_Negocio = evaluacionPadre.ID_Negocio;
                evaluacionPlan.DPA_Evaluacion.ID_Proceso_Evaluado = evaluacionPadre.ID_Proceso_Evaluado;
                evaluacionPlan.DPA_Evaluacion.Objetivo = evaluacionPadre.Objetivo;
                evaluacionPlan.DPA_Evaluacion.Centro_Costo = evaluacionPadre.Centro_Costo;
                evaluacionPlan.DPA_Evaluacion.Atributo_Universo = evaluacionPadre.Atributo_Universo;
                evaluacionPlan.DPA_Evaluacion.Tipo_Evaluacion = evaluacionPadre.Tipo_Evaluacion;
                evaluacionPlan.DPA_Evaluacion.ID_Equipo_Responsable = evaluacionPadre.ID_Equipo_Responsable;
                evaluacionPlan.DPA_Evaluacion.PAC = evaluacionPadre.PAC;
                evaluacionPlan.DPA_Evaluacion.Anexo_30 = evaluacionPadre.Anexo_30;
                evaluacionPlan.DPA_Evaluacion.Regulatorio = evaluacionPadre.Regulatorio;
                evaluacionPlan.DPA_Evaluacion.N_SBS = evaluacionPadre.N_SBS;
                evaluacionPlan.DPA_Evaluacion.Evaluacion_SBS = evaluacionPadre.Evaluacion_SBS;
                evaluacionPlan.DPA_Evaluacion.SOX = evaluacionPadre.SOX;
                evaluacionPlan.DPA_Evaluacion.Fase_SOX = evaluacionPadre.Fase_SOX;
                evaluacionPlan.DPA_Evaluacion.Nombre_Mesa = evaluacionPadre.Nombre_Mesa;
                evaluacionPlan.DPA_Evaluacion.Comentario = evaluacionPadre.Comentario;
                evaluacionPlan.DPA_Evaluacion.Squad_COE_Area = evaluacionPadre.Squad_COE_Area;
                evaluacionPlan.DPA_Evaluacion.Aplicaciones = evaluacionPadre.Aplicaciones;
                evaluacionPlan.DPA_Evaluacion.RPA = evaluacionPadre.RPA;
                evaluacionPlan.DPA_Evaluacion.Chatbot = evaluacionPadre.Chatbot;
                evaluacionPlan.DPA_Evaluacion.IA = evaluacionPadre.IA;
                evaluacionPlan.DPA_Evaluacion.API = evaluacionPadre.API;
                evaluacionPlan.DPA_Evaluacion.N_Riesgos = evaluacionPadre.N_Riesgos;
                evaluacionPlan.DPA_Evaluacion.N_Controles = evaluacionPadre.N_Controles;
                evaluacionPlan.DPA_Evaluacion.Accesos_Aplicacion = evaluacionPadre.Accesos_Aplicacion;
                evaluacionPlan.DPA_Evaluacion.Accesos_BD = evaluacionPadre.Accesos_BD;
                evaluacionPlan.DPA_Evaluacion.Ratio_Dias_Control = evaluacionPadre.Ratio_Dias_Control;
                evaluacionPlan.DPA_Evaluacion.Uso_Analitica_EWP = evaluacionPadre.Uso_Analitica_EWP;
                evaluacionPlan.DPA_Evaluacion.Uso_Analitica_Registrado = evaluacionPadre.Uso_Analitica_Registrado;
                evaluacionPlan.DPA_Evaluacion.Analítica_Tipo_Pruebas = evaluacionPadre.Analítica_Tipo_Pruebas;
                evaluacionPlan.DPA_Evaluacion.SIEMBRA = evaluacionPadre.SIEMBRA;
                evaluacionPlan.DPA_Evaluacion.SIEGA = evaluacionPadre.SIEGA;
                evaluacionPlan.DPA_Evaluacion.RIEGA = evaluacionPadre.RIEGA;

                //AGREGAR EVALUACIÓN RESULTANTE AL PLAN
                db2.DPA_Evaluacion_Plan.Add(evaluacionPlan);
                db2.SaveChanges();

                AgregarLogEvaluacion(evaluacionPlan.DPA_Evaluacion.ID_Evaluacion, evaluacionPlan.DPA_Evaluacion.Evaluacion, evaluacionPlan.DPA_Evaluacion.ID_Negocio, 1);
                AgregarLogEvaluacionPlan(evaluacionPlan.ID_Evaluacion_Plan, 1, "Nuevo por Fusión", 0);

                //RETIRO DE EVALUACIONES A FUSIONAR
                foreach (var item in listaEvalPlan)
                    AgregarLogEvaluacionPlan(item.ID_Evaluacion_Plan, 3, "Retirado por Fusión", evaluacionPlan.DPA_Evaluacion.ID_Evaluacion);

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluaciones Fusionadas";
                TempData["Text"] = "Se han guardado los cambios satisfactoriamente";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }
        public ActionResult PV_DividirEvaluacionPlan(int idEvaluacionPlan)
        {
            var evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            ViewBag.listaProcesoEvaluado = db2.DD_ProcesoEvaluado.Where(p => p.IdNegocios == evaluacionPlan.DPA_Evaluacion.ID_Negocio).OrderBy(p => p.Nombre).ToList();
            ViewBag.listaEquipoResponsable = db2.DPA_Equipo.ToList();
            ViewBag.listaRiesgos = db2.DPA_Riesgo_Asociado.Where(r => r.Estado == 1).ToList();

            ViewBag.evalPlanSel = evaluacionPlan;
            ViewBag.listaEvalPlan = db2.DPA_Evaluacion_Plan.Where(e => e.ID_Plan_Anual == evaluacionPlan.ID_Plan_Anual)
                                                                .Where(e => e.DPA_Evaluacion.ID_Negocio == evaluacionPlan.DPA_Evaluacion.ID_Negocio)
                                                                .OrderBy(e => e.DPA_Evaluacion.Evaluacion).ToList();

            ViewBag.listaSN = listaSN;
            ViewBag.listaTipoEvaluacion = listaTipoEvaluacion;
            ViewBag.listaUltimoCalificativo = listaUltimoCalificativo;
            ViewBag.listaFaseSOX = listaFaseSOX;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_DividirEvaluacionPlan(int idEvaluacionPlanAnterior, List<DPA_Evaluacion_Plan> listaEvalPlan)
        {
            if (ModelState.IsValid && listaEvalPlan != null)
            {
                //RETIRO DE EVALUACIÓN A DIVIDIR
                var evaluacionPadre = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlanAnterior);
                AgregarLogEvaluacionPlan(evaluacionPadre.ID_Evaluacion_Plan, 3, "Retirado por División", 0);

                foreach (var nuevaEvaluacionPlan in listaEvalPlan)
                {
                    //CREACIÓN DE NUEVA EVALUACIÓN
                    nuevaEvaluacionPlan.DPA_Evaluacion.Evaluacion = nuevaEvaluacionPlan.DPA_Evaluacion.Evaluacion.Trim().ToUpper();
                    nuevaEvaluacionPlan.DPA_Evaluacion.ID_Negocio = evaluacionPadre.DPA_Evaluacion.ID_Negocio;
                    nuevaEvaluacionPlan.DPA_Evaluacion.ID_Proceso_Evaluado = evaluacionPadre.DPA_Evaluacion.ID_Proceso_Evaluado;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Objetivo = evaluacionPadre.DPA_Evaluacion.Objetivo;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Centro_Costo = evaluacionPadre.DPA_Evaluacion.Centro_Costo;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Atributo_Universo = evaluacionPadre.DPA_Evaluacion.Atributo_Universo;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Tipo_Evaluacion = evaluacionPadre.DPA_Evaluacion.Tipo_Evaluacion;
                    nuevaEvaluacionPlan.DPA_Evaluacion.ID_Equipo_Responsable = evaluacionPadre.DPA_Evaluacion.ID_Equipo_Responsable;
                    nuevaEvaluacionPlan.DPA_Evaluacion.PAC = evaluacionPadre.DPA_Evaluacion.PAC;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Anexo_30 = evaluacionPadre.DPA_Evaluacion.Anexo_30;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Regulatorio = evaluacionPadre.DPA_Evaluacion.Regulatorio;
                    nuevaEvaluacionPlan.DPA_Evaluacion.N_SBS = evaluacionPadre.DPA_Evaluacion.N_SBS;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Evaluacion_SBS = evaluacionPadre.DPA_Evaluacion.Evaluacion_SBS;
                    nuevaEvaluacionPlan.DPA_Evaluacion.SOX = evaluacionPadre.DPA_Evaluacion.SOX;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Fase_SOX = evaluacionPadre.DPA_Evaluacion.Fase_SOX;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Nombre_Mesa = evaluacionPadre.DPA_Evaluacion.Nombre_Mesa;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Comentario = evaluacionPadre.DPA_Evaluacion.Comentario;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Squad_COE_Area = evaluacionPadre.DPA_Evaluacion.Squad_COE_Area;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Aplicaciones = evaluacionPadre.DPA_Evaluacion.Aplicaciones;
                    nuevaEvaluacionPlan.DPA_Evaluacion.RPA = evaluacionPadre.DPA_Evaluacion.RPA;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Chatbot = evaluacionPadre.DPA_Evaluacion.Chatbot;
                    nuevaEvaluacionPlan.DPA_Evaluacion.IA = evaluacionPadre.DPA_Evaluacion.IA;
                    nuevaEvaluacionPlan.DPA_Evaluacion.API = evaluacionPadre.DPA_Evaluacion.API;
                    nuevaEvaluacionPlan.DPA_Evaluacion.N_Riesgos = evaluacionPadre.DPA_Evaluacion.N_Riesgos;
                    nuevaEvaluacionPlan.DPA_Evaluacion.N_Controles = evaluacionPadre.DPA_Evaluacion.N_Controles;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Accesos_Aplicacion = evaluacionPadre.DPA_Evaluacion.Accesos_Aplicacion;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Accesos_BD = evaluacionPadre.DPA_Evaluacion.Accesos_BD;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Ratio_Dias_Control = evaluacionPadre.DPA_Evaluacion.Ratio_Dias_Control;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Uso_Analitica_EWP = evaluacionPadre.DPA_Evaluacion.Uso_Analitica_EWP;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Uso_Analitica_Registrado = evaluacionPadre.DPA_Evaluacion.Uso_Analitica_Registrado;
                    nuevaEvaluacionPlan.DPA_Evaluacion.Analítica_Tipo_Pruebas = evaluacionPadre.DPA_Evaluacion.Analítica_Tipo_Pruebas;
                    nuevaEvaluacionPlan.DPA_Evaluacion.SIEMBRA = evaluacionPadre.DPA_Evaluacion.SIEMBRA;
                    nuevaEvaluacionPlan.DPA_Evaluacion.SIEGA = evaluacionPadre.DPA_Evaluacion.SIEGA;
                    nuevaEvaluacionPlan.DPA_Evaluacion.RIEGA = evaluacionPadre.DPA_Evaluacion.RIEGA;

                    db2.DPA_Evaluacion_Plan.Add(nuevaEvaluacionPlan);
                    db2.SaveChanges();

                    AgregarLogEvaluacion(nuevaEvaluacionPlan.DPA_Evaluacion.ID_Evaluacion, nuevaEvaluacionPlan.DPA_Evaluacion.Evaluacion, nuevaEvaluacionPlan.DPA_Evaluacion.ID_Negocio, 1);
                    AgregarLogEvaluacionPlan(nuevaEvaluacionPlan.ID_Evaluacion_Plan, 1, "Nuevo por División", evaluacionPadre.DPA_Evaluacion.ID_Evaluacion);
                }

                TempData["Icon"] = "success";
                TempData["Title"] = "Evaluación Dividida";
                TempData["Text"] = "Se han guardado los cambios satisfactoriamente";
                return RedirectToAction("MantenimientoPlan", new { pa = evaluacionPadre.ID_Plan_Anual, ne = evaluacionPadre.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return RedirectToAction("MantenimientoPlan");
        }



        // VISTA DE LOG DE CAMBIOS        
        public ActionResult PV_LogEvaluacionPlan()
        {
            modelDB.SP_DPA_Log_Evaluacion_Plan = db2.SP_DPA_Log_Evaluacion_Plan();

            return PartialView(modelDB);
        }
        public ActionResult PV_LogEvaluacion()
        {
            modelDB.SP_DPA_Log_Evaluacion = db2.SP_DPA_Log_Evaluacion();

            return PartialView(modelDB);
        }
        public void AgregarLogEvaluacionPlan(int idEvaluacionPlan, int idTipoOperacion, string detalleOperacion, int idReferencia)
        {
            DPA_Log_Evaluacion_Plan logEvaluacionPlan = new DPA_Log_Evaluacion_Plan
            {
                ID_Usuario = Convert.ToInt32(Session["IdUser"]),
                ID_Evaluacion_Plan = idEvaluacionPlan,
                ID_Tipo_Operacion = idTipoOperacion,
                Detalle_Operacion = detalleOperacion,
                Timestamp = DateTime.Now
            };

            if (idReferencia != 0)
                logEvaluacionPlan.ID_Referencia = idReferencia;

            db2.DPA_Log_Evaluacion_Plan.Add(logEvaluacionPlan);
            db2.SaveChanges();
        }
        public void AgregarLogEvaluacion(int idEvaluacion, string nombreEvaluacion, int idnegocio, int idTipoOperacion)
        {
            DPA_Log_Evaluacion logEvaluacion = new DPA_Log_Evaluacion
            {
                ID_Usuario = Convert.ToInt32(Session["IdUser"]),
                ID_Evaluacion = idEvaluacion,
                Evaluacion = nombreEvaluacion,
                ID_Negocio = idnegocio,
                ID_Tipo_Operacion = idTipoOperacion,
                Timestamp = DateTime.Now
            };

            if (idTipoOperacion == 1)
                logEvaluacion.Detalle_Operacion = "Evaluación creada";
            else if (idTipoOperacion == 2)
                logEvaluacion.Detalle_Operacion = "Evaluación editada";
            else if (idTipoOperacion == 4)
                logEvaluacion.Detalle_Operacion = "Evaluación eliminada";

            db2.DPA_Log_Evaluacion.Add(logEvaluacion);
            db2.SaveChanges();
        }
        public void AgregarLogCambioNombreEv(int idEvaluacion, string nombreEvaluacion, int idnegocio, string nuevoNombre)
        {
            DPA_Log_Evaluacion logEvaluacion = new DPA_Log_Evaluacion
            {
                ID_Usuario = Convert.ToInt32(Session["IdUser"]),
                ID_Evaluacion = idEvaluacion,
                Evaluacion = nombreEvaluacion,
                Nuevo_Nombre = nuevoNombre,
                ID_Negocio = idnegocio,
                ID_Tipo_Operacion = 2,
                Detalle_Operacion = "Cambio de nombre",
                Timestamp = DateTime.Now
            };

            db2.DPA_Log_Evaluacion.Add(logEvaluacion);
            db2.SaveChanges();
        }


        // VISTA DE VOTACIONES
        public ActionResult VotacionAuditor(int? pa, int? ne)
        {
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.Anio_Plan >= 2022) select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();
            DPA_Negocio negocioSeleccionado = ne != null ? db2.DPA_Negocio.Find(ne) : negocios.First();

            ViewBag.PlanAnualSeleccionado = planAnualSeleccionado;
            ViewBag.NegocioSeleccionado = negocioSeleccionado;
            ViewBag.flagEdicionPlan = planAnualSeleccionado.Flag_Edicion;

            var votaciones = db2.SP_DPA_Votacion_Auditor_v2(idUsuario, planAnualSeleccionado.ID_Plan_Anual, negocioSeleccionado.ID_Negocio).ToList();

            modelDB.SP_DPA_Votacion_Auditor_v2 = votaciones;

            ViewBag.votosRealizados = votaciones.Where(v => v.flag_UsuarioHaVotado == 1).Count();
            ViewBag.votosPendientes = votaciones.Where(v => v.flag_UsuarioHaVotado == 0 && v.flag_Votacion_Obligatoria == 1).Count();

            return View(modelDB);
        }
        public ActionResult PV_VerVotacionEvaluacionPlan(int idEvaluacionPlan, string lastPage)
        {
            Session["lastPage"] = lastPage;
            var idUsuario = Convert.ToInt32(Session["IdUser"]);

            DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            ViewBag.evaluacionPlan = evaluacionPlan;
            ViewBag.evaluacion = evaluacionPlan.DPA_Evaluacion;
            modelDB.SP_DPA_Votacion_Evaluacion_v3 = db2.SP_DPA_Votacion_Evaluacion_v31(idEvaluacionPlan);

            ViewBag.flagEdicionPlan = evaluacionPlan.DPA_Plan_Anual.Flag_Edicion;

            var spEvalPlan = db2.SP_DPA_Votacion_Auditor_v2(idUsuario, evaluacionPlan.ID_Plan_Anual, evaluacionPlan.DPA_Evaluacion.ID_Negocio).Where(x => x.ID_Evaluacion_Plan == idEvaluacionPlan).First();
            //DPA_Votacion votacion = db2.DPA_Votacion.Where(v => v.ID_Usuario == idUsuario).Where(v => v.ID_Evaluacion_Plan == idEvaluacionPlan).FirstOrDefault();
            //ViewBag.HaVotado = (votacion != null ? 1 : 0);
            ViewBag.HaVotado = spEvalPlan.flag_UsuarioHaVotado;
            ViewBag.VotacionObligatoria = spEvalPlan.flag_Votacion_Obligatoria;

            return PartialView(modelDB);
        }
        public ActionResult PV_CrearVotacion(int idEvaluacionPlan)
        {
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idUsuario = idUsuario;

            DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
            ViewBag.evaluacionPlan = evaluacionPlan;

            ViewBag.CriticidadAnterior = db2.SP_DPA_Programacion_Final(evaluacionPlan.ID_Plan_Anual, evaluacionPlan.DPA_Evaluacion.ID_Negocio).Where(x => x.ID_Evaluacion_Plan == evaluacionPlan.ID_Evaluacion_Plan)
                                                                                                                                                .First().Valor_Criticidad_Ant;

            modelDB.DPA_Criterio_Votacion = db2.DPA_Criterio_VotacionSet.Where(x=>x.ID_Plan_Anual== evaluacionPlan.ID_Plan_Anual);
            modelDB.DPA_Opcion_Votacion = db2.DPA_Opcion_Votacion;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearVotacion(List<DPA_Votacion> votaciones)
        {
            string lastPage = Session["lastPage"].ToString();
            if (ModelState.IsValid && votaciones != null)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(votaciones.FirstOrDefault().ID_Evaluacion_Plan);
                int idPlanAnual = evaluacionPlan.ID_Plan_Anual;
                int idNegocio = evaluacionPlan.DPA_Evaluacion.ID_Negocio;

                db2.DPA_Votacion.AddRange(votaciones);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Votación agregada";
                TempData["Text"] = "Se ha agregado el registro satisfactoriamente";
                return Redirect(lastPage + "?pa=" + idPlanAnual + "&ne=" + idNegocio);
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return Redirect(lastPage);
        }
        public ActionResult PV_EditarVotacion(int idEvaluacionPlan)
        {
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idUsuario = idUsuario;

            DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
            ViewBag.CriticidadAnterior = db2.SP_DPA_Programacion_Final(evaluacionPlan.ID_Plan_Anual, evaluacionPlan.DPA_Evaluacion.ID_Negocio).Where(x => x.ID_Evaluacion_Plan == evaluacionPlan.ID_Evaluacion_Plan)
                                                                                                                                                .First().Valor_Criticidad_Ant;


            modelDB.DPA_Votacion = db2.DPA_Votacion.Where(v => v.ID_Usuario == idUsuario)
                                                    .Where(v => v.ID_Evaluacion_Plan == idEvaluacionPlan);
            modelDB.DPA_Opcion_Votacion = db2.DPA_Opcion_Votacion;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarVotacion(List<DPA_Votacion> votaciones)
        {
            string lastPage = Session["lastPage"].ToString();
            if (ModelState.IsValid && votaciones != null)
            {
                foreach (var votacion in votaciones)
                {
                    db2.Entry(votacion).State = EntityState.Modified;
                    db2.SaveChanges();
                }

                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(votaciones.FirstOrDefault().ID_Evaluacion_Plan);
                int idPlanAnual = evaluacionPlan.ID_Plan_Anual;
                int idNegocio = evaluacionPlan.DPA_Evaluacion.ID_Negocio;

                TempData["Icon"] = "success";
                TempData["Title"] = "Votación editada";
                TempData["Text"] = "Se ha editado el registro satisfactoriamente";
                return Redirect(lastPage + "?pa=" + idPlanAnual + "&ne=" + idNegocio);
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return Redirect(lastPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EliminarVotacion(int idEvaluacionPlan)
        {
            string lastPage = Session["lastPage"].ToString();

            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

            if (evaluacionPlan != null)
            {
                var votaciones = db2.DPA_Votacion.Where(v => v.ID_Usuario == idUsuario)
                                                    .Where(v => v.ID_Evaluacion_Plan == idEvaluacionPlan).ToList();
                int idPlanAnual = evaluacionPlan.ID_Plan_Anual;
                int idNegocio = evaluacionPlan.DPA_Evaluacion.ID_Negocio;

                db2.DPA_Votacion.RemoveRange(votaciones);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Votación eliminada";
                TempData["Text"] = "Se ha eliminado el registro satisfactoriamente";
                return Redirect(lastPage + "?pa=" + idPlanAnual + "&ne=" + idNegocio);
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operacion, intentelo nuevamente";
            return Redirect(lastPage);
        }


        //CALENDARIO DE TRABAJO
        public ActionResult CalendarioTrabajo(int? pa, int? pe)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.Anio_Plan >= 2022) select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var personas = (from p in db2.Persona.Where(x => x.Activo == 1) select p).OrderBy(p => p.Nombres);
            modelDB.Persona = personas.ToList();

            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();
            //DPA_Plan_Anual planAnualSeleccionado = db2.DPA_Plan_Anual.Find(pa);
            Persona personaSeleccionada = pe != null ? db2.Persona.Find(pe) : personas.First();

            ViewBag.PlanAnualSeleccionado = planAnualSeleccionado;
            ViewBag.PersonaSeleccionada = personaSeleccionada;
            ViewBag.flagEdicionPlan = planAnualSeleccionado.Flag_Edicion;

            modelDB.SP_DPA_Fechas_Auditor = db2.SP_DPA_Fechas_Auditor(personaSeleccionada.IdPersona, planAnualSeleccionado.ID_Plan_Anual);
            ViewBag.ProyectosAuditor = (from f in db2.SP_DPA_Fechas_Auditor(personaSeleccionada.IdPersona, planAnualSeleccionado.ID_Plan_Anual).Where(x => x.Tipo == 1) select f).OrderBy(f => f.Fecha_Inicio).ToList();
            ViewBag.TiemposExtraAuditor = (from f in db2.SP_DPA_Fechas_Auditor(personaSeleccionada.IdPersona, planAnualSeleccionado.ID_Plan_Anual).Where(x => x.Tipo == 2) select f).OrderBy(f => f.Fecha_Inicio).ToList();

            modelDB.SP_DPA_Fechas_No_Laborables = db2.SP_DPA_Fechas_No_Laborables(planAnualSeleccionado.Anio_Plan);

            return View(modelDB);
        }
        public ActionResult PV_CrearAuditorEvaluacion(int idPersona, int idPlanAnual)
        {
            ViewBag.AuditorSeleccionado = (from u in db2.Usuario.Where(x => x.IdPersona == idPersona) select u).First();

            modelDB.DPA_Negocio = db2.DPA_Negocio;

            modelDB.SP_DPA_View_Asignar_Proyecto = db2.SP_DPA_View_Asignar_Proyecto(idPersona, idPlanAnual);

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearAuditorEvaluacion(DPA_Auditor_Evaluacion auditorEvaluacion)
        {
            if (ModelState.IsValid && auditorEvaluacion != null)
            {
                var planAnual = db2.DPA_Evaluacion_Plan.Find(auditorEvaluacion.ID_Evaluacion_Plan);
                var persona = db2.Usuario.Find(auditorEvaluacion.ID_Usuario).Persona;
                db2.DPA_Auditor_Evaluacion.Add(auditorEvaluacion);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Creación realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("CalendarioTrabajo", new { pa = planAnual.ID_Plan_Anual, pe = persona.IdPersona });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EliminarAuditorEvaluacion(int idAuditorEvaluacion)
        {
            DPA_Auditor_Evaluacion auditorEvaluacion = db2.DPA_Auditor_Evaluacion.Find(idAuditorEvaluacion);

            if (auditorEvaluacion != null)
            {
                var idPlanAnual = auditorEvaluacion.DPA_Evaluacion_Plan.ID_Plan_Anual;
                var persona = auditorEvaluacion.Usuario.Persona;
                db2.DPA_Auditor_Evaluacion.Remove(auditorEvaluacion);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Asignación eliminada";
                TempData["Text"] = "Se ha eliminado el registro satisfactoriamente";
                return RedirectToAction("CalendarioTrabajo", new { pa = idPlanAnual, pe = persona.IdPersona });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        public ActionResult PV_CrearAuditorTiempoExtra(int idPersona, int idPlanAnual)
        {
            ViewBag.AuditorSeleccionado = (from u in db2.Usuario.Where(x => x.IdPersona == idPersona) select u).First();
            modelDB.DPA_Tipo_Tiempo_Extra = db2.DPA_Tipo_Tiempo_Extra;

            ViewBag.idPlanAnual = idPlanAnual;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_CrearAuditorTiempoExtra(DPA_Auditor_Tiempo_Extra auditorTiempoExtra, int idPlanAnual)
        {
            if (ModelState.IsValid && auditorTiempoExtra != null)
            {
                var persona = db2.Usuario.Find(auditorTiempoExtra.ID_Usuario).Persona;
                db2.DPA_Auditor_Tiempo_Extra.Add(auditorTiempoExtra);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Creación realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("CalendarioTrabajo", new { pa = idPlanAnual, pe = persona.IdPersona });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        public ActionResult PV_EditarAuditorTiempoExtra(int idAuditorTiempoExtra, int idPlanAnual)
        {
            ViewBag.AuditorTiempoExtraSel = db2.DPA_Auditor_Tiempo_Extra.Find(idAuditorTiempoExtra);
            modelDB.DPA_Tipo_Tiempo_Extra = db2.DPA_Tipo_Tiempo_Extra;
            ViewBag.idPlanAnual = idPlanAnual;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarAuditorTiempoExtra(DPA_Auditor_Tiempo_Extra auditorTiempoExtra, int idPlanAnual)
        {
            if (ModelState.IsValid && auditorTiempoExtra != null)
            {
                db2.Entry(auditorTiempoExtra).State = EntityState.Modified;
                db2.SaveChanges();

                var idPersona = db2.Usuario.Find(auditorTiempoExtra.ID_Usuario).Persona.IdPersona;

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("CalendarioTrabajo", new { pa = idPlanAnual, pe = idPersona });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EliminarAuditorTiempoExtra(int idAuditorTiempoExtra, int idPlanAnual)
        {
            DPA_Auditor_Tiempo_Extra auditorTiempoExtra = db2.DPA_Auditor_Tiempo_Extra.Find(idAuditorTiempoExtra);

            if (auditorTiempoExtra != null)
            {
                var persona = auditorTiempoExtra.Usuario.Persona;
                db2.DPA_Auditor_Tiempo_Extra.Remove(auditorTiempoExtra);
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Actividad eliminada";
                TempData["Text"] = "Se ha eliminado el registro satisfactoriamente";
                return RedirectToAction("CalendarioTrabajo", new { pa = idPlanAnual, pe = persona.IdPersona });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }




        //PROGRAMACIÓN FINAL


        public ActionResult ProgramacionFinal(int? pa, int? ne)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db2.UsuarioModuloRol where u.IdModulo == 12 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

            var planAnual = (from p in db2.DPA_Plan_Anual.Where(x => x.Anio_Plan >= 2022) select p).OrderByDescending(p => p.Anio_Plan);
            modelDB.DPA_Plan_Anual = planAnual.ToList();

            var negocios = (from n in db2.DPA_Negocio select n).OrderBy(n => n.ID_Negocio);
            modelDB.DPA_Negocio = negocios.ToList();

            DPA_Plan_Anual planAnualSeleccionado = pa != null ? db2.DPA_Plan_Anual.Find(pa) : planAnual.First();
            int idNegocioSeleccionado = ne != null ? (int)ne : negocios.First().ID_Negocio;

            ViewBag.PlanAnualSeleccionado = planAnualSeleccionado;
            ViewBag.idNegocioSeleccionado = idNegocioSeleccionado;

            var flagFaltaVotacion = db2.SP_DPA_Programacion_Final(planAnualSeleccionado.ID_Plan_Anual, idNegocioSeleccionado).Where(x => x.Cantidad_Votos == 0).Count() >= 1 ? 1 : 0;
            var flagEdicionPlan = flagFaltaVotacion == 1 ? 1 : planAnualSeleccionado.Flag_Edicion;

            ViewBag.flagFaltaVotacion = flagFaltaVotacion;
            ViewBag.flagEdicionPlan = flagEdicionPlan;

            modelDB.SP_DPA_Programacion_Final = db2.SP_DPA_Programacion_Final(planAnualSeleccionado.ID_Plan_Anual, idNegocioSeleccionado);

            return View(modelDB);
        }
        //CONTROL DEL SWITCH
        [HttpPost]
        public ActionResult ActualizarFlagEdicionPlan(int idplan, int flagEdicionPlan)
        {
            DPA_Plan_Anual plan_anual = db2.DPA_Plan_Anual.Find(idplan);
            plan_anual.Flag_Edicion = flagEdicionPlan;
            db2.Entry(plan_anual).State = EntityState.Modified;
            db2.SaveChanges();

            return RedirectToAction("ProgramacionFinal");
        }

        //ACA SE USA PARA EDITAR CRITICIDAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarCriticidad(int idEvaluacionPlan, string criticidad)
        {
            if (ModelState.IsValid)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
                evaluacionPlan.Criticidad_Elegida = criticidad;
                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        public ActionResult PV_EditarScoring(int idEvaluacionPlan)
        {
            var anioPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan).DPA_Plan_Anual.Anio_Plan;
            modelDB.DPA_Scoring_Evaluacion = db2.DPA_Scoring_Evaluacion.Where(s => s.ID_Evaluacion_Plan == idEvaluacionPlan).OrderBy(s => s.DPA_Criterio_Scoring.Criterio);
            modelDB.DPA_Criterio_Scoring = db2.DPA_Criterio_Scoring.Where(s => s.Anio_Plan == anioPlan)
                                                                    .OrderBy(s => s.Criterio);
            var listaScoring = db2.DPA_Scoring_Evaluacion.Where(s => s.ID_Evaluacion_Plan == idEvaluacionPlan).ToList();
            ViewBag.flagNuevo = listaScoring.Count() == 0 ? 1 : 0;
            ViewBag.idEvaluacionPlan = idEvaluacionPlan;

            ViewBag.flagEdicionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan).DPA_Plan_Anual.Flag_Edicion;

            return PartialView(modelDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarScoring(int flagNuevo, int idEvaluacionPlan, List<DPA_Scoring_Evaluacion> listaScoring)
        {
            if (ModelState.IsValid && listaScoring != null)
            {
                if (flagNuevo == 1) // CREAR SCORING
                {
                    DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

                    db2.DPA_Scoring_Evaluacion.AddRange(listaScoring);
                    db2.SaveChanges();

                    TempData["Icon"] = "success";
                    TempData["Title"] = "Scoring realizado";
                    TempData["Text"] = "Se han guardado los cambios en la base de datos";
                    return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
                }
                else if (flagNuevo == 0) // EDITAR SCORING
                {
                    DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

                    foreach (var scoring in listaScoring)
                    {
                        db2.Entry(scoring).State = EntityState.Modified;
                        db2.SaveChanges();
                    }

                    TempData["Icon"] = "success";
                    TempData["Title"] = "Edición realizada";
                    TempData["Text"] = "Se han guardado los cambios en la base de datos";
                    return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
                }
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarDiasAjustado(int idEvaluacionPlan, int diasAjustado)
        {
            if (ModelState.IsValid)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
                evaluacionPlan.Dias_Ajustado = diasAjustado;
                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarNAudAjustado(int idEvaluacionPlan, int nAudAjustado)
        {
            if (ModelState.IsValid)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);

                if (nAudAjustado == 0)
                {
                    evaluacionPlan.N_Aud_Ajustado = null;
                }
                else
                {
                    evaluacionPlan.N_Aud_Ajustado = nAudAjustado;
                }
                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarFechaInicio(int idEvaluacionPlan, string fechaInicio)
        {
            if (ModelState.IsValid)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
                evaluacionPlan.Fecha_Inicio = DateTime.Parse(fechaInicio);
                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarProgramadoFinal(int idEvaluacionPlan, string programadoFinal)
        {
            if (ModelState.IsValid)
            {
                DPA_Evaluacion_Plan evaluacionPlan = db2.DPA_Evaluacion_Plan.Find(idEvaluacionPlan);
                evaluacionPlan.Programado = programadoFinal;
                db2.Entry(evaluacionPlan).State = EntityState.Modified;
                db2.SaveChanges();

                TempData["Icon"] = "success";
                TempData["Title"] = "Edición realizada";
                TempData["Text"] = "Se han guardado los cambios en la base de datos";
                return RedirectToAction("ProgramacionFinal", new { pa = evaluacionPlan.ID_Plan_Anual, ne = evaluacionPlan.DPA_Evaluacion.ID_Negocio });
            }
            TempData["Icon"] = "error";
            TempData["Title"] = "Error";
            TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
            return RedirectToAction("ProgramacionFinal");
        }












    }
}