using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;

using MProjectWeb.Models.Postgres;
using MProjectWeb.Models.ModelController;

using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;
using Microsoft.AspNet.Hosting;
using System.Runtime.Remoting.Contexts;
using Microsoft.Net.Http.Headers;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MProjectWeb.Controllers
{

    public class AccountController : Controller
    {
        protected DBCUsuarios dbu;
        private MProjectContext db = new MProjectContext();
        private IHostingEnvironment _environment;

        /// <summary>
        /// Constructor de AccountController
        /// </summary>
        /// <param name="environment">Variable que permite obtener la ruta en donde ubicar la imagen de perfil</param>
        public AccountController(IHostingEnvironment environment)
        {
            dbu = new DBCUsuarios();
            _environment = environment;
        }
        
        /// <summary>
        /// realiza el inicio de cession si el usuario es valido
        /// </summary>
        /// <param name="q">Clase de ViewModels que almacena informacion necesaria del usuario</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(usuarios q)
        {
            Dictionary<string, string> dat = new Dictionary<string, string>();
            dat["email"] = q.e_mail;
            dat["pass"] = q.pass;
            q = dbu.loginUsuario(dat);
            if (q != null)
            {
                #region Variables Globales UsuNam: Nombres y apellidos del usuario;  idUsu: ID del usuario logeado
                HttpContext.Session.SetString("UsuNam", q.nombre + " " + q.apellido);
                HttpContext.Session.SetString("idUsu", q.id_usuario.ToString());
                #endregion
                return RedirectToAction("Index", "Projects");
            }
            TempData["err"] = true;
            return RedirectToAction("Index", "Index");
        }
      
        /// <summary>
        /// Registro de usuarios
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(ViewModels.usuarios q)
        {
            #region variables de session permiten mostrar mensajes
            HttpContext.Session.SetString("estReg", "");
            HttpContext.Session.SetString("estRegTime", "");
            #endregion

            #region validar password viewmodel => no genera error
            if (q.passChan == null)
            {
                ModelState.ClearValidationState("passChan");
                ModelState.MarkFieldValid("passChan");
            }
            #endregion

            DBCUsuarios dbUsu = new DBCUsuarios();
            #region Verifica que el modelo es valido
            bool stEmail = !dbUsu.verifyEmail(q.e_mail); //Verifica que el email no este repetido
            if (ModelState.IsValid && stEmail)
            {
                #region Asignar contenido del ViewModels Usuarios a la variable usu de DB.Usuarios
                usuarios usu = new usuarios();
                usu.nombre = q.nombre;
                usu.apellido = q.apellido;
                usu.e_mail = q.e_mail;
                usu.pass = q.pass;
                usu.genero = q.genero;
                usu.entidad = q.entidad;
                usu.cargo = q.cargo;
                usu.telefono = q.telefono;
                usu.administrador = false;
                #endregion

                bool st = false; //Variable que permite saber si se realizo exitosamente el registro del usuario

                #region Proceso que se realiza para el registro del usuario
                int i = 0;
                
                try
                {
                    //realiza 5 veces el registro cuando genere error por algun inconveniente 
                    //de lo contrario muestra mensaje de no registro
                    for (i = 0; i < 5; i++)
                    {
                        usu.id_usuario = dbUsu.regUser(usu);
                        if (usu.id_usuario != -1)
                        {
                            i = 5;
                            st = true;
                        }
                    }
                }
                catch
                {
                    q.aux = true;
                    ModelState.AddModelError(string.Empty, "Error: No se pudo registrar.");
                    HttpContext.Session.SetString("estReg", "false");
                    return RedirectToAction("Index", "Index", q);
                }
                #endregion

                if (!st)
                {
                    q.aux = true;
                    ModelState.AddModelError(string.Empty, "Error: No se pudo registrar. \n Intentelo mas tarde");
                    TempData["errReg"] = true;
                    HttpContext.Session.SetString("estRegTime", "false");
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    try
                    {
                        HttpContext.Session.SetString("estReg", "true");
                        DBCConfiguracion conf = new DBCConfiguracion();
                        string act = conf.getIpPlatServer() + "account/userActivate?id=" + usu.id_usuario;

                        string cont = "Bienvenido: para confirmar su registro a MProject por favor ingrese a: <br>" +
                            "<a href='" + act + "'>Confirmar</a>";
                        sendEmail(usu.e_mail, cont, "Confirmacion MProject");

                        #region Permite cargar la imagen de perfil del usuario a su respectivo repositorio en el servidor
                        IFormFile file = q.file;
                        if (file != null)
                        {
                            try
                            {
                                string path = createDirectory(usu.id_usuario.ToString());
                                var uploads = Path.Combine(_environment.WebRootPath, path);
                                await file.SaveAsAsync(Path.Combine(uploads, usu.imagen));
                            }
                            catch
                            {
                                return RedirectToAction("Index", "Index");
                            }

                        }
                        #endregion


                    }
                    catch
                    {
                        HttpContext.Session.SetString("estReg", "false");
                    }
                    return RedirectToAction("Index", "Index");
                }
            }
            else
            {
                if (!stEmail)
                {
                    ModelState.AddModelError(string.Empty, "El correo ya existe, intenta con otro.");
                }
                ModelState.AddModelError(string.Empty, "Error al registrar usuario.");

                HttpContext.Session.SetString("estReg", "false");
                HttpContext.Session.SetString("estRegTime", "false");
                return View(q);
            }
            #endregion
        }

        /// <summary>
        /// permite activar el usuario despues de haberse logeado => este es llamado desde el correo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult userActivate(long id)
        {
            DBCUsuarios us = new DBCUsuarios();
            try
            {
                usuarios q = us.userActivate(id);
                if (q != null)
                {
                    HttpContext.Session.SetString("UsuNam", q.nombre + " " + q.apellido);
                    HttpContext.Session.SetString("idUsu", q.id_usuario.ToString());
                    return RedirectToAction("Index", "Projects");
                }
                else
                    return RedirectToAction("Index", "Index");
            }
            catch
            {
                return RedirectToAction("Index", "Index");
            }
        }

        /// <summary>
        /// permite recuperar una contraseña
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult fogetPassword(string email)
        {
            DBCUsuarios usr = new DBCUsuarios();
            string pass = usr.forgetPassword(email.ToString());//genera nueva clave aleatoria y se guarda en la base de datos
            string cont = "Apreciado/a Su nueva clave para MProject es:   " + pass;
            sendEmail(email.ToString(), cont, "Recuperacion clave MProject");//envio de correo
            HttpContext.Session.SetString("estPass", "true");
            return Redirect("/Index/Index");
        }

        /// <summary>
        /// muestra los datos del usuario a modificar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditProfile()
        {
            try
            {
                DBCUsuarios dbUsr = new DBCUsuarios();
                long idUsu = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                ViewModels.usuarios usr = dbUsr.getUser(idUsu);
                try
                {
                    string path = usr.path;
                    if (usr.imagen != null)
                        ViewBag.srcImg = path + usr.imagen;
                }
                catch { }
                return View(usr);
            }
            catch
            {
                return RedirectToAction("Index", "Index");
            }
        }

        /// <summary>
        /// muestra y aplica la actualizacion de los datos del usuario
        /// </summary>
        /// <param name="usu"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditProfile(ViewModels.usuarios usu)
        {
            DBCUsuarios dbUsr = new DBCUsuarios();
            long idUsu = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
            ViewModels.usuarios usr = dbUsr.getUser(idUsu);

            #region Verifica que sean iguales los password para marcarlo como valido y no generar error en el modelState
            if (usu.passChan == usr.pass)
            {
                ModelState.ClearValidationState("passChan");
                ModelState.MarkFieldValid("passChan");
            }
            #endregion
            if (ModelState.IsValid)
            {
                usuarios axUsu = new usuarios();
                #region actualiza informacion de la cuenta
                if (usu.e_mail != null)
                {
                    #region Se asignan los valores a la clase Usuarios del Modelo segun la base de datos provenientes del ViewModel/Usuarios

                    //id del usuario
                    axUsu.id_usuario = usr.id_usuario;
                    //datos actualizables
                    axUsu.e_mail = usu.e_mail;
                    axUsu.nombre = usu.nombre;
                    axUsu.apellido = usu.apellido;
                    axUsu.genero = usu.genero;
                    axUsu.cargo = usu.cargo;
                    axUsu.telefono = usu.telefono;
                    axUsu.entidad = usu.entidad;

                    //datos no actualizables
                    axUsu.pass = usr.pass;
                    axUsu.administrador = usr.administrador;
                    axUsu.imagen = usr.imagen;
                    axUsu.disponible = usr.disponible;

                    #endregion

                    if (dbUsr.updateUserData(axUsu))
                        HttpContext.Session.SetString("UsuNam", axUsu.nombre + " " + axUsu.apellido);
                    HttpContext.Session.SetString("estDat", "true");

                }
                #endregion
                #region actualiza contraseña de usuario
                else if (usu.passChan != null)
                {
                    #region Se asignan los valores a la clase Usuarios del Modelo segun la base de datos provenientes del ViewModel/Usuarios
                    //id del usuario
                    axUsu.id_usuario = usr.id_usuario;
                    //datos actualizables
                    axUsu.pass = usu.newPass;
                    //datos no actualizables
                    axUsu.e_mail = usr.e_mail;
                    axUsu.nombre = usr.nombre;
                    axUsu.apellido = usr.apellido;
                    axUsu.genero = usr.genero;
                    axUsu.cargo = usr.cargo;
                    axUsu.telefono = usr.telefono;
                    axUsu.entidad = usr.entidad;
                    axUsu.administrador = usr.administrador;
                    axUsu.imagen = usr.imagen;
                    axUsu.disponible = usr.disponible;
                    #endregion
                    dbUsr.updateUserData(axUsu);
                    HttpContext.Session.SetString("estPass", "true");
                }
                #endregion
                #region actualiza imagen de perfil
                else if (usu.file != null)
                {
                    IFormFile file = usu.file;

                    if (file != null)
                    {
                        try
                        {
                            #region Realiza la copia de la imagen al respectivo repositorio del usuario en el servidor 
                            DBCConfiguracion conf = new DBCConfiguracion();
                            var uploads = Path.Combine(_environment.WebRootPath, conf.getPathServer() + "user" + usu.id_usuario);
                            await file.SaveAsAsync(Path.Combine(uploads, "PicProfile-" + usu.id_usuario + ".jpg"));
                            #endregion
                            HttpContext.Session.SetString("estImg", "true");
                        }
                        catch
                        {
                            return RedirectToAction("Index", "Projjects");
                        }

                    }
                }
                #endregion
                return RedirectToAction("Index", "Projects");
            }

            return View(usr);

        }


        //Metodos auxiliares

        //envio de correo electronico
        /// <summary>
        /// Permite enviar correos electronicos
        /// </summary>
        /// <param name="email"></param>
        /// <param name="content"></param>
        /// <param name="subject"></param>
        private void sendEmail(string email, string content, string subject)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("aslan310593@hotmail.com");//correo de Mproject
            mail.To.Add(email);//agrega el correo destinatario
            mail.Subject = subject;//asunto del mensaje
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = content;//Contenido del mensaje
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aslan310593@hotmail.com", "310593LIVE");//correo electronico de MProject con pass
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

        /// <summary>
        /// Crea las carpetas necesarias dentron del repositorio de mproject
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string createDirectory(string name)
        {
            try
            {
                DBCConfiguracion conf = new DBCConfiguracion();
                string pathString = conf.getPathServer() + "user" + name;
                System.IO.Directory.CreateDirectory(pathString);//root
                System.IO.Directory.CreateDirectory(pathString + "/multimedia");//multimedia
                System.IO.Directory.CreateDirectory(pathString + "/proyectos");//proyectos
                System.IO.Directory.CreateDirectory(pathString + "/log");//log
                System.IO.Directory.CreateDirectory(pathString + "/chat");
                return pathString;
            }
            catch
            {
                return "";
            }
        }

    }


}
