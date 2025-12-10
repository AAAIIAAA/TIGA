using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebTIGA.Models;
using WebTIGA.Controllers.Encriptar;
using System.Globalization;
using System.Data.Entity.Validation;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text.RegularExpressions;


using WebTIGA.Autorizacion;

namespace WebTIGA.Controllers

{
    [Logueado]
    public class WebSeguimientoObservacionesController : Controller
    {
        SesionData session = new SesionData();
        PROYECTOSIAV2Entities1 db = new PROYECTOSIAV2Entities1(); 
        ContenedorModelos modelDB = new ContenedorModelos();



        public ActionResult Inicio( string fechaCorte = null, int? prima_check = null, int? seguros_check = null,
                            int? o_check = null, int? o_m_check = null,
                            int? en_f_check = null, int? v_check = null, int? p_check = null,
                            int? c_check = null, int? i_check = null, string filtroAnio = null, string id = null)
        {
            prima_check = prima_check ?? 0;
            seguros_check = seguros_check ?? 1;
            o_check = o_check ?? 1;
            o_m_check = o_m_check ?? 0;
            en_f_check = en_f_check ?? 1;
            v_check = v_check ?? 1;
            p_check = p_check ?? 1;
            c_check = c_check ?? 0;
            i_check = i_check ?? 1;

            if (string.IsNullOrEmpty(fechaCorte))
            {
                DateTime fechaAct = DateTime.Now;
                fechaCorte = fechaAct.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime fechaParametro = DateTime.ParseExact(fechaCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                
                fechaParametro = fechaParametro.AddDays(1);

                
                fechaCorte = fechaParametro.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(filtroAnio))
            {
                filtroAnio = "2017";
            }

            var añosObservaciones = db.TG_Observacion_Cambio
                           .Select(o => o.AÑO)
                           .Distinct()
                           .OrderBy(año => año)
                           .ToList();

            ViewBag.AñosObservaciones = añosObservaciones;

           

            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db.UsuarioModuloRol where u.IdModulo == 14 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

           
            modelDB.fn_Stock_Observaciones_Integrado_v7 = db.fn_Stock_Observaciones_Integrado_v7(fechaCorte,prima_check,seguros_check, o_check, o_m_check, en_f_check, v_check, p_check, c_check, i_check, filtroAnio, id);




            return View(modelDB);
        }

        //OBTENER SUBGERENCIAS POR ID, PARA CARGAR OPCIONES DE LA PROXIMA GERENCIA
        public ActionResult ObtenerSubgerencias(int idGerenciaAnterior)
        {

          
            List<TG_Unidad_Responsable> unidadesResponsables = db.TG_Unidad_Responsable
                                            .Where(ur => (ur.ID_Padre == idGerenciaAnterior || ur.ID == idGerenciaAnterior)&&ur.flagActivo ==1)
                                            .ToList();


            var subgerencias = unidadesResponsables.Select(u => new { Value = u.ID, Text = u.Nombre }).ToList();

            return Json(subgerencias, JsonRequestBehavior.AllowGet);
        }

        //OBTENER OPCIONES DE LA GERENCIA QUE SE QUIERE MODIFICAR EN BASE A LA GERENIA ANTERIOR
        public ActionResult ObtenerSubgerenciasPorNombre(string nombreGerenciaAnterior)
        {


            var idGerenciaAnterior = db.TG_Unidad_Responsable
                .Where(ur => ur.Nombre == nombreGerenciaAnterior)
                .Select(ur => ur.ID)
                .FirstOrDefault();


            List<TG_Unidad_Responsable> unidadesResponsables = db.TG_Unidad_Responsable
                .Where(ur => (ur.ID_Padre == idGerenciaAnterior || ur.ID == idGerenciaAnterior)&&ur.flagActivo ==1)
                .ToList();

            var subgerencias = unidadesResponsables.Select(u => new { Value = u.ID, Text = u.Nombre }).ToList();

            return Json(subgerencias, JsonRequestBehavior.AllowGet);
        }

        public void DescargarReporteObservaciones(int? prima = null , int? seguros = null, string fechaCorte = null,
                            int? o_check = null, int? o_m_check = null,
                            int? en_f_check = null, int? v_check = null, int? p_check = null,
                            int? c_check = null, int? i_check = null, string filtroAnio = null,string id = null)
        {

            prima = prima ?? 0;
            seguros = seguros ?? 1;
            o_check = o_check ?? 1;
            o_m_check = o_m_check ?? 0;
            en_f_check = en_f_check ?? 1;
            v_check = v_check ?? 1;
            p_check = p_check ?? 1;
            c_check = c_check ?? 0;
            i_check = i_check ?? 1;

            if (string.IsNullOrEmpty(fechaCorte))
            {
                DateTime fechaAct = DateTime.Now;
                fechaCorte = fechaAct.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime fechaParametro = DateTime.ParseExact(fechaCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                fechaCorte = fechaParametro.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(filtroAnio))
            {
                filtroAnio = "2017";
            }

           
            List<fn_Stock_Observaciones_Integrado_v7_Result> reporteObs = db.fn_Stock_Observaciones_Integrado_v7(fechaCorte,prima,seguros, o_check, o_m_check, en_f_check, v_check, p_check, c_check, i_check, filtroAnio, id).ToList();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReporteObservaciones");


            ws.Cells["A1"].Value = "Fuente";
            ws.Cells["B1"].Value = "ID";
            ws.Cells["C1"].Value = "Proyecto";
            ws.Cells["D1"].Value = "Nombre del Proyecto";
            ws.Cells["E1"].Value = "Título Observación";
            ws.Cells["F1"].Value = "Emisor";
            ws.Cells["G1"].Value = "Observación";
            ws.Cells["H1"].Value = "Recomendación";
            ws.Cells["I1"].Value = "Plan de Acción del Responsable";

            ws.Cells["J1"].Value = "Unidad Responsable";
            ws.Cells["K1"].Value = "Propietario";
            ws.Cells["L1"].Value = "Email Propietario";
            ws.Cells["M1"].Value = "Observador(es)";
            ws.Cells["N1"].Value = "Colaborador(es)";
            ws.Cells["O1"].Value = "Fecha Emisión";
            ws.Cells["P1"].Value = "Riesgo";
            ws.Cells["Q1"].Value = "Fecha Estimada";
            ws.Cells["R1"].Value = "Fecha de Vencimiento";
            ws.Cells["S1"].Value = "Última Respuesta";
            ws.Cells["T1"].Value = "Fecha de última respuesta";
            ws.Cells["U1"].Value = "Última Implementación";
            ws.Cells["V1"].Value = "Fecha última implementación";
            ws.Cells["W1"].Value = "Descripcion de Cierre";
            ws.Cells["X1"].Value = "Fecha de Cierre";
            ws.Cells["Y1"].Value = "Fecha de Carga";
            ws.Cells["Z1"].Value = "Coordinador";
            ws.Cells["AA1"].Value = "Tipo de Observación";
            ws.Cells["AB1"].Value = "Estado";
            ws.Cells["AC1"].Value = "Año";
            ws.Cells["AD1"].Value = "Fecha de Corte";
            ws.Cells["AE1"].Value = "Gerencia Nivel 0";
            ws.Cells["AF1"].Value = "Gerencia Nivel 1";
            ws.Cells["AG1"].Value = "Gerencia Nivel 2";
            ws.Cells["AH1"].Value = "Gerencia Nivel 3";
            ws.Cells["AI1"].Value = "Gerencia Nivel 4";
            ws.Cells["AJ1"].Value = "Gerencia Nivel 5";
            ws.Cells["AK1"].Value = "Negocio";
            ws.Cells["AL1"].Value = "Situación";
            ws.Cells["AM1"].Value = "Informe";
            ws.Cells["AN1"].Value = "Días Implementado";
            ws.Cells["AO1"].Value = "Ampliaciones Aprobadas";
            ws.Cells["AP1"].Value = "Último Aprobador";
            ws.Cells["AQ1"].Value = "Motivo";
            ws.Cells["AR1"].Value = "Nuevo Plan";
            ws.Cells["AS1"].Value = "Fecha Propuesta";
            ws.Cells["AT1"].Value = "Retirado";





            var range = ws.Cells["A1:AT1"];

            
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
            foreach (var item in reporteObs)
            {
                ws.Cells[string.Format("A{0}:AT{0}", rowStart)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AT{0}", rowStart)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AT{0}", rowStart)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A{0}:AT{0}", rowStart)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                string identificador = item.Identificador;

                
                Match match = Regex.Match(identificador, @"([A-Z]+)-\d+");

                if (match.Success)
                {
                    
                    string letras = match.Groups[1].Value;

                    
                    ws.Cells[string.Format("A{0}", rowStart)].Value = letras;
                }

                Match match2 = Regex.Match(identificador, @"\d+");

                if (match.Success)
                {
                    // Obtiene la parte numérica
                    string numeros = match2.Value;

                    // Asigna los números al reporte
                    ws.Cells[string.Format("B{0}", rowStart)].Value = numeros;
                }
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.PROYECTO;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.NOMBRE_PROYECTO;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.TITULO_OBSERVACION;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.EMISOR;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.OBSERVACION;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.RECOMENDACION;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.PRIMERA_RESPUESTA;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.UNIDAD_RESPONSABLE;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.PROPIETARIO;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.EMAIL_PROPIETARIO;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.OBSERVADOR;
                ws.Cells[string.Format("N{0}", rowStart)].Value = item.COLABORADOR;
                if (item.FECHA_EMISIÓN.HasValue)
                {
              
                    DateTime fechaEmision = DateTime.ParseExact(item.FECHA_EMISIÓN.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("O{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("O{0}", rowStart)].Value = fechaEmision.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("O{0}", rowStart)].Value = "-";
                }

                
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.RIESGO;
                if (item.FECHA_ESTIMADA.HasValue)
                {

                    DateTime fechaEstimada = DateTime.ParseExact(item.FECHA_ESTIMADA.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("Q{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("Q{0}", rowStart)].Value = fechaEstimada.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("Q{0}", rowStart)].Value = "-";
                }
                if (item.FECHA_DE_VENCIMIENTO.HasValue)
                {

                    DateTime fechaVencimiento = DateTime.ParseExact(item.FECHA_DE_VENCIMIENTO.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("R{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("R{0}", rowStart)].Value = fechaVencimiento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("R{0}", rowStart)].Value = "-";
                }
                ws.Cells[string.Format("S{0}", rowStart)].Value = item.ÚLTIMA_RESPUESTA;

                if (item.FECHA_ÚLTIMA_RESPUESTA.HasValue)
                {

                    DateTime fechaUltimaRespuesta = DateTime.ParseExact(item.FECHA_ÚLTIMA_RESPUESTA.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("T{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("T{0}", rowStart)].Value = fechaUltimaRespuesta.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("T{0}", rowStart)].Value = "-";
                }

                ws.Cells[string.Format("U{0}", rowStart)].Value = item.ÚLTIMA_IMPLEMENTACIÓN;

                if (item.FECHA_ÚLTIMA_IMPLEMENTACIÓN.HasValue)
                {

                    DateTime fechaUltimaImplementacion = DateTime.ParseExact(item.FECHA_ÚLTIMA_IMPLEMENTACIÓN.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("V{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("V{0}", rowStart)].Value = fechaUltimaImplementacion.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("V{0}", rowStart)].Value = "-";
                }
                
                ws.Cells[string.Format("W{0}", rowStart)].Value = item.DESCRIPCIÓN_CIERRE;
                if (item.FECHA_CIERRE.HasValue)
                {

                    DateTime fechaCierre = DateTime.ParseExact(item.FECHA_CIERRE.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("X{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("X{0}", rowStart)].Value = fechaCierre.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("X{0}", rowStart)].Value = "-";
                }
                if (item.FECHA_CARGA.HasValue)
                {

                    DateTime fechaCarga = DateTime.ParseExact(item.FECHA_CARGA.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("Y{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("Y{0}", rowStart)].Value = fechaCarga.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("Y{0}", rowStart)].Value = "-";
                }
                ws.Cells[string.Format("Z{0}", rowStart)].Value = item.COORDINADOR;
                ws.Cells[string.Format("AA{0}", rowStart)].Value = item.TIPO_OBSERVACIÓN;
                ws.Cells[string.Format("AB{0}", rowStart)].Value = item.ESTADO;
                ws.Cells[string.Format("AC{0}", rowStart)].Value = Convert.ToInt32(item.AÑO);
                if (item.FECHA_CORTE.HasValue)
                {

                    DateTime fechaCorte2 = DateTime.ParseExact(item.FECHA_CORTE.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    fechaCorte2 = fechaCorte2.AddDays(1);
                    ws.Cells[string.Format("AD{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("AD{0}", rowStart)].Value = fechaCorte2.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("AD{0}", rowStart)].Value = "-";
                }
                ws.Cells[string.Format("AE{0}", rowStart)].Value = item.GERENCIA_NIVEL_0;
                ws.Cells[string.Format("AF{0}", rowStart)].Value = item.GERENCIA_NIVEL_1;
                ws.Cells[string.Format("AG{0}", rowStart)].Value = item.GERENCIA_NIVEL_2;
                ws.Cells[string.Format("AH{0}", rowStart)].Value = item.GERENCIA_NIVEL_3;
                ws.Cells[string.Format("AI{0}", rowStart)].Value = item.GERENCIA_NIVEL_4;
                ws.Cells[string.Format("AJ{0}", rowStart)].Value = item.GERENCIA_NIVEL_5;
                ws.Cells[string.Format("AK{0}", rowStart)].Value = item.NEGOCIO;
                ws.Cells[string.Format("AL{0}", rowStart)].Value = item.SITUACION;
                ws.Cells[string.Format("AM{0}", rowStart)].Value = item.INFORME;
                ws.Cells[string.Format("AN{0}", rowStart)].Value = Convert.ToInt32(item.DÍAS_IMPLEMENTADO);
                ws.Cells[string.Format("AO{0}", rowStart)].Value = Convert.ToInt32(item.AMPLIACIONES_APROBADAS);
                ws.Cells[string.Format("AP{0}", rowStart)].Value = item.ÚLTIMO_APROBADOR;
                ws.Cells[string.Format("AQ{0}", rowStart)].Value = item.MOTIVO;
                ws.Cells[string.Format("AR{0}", rowStart)].Value = item.NUEVO_PLAN;
                if (item.FECHA_PROPUESTA.HasValue)
                {

                    DateTime fechaPropuesta = DateTime.ParseExact(item.FECHA_PROPUESTA.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    ws.Cells[string.Format("AS{0}", rowStart)].Style.Numberformat.Format = "dd/MM/yyyy";

                    ws.Cells[string.Format("AS{0}", rowStart)].Value = fechaPropuesta.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {

                    ws.Cells[string.Format("AS{0}", rowStart)].Value = "-";
                }

                ws.Cells[string.Format("AT{0}", rowStart)].Value = item.Retirado;


                rowStart++;
            }

            int anchoColumnas = 30;

            
            for (int i = 1; i <= ws.Dimension.End.Column; i++)
            {
                ws.Column(i).Width = anchoColumnas;
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte_Observaciones_corte_" + fechaCorte+".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }




        public ActionResult PV_DetalleFormulario( int prima , int seguros , string fechaCorte,
                            int o_check , int o_m_check,
                            int en_f_check , int v_check , int p_check ,
                            int c_check , int i_check , string filtroAnio , string id )
        {


            if (string.IsNullOrEmpty(fechaCorte))
            {
                DateTime fechaAct = DateTime.Now;
                fechaCorte = fechaAct.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime fechaParametro = DateTime.ParseExact(fechaCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                fechaCorte = fechaParametro.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(filtroAnio))
            {
                filtroAnio = "2017";
            }



            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            var idUsuario = Convert.ToInt32(Session["IdUser"]);
            Session["idRol"] = (from u in db.UsuarioModuloRol where u.IdModulo == 14 && u.IdUsuario == idUsuario select u.IdRol).First();
            ViewBag.idRol = Session["idRol"];

             modelDB.fn_Stock_Observaciones_Integrado_v7 = db.fn_Stock_Observaciones_Integrado_v7(fechaCorte,prima,seguros, o_check, o_m_check, en_f_check, v_check, p_check, c_check, i_check, filtroAnio, id);

            List<TG_Unidad_Responsable> unidadesResponsables = db.TG_Unidad_Responsable
                                            .Where(ur => ur.ID_Padre == 0)
                                            .ToList();

           
            ViewBag.UnidadesResponsables = unidadesResponsables;

            modelDB.fn_Detalle_Ampliaciones = db.fn_Detalle_Ampliaciones(id);
            modelDB.fn_Detalle_Acciones_Obs = db.fn_Detalle_Acciones_Obs(id);

            var aprobadores = db.TG_Aprobador
   .Where(aprovador => aprovador.Estado == 1)
   .Select(o => o.Nombre)
   .Distinct()
   .ToList();


            ViewBag.aprovadores = aprobadores;

            var emisores = db.TG_Observacion_Cambio
  
  .Select(o => o.EMISOR)
  .Distinct()
  .ToList();


            ViewBag.emisores = emisores;





            return PartialView(modelDB);
        }





        [HttpPost]
        public ActionResult PV_DetalleFormulario_Guardar(
            string idObservacion,
            int añoSeleccionado, string unidad_responsable,
            string tiponegocios, string informe,
            string emisor
            )
        {
            try
            {
                string usuario = Convert.ToString(Session["usuario"]);

                if (string.IsNullOrEmpty(emisor))
                {
                    emisor = null;
                }
                if (string.IsNullOrEmpty(tiponegocios))
                {
                    tiponegocios = null;
                }


                int filasAfectadas = db.SP_Observacion_Cambio_Ins_v3(idObservacion, unidad_responsable, tiponegocios, informe, emisor, añoSeleccionado, usuario);

               
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
        public ActionResult PV_Ampliaciones_Guardar(
            string idObservacion,
           string fecha_registro,
           string fecha_propuesta, string motivo,
           string plan, string aprovador
          
           )
        {
            try
            {
                string usuario = Convert.ToString(Session["usuario"]);


                int filasAfectadas = db.SP_Ampliacion_Ins_v1(idObservacion, fecha_registro, fecha_propuesta, motivo, plan, aprovador, usuario);


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





        //ESTRUCTURA DE SEGUIMIENTO




        [HttpGet]
        public ActionResult EstructuraSeguimiento()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);


            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 14 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            modelDB.TG_Unidad_Responsable = from u in db.TG_Unidad_Responsable select u; 
            return View(modelDB);
        }



        public ActionResult PV_VerUnidadResponsable(int idUnidad)
        {
            modelDB.TG_Estructura_Contactos = db.pa_man_TG_Estructura_Contactos(idUnidad);
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 14 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();


            TG_Unidad_Responsable unidadResponsable = db.TG_Unidad_Responsable.Find(idUnidad);
            ViewBag.unidadResponsable = unidadResponsable;
            ViewBag.gerencia0 = new TG_Unidad_Responsable();
            ViewBag.gerencia1 = new TG_Unidad_Responsable();
            ViewBag.gerencia2 = new TG_Unidad_Responsable();
            ViewBag.gerencia3 = new TG_Unidad_Responsable();
            ViewBag.gerencia4 = new TG_Unidad_Responsable();
            ViewBag.gerencia5 = new TG_Unidad_Responsable();

            switch (unidadResponsable.Nivel)
            {
                case 0:
                    ViewBag.gerencia0 = unidadResponsable;
                    break;

                case 1:
                    ViewBag.gerencia0 = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia1 = unidadResponsable;
                    break;
                case 2:
                    ViewBag.gerencia0 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia2 = unidadResponsable;
                    break;
                    
                case 3:
                    ViewBag.gerencia0 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia3 = unidadResponsable;
                    break;
                case 4:
                    ViewBag.gerencia0 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia3 = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia4 = unidadResponsable;
                    break;
                case 5:
                    ViewBag.gerencia0 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia3 = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia4 = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia5 = unidadResponsable;
                    break;
            }

            return PartialView(modelDB);
        }

        public ActionResult PV_AgregarUnidadResponsable(int idUnidad, int? nivel)
        {
            var nivelPadre = 0;

            if (nivel == null)
            {
                TG_Unidad_Responsable unidadResponsable = db.TG_Unidad_Responsable.Find(idUnidad);
                nivelPadre = unidadResponsable.Nivel - 1;
                ViewBag.unidadSeleccionada = unidadResponsable;
                ViewBag.nivelSeleccionado = unidadResponsable.Nivel;
            }
            else
            {
                nivelPadre = (int)nivel - 1;
                ViewBag.unidadSeleccionada = new TG_Unidad_Responsable();
                ViewBag.nivelSeleccionado = nivel;
            }
            ViewBag.gerenciasPadre = (from ur in db.TG_Unidad_Responsable where ur.Nivel == nivelPadre select ur).OrderBy(u => u.Nombre);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_AgregarUnidadResponsable([Bind(Include = "ID, ID_Padre, Nombre, Nivel")] TG_Unidad_Responsable unidadResponsable)
        {
            if (ModelState.IsValid)
            {
                var mismaUnidad = (from u in db.TG_Unidad_Responsable where u.Nombre == unidadResponsable.Nombre select u).Count();

                if (unidadResponsable != null)
                {
                    if (mismaUnidad >= 1)
                    {
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "Ya existe una gerencia creada con el nombre ingresado.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else
                    {
                        unidadResponsable.flagActivo = 1;
                        
                        
                        DateTime fechaActual = DateTime.Now;
                        TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                        {
                            Unidad_Responsable = unidadResponsable.Nombre,
                            Campo = "Unidad Responsable",
                            Accion = "Agregar",
                            Campo_After = unidadResponsable.Nombre,
                            Usuario = Convert.ToString(Session["usuario"]),                       
                            Fecha_Cambio = fechaActual
                           
                        };


                        db.TG_Unidad_Responsable.Add(unidadResponsable);
                        db.TG_Estructura_Seguimiento_Historial.Add(historial);
                        db.SaveChanges();

                        TempData["Icon"] = "success";
                        TempData["Title"] = "Gerencia Creada";
                        TempData["Text"] = "Se han guardatos los cambios satisfactoriamente";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                }
                else
                {
                    TempData["Icon"] = "error";
                    TempData["Title"] = "Error";
                    TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                    return RedirectToAction("EstructuraSeguimiento");
                }

            }
            return PartialView();
        }

        public ActionResult PV_EditarUnidadResponsable(int idUnidad, int? nivel)
        {
            TG_Unidad_Responsable unidadResponsable = db.TG_Unidad_Responsable.Find(idUnidad);
            ViewBag.unidadSeleccionada = unidadResponsable;

            var nivelPadre = 0;

            if (nivel == null)
            {
                ViewBag.nivelSeleccionado = unidadResponsable.Nivel;
                nivelPadre = unidadResponsable.Nivel - 1;
            }
            else
            {
                ViewBag.cambioNivel = 1;
                ViewBag.nivelSeleccionado = nivel;
                nivelPadre = (int)nivel - 1;
            }
            ViewBag.gerenciasPadre = (from ur in db.TG_Unidad_Responsable where ur.Nivel == nivelPadre select ur).OrderBy(u => u.Nombre);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarUnidadResponsable([Bind(Include = "ID, ID_Padre, Nombre, Nivel")] TG_Unidad_Responsable unidadResponsable)
        {
            if (ModelState.IsValid)
            {
                TG_Unidad_Responsable unidadResponsableAnt = db.TG_Unidad_Responsable.AsNoTracking().Where(u => u.ID == unidadResponsable.ID).First();

                bool noHayCambios = (unidadResponsable.ID_Padre == unidadResponsableAnt.ID_Padre) && (unidadResponsable.Nombre == unidadResponsableAnt.Nombre) && (unidadResponsable.Nivel == unidadResponsableAnt.Nivel);

                if (noHayCambios == true)
                {
                    TempData["Icon"] = "error";
                    TempData["Title"] = "Error";
                    TempData["Text"] = "No se han ingresado cambios en la gerencia seleccionada";
                    return RedirectToAction("EstructuraSeguimiento");
                }
                else
                {
                    TG_Unidad_Responsable_Referencia unidadResponsableReferencia = new TG_Unidad_Responsable_Referencia
                    {
                        ID_Unidad_Responsable = unidadResponsableAnt.ID,
                        Nombre = unidadResponsableAnt.Nombre
                    };

                    DateTime fechaActual = DateTime.Now;
                    TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                    {
                        Unidad_Responsable = unidadResponsable.Nombre,
                        Campo = "Unidad Responsable",
                        Accion = "Editar",
                        Campo_After = unidadResponsable.Nombre,
                        Campo_Before = unidadResponsableAnt.Nombre,
                        Usuario = Convert.ToString(Session["usuario"]),
                        Fecha_Cambio = fechaActual

                    };
                    unidadResponsable.flagActivo = 1;
                    db.Entry(unidadResponsable).State = EntityState.Modified;
                    db.TG_Unidad_Responsable_Referencia.Add(unidadResponsableReferencia);
                    db.TG_Estructura_Seguimiento_Historial.Add(historial);
                    db.SaveChanges();

                    TempData["Icon"] = "success";
                    TempData["Title"] = "Actualización realizada";
                    TempData["Text"] = "Se han guardatos los cambios satisfactoriamente";
                    return RedirectToAction("EstructuraSeguimiento");
                }
            }
            else
            {
                TempData["Icon"] = "error";
                TempData["Title"] = "Error";
                TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                return RedirectToAction("EstructuraSeguimiento");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUnidad(int idUnidad)
        {
            TG_Unidad_Responsable unidadResponsable = db.TG_Unidad_Responsable.Find(idUnidad);

            if (unidadResponsable != null)
            {
                unidadResponsable.flagActivo = 0;
                db.Entry(unidadResponsable).State = EntityState.Modified;

                List<int> listaIdPadre = new List<int> { idUnidad };
                List<TG_Unidad_Responsable> listaEliminar = new List<TG_Unidad_Responsable>();

                if (unidadResponsable.Nivel < 5)
                {
                    for (int i = unidadResponsable.Nivel + 1; i <= 5; i++) //POR CADA NIVEL
                    {
                        foreach (var idPadre in listaIdPadre)
                            listaEliminar.AddRange(db.TG_Unidad_Responsable.Where(u => u.Nivel == i && u.ID_Padre == idPadre).ToList());

                        if (listaEliminar.Count() == 0)
                            break;
                        else
                        {
                            foreach (var listaE in listaEliminar)
                            {
                                listaE.flagActivo = 0;
                                db.Entry(listaE).State = EntityState.Modified;
                            }
                        }

                        listaIdPadre.Clear();
                        foreach (var item in listaEliminar)
                            listaIdPadre.Add(item.ID);

                        listaEliminar.Clear();
                    }
                }


                try
                {
                    DateTime fechaActual = DateTime.Now;
                    TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                    {
                        Unidad_Responsable = unidadResponsable.Nombre,
                        Campo = "Unidad Responsable",
                        Accion = "Eliminar",
                        Campo_Before = unidadResponsable.Nombre,
                        Campo_After = "-",

                        Usuario = Convert.ToString(Session["usuario"]),
                        Fecha_Cambio = fechaActual

                    };
                    db.TG_Estructura_Seguimiento_Historial.Add(historial);

                    db.SaveChanges();

                    
                }

                catch (DbEntityValidationException ex)
                {
                    // Manejar los errores de validación aquí
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} - Error: {validationError.ErrorMessage}");
                        }
                    }

                    // Tratar el error como sea necesario, como mostrar mensajes de error al usuario
                }



                TempData["Icon"] = "success";
                TempData["Title"] = "Eliminación realizada";
                TempData["Text"] = "Se han eliminado los datos de la base de datos";
                return RedirectToAction("EstructuraSeguimiento");
            }
            else
            {
                TempData["Icon"] = "error";
                TempData["Title"] = "Error";
                TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                return RedirectToAction("EstructuraSeguimiento");
            }
        }
        // FUNCION PARA OBTENER EN QUE GERENCIA DE NIVEL 0 SE ESTA HACIENDO EL CAMBIO, ESTE DEVUELVE EL OBJETO
        public TG_Unidad_Responsable ObtenerGerencia0(int idUnidad)
        {
            TG_Unidad_Responsable unidadResponsable = db.TG_Unidad_Responsable.Find(idUnidad);
            ViewBag.unidadResponsable = unidadResponsable;
            ViewBag.gerencia0_ = new TG_Unidad_Responsable();

            switch (unidadResponsable.Nivel)
            {
                case 0:
                    ViewBag.gerencia0_ = unidadResponsable;
                    break;

                case 1:
                    ViewBag.gerencia0_ = db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    break;

                case 2:
                    ViewBag.gerencia0_ = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    break;

                case 3:
                    ViewBag.gerencia0_ = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    break;

                case 4:
                    ViewBag.gerencia0_ = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    break;

                case 5:
                    ViewBag.gerencia0_ = db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(db.TG_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    break;
            }

            return ViewBag.gerencia0_ as TG_Unidad_Responsable; // Retornar el valor final de gerencia0_
        }

        // GET PARA OBTENER LOS VALORES O PERSONAS POSIBLES O DISPONIBLES PARA CREAR U NUEVO CONTACTO
        public ActionResult PV_AgregarUnidadContacto(int idUnidad, int? idNuevoRol)
        {
            ViewBag.unidadSeleccionada = db.TG_Unidad_Responsable.Find(idUnidad);
            modelDB.TG_Rol = db.TG_Rol.OrderBy(r => r.Nombre);
            // VERIFICAMOS A QUE GERENCIA SE ESTA HACIENDO CAMBIOS, SI PS O PRIMA Y SEGUNESO SE LE PASA EL VALOR AL RPOCEDURE
            var gerencia0 = ObtenerGerencia0(idUnidad);
            var entidad = "PS";

            if (gerencia0.ID == 426) // Utilizando el valor de gerencia0_ obtenido anteriormente
            {
                entidad = "PRI";
            }

            modelDB.TG_AU_Drop_Contactos = db.TG_AU_Drop_ContactosTM4(0,entidad);

            if (idNuevoRol != null) //SI HAY UN CAMBIO EN EL ROL
            {
                ViewBag.rolSeleccionado = idNuevoRol;

                modelDB.TG_AU_Drop_Contactos = db.TG_AU_Drop_ContactosTM4(idNuevoRol,entidad);
            }

            return PartialView(modelDB);
        }

        //POST PARA AGREGAR CONTACTO_UNIDAD 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_AgregarUnidadContacto([Bind(Include = "ID_Unidad_Responsable, ID_Contacto, ID_Rol")] TG_Unidad_Responsable_Contacto unidadResponsableContacto)
        {
            if (ModelState.IsValid)
            {
                var propietarios = (from c in db.TG_Unidad_Responsable_Contacto
                                    where c.ID_Unidad_Responsable == unidadResponsableContacto.ID_Unidad_Responsable && c.ID_Rol == 2
                                    select c).Count();
                var coordinadores = (from c in db.TG_Unidad_Responsable_Contacto
                                     where c.ID_Unidad_Responsable == unidadResponsableContacto.ID_Unidad_Responsable && c.ID_Rol == 5
                                     select c).Count();

                var mismoContacto = (from c in db.TG_Unidad_Responsable_Contacto
                                     where c.ID_Unidad_Responsable == unidadResponsableContacto.ID_Unidad_Responsable && c.ID_Contacto == unidadResponsableContacto.ID_Contacto
                                     select c).Count();

                
             

                if (unidadResponsableContacto != null)
                {
                    if ((unidadResponsableContacto.ID_Rol == 2 && propietarios > 0) || (unidadResponsableContacto.ID_Rol == 5 && coordinadores > 0)) // VALIDACION NO MÁS DE 1 PROPIETARIO Y COORDINADOR EN UNIDAD RESPONSABLE
                    {
                        TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "Las unidades responsables solo pueden tener asignados un propietario y un coordinador.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else if (mismoContacto >= 1) // SI SE INTENTA GUARDAR UN CONTACTO QUE YA ESTÁ ASIGNADO EN LA UNIDAD 
                    {
                        TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "El contacto ya se encuentra asignado a esta unidad responsable.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else
                    {

                        //PROCEDURE QUE HACE UN CRUD SEGUN LA ACCION A LA TABLA TG_CONTACTO (ACCION 1: INSERT)

                        
                        db.TG_AU_CRUD_URC(1, unidadResponsableContacto.ID_Unidad_Responsable, unidadResponsableContacto.ID_Rol, unidadResponsableContacto.ID_Contacto, 0, "NULL");
                        
                        DateTime fechaActual = DateTime.Now;

                        TG_Unidad_Responsable unidad_responsable = db.TG_Unidad_Responsable.FirstOrDefault(c => c.ID == unidadResponsableContacto.ID_Unidad_Responsable);
                        TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                        {
                            Unidad_Responsable = unidad_responsable.Nombre,
                            Campo = "Unidad Contacto",
                            Accion = "Agregar",
                            Campo_After = unidadResponsableContacto.ID_Contacto,

                            Usuario = Convert.ToString(Session["usuario"]),
                            Fecha_Cambio = fechaActual

                        };
                        db.TG_Estructura_Seguimiento_Historial.Add(historial);
                        db.SaveChanges();




                        TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                        TempData["Icon"] = "success";
                        TempData["Title"] = "Asignación realizada";
                        TempData["Text"] = "Se han guardatos los cambios satisfactoriamente";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                }
                else
                {
                    TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                    TempData["Icon"] = "error";
                    TempData["Title"] = "Error";
                    TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                    return RedirectToAction("EstructuraSeguimiento");
                }

            }
            return PartialView();
        }

        // GET PARA OBTENER VALORES ANTES DE EDITAR LA UNIDAD_CONTACTO
        public ActionResult PV_EditarUnidadContacto(int idUnidadAnt, int idRolAnt, string idContactoAnt, int? idNuevoRol)
        {

            ViewBag.idUnidadAnterior = idUnidadAnt;
            ViewBag.idContactoAnterior = idContactoAnt;
            ViewBag.idRolAnterior = idRolAnt;

            ViewBag.unidadSeleccionada = db.TG_Unidad_Responsable.Find(idUnidadAnt);
            // OBTENEMOS A QUE GERENCIA PERTENECE PARA MOSTRAR LOS VALORES POSIBLES SEGUN SI ES PS O PRIMA

            var gerencia0 = ObtenerGerencia0(idUnidadAnt);
            var entidad = "PS";

            if (gerencia0.ID == 426) 
            {
                entidad = "PRI";
            }

            modelDB.TG_Rol = db.TG_Rol.OrderBy(r => r.Nombre);
            if (idNuevoRol != null) //SI HAY UN CAMBIO EN EL ROL
            {
                ViewBag.rolSeleccionado = db.TG_Rol.Find(idNuevoRol);
                ViewBag.contactoSeleccionado = new TM_User();
                // PROCEDURE PARA ALIMENTAR POSIBLES PERONAS DEPENDIENDO DEL ROL Y SI ES PS O PRIMA
                modelDB.TG_AU_Drop_Contactos = db.TG_AU_Drop_ContactosTM4(idNuevoRol,entidad);
            }
            else
            {
                ViewBag.rolSeleccionado = db.TG_Rol.Find(idRolAnt);
                
                ViewBag.contactoSeleccionado = db.TG_Contacto.Find(idContactoAnt);
                modelDB.TG_AU_Drop_Contactos = db.TG_AU_Drop_ContactosTM4(idRolAnt,entidad);
            }

            return PartialView(modelDB);
        }
        // POST PARA EDITAR UNIDAD_CONTACTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarUnidadContacto(int idUnidadAnt, int idRolAnt, string idContactoAnt, int idNuevoRol, string idNuevoContacto)
        {

            if (ModelState.IsValid)


            {

                TG_Unidad_Responsable_Contacto unidadResponsableContactoAnt = new TG_Unidad_Responsable_Contacto
                {
                    ID_Unidad_Responsable = idUnidadAnt,
                    ID_Contacto = idContactoAnt,
                    ID_Rol = idRolAnt
                };

                TG_Unidad_Responsable_Contacto unidadResponsableContacto = new TG_Unidad_Responsable_Contacto
                {
                    ID_Unidad_Responsable = idUnidadAnt,
                   
                     ID_Contacto = idContactoAnt,
                    ID_Rol = idNuevoRol
                };

                

                var propietarios = (from c in db.TG_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Rol == 2 && c.ID_Contacto != idContactoAnt select c).Count();
                var coordinadores = (from c in db.TG_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Rol == 5 && c.ID_Contacto != idContactoAnt select c).Count();

                var mismoContacto = (from c in db.TG_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Contacto == idNuevoContacto && (c.ID_Rol != idRolAnt || c.ID_Contacto != idContactoAnt) select c).Count();

                if (unidadResponsableContactoAnt != null)
                {
                    if ((idNuevoRol == 2 && propietarios > 0) || (idNuevoRol == 5 && coordinadores > 0)) // VALIDACION NO MÁS DE 1 PROPIETARIO Y COORDINADOR EN UNIDAD RESPONSABLE
                    {
                        TempData["IdUnidad"] = idUnidadAnt;
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "Las unidades responsables solo pueden tener asignados un propietario y un coordinador.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else if (idContactoAnt == idNuevoContacto && idRolAnt == idNuevoRol) // SI SE INTENTA GUARDAR EL MISMO CONTACTO SIN CAMBIOS
                    {
                        TempData["IdUnidad"] = idUnidadAnt;
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "El contacto con el rol asignado ya se encuentra asignado a esta unidad responsable.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else if (mismoContacto >= 1) // SI SE INTENTA GUARDAR UN CONTACTO QUE YA ESTÁ ASIGNADO EN LA UNIDAD 
                    {
                        TempData["IdUnidad"] = idUnidadAnt;
                        TempData["Icon"] = "error";
                        TempData["Title"] = "Error";
                        TempData["Text"] = "El contacto ya se encuentra asignado a esta unidad responsable.";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                    else
                    {

                        //PROCEDURE QUE HACE UN CRUD SEGUN LA ACCION A LA TABLA TG_CONTACTO (ACCION 2: UPDATE)
                        db.TG_AU_CRUD_URC(2, idUnidadAnt, idRolAnt, idContactoAnt, idNuevoRol, idNuevoContacto);

                        DateTime fechaActual = DateTime.Now;


                       

                        TG_Unidad_Responsable unidad_responsable = db.TG_Unidad_Responsable.FirstOrDefault(c => c.ID == idUnidadAnt);
                        TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                        {
                            Unidad_Responsable = unidad_responsable.Nombre,
                            Campo = "Unidad Contacto",
                            Accion = "Editar",
                            Campo_Before = idContactoAnt,
                            Campo_After = idNuevoContacto,

                            Usuario = Convert.ToString(Session["usuario"]),
                            Fecha_Cambio = fechaActual

                        };
                        db.TG_Estructura_Seguimiento_Historial.Add(historial);
                        db.SaveChanges();



                        TempData["IdUnidad"] = idUnidadAnt;
                        TempData["Icon"] = "success";
                        TempData["Title"] = "Actualización realizada";
                        TempData["Text"] = "Se han guardatos los cambios satisfactoriamente";
                        return RedirectToAction("EstructuraSeguimiento");
                    }
                }
                else
                {
                    TempData["IdUnidad"] = idUnidadAnt;
                    TempData["Icon"] = "error";
                    TempData["Title"] = "Error";
                    TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                    return RedirectToAction("EstructuraSeguimiento");
                }
            }
            return PartialView();
        }

        // POST PARA ELIMINAR CONTACTO

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUnidadContacto([Bind(Include = "ID_Unidad_Responsable, ID_Contacto, ID_Rol")] TG_Unidad_Responsable_Contacto unidadResponsableContacto)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            if (unidadResponsableContacto != null)
            {

                //PROCEDURE QUE HACE UN CRUD SEGUN LA ACCION A LA TABLA TG_CONTACTO (ACCION 3: DELETE)
                db.TG_AU_CRUD_URC(3, unidadResponsableContacto.ID_Unidad_Responsable, unidadResponsableContacto.ID_Rol, unidadResponsableContacto.ID_Contacto, 0, "NULL");


                DateTime fechaActual = DateTime.Now;

                TG_Unidad_Responsable unidad_responsable = db.TG_Unidad_Responsable.FirstOrDefault(c => c.ID == unidadResponsableContacto.ID_Unidad_Responsable);
                TG_Estructura_Seguimiento_Historial historial = new TG_Estructura_Seguimiento_Historial
                {
                    Unidad_Responsable = unidad_responsable.Nombre,
                    Campo = "Unidad Contacto",
                    Accion = "Eliminar",
                    Campo_Before = unidadResponsableContacto.ID_Contacto,
                    Campo_After = "-",

                    Usuario = Convert.ToString(Session["usuario"]),
                    Fecha_Cambio = fechaActual

                };
                db.TG_Estructura_Seguimiento_Historial.Add(historial);
                db.SaveChanges();
                TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                TempData["Icon"] = "success";
                TempData["Title"] = "Eliminación realizada";
                TempData["Text"] = "Se han guardatos los cambios satisfactoriamente";
                return RedirectToAction("EstructuraSeguimiento");
            }
            else
            {
                TempData["IdUnidad"] = unidadResponsableContacto.ID_Unidad_Responsable;
                TempData["Icon"] = "error";
                TempData["Title"] = "Error";
                TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
                return RedirectToAction("EstructuraSeguimiento");
            }
        }




    }
}