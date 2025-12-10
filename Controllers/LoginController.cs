using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTIGA.Controllers.Encriptar;
using WebTIGA.Models;

namespace WebTIGA.Controllers
{
    public class LoginController : Controller
    {

        SesionData session = new SesionData();
        PROYECTOSIAV2Entities1 db2 = new PROYECTOSIAV2Entities1();
        ContenedorModelos model = new ContenedorModelos();
    
        string urlDomain = "http://172.30.5.41/WebTEC/";

        [AllowAnonymous]
        // GET: Login
        public ActionResult Login_(string comunicado, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (comunicado  ==null)
            {
                ViewBag.Comunicado = "Bienvenido, para acceder ingresar su usuario y contraseña; de no tenerla comunicarse con soporteauditoria@pacifico.com.pe .";
            }
            else
            {
                ViewBag.Comunicado = comunicado;
            }
            return View("Login_");
        }


        [HttpPost]
        [AllowAnonymous]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Login_(UserLogin datos, string returnUrl)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (ModelState.IsValid)
            {
                if (datos.logeo() == true)
                {
                    session.setSession("usuario", datos.Nombres + " " + datos.Apellidos);
                    session.setSession("rol", datos.Rol.ToString());
                    session.setSession("IdUser", datos.ID.ToString());
                    session.setSession("Modulo", datos.Modulo.ToString());
                    ViewBag.User = session.getSession("usuario");

                    Usuario_Log usuarioLog = new Usuario_Log()
                    {
                        ID_Usuario = Convert.ToInt32(Session["IdUser"]),
                        Timestamp = DateTime.Now
                    };

                    db2.Usuario_Log.Add(usuarioLog);
                    db2.SaveChanges();
                    //Session["usuario"] = datos.Usuario;

                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Message1 = "El usuario y la contraseña es incorrecto ";
                    return View("Login_");
                }
            }
            else
            {
                return View("Login_");
            }
        }

        public ActionResult Notificacion_token()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Forgot_Password()
        {
            Models.RecoveryViewModel model = new Models.RecoveryViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Forgot_Password(Models.RecoveryViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.validar_usuario() == true)
            {
                string token = GetSha256(Guid.NewGuid().ToString());
                var IdPers = db2.Usuario.Where(x => x.Usuario1 == model.Usuario).First().IdPersona;
                var datoemail = db2.Persona.Where(y => y.IdPersona == IdPers).First().Email;
                var nombre = db2.Persona.Where(y => y.IdPersona == IdPers).First().Nombres;
                var oUser = db2.Usuario.Where(z => z.Usuario1 == model.Usuario).FirstOrDefault();
                
                if (oUser != null)
                {
                    oUser.token_recovery = token;
                    db2.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                    db2.SaveChanges();
                    ////enviar mail
                    SendEmail(datoemail, token,nombre);
                    session.setSession("Comunicado", "Se envió el correo de restablecimiento de contraseña satisfactoriamente .");
                }
            }
            else
            {
                ViewBag.Message = "El usuario es incorrecto.";
                return View();
            }
            return RedirectToAction("Login_", "Login", new { @comunicado = "Se envió el correo de restablecimiento de contraseña satisfactoriamente ." });
        }

        [HttpGet]
        public ActionResult Recovery(string token)
        {
            Models.RecoveryPasswordViewModel model = new Models.RecoveryPasswordViewModel();
            model.token = token;

            var oUser = db2.Usuario.Where(d => d.token_recovery == model.token).FirstOrDefault();
            if (oUser == null)
            {
                ViewBag.Error = "Tu token ha expirado";
                return RedirectToAction("Notificacion_token", "Login");
            }
            else
            {
                return View(model);
            }            
        }
        [HttpPost]
        public ActionResult Recovery(Models.RecoveryPasswordViewModel model)
        {
            session.setSession("Comunicado", "Restablecimiento de contraseña correcto!");
            if (!ModelState.IsValid)
            {
                return View(model);

            }else
            {            
                var oUser = db2.Usuario.Where(d => d.token_recovery == model.token).FirstOrDefault();
                //encriptar
                string contraseña_encrypt = Encrypt.Base64_Encode(model.Password);
                //
                oUser.Contraseña = contraseña_encrypt;
                oUser.token_recovery = null;
                db2.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                return RedirectToAction("Login_", "Login", new { @comunicado = "Restablecimiento de contraseña correcto!" });
            }

        }



        #region HELPERS
        private string GetSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        private void SendEmail(string EmailDestino, string token,string nombre)
        {
            if (ModelState.IsValid)
            {
                string EmailOrigen = "soporteauditoria@pacifico.com.pe";
                //string Contraseña = "Operador14";
                string url = urlDomain + "/Login/Recovery/?token=" + token;

                MailMessage mm = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña",
                    "<p>Estimado " + nombre + ",nos has solicitado restablecer tu contraseña, para ingresar tu nueva</p>" +
                    "<p>contraseña has click en este link </p>" +
                    "<a href='" + url + "'>Click para recuperar</a>" +
                    "<p>Recuerda que:</p>" +
                    "<p>Si tienes dudas no dudes en contactar en soporteauditoria@pacifico.com.pe</p><br>");

                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "pvida96.pacificovida.com";
                    NetworkCredential NetworkCred = new NetworkCredential("soporteauditoria@pacifico.com.pe", "a");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 25;
                    smtp.Send(mm);
                    mm.Dispose();
                    smtp.Dispose();

                }
            }


        }
                          
        #endregion

    }
}