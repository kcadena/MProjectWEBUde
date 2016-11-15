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
        public AccountController(IHostingEnvironment environment)
        {
            dbu = new DBCUsuarios();
            _environment = environment;
        }
        private IHostingEnvironment _environment;

        //realiza el inicio de cession si el usuario es valido
        [HttpPost]
        public IActionResult Login(usuarios q)
        {
            Dictionary<string, string> dat = new Dictionary<string, string>();
            dat["email"] = q.e_mail;
            dat["pass"] = q.pass;
            q = dbu.loginUsuario(dat);
            if (q != null)
            {
                HttpContext.Session.SetString("UsuNam", q.nombre + " " + q.apellido);
                HttpContext.Session.SetString("idUsu", q.id_usuario.ToString());
                return RedirectToAction("Index", "Projects");
            }
            TempData["err"] = true;
            return RedirectToAction("Index", "Index");
        }
        //Registro de usuarios       
        [HttpPost]
        public async Task<IActionResult> Register(ViewModels.usuarios q)
        {
            //variables de session permiten mostrar mensajes
            HttpContext.Session.SetString("estReg", "");
            HttpContext.Session.SetString("estRegTime", "");

            //validar password viewmodel => no genera error
            if (q.passChan == null)
            {
                ModelState.ClearValidationState("passChan");
                ModelState.MarkFieldValid("passChan");
            }

            DBCUsuarios dbUsu = new DBCUsuarios();
            //verificar que el modelo es valido
            bool stEmail= !dbUsu.verifyEmail(q.e_mail);//revisa que el email no este repetido
            if (ModelState.IsValid && stEmail)
            {
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
                IFormFile file = q.file;
                if (file != null)
                {
                    usu.imagen = "PicProfile-"+usu.id_usuario + ".jpg";
                }
                    int i = 0;
                bool st = false;
                try
                {
                    for (i = 0; i < 5; i++)//realiza 5 veces el registro de generarse error por algun inconveniente de lo contrario muestra mensaje de no registro
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
                        

                        string cont = "Bienvenido: para confirmar su registro a MProject por favor ingrese a: <br>" +
                            "<a href='http://190.254.4.6:94/account/userActivate?id=" + usu.id_usuario + "'>Confirmar</a>";
                        sendEmail(usu.e_mail, cont, "Confirmacion MProject");

                        string path = createDirectory(usu.id_usuario.ToString());
                        
                        if (file != null)
                        {
                            try
                            {
                                var uploads = Path.Combine(_environment.WebRootPath, path);
                                await file.SaveAsAsync(Path.Combine(uploads, usu.imagen));

                            }
                            catch
                            {
                                return RedirectToAction("Index", "Index");
                            }

                        }


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
        }
        //permite activar el usuario despues de haberse logeado => este es llamado desde el correo
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
        //permite recuperar una contraseña
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
        //muestra los datos del usuario a modificar
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
                    if (path != null)
                        ViewBag.srcImg = path;
                }
                catch { }
                return View(usr);
            }
            catch
            {
                return RedirectToAction("Index", "Index");
            }
        }
        //muestra y aplica la actualizacion de los datos del usuario
        [HttpPost]
        public async Task<IActionResult> EditProfile(ViewModels.usuarios usu)
        {
            DBCUsuarios dbUsr = new DBCUsuarios();
            long idUsu = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
            ViewModels.usuarios usr = dbUsr.getUser(idUsu);

            if (usu.passChan == usr.pass)
            {
                ModelState.ClearValidationState("passChan");
                ModelState.MarkFieldValid("passChan");
            }
            if (ModelState.IsValid)
            {
                usuarios axUsu = new usuarios();
                //actualiza informacion de la cuenta
                if (usu.e_mail != null)
                {
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

                    if (dbUsr.updateUserData(axUsu))
                        HttpContext.Session.SetString("UsuNam", axUsu.nombre + " " + axUsu.apellido);
                    HttpContext.Session.SetString("estDat", "true");

                }
                //actualiza contraseña de usuario
                else if (usu.passChan != null)
                {
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
                    dbUsr.updateUserData(axUsu);
                    HttpContext.Session.SetString("estPass", "true");
                }
                //actualiza imagen de perfil
                else if (usu.file != null)
                {
                    IFormFile file = usu.file;

                    if (file != null)
                    {
                        try
                        {
                            var uploads = Path.Combine(_environment.WebRootPath, @"C:\MP\RepositoriosMProject\user" + usu.id_usuario);
                            await file.SaveAsAsync(Path.Combine(uploads, usu.id_usuario+".jpg"));
                            HttpContext.Session.SetString("estImg", "true");
                        }
                        catch
                        {
                            return RedirectToAction("Index", "Projjects");
                        }

                    }
                } 
                return RedirectToAction("Index", "Projects");
            }

            return View(usr);

        }



        //Metodos auxiliares

        //envio de correo electronico
        private void  sendEmail(string email, string content, string subject)
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

        //crea la carpeta dentron del repositorio de mproject
        private string createDirectory(string name)
        {
            try
            {
                string pathString = @"C:\MP\RepositoriosMProject\user" + name;
                System.IO.Directory.CreateDirectory(pathString);//root
                System.IO.Directory.CreateDirectory(pathString + "/multimedia");//multimedia
                System.IO.Directory.CreateDirectory(pathString + "/Proyectos");//proyectos
                System.IO.Directory.CreateDirectory(pathString + "/Log");//log
                return pathString;
            }
            catch
            {
                return "";
            }
        }

    }


}
