using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using OfficeOpenXml;
using System.Data.SqlClient;

using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;
using OfficeOpenXml.Style;
using System.Globalization;
using WebTIGA.Autorizacion;

namespace WebTIGA.Controllers
{
    [Logueado]
    public class CuentasSignificativasController : Controller
    {
        SesionData session = new SesionData();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos modelDB = new ContenedorModelos();

        [HttpGet]
        public ActionResult Inicio(string alert)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);

            if (alert != null)
                ViewBag.alerta = alert;          


            modelDB.VIEW_BC_HOJAS_CARGADAS = db2.View_BC_Hojas_Cargadas;
            modelDB.BCHojaTrabajos = db2.basesBCHojaTrabajo;

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int? Anio, int? Mes)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            try
            {
                var Periodo = Convert.ToInt32(Convert.ToString(Anio) + Convert.ToString(Mes).PadLeft(2, '0'));
                if (file.ContentLength > 0)// && Tipo == "EEFF")
                {
                    string _FileName = Path.GetFileName(file.FileName);

                    string _path = Path.Combine(Server.MapPath("~/Content/uploads"), _FileName);
                    file.SaveAs(_path);
                    var excelFile = new LinqToExcel.ExcelQueryFactory(_path);
                    ViewBag.MensajeLoad = "Visible";
                    //Leemos el CSV y lo pasamos a una lista
                    List<basesBCHojaTrabajo> listaColaboradores = (from p in excelFile.Worksheet("HOJA TRABAJO")
                                                                   select new basesBCHojaTrabajo
                                                                   {
                                                                       AÑO = Anio,
                                                                       PERIODO = Periodo,
                                                                       CUENTA_CONTABLE = p["CUENTA CONTABLE"].Cast<string>(),
                                                                       DESCRIPCION = p["DESCRIPCION"].Cast<string>(),
                                                                       MOVIMIENTO_LOCAL = p["MOVIMIENTO LOCAL"].Cast<decimal>(),
                                                                       SALDO_FINAL_LOCAL = p["SALDO FINAL LOCAL"].Cast<decimal>(),
                                                                       INFORME_VARIACIONES_BG = p["INFORME VARIACIONES BG"].Cast<string>(),
                                                                       INFORME_VARIACIONES_GYP = p["INFORME VARIACIONES GYP"].Cast<string>(),
                                                                       PL_PACIFICO__CONCEPTO_ = p["PL PACIFICO (CONCEPTO)"].Cast<string>(),
                                                                       PL_PACIFICO__AGRUPACION_ = p["PL PACIFICO (AGRUPACION)"].Cast<string>(),
                                                                       FECHA_CARGA = DateTime.Now
                                                                   }).ToList();

                    //Guardamos toda la información de esa lista en base de datos
                    using (var context = new PROYECTOSIAV2Entities1())
                    {
                        foreach (var colaborador in listaColaboradores)
                            if(colaborador.CUENTA_CONTABLE != null)
                            {
                                if (colaborador.INFORME_VARIACIONES_BG == "0") colaborador.INFORME_VARIACIONES_BG = "";
                                if (colaborador.INFORME_VARIACIONES_GYP == "0") colaborador.INFORME_VARIACIONES_GYP = "";
                                if (colaborador.PL_PACIFICO__AGRUPACION_ == "0") colaborador.PL_PACIFICO__AGRUPACION_ = "";
                                if (colaborador.PL_PACIFICO__CONCEPTO_ == "0") colaborador.PL_PACIFICO__CONCEPTO_ = "";

                                context.basesBCHojaTrabajo.Add(colaborador);

                            }
                        //context.basesBCHojaTrabajo.AddRange(listaColaboradores);
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("Inicio", "CuentasSignificativas", new { @alert = "confirm" });

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return RedirectToAction("Inicio", "CuentasSignificativas", new { @alert = "error" });
            }
        }

        public void DescargarHojaTrabajo(int periodo)
        {
            var mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int32.Parse(periodo.ToString().Substring(4, 2)));
            var anio = periodo.ToString().Substring(2, 2);

            List<SP_BC_HOJA_PERIODO_Result> hojaTrabajo = db2.SP_BC_HOJA_PERIODO(periodo).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Cuenta Contable";
            ws.Cells["B1"].Value = "Descipción";
            ws.Cells["C1"].Value = "Movimiento Local";
            ws.Cells["D1"].Value = "Saldo Final Local";
            ws.Cells["E1"].Value = "Informe Variaciones BG";
            ws.Cells["F1"].Value = "Informe Variaciones GyP";
            ws.Cells["G1"].Value = "PL Pacífico (Concepto)";
            ws.Cells["H1"].Value = "PL Pacífico (Agrupación)";

            ws.Cells["A1:H1"].Style.Font.Bold = true;
            ws.Cells[ws.Dimension.Address].AutoFilter = true;
            ws.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            int rowStart = 2;
            foreach(var item in hojaTrabajo)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.CUENTA_CONTABLE;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.DESCRIPCION;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.MOVIMIENTO_LOCAL;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.SALDO_FINAL_LOCAL;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.INFORME_VARIACIONES_BG;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.INFORME_VARIACIONES_GYP;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.PL_PACIFICO__CONCEPTO_;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.PL_PACIFICO__AGRUPACION_;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Carga_"+ mes + "-"+ anio +".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        [HttpPost, ActionName("DeleteFile")]
        public ActionResult DeleteFile(int periodo)
        {
            try
            {
                db2.basesBCHojaTrabajo.RemoveRange(db2.basesBCHojaTrabajo.Where(x => x.PERIODO == periodo));                
                db2.SaveChanges();
                return RedirectToAction("Inicio", "CuentasSignificativas", new { @alert = "deleted" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return RedirectToAction("Inicio", "CuentasSignificativas", new { @alert = "delete-error" });
            }
        }

        public ActionResult AdministracionCuentas(string alert)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            if (alert != null)
                ViewBag.alerta = alert;

            modelDB.VIEW_BC_CUENTAS_PROYECTOS = db2.View_BC_Cuentas_Proyectos;

            return View(modelDB);
        }

        public ActionResult CrearCuentaProyecto(int? idPlan, string alert)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            if (alert != null)
                ViewBag.alerta = alert;

            if (idPlan == null)
                idPlan = Convert.ToInt32((from p in db2.DD_PlanAnual select p.IdPlanAnual).OrderByDescending(p => p).First());

            ViewBag.IdPlanSeleccionado = idPlan;
            ViewBag.NombrePlanSeleccionado = db2.DD_PlanAnual.Find(idPlan).Nombre;

            var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);

            modelDB.DD_PlanAnual = planAnual.ToList();
            ViewBag.ID_Proyecto = new SelectList(db2.SP_BC_PROYECTO_PLAN(idPlan), "IdProyecto", "Codigo");

            return View(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCuentaProyecto([Bind(Include = "Num_Cuenta,ID_Plan,ID_Proyecto,ID")] BC_Cuentas_Proyectos cuentaProyecto)
        {            
            if (ModelState.IsValid)
            {
                var query = from c in db2.BC_Cuentas_Proyectos
                            where c.Num_Cuenta == cuentaProyecto.Num_Cuenta && c.ID_Plan == cuentaProyecto.ID_Plan && c.ID_Proyecto == cuentaProyecto.ID_Proyecto
                            select c;

                if (query.Count() > 0)
                    return RedirectToAction("CrearCuentaProyecto", "CuentasSignificativas", new { @alert = "fallo" });
                else
                {
                    db2.BC_Cuentas_Proyectos.Add(cuentaProyecto);
                    db2.SaveChanges();
                    return RedirectToAction("AdministracionCuentas", "CuentasSignificativas", new { @alert = "creado" });
                }
            }

            return View(cuentaProyecto);
        } 

        public ActionResult EditarCuentaProyecto(int id, int? idPlan, string alert)
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            if (alert != null)
                ViewBag.alerta = alert;

            try
            {
                BC_Cuentas_Proyectos cuentaProyecto = db2.BC_Cuentas_Proyectos.Find(id);
                if(cuentaProyecto != null)
                {
                    var planAnual = (from p in db2.DD_PlanAnual select p).OrderByDescending(p => p.Nombre);

                    modelDB.DD_PlanAnual = planAnual.ToList();

                    if (idPlan == null)
                    {
                        idPlan = cuentaProyecto.ID_Plan;                        

                        ViewBag.IdProyecto = cuentaProyecto.ID_Proyecto;
                        ViewBag.CodProyecto = db2.DD_Proyecto.Find(cuentaProyecto.ID_Proyecto).Codigo;

                    }

                    modelDB.SP_BC_PROYECTO_PLAN = db2.SP_BC_PROYECTO_PLAN(idPlan);

                    ViewBag.NumCuenta = cuentaProyecto.Num_Cuenta;
                    ViewBag.IdPlan = idPlan;
                    ViewBag.NombrePlan = db2.DD_PlanAnual.Find(idPlan).Nombre;

                    ViewBag.Id = id;
                    return View(modelDB);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return RedirectToAction("AdministracionCuentas", "CuentasSignificativas", new { @alert = "error-edicion" });
            }
            return View(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCuentaProyecto([Bind(Include = "Num_Cuenta,ID_Plan,ID_Proyecto,ID")] BC_Cuentas_Proyectos cuentaProyecto)
        {           
            if (ModelState.IsValid)
            {
                var query = from bc in db2.BC_Cuentas_Proyectos
                            where bc.Num_Cuenta == cuentaProyecto.Num_Cuenta && bc.ID_Plan == cuentaProyecto.ID_Plan && bc.ID_Proyecto == cuentaProyecto.ID_Proyecto 
                            select bc;

                if (query.Count() > 0)
                {
                    return RedirectToAction("EditarCuentaProyecto", "CuentasSignificativas", new { @id = cuentaProyecto.ID, @alert = "edit-error" });
                }
                else
                {
                    db2.Entry(cuentaProyecto).State = EntityState.Modified;
                    db2.SaveChanges();
                    return RedirectToAction("AdministracionCuentas", "CuentasSignificativas", new { @alert = "editado" });
                }

            }

            return View();
        }

        [HttpPost, ActionName("EliminarCuentaProyecto")]
        public ActionResult EliminarCuentaProyecto(int id)
        {
            try
            {
                BC_Cuentas_Proyectos cuentaProyecto = db2.BC_Cuentas_Proyectos.Find(id);

                db2.BC_Cuentas_Proyectos.Remove(cuentaProyecto);
                db2.SaveChanges();
                return RedirectToAction("AdministracionCuentas", "CuentasSignificativas", new { @alert = "eliminado" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return RedirectToAction("AdministracionCuentas", "CuentasSignificativas", new { @alert = "error-eliminar" });
            }            
        }

        public ActionResult InformeBalanceGeneral()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            int errorTolerable = 127430000;

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            int periodo = Int32.Parse(periodoVista.First().ToString());
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");

            DateTime periodoDate = Convert.ToDateTime("01/" + periodo.ToString().Substring(4, 2) + "/" + periodo.ToString().Substring(0, 4));
            ViewBag.NombreMes= periodoDate.ToString("MMM yyyy");
            ViewBag.NombreMesAnterior = periodoDate.AddMonths(-1).ToString("MMM yyyy");
                        
            ViewBag.ErrorTolerable = errorTolerable;
            modelDB.SP_BC_BG_INFORME = db2.SP_BC_BG_INFORME(periodo, errorTolerable);           

            return View(modelDB);
        }

        [HttpPost]
        public ActionResult InformeBalanceGeneral(string periodo, int errorTolerable)
        {
            int periodoInt = Int32.Parse(periodo);

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");

            DateTime periodoDate = Convert.ToDateTime("01/" + periodo.Substring(4, 2) + "/" + periodo.Substring(0, 4));
            ViewBag.NombreMes = periodoDate.ToString("MMM yyyy");
            ViewBag.NombreMesAnterior = periodoDate.AddMonths(-1).ToString("MMM yyyy");

            ViewBag.ErrorTolerable = errorTolerable;
            modelDB.SP_BC_BG_INFORME = db2.SP_BC_BG_INFORME(periodoInt, errorTolerable);            

            return View(modelDB);
        }

        public ActionResult InformeGyP()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            int errorTolerable = 127430000;

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            int periodo = Int32.Parse(periodoVista.First().ToString());
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");

            DateTime periodoDate = Convert.ToDateTime("01/" + periodo.ToString().Substring(4, 2) + "/" + periodo.ToString().Substring(0, 4));
            ViewBag.NombreMes = periodoDate.ToString("MMM yyyy");
            ViewBag.NombreMesAnterior = periodoDate.AddMonths(-1).ToString("MMM yyyy");

            ViewBag.ErrorTolerable = errorTolerable;
            modelDB.SP_BC_GYP_INFORME = db2.SP_BC_GYP_INFORME(periodo, errorTolerable);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult InformeGyP(string periodo, int errorTolerable)
        {
            //Cambio de date a int de periodo
            int periodoInt = Int32.Parse(periodo);

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");

            DateTime periodoDate = Convert.ToDateTime("01/" + periodo.Substring(4, 2) + "/" + periodo.Substring(0, 4));
            ViewBag.NombreMes = periodoDate.ToString("MMM yyyy");
            ViewBag.NombreMesAnterior = periodoDate.AddMonths(-1).ToString("MMM yyyy");

            ViewBag.ErrorTolerable = errorTolerable;
            modelDB.SP_BC_GYP_INFORME = db2.SP_BC_GYP_INFORME(periodoInt, errorTolerable);

            return View(modelDB);
        }

        public ActionResult CuentasSignificativas()
        {
            ViewBag.nombre = Convert.ToString(Session["usuario"]);
            int errorTolerable = 127430000;

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            int periodo = Int32.Parse(periodoVista.First().ToString());
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");
            ViewBag.PeriodoSelec = periodo;
            ViewBag.ErrorTolerable = errorTolerable;

            modelDB.SP_BC_RESUMEN_CCSS = db2.SP_BC_RESUMEN_CCSS(periodo, errorTolerable);

            return View(modelDB);
        }
        [HttpPost]
        public ActionResult CuentasSignificativas(string periodo, int errorTolerable)
        {
            int periodoInt = Int32.Parse(periodo);

            var periodoVista = ((from c in db2.basesBCHojaTrabajo select c.PERIODO).Distinct()).OrderByDescending(c => c);
            ViewBag.Periodo = new SelectList(periodoVista.ToList(), "Periodo");
            ViewBag.PeriodoSelec = periodo;
            ViewBag.ErrorTolerable = errorTolerable;

            modelDB.SP_BC_RESUMEN_CCSS = db2.SP_BC_RESUMEN_CCSS(periodoInt, errorTolerable);

            return View(modelDB);
        }

        public void DescargarReporteCCSS(int periodo, int errorTolerable)
        {
            var mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int32.Parse(periodo.ToString().Substring(4, 2)));
            var anio = periodo.ToString().Substring(2, 2);

            List<SP_BC_CUENTAS_CONTABLES_Result> hojaTrabajo = db2.SP_BC_CUENTAS_CONTABLES(periodo, errorTolerable).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");

            ws.Cells["A1"].Value = "BG/PL";
            ws.Cells["B1"].Value = "Cta.";
            ws.Cells["C1"].Value = "Cta 2.";
            ws.Cells["D1"].Value = "Cta 3.";
            ws.Cells["E1"].Value = "Ramo";
            ws.Cells["F1"].Value = "Cuenta Contable";
            ws.Cells["G1"].Value = "Descripción";
            ws.Cells["H1"].Value = "Informe Variaciones BG";
            ws.Cells["I1"].Value = "Informe Variaciones GyP";
            ws.Cells["J1"].Value = "Pacífico (Concepto)";
            ws.Cells["K1"].Value = "Pacífico (Agrupación)";
            ws.Cells["L1"].Value = "Saldo Final Local";
            ws.Cells["M1"].Value = "Movimiento Local";
            ws.Cells["N1"].Value = "Abs.";
            ws.Cells["O1"].Value = "Proyectado a Diciembre";
            ws.Cells["P1"].Value = "Cuenta Significativa Formulada";
            ws.Cells["Q1"].Value = "Cuentas Significativas";
            ws.Cells["R1"].Value = "Cobertura";
            ws.Cells["S1"].Value = "Macroproceso";
            ws.Cells["T1"].Value = "Evaluación";
            ws.Cells["U1"].Value = "Proyecto";

            ws.Cells["A1:U1"].Style.Font.Bold = true;
            ws.Cells[ws.Dimension.Address].AutoFilter = true;
            ws.Cells["A1:U1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            int rowStart = 2;
            foreach (var item in hojaTrabajo)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.BG_PL;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.CTA_;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.CTA_2;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.CTA_3;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.RAMO;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.CUENTA_CONTABLE;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.DESCRIPCION;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.INFORME_VARIACIONES_BG;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.INFORME_VARIACIONES_GYP;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.PACIFICO__CONCEPTO_;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.PACIFICO__AGRUPACIÓN_;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.SALDO_FINAL_LOCAL;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.MOVIMIENTO_LOCAL;
                ws.Cells[string.Format("N{0}", rowStart)].Value = item.ABS;
                ws.Cells[string.Format("O{0}", rowStart)].Value = item.PROYECTADO_A_DICIEMBRE;
                ws.Cells[string.Format("P{0}", rowStart)].Value = item.CUENTA_SIGNIFICATIVA_FORMULADA;
                ws.Cells[string.Format("Q{0}", rowStart)].Value = item.CUENTAS_SIGNIFICATIVAS;
                ws.Cells[string.Format("R{0}", rowStart)].Value = item.COBERTURA;
                ws.Cells[string.Format("S{0}", rowStart)].Value = item.Macroproceso;
                ws.Cells[string.Format("T{0}", rowStart)].Value = item.Evaluación;
                ws.Cells[string.Format("U{0}", rowStart)].Value = item.Codigo;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename= Reporte_Cuentas_" + mes + "-" + anio + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public ActionResult PV_DetalleCuentasSignificativas(string periodo, int errorTolerable)
        {
            int periodoInt = Int32.Parse(periodo);

            //if(macroproceso != null)
                modelDB.SP_BC_CUENTAS_SIGNIFICATIVAS = db2.SP_BC_CUENTAS_SIGNIFICATIVAS(periodoInt, errorTolerable);
            //else
            //{
            //    var query = from bc in db2.SP_BC_CUENTAS_SIGNIFICATIVAS(periodoInt, errorTolerable)
            //                where bc.Macroproceso == macroproceso
            //                select bc;
            //    modelDB.SP_BC_CUENTAS_SIGNIFICATIVAS = query.ToList();
            //}

            return PartialView(modelDB);
        }
    }
}