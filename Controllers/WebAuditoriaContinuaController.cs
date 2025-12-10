using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Models;

namespace WebTIGA.Controllers
{
    public class WebAuditoriaContinuaController : Controller
    {
        

        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos modelDB = new ContenedorModelos();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdministrarScript()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var scripts = db2.View_DAC_Vista_List_Scripts;
            modelDB.VIEW_LISTA_SCRIPTS = scripts;
            return View(modelDB);
        }

        public ActionResult RegistrarScript()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var Gerente_Adjunto = from u in db2.Persona
                               where u.Función == "03-Gerente Adjunto" 
                               where u.Activo == 1
                               select new
                               {
                                   id =u.IdPersona,
                                   nombre = u.Nombres +" "+ u.Apellidos
                               };
            var Auditor = from u in db2.Persona
                                  where u.Función == "01-Auditor"
                                  where u.Activo == 1
                                  select new
                                  {
                                      id = u.IdPersona,
                                      nombre = u.Nombres + " " + u.Apellidos
                                  };
            var Area_Auditada = from m in db2.TS_Unidad_Responsable
                                 where m.Nivel == 2
                                 select new
                                 {
                                     id = m.ID,
                                     nombre = m.Nombre
                                 };
            var Contacto = from m in db2.TS_Contacto
                               
