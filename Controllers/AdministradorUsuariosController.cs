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

using WebTIGA.Autorizacion;

namespace WebTIGA.Controllers
{
    [Logueado]
    public class AdministradorUsuariosController : Controller
    {
        SesionData session = new SesionData();
        PROYECTOSIAV2Entities1 db = new PROYECTOSIAV2Entities1();
        TeamMateR12Entities db2 = new TeamMateR12Entities();
        ContenedorModelos modelDB = new ContenedorModelos();
        RecoveryViewModel model = new RecoveryViewModel();

        public ActionResult Administrador_U(string mensaje_editar, string mensaje_eliminar, string mensaje_crear)
        {
            ViewBag.mensaje = mensaje_editar;
            ViewBag.delete = mensaje_eliminar;
            ViewBag.create = mensaje_crear;

            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            var PersonaVista = from c in db.Persona where c.Activo == 1 select new { c.IdPersona, Nombre_Ape = c.Nombres + " " + c.Apellidos };

            ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape");

            var usuarioModuloRol = db.UsuarioModuloRol.Include(u => u.ModuloRol).Include(u => u.Usuario).OrderBy(u => u.Usuario.Persona.Nombres);
            return View(usuarioModuloRol.ToList());
        }
        [HttpPost]
        public ActionResult Administrador_U(UsuarioModuloRol usuarioModuloRol)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            ViewBag.Message = "Ya fue asignado el registro";
            var query = from u in db.UsuarioModuloRol
                        where u.IdUsuario == usuarioModuloRol.IdUsuario
                        select u;

            var PersonaVista = from c in db.Persona where c.Activo == 1 select new { c.IdPersona, Nombre_Ape = c.Nombres + " " + c.Apellidos };

            ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape", usuarioModuloRol.IdUsuario);

            if (query.Count() > 0)
            {
                var datos = query.ToList();
                return View(datos);
            }
            else
            {
                var datos = query.ToList();
                return View(datos);
            }
        }


        public ActionResult Create(string mensaje_crear_falla)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            ViewBag.Message = mensaje_crear_falla;
            Session["IdModulo"] = 1;
            int IdModulo = Convert.ToInt32(Session["IdModulo"]);
            ViewBag.IdModulo = IdModulo;

            modelDB.Usuario = db.Usuario.ToList();
            modelDB.Modulo1 = db.Modulo.ToList();
            modelDB.SP_WT_ROLES_MODULO_Result = db.SP_WT_ROLES_MODULO(IdModulo);

            var PersonaVista = from c in db.Persona where c.Activo == 1  select new { c.IdPersona, Nombre_Ape = c.Nombres + " " + c.Apellidos };

            ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape");
            ViewBag.ModuloRol = db.SP_WT_ROLES_MODULO(IdModulo);
            ViewBag.IdModulo = new SelectList(db.Modulo, "IdModulo", "Nombre");
            ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(IdModulo), "IdRol", "Tiporol");
         
            return View(modelDB);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,IdModulo,IdRol,ID")] UsuarioModuloRol usuarioModuloRol)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            if (usuarioModuloRol.IdRol == 0)
            {
                var Persona = from c in db.Persona where c.Activo == 1  select new {  c.IdPersona,Nombre_Ape = c.Nombres + " " + c.Apellidos };

                ViewBag.IdUsuario = new SelectList(Persona.ToList(), "IdPersona", "Nombre_Ape");
                modelDB.Usuario = db.Usuario.ToList();
                modelDB.Modulo1 = db.Modulo.ToList();
                modelDB.SP_WT_ROLES_MODULO_Result = db.SP_WT_ROLES_MODULO(usuarioModuloRol.IdModulo);
                ViewBag.IdModulo = new SelectList(db.Modulo, "IdModulo", "Nombre", usuarioModuloRol.IdUsuario);
                ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(usuarioModuloRol.IdModulo), "IdRol", "Tiporol", usuarioModuloRol.IdUsuario);
                return View(modelDB);
            }
            if (ModelState.IsValid)
            {
                var query = from u in db.UsuarioModuloRol
                            where u.IdUsuario == usuarioModuloRol.IdUsuario && u.IdModulo == usuarioModuloRol.IdModulo && u.IdRol == usuarioModuloRol.IdRol
                            select u;

                if (query.Count() > 0)
                {
                    return RedirectToAction("Create", "AdministradorUsuarios", new {  @mensaje_crear_falla = " Ya fue creado el registro con este rol ." });
                }
                else
                {
                    //para capturara rol 
                    if (usuarioModuloRol.IdModulo == 7)
                    {
                        session.setSession("RolVacaciones", usuarioModuloRol.IdRol.ToString());

                    }                  

                    usuarioModuloRol.Estado = 1;
                    db.UsuarioModuloRol.Add(usuarioModuloRol);
                    db.SaveChanges();
                    return RedirectToAction("Administrador_U", "AdministradorUsuarios", new { @mensaje_crear = " El Registro se creo con exito ." });
 
                }

            }
            var PersonaVista = from c in db.Persona select new { c.IdPersona, Nombre_Ape = c.Nombres + " " + c.Apellidos };
            ViewBag.IdUsuario = new SelectList(PersonaVista.ToList(), "IdPersona", "Nombre_Ape", usuarioModuloRol.IdUsuario);
            ViewBag.IdModulo = new SelectList(db.ModuloRol, "IdModulo", "IdModulo", usuarioModuloRol.IdModulo);

            return View(usuarioModuloRol);
        }


        // GET: AdministradorUsuarios/Edit/5
        public ActionResult Edit(int? id,int? idusuario, int? idmodulo,int? idrol,int? Estadoid,string mensaje_editar_falla)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            ViewBag.Message = mensaje_editar_falla;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioModuloRol usuarioModuloRol = db.UsuarioModuloRol.Find(id);
            ViewBag.userN = usuarioModuloRol.Usuario.Persona.Nombres + " " + usuarioModuloRol.Usuario.Persona.Apellidos;

            var usuarioN = from a in db.Usuario
                           where a.IdUsuario==idusuario
                           select a;

            var tiporol = from t in db.ModuloRol
                            where t.IdModulo == idmodulo
                            select t;
            var nombreModulo = from n in db.UsuarioModuloRol
                               where n.ID == id 
                               select n;
            var estadoid = from e in db.ModuloRol
                           where e.IdModulo == idmodulo && e.Estado==Estadoid
                           select e;
            modelDB.ModuloRol = tiporol.ToList();
            modelDB.UsuarioModuloRol = nombreModulo.ToList();
            modelDB.Usuario = usuarioN.ToList();
            modelDB.ModuloRol = estadoid.ToList();
            ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(idmodulo), "IdRol", "Tiporol");
         

            return View(modelDB);
        }

        // POST: AdministradorUsuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,IdModulo,IdRol,ID")] UsuarioModuloRol usuarioModuloRol)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            if (ModelState.IsValid)
            {

                var query = from u in db.UsuarioModuloRol
                            where u.IdUsuario == usuarioModuloRol.IdUsuario && u.IdModulo == usuarioModuloRol.IdModulo && u.IdRol == usuarioModuloRol.IdRol
                            select u;

                if (query.Count() > 0)
                {
                    return RedirectToAction("Edit","AdministradorUsuarios", new { @id= usuarioModuloRol.ID, @idusuario=usuarioModuloRol.IdUsuario, @idmodulo=usuarioModuloRol.IdModulo, @idrol=usuarioModuloRol.IdRol ,@mensaje_editar_falla=" Ya fue creado el registro con este rol ."});
                }
                else
                {
                    usuarioModuloRol.Estado = 1;
                    db.Entry(usuarioModuloRol).State = EntityState.Modified;
                    db.SaveChanges();
                    session.setSession("mensaje", "El Registro se cambio con exito .");
                    return RedirectToAction("Administrador_U", "AdministradorUsuarios", new { @mensaje_editar = "El Registro se edito con exito " });
                }

            }

            ViewBag.IdRol = new SelectList(db.SP_WT_ROLES_MODULO(usuarioModuloRol.IdModulo), "IdRol", "Tiporol", usuarioModuloRol.IdUsuario);

            return View(usuarioModuloRol);
        }

        // GET: AdministradorUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioModuloRol usuarioModuloRol = db.UsuarioModuloRol.Find(id);
            ViewBag.userN = usuarioModuloRol.Usuario.Persona.Nombres + " " + usuarioModuloRol.Usuario.Persona.Apellidos;
            ViewBag.modulo = usuarioModuloRol.ModuloRol.Modulo.Nombre;
            ViewBag.rol = usuarioModuloRol.ModuloRol.Rol.Tiporol;
            ViewBag.estado = usuarioModuloRol.Estado;
            if (usuarioModuloRol == null)
            {
                return HttpNotFound();
            }
            return View(usuarioModuloRol);
        }

        // POST: AdministradorUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            UsuarioModuloRol usuarioModuloRol = db.UsuarioModuloRol.Find(id);

            ViewBag.userN = usuarioModuloRol.Usuario.Persona.Nombres + " " + usuarioModuloRol.Usuario.Persona.Apellidos;
            ViewBag.modulo = usuarioModuloRol.ModuloRol.Modulo.Nombre;
            ViewBag.rol = usuarioModuloRol.ModuloRol.Rol.Tiporol;
            ViewBag.estado = usuarioModuloRol.Estado;
            db.UsuarioModuloRol.Remove(usuarioModuloRol);
            db.SaveChanges();
            //return RedirectToAction("Administrador_U");
            return RedirectToAction("Administrador_U", "AdministradorUsuarios", new { @mensaje_eliminar = "El Registro se eliminó con exito ." });
        }

        public ActionResult Administrador_Usuarios(string alert)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            if (alert != null)
                ViewBag.alerta = alert;

            modelDB.View_AU_Administrador_Usuarios = db.View_AU_Administrador_Usuarios;
            return View(modelDB);
        }

        public ActionResult CrearUsuario(int? idUsuarioTM, string alert)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            if (alert != null)
                ViewBag.alerta = alert;

            modelDB.View_AU_Drop_UsuariosTM = db.View_AU_Drop_UsuariosTM.OrderBy(o=>o.Nombre);
            modelDB.Compañia = from c in db.Compañia select c;

            ViewBag.nombres = "";
            ViewBag.apellidos = "";
            ViewBag.nombreSharepoint = "";
            ViewBag.email = "";
            ViewBag.usuario = "";

            if (idUsuarioTM != null)
            {
                SP_AU_UsuariosTM_Result usuarioTMSelec = db.SP_AU_UsuariosTM(idUsuarioTM).First();

                ViewBag.idUsuarioTMSelec = idUsuarioTM;

                ViewBag.nombres = usuarioTMSelec.FirstName;
                ViewBag.apellidos = usuarioTMSelec.LastName;
                ViewBag.nombreSharepoint = usuarioTMSelec.FirstName + " " + usuarioTMSelec.LastName;
                ViewBag.email = usuarioTMSelec.Email;
                ViewBag.usuario = usuarioTMSelec.LoginName;
            }

            return View(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario([Bind(Include = "IdPersona,IdCompañia,Nombres,Apellidos,Email,Dni,Función,Matrícula,SubGerencia,Equipo,FechaIngreso," +
            "NombreSharepoint,IdUsuario,Usuario,Contraseña,IdRotacion,FechaSalida,Cargo,Planilla,IdJefeDirecto")] View_AU_Persona_Usuario_Rotacion persona_Usuario_Rotacion)
        {
            if (ModelState.IsValid)
            {
                Persona persona = new Persona {
                    IdPersona = (from p in db.Persona select p).OrderByDescending(o => o.IdPersona).First().IdPersona + 1,
                    IdCompañia = persona_Usuario_Rotacion.IdCompañia,
                    Nombres = persona_Usuario_Rotacion.Nombres,
                    Apellidos = persona_Usuario_Rotacion.Apellidos,
                    Email = persona_Usuario_Rotacion.Email,
                    Dni = persona_Usuario_Rotacion.Dni,
                    Activo = 1,
                    Función = persona_Usuario_Rotacion.Función,
                    Matrícula = persona_Usuario_Rotacion.Matrícula,
                    SubGerencia = persona_Usuario_Rotacion.SubGerencia,
                    Equipo = persona_Usuario_Rotacion.Equipo,
                    FechaIngreso = persona_Usuario_Rotacion.FechaIngreso,
                    NombreSharepoint = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint2 = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint3 = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint4 = persona_Usuario_Rotacion.NombreSharepoint
                };

                Usuario usuario = new Usuario
                {
                    IdPersona = persona.IdPersona,
                    IdUsuario = (from u in db.Usuario select u).OrderByDescending(u => u.IdUsuario).First().IdUsuario + 1,
                    Usuario1 = persona_Usuario_Rotacion.Usuario,
                    Contraseña = Encrypt.Base64_Encode(persona_Usuario_Rotacion.Contraseña)
                };

                Rotacion rotacion = new Rotacion
                {
                    IdRotacion = persona_Usuario_Rotacion.IdRotacion,
                    IdPersona = persona.IdPersona,
                    FechaIngreso = persona_Usuario_Rotacion.FechaIngreso,
                    FechaSalida = persona_Usuario_Rotacion.FechaSalida,
                    Función = persona_Usuario_Rotacion.Función,
                    Cargo = persona_Usuario_Rotacion.Cargo,
                    Matrícula = persona_Usuario_Rotacion.Matrícula,
                    SubGerencia = persona_Usuario_Rotacion.SubGerencia,
                    Equipo = persona_Usuario_Rotacion.Equipo,
                    Planilla = persona_Usuario_Rotacion.Planilla,
                    IdJefeDirecto = persona_Usuario_Rotacion.IdJefeDirecto
                };                

                var personaExiste = from p in db.Persona where p.Nombres == persona.Nombres && p.Apellidos == persona.Apellidos select p;
                var usuarioExiste = from u in db.Usuario where u.Usuario1 == usuario.Usuario1 select u;

                if (personaExiste.Count() > 0 || usuarioExiste.Count() > 0) //SI LA PERSONA O EL USUARIO YA EXISTE
                    return RedirectToAction("CrearUsuario", "AdministradorUsuarios", new { @alert = "fallo"});
                else
                {
                    db.Persona.Add(persona);
                    db.Usuario.Add(usuario);
                    db.Rotacion.Add(rotacion);
                    db.SaveChanges();
                    return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "creado" });
                }
            }
            return View();
        }

        public ActionResult PV_EditarUsuario(int idUsuario)
        {
            try
            {
                modelDB.Compañia = from c in db.Compañia select c;
                ViewBag.usuario = db.Usuario.Find(idUsuario);
                ViewBag.persona = db.Persona.Find(db.Usuario.Find(idUsuario).IdPersona);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "error-edicion" });
            }

            return PartialView(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarUsuario([Bind(Include = "IdPersona,IdUsuario,Usuario,Activo,Nombres,Apellidos,NombreSharepoint,Email,Dni,Función,Matrícula,SubGerencia," +
            "Equipo,FechaIngreso,IdCompañia")] View_AU_Persona_Usuario_Rotacion persona_Usuario_Rotacion)
        {
            if (ModelState.IsValid)
            {              
                Usuario usuario = db.Usuario.Find(persona_Usuario_Rotacion.IdUsuario);
                usuario.Usuario1 = persona_Usuario_Rotacion.Usuario;

                Persona persona = new Persona
                {
                    IdPersona = usuario.IdPersona,
                    IdCompañia = persona_Usuario_Rotacion.IdCompañia,
                    Nombres = persona_Usuario_Rotacion.Nombres,
                    Apellidos = persona_Usuario_Rotacion.Apellidos,
                    Email = persona_Usuario_Rotacion.Email,
                    Dni = persona_Usuario_Rotacion.Dni,
                    Activo = persona_Usuario_Rotacion.Activo,
                    Función = persona_Usuario_Rotacion.Función,
                    Matrícula = persona_Usuario_Rotacion.Matrícula,
                    SubGerencia = persona_Usuario_Rotacion.SubGerencia,
                    Equipo = persona_Usuario_Rotacion.Equipo,
                    FechaIngreso = persona_Usuario_Rotacion.FechaIngreso,
                    NombreSharepoint = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint2 = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint3 = persona_Usuario_Rotacion.NombreSharepoint,
                    NombreSharepoint4 = persona_Usuario_Rotacion.NombreSharepoint
                };

                var usuarioExiste = from u in db.Usuario where u.Usuario1 == persona_Usuario_Rotacion.Usuario && u.IdUsuario != persona_Usuario_Rotacion.IdUsuario select u;

                if (usuarioExiste.Count() > 0)
                {
                    return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "error-edicion" });
                }
                else
                {
                    db.Entry(persona).State = EntityState.Modified;
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "editado" });
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int idUsuario)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            Usuario usuario = db.Usuario.Find(idUsuario);
            Persona persona = db.Persona.Find(usuario.IdPersona);


            if (persona != null)
            {
                db.Rotacion.RemoveRange(db.Rotacion.Where(r => r.IdPersona == persona.IdPersona));
                db.UsuarioModuloRol.RemoveRange(db.UsuarioModuloRol.Where(u => u.IdUsuario == usuario.IdUsuario));
                db.Usuario.Remove(usuario);
                db.Persona.Remove(persona);
                db.SaveChanges();
                return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "eliminado" });
            }
            else
            {
                return RedirectToAction("Administrador_Usuarios", "AdministradorUsuarios", new { @alert = "error" });
            }
        }

        public ActionResult Administrador_Rotacion(int idPersona, string alert)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            if (alert != null)
                ViewBag.alerta = alert;

            ViewBag.idPersonaSelec = idPersona;

            modelDB.View_AU_Administrador_Usuarios = db.View_AU_Administrador_Usuarios.OrderBy(p=>p.Nombre);
            modelDB.SP_AU_Rotacion_Usuario = db.SP_AU_Rotacion_Usuario(idPersona);

            return View(modelDB);
        }

        public ActionResult PV_AgregarRotacion(int idPersona)
        {
            ViewBag.idPersona = idPersona;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_AgregarRotacion([Bind(Include = "IdRotacion,IdPersona,FechaIngreso,FechaSalida,Planilla,Matrícula,Función,Cargo,SubGerencia,Equipo,IdJefeDirecto")] Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                Persona persona = db.Persona.Find(rotacion.IdPersona);                         

                if (persona != null)
                {
                    db.Rotacion.Add(rotacion);
                    db.SaveChanges();
                    return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "creado" });
                }
                else
                {
                    return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "error" });
                }
            }
            return View();
        }

        public ActionResult PV_EditarRotacion(int idRotacion)
        {
            ViewBag.rotacion = db.Rotacion.Find(idRotacion);
            modelDB.View_AU_Drop_JefeDirectos = db.View_AU_Drop_JefeDirecto;

            return PartialView(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarRotacion([Bind(Include = "IdRotacion,IdPersona,FechaIngreso,FechaSalida,Planilla,Matrícula,Función,Cargo,SubGerencia,Equipo,IdJefeDirecto")] Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                Persona persona = db.Persona.Find(rotacion.IdPersona);

                if (persona != null)
                {
                    db.Entry(rotacion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "editado" });
                }
                else
                {
                    return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "error" });
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRotacion(int idRotacion)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            Rotacion rotacion = db.Rotacion.Find(idRotacion);
            if(rotacion != null)
            {
                db.Rotacion.Remove(rotacion);
                db.SaveChanges();
                return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "eliminado" });
            }
            else
            {
                return RedirectToAction("Administrador_Rotacion", "AdministradorUsuarios", new { @idPersona = rotacion.IdPersona, @alert = "error" });
            }
        }

        [HttpGet]
        public ActionResult EstructuraSeguimiento()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;

            var idUsuarioValidacion = Convert.ToInt32(Session["IdUser"]);
            ViewBag.idRolValidacion = (from u in db.UsuarioModuloRol where u.IdModulo == 9 && u.IdUsuario == idUsuarioValidacion select u.IdRol).First();

            modelDB.TS_Unidad_Responsable = from u in db.TS_Unidad_Responsable select u;
            return View(modelDB);
        }

        public ActionResult PV_VerUnidadResponsable(int idUnidad)
        {
            modelDB.DS_Estructura_Contactos = db.pa_man_DS_Estructura_Contactos(idUnidad);

            TS_Unidad_Responsable unidadResponsable = db.TS_Unidad_Responsable.Find(idUnidad);
            ViewBag.unidadResponsable = unidadResponsable;
            ViewBag.gerencia0 = new TS_Unidad_Responsable();
            ViewBag.gerencia1 = new TS_Unidad_Responsable();
            ViewBag.gerencia2 = new TS_Unidad_Responsable();
            ViewBag.gerencia3 = new TS_Unidad_Responsable();
            ViewBag.gerencia4 = new TS_Unidad_Responsable();
            ViewBag.gerencia5 = new TS_Unidad_Responsable();

            switch (unidadResponsable.Nivel)
            {
                case 0:                        
                    ViewBag.gerencia0 = unidadResponsable;
                    break;
                        
                case 1:
                    ViewBag.gerencia0 = db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia1 = unidadResponsable;
                    break;
                case 2:
                    ViewBag.gerencia0 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia2 = unidadResponsable;
                    break;
                case 3:
                    ViewBag.gerencia0 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia3 = unidadResponsable;
                    break;
                case 4:
                    ViewBag.gerencia0 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia3 = db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
                    ViewBag.gerencia4 = unidadResponsable;
                    break;
                case 5:
                    ViewBag.gerencia0 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia1 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia2 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre).ID_Padre);
                    ViewBag.gerencia3 = db.TS_Unidad_Responsable.Find(db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre).ID_Padre);
                    ViewBag.gerencia4 = db.TS_Unidad_Responsable.Find(unidadResponsable.ID_Padre);
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
        public ActionResult PV_AgregarUnidadResponsable([Bind(Include = "ID, ID_Padre, Nombre, Nivel")] TS_Unidad_Responsable unidadResponsable)
        {
            if (ModelState.IsValid)
            {
                var mismaUnidad = (from u in db.TS_Unidad_Responsable where u.Nombre == unidadResponsable.Nombre select u).Count();

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
                        db.TS_Unidad_Responsable.Add(unidadResponsable);
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
            TS_Unidad_Responsable unidadResponsable = db.TS_Unidad_Responsable.Find(idUnidad);
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
            ViewBag.gerenciasPadre = (from ur in db.TS_Unidad_Responsable where ur.Nivel == nivelPadre select ur).OrderBy(u => u.Nombre);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarUnidadResponsable([Bind(Include = "ID, ID_Padre, Nombre, Nivel")] TS_Unidad_Responsable unidadResponsable)
        {
            if (ModelState.IsValid)
            {
                TS_Unidad_Responsable unidadResponsableAnt = db.TS_Unidad_Responsable.AsNoTracking().Where(u => u.ID == unidadResponsable.ID).First();

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
                    TS_Unidad_Responsable_Referencia unidadResponsableReferencia = new TS_Unidad_Responsable_Referencia
                    {
                        ID_Unidad_Responsable = unidadResponsableAnt.ID,
                        Nombre = unidadResponsableAnt.Nombre
                    };
                    unidadResponsable.flagActivo = 1;
                    db.Entry(unidadResponsable).State = EntityState.Modified;
                    db.TS_Unidad_Responsable_Referencia.Add(unidadResponsableReferencia);
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EliminarUnidad(int idUnidad)
        //{
        //    TS_Unidad_Responsable unidadResponsable = db.TS_Unidad_Responsable.Find(idUnidad);

        //    if (unidadResponsable != null)
        //    {
        //        db.TS_Unidad_Responsable.Remove(unidadResponsable);

        //        List<int> listaIdPadre = new List<int> { idUnidad };
        //        List<TS_Unidad_Responsable> listaEliminar = new List<TS_Unidad_Responsable>();

        //        if (unidadResponsable.Nivel < 5)
        //        {
        //            for (int i = unidadResponsable.Nivel + 1; i <= 5; i++) //POR CADA NIVEL
        //            {
        //                foreach (var idPadre in listaIdPadre)
        //                {
        //                    listaEliminar.AddRange(db.TS_Unidad_Responsable.Where(u => u.Nivel == i && u.ID_Padre == idPadre).ToList());
        //                }

        //                if (listaEliminar.Count() == 0)
        //                    break;
        //                else
        //                    db.TS_Unidad_Responsable.RemoveRange(listaEliminar);

        //                listaIdPadre.Clear();
        //                foreach (var item in listaEliminar)
        //                {
        //                    listaIdPadre.Add(item.ID);
        //                }
        //                listaEliminar.Clear();
        //            }
        //        }                

        //        db.SaveChanges();

        //        TempData["Icon"] = "success";
        //        TempData["Title"] = "Eliminación realizada";
        //        TempData["Text"] = "Se han eliminado los datos de la base de datos";
        //        return RedirectToAction("EstructuraSeguimiento");
        //    }
        //    else
        //    {
        //        TempData["Icon"] = "error";
        //        TempData["Title"] = "Error";
        //        TempData["Text"] = "Ha ocurrido un error en la operación, inténtelo nuevamente";
        //        return RedirectToAction("EstructuraSeguimiento");
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUnidad(int idUnidad)
        {
            TS_Unidad_Responsable unidadResponsable = db.TS_Unidad_Responsable.Find(idUnidad);

            if (unidadResponsable != null)
            {
                unidadResponsable.flagActivo = 0;
                db.Entry(unidadResponsable).State = EntityState.Modified;

                List<int> listaIdPadre = new List<int> { idUnidad };
                List<TS_Unidad_Responsable> listaEliminar = new List<TS_Unidad_Responsable>();

                if (unidadResponsable.Nivel < 5)
                {
                    for (int i = unidadResponsable.Nivel + 1; i <= 5; i++) //POR CADA NIVEL
                    {
                        foreach (var idPadre in listaIdPadre)                        
                            listaEliminar.AddRange(db.TS_Unidad_Responsable.Where(u => u.Nivel == i && u.ID_Padre == idPadre).ToList());

                        if (listaEliminar.Count() == 0)
                            break;
                        else
                        {
                            foreach(var listaE in listaEliminar)
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

                db.SaveChanges();

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

        public ActionResult PV_AgregarUnidadContacto(int idUnidad, int? idNuevoRol)
        {           
            ViewBag.unidadSeleccionada = db.TS_Unidad_Responsable.Find(idUnidad);
            modelDB.TS_Rol = db.TS_Rol.OrderBy(r => r.Nombre);
            modelDB.sP_AU_Drop_Contactos = db.SP_AU_Drop_ContactosTM(0);

            if (idNuevoRol != null) //SI HAY UN CAMBIO EN EL ROL
            {
                ViewBag.rolSeleccionado = idNuevoRol;
                modelDB.sP_AU_Drop_Contactos = db.SP_AU_Drop_ContactosTM(idNuevoRol);
            }

            return PartialView(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_AgregarUnidadContacto([Bind(Include = "ID_Unidad_Responsable, ID_Contacto, ID_Rol")] TS_Unidad_Responsable_Contacto unidadResponsableContacto)
        {
            if (ModelState.IsValid)
            {
                var propietarios = (from c in db.TS_Unidad_Responsable_Contacto
                                    where c.ID_Unidad_Responsable == unidadResponsableContacto.ID_Unidad_Responsable && c.ID_Rol == 2
                                    select c).Count();
                var coordinadores = (from c in db.TS_Unidad_Responsable_Contacto
                                     where c.ID_Unidad_Responsable == unidadResponsableContacto.ID_Unidad_Responsable && c.ID_Rol == 5
                                     select c).Count();

                var mismoContacto = (from c in db.TS_Unidad_Responsable_Contacto
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
                        db.SP_AU_CRUD_URC(1, unidadResponsableContacto.ID_Unidad_Responsable, unidadResponsableContacto.ID_Rol, unidadResponsableContacto.ID_Contacto, 0, 0);

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

        public ActionResult PV_EditarUnidadContacto(int idUnidadAnt, int idRolAnt, int idContactoAnt, int? idNuevoRol)
        {          

            ViewBag.idUnidadAnterior= idUnidadAnt;
            ViewBag.idContactoAnterior = idContactoAnt;
            ViewBag.idRolAnterior = idRolAnt;

            ViewBag.unidadSeleccionada = db.TS_Unidad_Responsable.Find(idUnidadAnt);

            modelDB.TS_Rol = db.TS_Rol.OrderBy(r => r.Nombre);
            if(idNuevoRol != null) //SI HAY UN CAMBIO EN EL ROL
            {
                ViewBag.rolSeleccionado = db.TS_Rol.Find(idNuevoRol);
                ViewBag.contactoSeleccionado = new TM_User();
                modelDB.sP_AU_Drop_Contactos = db.SP_AU_Drop_ContactosTM(idNuevoRol);
            }
            else
            {
                ViewBag.rolSeleccionado = db.TS_Rol.Find(idRolAnt);
                ViewBag.contactoSeleccionado = db2.TM_User.Find(idContactoAnt);
                modelDB.sP_AU_Drop_Contactos = db.SP_AU_Drop_ContactosTM(idRolAnt);
            }

            return PartialView(modelDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PV_EditarUnidadContacto(int idUnidadAnt, int idRolAnt, int idContactoAnt, int idNuevoRol, int idNuevoContacto)
        {
            if (ModelState.IsValid)
            {
                TS_Unidad_Responsable_Contacto unidadResponsableContactoAnt = new TS_Unidad_Responsable_Contacto
                {
                    ID_Unidad_Responsable = idUnidadAnt,
                    ID_Contacto = idContactoAnt,
                    ID_Rol = idRolAnt
                };

                TS_Unidad_Responsable_Contacto unidadResponsableContacto = new TS_Unidad_Responsable_Contacto
                {
                    ID_Unidad_Responsable = idUnidadAnt,
                    ID_Contacto = idNuevoContacto,
                    ID_Rol = idNuevoRol
                };

                var propietarios = (from c in db.TS_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Rol == 2 && c.ID_Contacto != idContactoAnt select c).Count();
                var coordinadores = (from c in db.TS_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Rol == 5 && c.ID_Contacto != idContactoAnt select c).Count();

                var mismoContacto = (from c in db.TS_Unidad_Responsable_Contacto where c.ID_Unidad_Responsable == idUnidadAnt && c.ID_Contacto == idNuevoContacto && (c.ID_Rol != idRolAnt || c.ID_Contacto != idContactoAnt) select c).Count();

                if (unidadResponsableContactoAnt != null)
                {
                    if ((idNuevoRol == 2 && propietarios > 0) || (idNuevoRol == 5 && coordinadores > 0) ) // VALIDACION NO MÁS DE 1 PROPIETARIO Y COORDINADOR EN UNIDAD RESPONSABLE
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
                        db.SP_AU_CRUD_URC(2, idUnidadAnt, idRolAnt, idContactoAnt, idNuevoRol, idNuevoContacto);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUnidadContacto([Bind(Include = "ID_Unidad_Responsable, ID_Contacto, ID_Rol")] TS_Unidad_Responsable_Contacto unidadResponsableContacto)
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
                        
            if (unidadResponsableContacto != null)
            {
                db.SP_AU_CRUD_URC(3, unidadResponsableContacto.ID_Unidad_Responsable, unidadResponsableContacto.ID_Rol, unidadResponsableContacto.ID_Contacto, 0, 0);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Administrador_Modulos()
        {
            var nombre = Convert.ToString(Session["usuario"]);
            ViewBag.nombre = nombre;
            ContenedorModelos mymodel = new ContenedorModelos();
        
            return View(mymodel);
        }
        


    }
}