                                select new
                                {
                                    id = m.ID,
                                    nombre = m.Nombre + " " +m.Apellido
                                };

            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre");
            ViewBag.idAuditor = new SelectList(Auditor, "id", "nombre");
            ViewBag.idGerenteAdjunto = new SelectList(Gerente_Adjunto, "id", "nombre");
            ViewBag.idAreaAuditada = new SelectList(Area_Auditada, "id", "nombre");
            ViewBag.idContactoAuditado = new SelectList(Contacto, "id", "nombre");
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre"); 
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            modelDB.DAC_ScriptsContinua = db2.DAC_ScriptsContinua;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarScript([Bind(Include = "ID,Código_Script,Año,Negocio,Nombre_Script,Script_Abreviado,Descripción_de_Script,Fuente,idTipScript,Gestor_Responsable,idGerenteAdjunto,idAuditor,idAreaAuditada,idContactoAuditado,Fase_1,Fase_2,Fase_3,Fase_4,Ejecución_de_Script,Cantidad_de_Meses_de_análisis,Ruta")] DAC_ScriptsContinua scripts)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            if (ModelState.IsValid)
            {
                var query = from u in db2.DAC_ScriptsContinua
                            where u.ID == scripts.ID
                            select u;
                if (query.Count() > 0)
                {
                    return RedirectToAction("RegistrarScript", "WebAuditoriaContinua", new { @mensaje_crear_falla = " Ya fue creado el registro con este rol ." });
                }
                else
                {
                    db2.DAC_ScriptsContinua.Add(scripts);
                    db2.SaveChanges();
                    return RedirectToAction("AdministrarScript", "WebAuditoriaContinua", new { @mensaje_crear = " El Registro se creo con exito ." });
                }

            }
            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre");
            ViewBag.idAuditor = new SelectList(db2.Persona, "idPersona", "Nombres");
            ViewBag.idGerenteAdjunto = new SelectList(db2.Persona, "idPersona", "Nombres");
            ViewBag.idAreaAuditada = new SelectList(db2.DD_Proyecto, "ID", "Nombre");
            ViewBag.idContactoAuditado = new SelectList(db2.DD_Proyecto, "ID", "Nombre");
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            return View(scripts);

        }

        public ActionResult EditarRegistro(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAC_ScriptsContinua scripts = db2.DAC_ScriptsContinua.Find(id);

            ViewBag.Nombre_Script = scripts.Nombre_Script;
            ViewBag.Código_Script = scripts.Código_Script;
            ViewBag.Año = scripts.Año;
            ViewBag.Negocio = scripts.Negocio;
            ViewBag.Script_Abreviado = scripts.Script_Abreviado;
            ViewBag.Descripción_de_Script = scripts.Descripción_de_Script;
            ViewBag.Fuente = scripts.Fuente;
            ViewBag.Gestor_Responsable = scripts.Gestor_Responsable;
            ViewBag.Ruta = scripts.Ruta;
            ViewBag.Ejecución_de_Script = scripts.Ejecución_de_Script;
            ViewBag.Cantidad_de_Meses_de_análisis = scripts.Cantidad_de_Meses_de_análisis;

            var Gerente_Adjunto = from u in db2.Persona
                                  where u.Función == "03-Gerente Adjunto"
                                  where u.Activo == 1
                                  select new
                                  {
                                      id = u.IdPersona,
                                      nombre = u.Nombres + " " + u.Apellidos
                                  };
            var Auditor = from u in db2.Persona
                          where u.Función == "01-Auditor"
                          where u.Activo == 1
                          select new
                          {
                              id = u.IdPersona,
                              nombre = u.Nombres + " " + u.Apellidos
                          };
            var Area_Auditada = from m in db2.TS_Unidad_Responsable
                                where m.Nivel == 2
                                select new
                                {
                                    id = m.ID,
                                    nombre = m.Nombre
                                };
            var Contacto = from m in db2.TS_Contacto

                           select new
                           {
                               id = m.ID,
                               nombre = m.Nombre + " " + m.Apellido
                           };
            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre");
            ViewBag.idAuditor = new SelectList(Auditor, "id", "nombre");
            ViewBag.idGerenteAdjunto = new SelectList(Gerente_Adjunto, "id", "nombre");
            ViewBag.idAreaAuditada = new SelectList(Area_Auditada, "id", "nombre");
            ViewBag.idContactoAuditado = new SelectList(Contacto, "id", "nombre");
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre");
            ViewBag.idScript = scripts.ID;
            modelDB.DAC_ScriptsContinua = db2.DAC_ScriptsContinua;

            return View(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRegistro([Bind(Include = "ID,Código_Script,Año,Negocio,Nombre_Script,Script_Abreviado,Descripción_de_Script,Fuente,idTipScript,Gestor_Responsable,idGerenteAdjunto,idAuditor,idAreaAuditada,idContactoAuditado,Fase_1,Fase_2,Fase_3,Fase_4,Ejecución_de_Script,Cantidad_de_Meses_de_análisis,Ruta")] DAC_ScriptsContinua scripts2)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            if (ModelState.IsValid)
            {
                    db2.Entry(scripts2).State = EntityState.Modified;
                    db2.SaveChanges();
                    return RedirectToAction("AdministrarScript", "WebAuditoriaContinua", new { @mensaje_editar = "El Registro se edito con exito " });
            }
            DAC_ScriptsContinua scripts = db2.DAC_ScriptsContinua.Find(scripts2.ID);

            ViewBag.nombreScript = scripts.Nombre_Script;
            ViewBag.codigoScript = scripts.Código_Script;
            ViewBag.Año = scripts.Año;
            ViewBag.idNegocio = scripts.Negocio;
            ViewBag.Script_Abreviado = scripts.Script_Abreviado;
            ViewBag.Descripción_de_Script = scripts.Descripción_de_Script;
            ViewBag.Fuente = scripts.Fuente;
            ViewBag.Gestor_Responsable = scripts.Gestor_Responsable;
            ViewBag.Ruta = scripts.Ruta;
            ViewBag.Ejecución_de_Script = scripts.Ejecución_de_Script;
            ViewBag.Cantidad_de_Meses_de_análisis = scripts.Cantidad_de_Meses_de_análisis;
            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre", scripts.idTipScript);
            ViewBag.idAuditor = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idAuditor);
            ViewBag.idGerenteAdjunto = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idGerenteAdjunto);
            ViewBag.idAreaAuditada = new SelectList(db2.TS_Unidad_Responsable, "ID", "Nombre", scripts.idAreaAuditada);
            ViewBag.idContactoAuditado = new SelectList(db2.TS_Contacto, "ID", "Nombre", scripts.idContactoAuditado);
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_1);
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_2);
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_3);
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_4);
            ViewBag.idScript = scripts.ID;
            modelDB.DAC_ScriptsContinua = db2.DAC_ScriptsContinua;
            return View(scripts);
        }

        public ActionResult Delete(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var scripts = db2.DAC_ScriptsContinua.AsNoTracking().Single(x => x.ID == id);
            ViewBag.ID = scripts.ID;
            ViewBag.nombreScript = scripts.Nombre_Script;
            ViewBag.codigoScript = scripts.Código_Script;
            ViewBag.Año = scripts.Año;
            ViewBag.idNegocio = scripts.Negocio;
            ViewBag.Script_Abreviado = scripts.Script_Abreviado;
            ViewBag.Descripción_de_Script = scripts.Descripción_de_Script;
            ViewBag.Fuente = scripts.Fuente;
            ViewBag.Gestor_Responsable = scripts.Gestor_Responsable;
            ViewBag.Ruta = scripts.Ruta;
            ViewBag.Ejecución_de_Script = scripts.Ejecución_de_Script;
            ViewBag.Cantidad_de_Meses_de_análisis = scripts.Cantidad_de_Meses_de_análisis;

            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre", scripts.idTipScript);
            ViewBag.idAuditor = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idAuditor);
            ViewBag.idGerenteAdjunto = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idGerenteAdjunto);
            ViewBag.idAreaAuditada = new SelectList(db2.TS_Unidad_Responsable, "ID", "Nombre", scripts.idAreaAuditada);
            ViewBag.idContactoAuditado = new SelectList(db2.TS_Contacto, "ID", "Nombre", scripts.idContactoAuditado);
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_1);
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_2);
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_3);
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_4);






            if (scripts == null)
            {
                return HttpNotFound();
            }
            return View(scripts);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

     
            DAC_ScriptsContinua scripts = db2.DAC_ScriptsContinua.Find(id);
            ViewBag.ID = scripts.ID;
            ViewBag.nombreScript = scripts.Nombre_Script;
            ViewBag.codigoScript = scripts.Código_Script;
            ViewBag.Año = scripts.Año;
            ViewBag.idNegocio = scripts.Negocio;
            ViewBag.Script_Abreviado = scripts.Script_Abreviado;
            ViewBag.Descripción_de_Script = scripts.Descripción_de_Script;
            ViewBag.Fuente = scripts.Fuente;
            ViewBag.Gestor_Responsable = scripts.Gestor_Responsable;
            ViewBag.Ruta = scripts.Ruta;
            ViewBag.Ejecución_de_Script = scripts.Ejecución_de_Script;
            ViewBag.Cantidad_de_Meses_de_análisis = scripts.Cantidad_de_Meses_de_análisis;

            ViewBag.idTipScript = new SelectList(db2.DAC_Tipologia_Script, "idTipologiaScript", "nombre", scripts.idTipScript);
            ViewBag.idAuditor = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idAuditor);
            ViewBag.idGerenteAdjunto = new SelectList(db2.Persona, "idPersona", "Nombres", scripts.idGerenteAdjunto);
            ViewBag.idAreaAuditada = new SelectList(db2.TS_Unidad_Responsable, "ID", "Nombre", scripts.idAreaAuditada);
            ViewBag.idContactoAuditado = new SelectList(db2.TS_Contacto, "ID", "Nombre", scripts.idContactoAuditado);
            ViewBag.Fase_1 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_1);
            ViewBag.Fase_2 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_2);
            ViewBag.Fase_3 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_3);
            ViewBag.Fase_4 = new SelectList(db2.DAC_Estado, "idEstado", "nombre", scripts.Fase_4);

            db2.DAC_ScriptsContinua.Remove(scripts);
            db2.SaveChanges();
       
            return RedirectToAction("AdministrarScript", "WebAuditoriaContinua", new { @mensaje_eliminar = "El Registro se eliminó con exito ." });
        }




        public JsonResult JsonGRAFScripts_por_Estado()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
           
            List<SP_DAC_SCRIPTS_POR_ESTADOS_Result> items = new List<SP_DAC_SCRIPTS_POR_ESTADOS_Result>();
            foreach (var item in (db2.SP_DAC_SCRIPTS_POR_ESTADOS()))
            {
                items.Add(new SP_DAC_SCRIPTS_POR_ESTADOS_Result() { nombre = item.nombre, total = item.total });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }


        public ActionResult Reporte1_Script(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var scripts = db2.R1_DupNAB_;
            modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
            return View(scripts.ToList());

        }

        public ActionResult Detalle_Script(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var scripts = db2.R1_DupNAB_;
            modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
            return View(scripts.ToList());

        }
       
        public JsonResult MostrarScripts()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            List<R1_DupNAB_> items = new List<R1_DupNAB_>();
            foreach (var item in (db2.R1_DupNAB_))
            {
                items.Add(new R1_DupNAB_()
                {
                    ID = item.ID,
                    FECDOCOTR = item.FECDOCOTR,
                    CODMONEDA = item.CODMONEDA
                });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        public ActionResult ScriptDetail(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            try
            {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {
                    DAC_ScriptsContinua scripts = db2.DAC_ScriptsContinua.Find(id);
                    

                    Session["Scriptid"] = scripts.ID;
                    int scriptt = Convert.ToInt32(Session["Scriptid"]);
                    ViewBag.IDScript = scriptt;

                    Session["nombreScript"] = scripts.Nombre_Script;
                    string nombrescript = Convert.ToString(Session["nombreScript"]);
                    ViewBag.NombreScript = nombrescript;

                    Session["graficaPowerBI"] = scripts.graficaPowerBI;
                    string graficaPowerBI = Convert.ToString(Session["graficaPowerBI"]);
                    ViewBag.graficaPowerBI = graficaPowerBI;



                    var IDUser = Convert.ToString(Session["IdUser"]);
                    ViewBag.IDUser = IDUser;
                   
                    modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
                    modelDB.DAC_HistorialReportesScript = db2.DAC_HistorialReportesScript;

                    var iduser = Convert.ToInt32(Session["IdUser"]);
                    ViewBag.id = id;
                    var rolv = db2.UsuarioModuloRol.Where(x => x.IdUsuario == iduser).Where(x => x.IdModulo == 11).First().IdRol;
                    ViewBag.rol = rolv;
                    var nombreRol = db2.Rol.Where(x => x.IdRol == rolv).First().Tiporol;
                    ViewBag.nombreRol = nombreRol;
                    return View(modelDB);



                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
            }
            catch
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }

        

        }


        [HttpPost]
        public ActionResult ScriptDetail(HttpPostedFileBase postedFile, int? IDScripts, string Estado, int? IDUser)
        {
            //try
            //{
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var nombrescript = Convert.ToString(Session["nombreScript"]);
            ViewBag.NombreScript = nombrescript;
            if (Convert.ToInt32(Session["IdUser"]) > 0)
            {
                var sc = Convert.ToInt32(Session["Scriptid"]);
                IDScripts = sc;
                var iduser = Convert.ToInt32(Session["IdUser"]);
                IDUser = iduser;

                DateTime localDateTime = DateTime.Now;
                CorreoModel model = new CorreoModel();
                byte[] bytes;
                string Nombre = "";
                int value;
                string Usuario = Convert.ToString(Session["usuario"]);
                string Email = Convert.ToString(Session["Email"]);

                string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    if (postedFile != null)
                    {
                        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                        {
                            bytes = br.ReadBytes(postedFile.ContentLength);
                            value = postedFile.ContentLength;
                        }
                        if (Path.GetFileName(postedFile.FileName).Length > 46)
                        {
                            Nombre = Path.GetFileName(postedFile.FileName).ToString().Substring(1, 46).Trim();
                        }
                        else
                        {
                            Nombre = Path.GetFileName(postedFile.FileName);
                        }
                        string query = "INSERT INTO DAC_HistorialReportesScript (idScript,idUsuario,estado,nombreArchivo,fechaModificacion,contentType,data) VALUES (@idScript,@idUsuario,@estado,@nombreArchivo,@fechaModificacion,@contentType,@data)";

                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@idScript", IDScripts);
                            cmd.Parameters.AddWithValue("@idUsuario", IDUser);
                            cmd.Parameters.AddWithValue("@estado", "");
                            cmd.Parameters.AddWithValue("@nombreArchivo", Nombre);
                            cmd.Parameters.AddWithValue("@fechaModificacion", localDateTime.ToLocalTime());
                            cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                            cmd.Parameters.AddWithValue("@Data", bytes);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }

                }
                return RedirectToAction("ScriptDetail", "WebAuditoriaContinua");

            }
            else
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }
           
        }


        [HttpPost]
        public ActionResult DeleteFileReport(int? fileId, int? IdItem)
        {
            try
            {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {
                    string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];

                    using (SqlConnection con = new SqlConnection(cadena))
                    {
                        string query = "DELETE FROM DAC_HistorialReportesScript WHERE idHistorialReporte=@idHistorialReporte";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@idHistorialReporte", fileId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    return RedirectToAction("ScriptDetail", "WebAuditoriaContinua", new { ID = IdItem });

                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
            }
            catch
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }
        }

        [HttpPost]
        public FileResult DownloadFileReport(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];
            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT nombreArchivo, data, contentType FROM DAC_HistorialReportesScript WHERE idHistorialReporte=@idHistorialReporte";
                    cmd.Parameters.AddWithValue("@idHistorialReporte", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["data"];
                        contentType = sdr["contentType"].ToString();
                        fileName = sdr["nombreArchivo"].ToString();
                    }
                    con.Close();
                }
            }
            return File(bytes, contentType, fileName);
        }



        public ActionResult CargarArchivos(int? id)
        {
            try
            {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {
                    R1_DupNAB_ scripts = db2.R1_DupNAB_.Find(id);
                    ViewBag.IDReporte = scripts.ID;
                    var IDUser = Convert.ToString(Session["IdUser"]);
                    ViewBag.IDUser = IDUser;
                    ViewBag.Estado = new SelectList(db2.R1_DupNAB_, "Estado", "Estado", scripts.Estado);
                    ViewBag.Detalle =  scripts.Detalle;
                    ViewBag.Estado1 = scripts.Estado;
                    modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
                    modelDB.DAC_HistorialCambios = db2.DAC_HistorialCambios;

                    var iduser = Convert.ToInt32(Session["IdUser"]);
                    ViewBag.id = id;
                    var rolv = db2.UsuarioModuloRol.Where(x => x.IdUsuario == iduser).Where(x => x.IdModulo == 11).First().IdRol;
                    ViewBag.rol = rolv;
                    var nombreRol = db2.Rol.Where(x => x.IdRol == rolv).First().Tiporol;
                    ViewBag.nombreRol = nombreRol;
                    return View(modelDB);    

                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
            }
            catch
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }
            
              
            
        }
        //registrar cambio en la tabla historial cambios
        [HttpPost]
        public ActionResult CargarArchivos(HttpPostedFileBase postedFile, int IDReporte,string Estado, string Detalle,int IDUser)
        {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {

                    DateTime localDateTime = DateTime.Now;
                    CorreoModel model = new CorreoModel();
                    byte[] bytes;
                    string Nombre = "";
                    int value;
                    string Usuario = Convert.ToString(Session["usuario"]);
                    string Email = Convert.ToString(Session["Email"]);

                string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];
                using (SqlConnection con = new SqlConnection(cadena))
                    {
                        if (postedFile != null)
                        {
                            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                            {
                                bytes = br.ReadBytes(postedFile.ContentLength);
                                value = postedFile.ContentLength;
                            }
                            if (Path.GetFileName(postedFile.FileName).Length > 46)
                            {
                                Nombre = Path.GetFileName(postedFile.FileName).ToString().Substring(1, 46).Trim();
                            }
                            else
                            {
                                Nombre = Path.GetFileName(postedFile.FileName);
                            }
                            string query = "INSERT INTO DAC_HistorialCambios (idScript,idReporte,idUsuario,estado,detalle,nombreArchivo,fechaModificacion,contentType,data) VALUES (@idScript,@idReporte,@idUsuario,@estado,@detalle,@nombreArchivo,@fechaModificacion,@contentType,@data)";

                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@idScript", 2);
                                cmd.Parameters.AddWithValue("@idReporte", IDReporte);
                                cmd.Parameters.AddWithValue("@idUsuario", IDUser);
                                cmd.Parameters.AddWithValue("@estado", Estado);
                                cmd.Parameters.AddWithValue("@detalle", Detalle);
                                cmd.Parameters.AddWithValue("@nombreArchivo", Nombre);
                                cmd.Parameters.AddWithValue("@fechaModificacion", localDateTime.ToLocalTime());
                                cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                                cmd.Parameters.AddWithValue("@Data", bytes);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();

                            }
                        }

                    }
                    return RedirectToAction("CargarArchivos", "WebAuditoriaContinua");

                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
        
        }


        [HttpPost]
        public ActionResult DeleteFile(int? fileId, int? IdItem)
        {
            try
            {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {
                    string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];

                    using (SqlConnection con = new SqlConnection(cadena))
                    {
                        string query = "DELETE FROM DAC_HistorialCambios WHERE idHistorico=@idHistorico";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@idHistorico", fileId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    return RedirectToAction("CargarArchivos", "WebAuditoriaContinua", new { ID = IdItem });

                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
            }
            catch
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }
        }

        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string cadena = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ToString().Split('"')[1];
            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT nombreArchivo, data, contentType FROM DAC_HistorialCambios WHERE idHistorico=@idHistorico";
                    cmd.Parameters.AddWithValue("@idHistorico", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["data"];
                        contentType = sdr["contentType"].ToString();
                        fileName = sdr["nombreArchivo"].ToString();
                    }
                    con.Close();
                }
            }
            return File(bytes, contentType, fileName);
        }



        public JsonResult JsonGRAFReporte1_Script()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;


            var Query =
            from p in db2.R1_DupNAB_
            group p by p.Estado into g
            select new { Estado = g.Key, Count = g.Count() };

            var q = Query.ToList();

            List<Reporte_script> items = new List<Reporte_script>();
            foreach (var item in (Query.ToList()))
            {
                items.Add(new Reporte_script() { nombre =item.Estado , total = item.Count });
            }
            return (Json(items, JsonRequestBehavior.AllowGet));

        }
        
          public ActionResult EditarReporte1(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            R1_DupNAB_ scripts = db2.R1_DupNAB_.Find(id);

            ViewBag.ID = scripts.ID;
           

            ViewBag.Estado = new SelectList(db2.R1_DupNAB_, "Estado", "Estado", scripts.Estado);
            ViewBag.Estado1 = scripts.Estado;

            ViewBag.aux =                 scripts.AUX;
            ViewBag.AÑOCORTE =         scripts.AÑOCORTE;
            ViewBag.CODAGENCIA =         scripts.CODAGENCIA;
            ViewBag.CODCAJA =                scripts.CODCAJA;
            ViewBag.CODCAJERO    =        scripts.CODCAJERO;
            ViewBag.CODENTFINANING   = scripts.CODENTFINANING;
            ViewBag.CODMONEDA      =   scripts.CODMONEDA;
            ViewBag.CODTARJCRED      = scripts.CODTARJCRED;
            ViewBag.DESCCAJA     =     scripts.DESCCAJA;
            ViewBag.DESCCTAENTFINAN  = scripts.DESCCTAENTFINAN;
            ViewBag.DESCENTFINAN    =  scripts.DESCENTFINAN;
            ViewBag.DIROFIBCP      =   scripts.DIROFIBCP;
            ViewBag.FECDOCOTR     =    scripts.FECDOCOTR;
            ViewBag.FECING      =      scripts.FECING;
            ViewBag.FECVALDOCING    =  scripts.FECVALDOCING;
            ViewBag.IdUsuario      =   scripts.IdUsuario;
            ViewBag.INDCONCIL        = scripts.INDCONCIL;
            ViewBag.MESCORTE        =  scripts.MESCORTE;
            ViewBag.MTODOCINGLOCAL   = scripts.MTODOCINGLOCAL;
            ViewBag.MTODOCINGMONEDA  = scripts.MTODOCINGMONEDA;
            ViewBag.NOMBRE_TERCERO   = scripts.NOMBRE_TERCERO;
            ViewBag.NOMOFIBCP    =     scripts.NOMOFIBCP;
            ViewBag.NUMCTAENTFINAN   = scripts.NUMCTAENTFINAN;
            ViewBag.NUMREFDOCAUX     = scripts.NUMREFDOCAUX;
            ViewBag.NUMRELCTA      =   scripts.NUMRELCTA;
            ViewBag.NUMRELING        = scripts.NUMRELING;
            ViewBag.OBSERVACION      = scripts.OBSERVACION;
            ViewBag.OBSRELING       =  scripts.OBSRELING;
            ViewBag.STSDOCING        = scripts.STSDOCING;
            ViewBag.STSRELING        = scripts.STSRELING;
            ViewBag.TIPO             = scripts.TIPO;
            ViewBag.TIPOCAMBIO       = scripts.TIPOCAMBIO;
            ViewBag.TIPODOCING       = scripts.TIPODOCING;
            ViewBag.TIPOOFIBCP       = scripts.TIPOOFIBCP;
            ViewBag.Detalle = scripts.Detalle;
            modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
            modelDB.DAC_HistorialCambios = db2.DAC_HistorialCambios;
            return View(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarReporte1([Bind(Include = "ID,Estado,Detalle")] R1_DupNAB_ scripts2)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idscript = db2.R1_DupNAB_.AsNoTracking().Single(x => x.ID == scripts2.ID);

            if (ModelState.IsValid)
            {

                scripts2.AUX = idscript.AUX;
                scripts2.AÑOCORTE = idscript.AÑOCORTE;
                scripts2.CODAGENCIA = idscript.CODAGENCIA;
                scripts2.CODCAJA = idscript.CODCAJA;
                scripts2.CODCAJERO = idscript.CODCAJERO;
                scripts2.CODENTFINANING = idscript.CODENTFINANING;
                scripts2.CODMONEDA = idscript.CODMONEDA;
                scripts2.CODTARJCRED = idscript.CODTARJCRED;
                scripts2.DESCCAJA = idscript.DESCCAJA;
                scripts2.DESCCTAENTFINAN = idscript.DESCCTAENTFINAN;
                scripts2.DESCENTFINAN = idscript.DESCENTFINAN;
                scripts2.DIROFIBCP = idscript.DIROFIBCP;
                scripts2.FECDOCOTR = idscript.FECDOCOTR;
                scripts2.FECING = idscript.FECING;
                scripts2.FECVALDOCING = idscript.FECVALDOCING;
                scripts2.IdUsuario = idscript.IdUsuario;
                scripts2.INDCONCIL = idscript.INDCONCIL;
                scripts2.MESCORTE = idscript.MESCORTE;
                scripts2.MTODOCINGLOCAL = idscript.MTODOCINGLOCAL;
                scripts2.MTODOCINGMONEDA = idscript.MTODOCINGMONEDA;
                scripts2.NOMBRE_TERCERO = idscript.NOMBRE_TERCERO;
                scripts2.NOMOFIBCP = idscript.NOMOFIBCP;
                scripts2.NUMCTAENTFINAN = idscript.NUMCTAENTFINAN;
                scripts2.NUMREFDOCAUX = idscript.NUMREFDOCAUX;
                scripts2.NUMRELCTA = idscript.NUMRELCTA;
                scripts2.NUMRELING = idscript.NUMRELING;
                scripts2.OBSERVACION = idscript.OBSERVACION;
                scripts2.OBSRELING = idscript.OBSRELING;
                scripts2.STSDOCING = idscript.STSDOCING;
                scripts2.STSRELING = idscript.STSRELING;
                scripts2.TIPO = idscript.TIPO;
                scripts2.TIPOCAMBIO = idscript.TIPOCAMBIO;
                scripts2.TIPODOCING = idscript.TIPODOCING;
                scripts2.TIPOOFIBCP = idscript.TIPOOFIBCP;
              
                db2.Entry(scripts2).State = EntityState.Modified;
                db2.SaveChanges();

                return RedirectToAction("Reporte1_Script", "WebAuditoriaContinua", new { @mensaje_editar = "El Registro se edito con exito " });

            }
            ViewBag.ID = idscript.ID;
            ViewBag.Estado = new SelectList(db2.R1_DupNAB_, "ID", "Estado", idscript.ID);
            modelDB.R1_DupNAB_ = db2.R1_DupNAB_;
            modelDB.DAC_HistorialCambios = db2.DAC_HistorialCambios;
            return View(idscript);
        }



        [HttpPost]
        public ActionResult NuevoArchivo(HttpPostedFileBase postedFile, int IdItem)
        {
            var idUser = Convert.ToString(Session["IdUser"]);
            try
            {
                if (Convert.ToInt32(Session["IdUser"]) > 0)
                {
                    DateTime localDateTime = DateTime.Now;
                    byte[] bytes;
                    string Nombre = "";
                    int value;
                    using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    {
                        bytes = br.ReadBytes(postedFile.ContentLength);
                        value = postedFile.ContentLength;
                    }
                    if (Path.GetFileName(postedFile.FileName).Length > 46)
                    {
                        Nombre = Path.GetFileName(postedFile.FileName).ToString().Substring(1, 46).Trim();
                    }
                    else
                    {
                        Nombre = Path.GetFileName(postedFile.FileName);
                    }
                    string constr = ConfigurationManager.ConnectionStrings["PROYECTOSIAV2Entities1"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "INSERT INTO DAC_HistorialCambios (idScript,idReporte,idUsuario,estado,detalle,nombreArchivo,fechaModificacion) VALUES (@idScript,@idReporte,@idUsuario,@estado,@detalle,@nombreArchivo,@fechaModificacion)";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@idScript", 1);
                            cmd.Parameters.AddWithValue("@detalle", postedFile.ContentType);
                            cmd.Parameters.AddWithValue("@fechaModificacion", localDateTime.ToLocalTime());
                            cmd.Parameters.AddWithValue("@idReporte", IdItem);
                            cmd.Parameters.AddWithValue("@idUsuario",idUser);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    return RedirectToAction("Reporte1_Script", "WebAuditoriaContinua", new { ID = IdItem });
                }
                else
                {
                    return RedirectToAction("Error", "WebAuditoriaContinua");
                }
            }
            catch
            {
                return RedirectToAction("Error", "WebAuditoriaContinua");
            }
        }




        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = "~/Files/" + ImageName;
            return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));
        }

        private List<string> GetFiles()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Files"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");

            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }

    }
}