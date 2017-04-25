using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using MProjectWeb.Models.ModelController;
using MProjectWeb.ViewModels;
using Newtonsoft.Json;

using MProjectWeb.Models.Lucene;

using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using MProjectWeb.LuceneIR;
using MProjectWeb.Models.Postgres;
using Microsoft.AspNet.Cors;




// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MProjectWeb.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            HttpContext.Session.SetString("stFile", "Y");
            DBCProjects h = new DBCProjects();
            try
            {
                long user = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                //var x = h.listProjectsUsers(user);
                //  TempData["prj"] = x;
            }
            catch { }
            HttpContext.Session.SetString("op", webOptions().ToString());
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra todos los proyectos que posee el usuario
        /// </summary>
        /// <returns></returns>
        public IActionResult Projects()
        {
            //HttpContext.Session.Remove("id_prj");
            DBCProjects h = new DBCProjects();
            long user = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
            ViewBag.projects = h.listProjectsUsers(user);
            HttpContext.Session.SetString("infAct", "");
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra todos los proyectos publicos que exista en la base de datos
        /// </summary>
        /// <param name="p">Contiene la informacion de la caracteristica ya sea proyecto o actividad</param>
        /// <returns></returns>
        public IActionResult PublicProjects(string p)
        {
            if (p == null || p.Length == 0)
            {
                HttpContext.Session.SetString("infAct", "");
                HttpContext.Session.SetString("carAct", "");
                p = "";
            }
                
            HttpContext.Session.SetString("stFile", "y");
            string carAct = HttpContext.Session.GetString("infAct");
            if (carAct != p && p.Length>0) 
            {
                DBCActivities dbcAct = new DBCActivities();
                try
                {
                    
                    string[] pro = p.Split('-');

                    try
                    {
                        #region Obtiene la informacion necesaria para mostrar la pagina web basado en proyectos
                        DBCActivities.ActivityInfo car = dbcAct.getInfoPrj(pro);
                        ViewBag.key = car.keym;
                        ViewBag.idCar = car.id_caracteristica;
                        ViewBag.idUsu = car.id_usuario;

                        string pathRep = car.ruta_repositorio + "Web" + car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario + ".html";//ruta 
                        int pos = pathRep.LastIndexOf("/user");
                        int pos2 = pathRep.LastIndexOf("/Web");
                        string usr = pathRep.Substring(pos ,pos2-pos+1);
                        pathRep = pathRep.Substring(pos + 1);

                        pathRep = dbcAct.db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + pathRep;
                        pathRep = pathRep.Replace(@"\", "/");
                        string pathSer = car.ruta_repositorio + "Web" + car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario + ".html";//ruta 

                        if (System.IO.File.Exists(pathRep))
                        {
                            ViewBag.Pagina = pathSer;
                            ViewBag.stPublicPage = true;
                        }
                        else 
                        {
                            pathRep = pathRep.Replace(usr, "/user" + car.usuario_asignado + "/");
                            if (System.IO.File.Exists(pathRep))
                            {
                                pathSer = pathSer.Replace("user" + car.id_usuario, "user" + car.usuario_asignado);
                                ViewBag.Pagina = pathSer;
                                ViewBag.stPublicPage = true;
                            }
                            else
                            {
                                ViewBag.Pagina = pathSer;
                                ViewBag.stPublicPage = false;
                            }
                                
                            
                        }

                        HttpContext.Session.SetString("carAct", car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario);

                        //ViewBag.Pagina = car.ruta_repositorio + car.nombre.ToLower().Replace(" ", "_") + ".html";//ruta 
                        
                        return View();
                        #endregion
                    }
                    catch
                    {
                        #region Obtiene la informacion necesaria para mostrar la pagina web basado en actividades
                        DBCActivities.ActivityInfo car = dbcAct.getInfoAct(pro);
                        ViewBag.key = car.keym;
                        ViewBag.idCar = car.id_caracteristica;
                        ViewBag.idUsu = car.id_usuario;

                        string pathRep = car.ruta_repositorio + "Web" + car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario + ".html";//ruta 
                        int pos = pathRep.LastIndexOf("/user");
                       // pathRep = pathRep.Substring(pos + 1);

                        int pos2 = pathRep.LastIndexOf("/Web");
                        string usr = pathRep.Substring(pos, pos2 - pos + 1);
                        pathRep = pathRep.Substring(pos + 1);

                        pathRep = dbcAct.db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + pathRep;
                        pathRep = pathRep.Replace(@"\", "/");

                        string pathSer = car.ruta_repositorio + "Web" + car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario + ".html";//ruta 

                        if (System.IO.File.Exists(pathRep))
                        {
                            ViewBag.Pagina = pathSer;
                            ViewBag.stPublicPage = true;
                        }
                        else
                        {
                            //pathSer = pathSer.Replace("user" + car.id_usuario, "user" + car.usuario_asignado);
                            //ViewBag.Pagina = pathSer;



                            pathRep = pathRep.Replace(usr, "/user" + car.usuario_asignado + "/");
                            if (System.IO.File.Exists(pathRep))
                            {
                                pathSer = pathSer.Replace("user" + car.id_usuario, "user" + car.usuario_asignado);
                                ViewBag.Pagina = pathSer;
                                ViewBag.stPublicPage = true;
                            }
                            else
                            {
                                ViewBag.Pagina = pathSer;
                                ViewBag.stPublicPage = false;
                            }


                        }
                        
                        HttpContext.Session.SetString("carAct", car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario);
                        //ViewBag.stPublicPage = true;
                        //ViewBag.Pagina = car.ruta_repositorio + car.nombre.Replace(" ", "_") + ".html";//ruta 
                        return View();
                        #endregion
                    }
                    //ViewBag.Pagina = "http://172.16.10.248/prueba%20web/principal1.html";
                    
                }
                catch
                {
                    ViewBag.stPublicPage = false;
                }
                //DBCProjects h = new DBCProjects();
                //ViewBag.projects = h.listPublicProjectsUsers();

                return View();
            }


            DBCProjects dbPrj = new DBCProjects();
            List<ListWebPage> lstWp = dbPrj.seachWebPage();
            ViewBag.lsWp = lstWp;

            ViewBag.stPublicPage = true;
            return View();
        }


        //==========================================       VIEWS HELP       ===============================================//
        /// <summary>
        /// Llama a la vista que muestra un panel extra en donde se visualiza informacion adicional del proyecto.
        /// El llamado se hace a travez de ajax JavaScrip
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PanelProject()
        {
            //var s = JObject.Parse(this.Request.Form.ElementAt(0).Key);
            dynamic dat = Request.Form;
            //var s = json.GetValue("id");
            //var ds = json.GetValue("id");
            string x = dat["id"];

            ViewBag.id_prj = x;

            HttpContext.Session.SetString("id_prj", x);
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra informacion extra de las actividades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ActMoreInfo()
        {
            dynamic dat = Request.Form;
            long id = Convert.ToInt64(dat["id_car"]);
            ViewBag.id_car = id;

            return View();
        }

        //==========================================   VISTAS SUBOPCIONES   ===============================================//
        /// <summary>
        /// Llama a la vista que muestra las actividades correspondientes a un proyecto o una actividad padre
        /// </summary>
        /// <param name="keym"></param>
        /// <param name="idCar"></param>
        /// <param name="usu"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public IActionResult Activity(long keym, long idCar, long usu, int opt)
        {
            try
            {
                long usuAct = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                ViewBag.usuAct = usuAct;
                ViewBag.back = true;
                ViewBag.st = true;
                #region No proviene de proyectos o actividades. solo visualiza la caracteristica actual
                if (opt == 0)
                {
                    try
                    {
                        #region Permite identificar si la caracteristica actual es igual al proyectto para asi no mostrar boton atras de lo contrario lo muestra
                        //el boton atras es un boton que permite regresar o navegar hacia la caracteristica padre y mostrar sus respectivas subactividades
                        string actCarX = HttpContext.Session.GetString("actCar");
                        string actPrjX = HttpContext.Session.GetString("actPrj");
                        if (actCarX.Equals(actPrjX))
                            ViewBag.back = false;
                        #endregion

                        #region Asigna el keym, idCar y usu a sus respectivas variables independientes
                        string[] actCar = actCarX.Split('-');
                        keym = Convert.ToInt64(actCar[0]);
                        idCar = Convert.ToInt64(actCar[1]);
                        usu = Convert.ToInt64(actCar[2]);
                        #endregion

                        #region Obtiene la lista de las actividades correspondientes a la caracteristica
                        DBCActivities actx = new DBCActivities();

                        List<ActivityList> act_lstx = actx.getActivityList(keym, idCar, usu, 1, usuAct);
                        #endregion

                        ViewBag.idCar = act_lstx.First().parCar;
                        ViewBag.usuCar = act_lstx.First().parUsu;
                        ViewBag.keym = act_lstx.First().parKeym;
                        HttpContext.Session.SetString("infAct", act_lstx.First().keym + "-" + act_lstx.First().parCar + "-" + act_lstx.First().parUsu);

                        ViewBag.act_lst = act_lstx;
                    }
                    catch
                    {
                        ViewBag.st = false;
                    }
                    return View();
                }
                #endregion
                #region Origen Proyectos   (  opt = 3  )
                else if (opt == 3)
                {
                    try
                    {
                        ViewBag.back = false;
                        string actCar = keym + "-" + idCar + "-" + usu;
                        HttpContext.Session.SetString("actCar", actCar);
                        HttpContext.Session.SetString("actPrj", actCar);

                        DBCActivities actx = new DBCActivities();
                        List<ActivityList> act_lstx = actx.getActivityList(keym, idCar, usu, 1, usuAct);

                        ViewBag.idCar = act_lstx.First().parCar;
                        ViewBag.usuCar = act_lstx.First().parUsu;
                        ViewBag.keym = act_lstx.First().parKeym;
                        HttpContext.Session.SetString("infAct", act_lstx.First().keym + "-" + act_lstx.First().parCar + "-" + act_lstx.First().parUsu);

                        ViewBag.act_lst = act_lstx;
                    }
                    catch
                    {
                        ViewBag.st = false;
                    }
                    return View();
                }
                #endregion
                #region Origen Actividades en este caso    (  opt = 1    o    opt = 2  )
                else
                {
                    DBCActivities act = new DBCActivities();

                    List<ActivityList> act_lst = act.getActivityList(keym, idCar, usu, opt, usuAct);
                    ViewBag.act_lst = act_lst;
                    try
                    {
                        ViewBag.idCar = act_lst.First().parCar;
                        ViewBag.usuCar = act_lst.First().parUsu;
                        ViewBag.keym = act_lst.First().parKeym;

                        string actCar = act_lst.First().parKeym + "-" + act_lst.First().parCar + "-" + act_lst.First().parUsu;
                        //HttpContext.Session.SetString("actCar", keym + "-" + idCar + "-" + usu);
                        HttpContext.Session.SetString("actCar", actCar);
                        string actPrjX = HttpContext.Session.GetString("actPrj");

                        if (actCar.Equals(actPrjX))
                            ViewBag.back = false;

                        HttpContext.Session.SetString("infAct", act_lst.First().keym + "-" + act_lst.First().parCar + "-" + act_lst.First().parUsu);
                    }
                    catch
                    {
                        return Content("0");
                    }
                }
                #endregion 
            }
            catch { }
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra todos los archivos publicos
        /// </summary>
        /// <param name="type">Corresponde al tipo de archivo que se mostrara</param>
        /// <param name="text">Es el texto con el cual se realizara la buequeda de los archivos</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PublicFiles(string type, string text)
        {
            ViewBag.flag = false;
            if (type == null)
            {
                type = "img";
                ViewBag.flag = true;
            }

            if (text == null)
                text = "";

            LuceneAct lc = new LuceneAct();
            List<Lucene.Net.Search.ScoreDoc> lstDoc = lc.publicFileSearch(text, type);
            ViewBag.lstDoc = lstDoc;
            ViewBag.type = type;
            ViewBag.searcher = lc.searcher;
            if (lc.totSear > 0)
                ViewBag.st = true;
            else
                ViewBag.st = false;

            ViewBag.op = "Imagenes";
            try
            {
                string op = type;
                switch (op)
                {
                    case "img":
                        ViewBag.op = "Imagenes";
                        break;
                    case "vid":
                        ViewBag.op = "Videos";
                        break;
                    case "son":
                        ViewBag.op = "Sonidos";
                        break;
                    case "doc":
                        ViewBag.op = "Documentos";
                        break;
                    case "oth":
                        ViewBag.op = "Otros";
                        break;
                }
            }
            catch { }

            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra los archivos correspondiente a una caracteristica perteneciente al usuario logeado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Files()
        {
            try
            {
                long keym;
                long idCar;
                long idUsu;
                string type;
                string text = "";
                bool pubFil = false;

                try
                {
                    dynamic dat = Request.Form;//Obtiene los datos que llegan desde la llamada AJAX
                    keym = Convert.ToInt64(dat["keym"]);
                    idCar = Convert.ToInt64(dat["idCar"]);
                    idUsu = Convert.ToInt64(dat["idUsu"]);
                    type = dat["type"];
                    pubFil = Convert.ToBoolean(dat["publicFile"]);

                    ViewBag.keym = keym;
                    ViewBag.idCar = idCar;
                    ViewBag.idUsu = idUsu;
                    ViewBag.type = type;

                    try
                    {
                        text = dat["text"];
                    }
                    catch { }
                }
                catch
                {
                    string[] cad = null;
                    try
                    {
                        cad = HttpContext.Session.GetString("actCar").Split('-');
                    }
                    catch { }
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                    type = "img";

                    ViewBag.keym = keym;
                    ViewBag.idCar = idCar;
                    ViewBag.idUsu = idUsu;
                    ViewBag.type = type;
                }

                ArchivosMultimedia arc = new ArchivosMultimedia();

                string car = "", usr = "";
                try
                {
                    car = arc.getCaracteriscaChildren(keym, idUsu, idCar);
                    usr = arc.getUsersCaracteristicas(keym, idUsu, idCar);
                }
                catch (Exception err) { }


                LuceneAct lc = new LuceneAct();


                ViewBag.pubFil = pubFil;
                List<Lucene.Net.Search.ScoreDoc> lstDoc;
                if (!pubFil)
                {
                    Dictionary<string, string> dt = new Dictionary<string, string>();
                    dt["usuAct"] = HttpContext.Session.GetString("idUsu");

                    lstDoc = lc.search(dt, text, type, car, usr);

                }
                else
                {
                    lstDoc = lc.publicSearch(text, type, car);
                }
                //Lucene.Net.Documents.Document doc = lc.searcher.Doc(lstDoc.ElementAt(0).Doc);
                ViewBag.op = "Imagenes";
                try
                {
                    string op = type;
                    switch (op)
                    {
                        case "img":
                            ViewBag.op = "Imagenes";
                            break;
                        case "vid":
                            ViewBag.op = "Videos";
                            break;
                        case "son":
                            ViewBag.op = "Sonidos";
                            break;
                        case "doc":
                            ViewBag.op = "Documentos";
                            break;
                        case "oth":
                            ViewBag.op = "Otros";
                            break;
                    }
                }
                catch { }
                ViewBag.lstDoc = lstDoc;
                ViewBag.searcher = lc.searcher;
                if (lc.totSear > 0)
                    ViewBag.st = true;
                else
                    ViewBag.st = false;
            }
            catch (Exception err)
            {
                ViewBag.st = false;
            }
            return View();
        }

        [HttpPost]
        public IActionResult SearchWebPage(string txt)
        {
            DBCProjects dbPrj = new DBCProjects();
            List<ListWebPage> lstWp = null;
            if (txt != null)
            {
                string cad = txt.Trim();
                if (cad.Length > 0)
                    lstWp = dbPrj.seachWebPage(txt);
                else
                    lstWp = dbPrj.seachWebPage();
                ViewBag.lsWp = lstWp;
                
            }
            lstWp = dbPrj.seachWebPage();
            ViewBag.lsWp = lstWp;

            return View();
        }

        public IActionResult Georeference()
        {
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra las estadisticas correspondientes al proyecto o actividades que el usuario logeado tenga activa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Charts(long keym, long idCar, long idUsu, bool ori)
        {
            //dynamic dat = Request.Form;//Obtiene los datos que llegan desde la llamada AJAX
            try
            {
                #region Asignar valores de la caracteristica actual a las variables
                long usuAct = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                if (!ori)
                {
                    string[] cad = HttpContext.Session.GetString("actCar").Split('-');
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                }
                #endregion
            }
            catch { }
            //ViewBag.keym = keym;
            //ViewBag.idCar = idCar;
            //ViewBag.idUsu = idUsu;
            DBCActivities dbAct = new DBCActivities();
            try
            {
                Dictionary<string, string> x = dbAct.getChildPieChart(keym, idUsu, idCar);
                ViewBag.percentAsign = x["perAsig"];
                ViewBag.percentComplete = x["perComp"];
                ViewBag.complete = x["comp"];
            }
            catch { }

            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra el Reporte correspondiente al proyecto o actividades que el usuario logeado tenga activa
        /// </summary>
        /// <returns></returns>
        public IActionResult Reports(long keym, long idCar, long idUsu, bool ori)
        {

            #region variables de estado de la vista, sirve para mostrar mensajes en las vista
            bool st = false;
            #endregion

            try
            {
                #region Asignar valores de la caracteristica actual a las variables
                long usuAct = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                if (!ori)
                {
                    string[] cad = HttpContext.Session.GetString("actCar").Split('-');
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                }
                #endregion

                #region obtiene la lista de inventario de la tabla caracteristicas
                DBCReports rep = new DBCReports();
                string x = rep.getReport(usuAct, keym, idCar, idUsu);
                
                if (x.Length>0)
                    st = true;
                x = "http://docs.google.com/gview?url=" + x + "&embedded=true";
                ViewBag.doc = x;
                #endregion

                #region Asigna la lista a una variable ViewBag para ser usada desde la vista


                ViewBag.st = st;
                #endregion
            }
            catch
            {
            }
            return View();

        }

        /// <summary>
        /// Llama a la vista que muestra los recursos financieros correspondientes al proyecto o actividades que el usuario logeado tenga activa
        /// </summary>
        /// <returns></returns>
        public IActionResult Financial(long keym, long idCar, long idUsu, bool ori)
        {

            #region variables de estado de la vista, sirve para mostrar mensajes en las vista
            ViewBag.st = false;
            #endregion

            try
            {

                #region Asignar valores de la caracteristica actual a las variables
                if (!ori)
                {
                    string[] cad = HttpContext.Session.GetString("actCar").Split('-');
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                }
                #endregion


                #region obtiene la lista de inventario de la tabla caracteristicas
                DBCFinancial dbcFin = new DBCFinancial();

                List<presupuesto> lstRes = dbcFin.getResourcesList(keym, idCar, idUsu);
                List<costos> lstCos = dbcFin.getCostsList(keym, idCar, idUsu);
                #endregion

                #region Asigna la lista a una variable ViewBag para se usada desde la vista

                //Recursos financieros
                if (lstRes != null && lstRes.Count == 0)
                    ViewBag.stRes = false;
                else
                {
                    ViewBag.lstRes = lstRes;
                    ViewBag.stRes = true;
                }

                //Costos
                if (lstCos != null && lstCos.Count == 0)
                    ViewBag.stCos = false;
                else
                {
                    ViewBag.lstCos = lstCos;
                    ViewBag.stCos = true;
                }


                ViewBag.st = true;
                #endregion

            }
            catch (Exception err)
            {

            }
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra el inventario correspondiente al proyecto o actividades que el usuario logeado tenga activa
        /// </summary>
        /// <returns></returns>
        public IActionResult Inventory(long keym, long idCar, long idUsu, bool ori)
        {
            #region variables de estado de la vista, sirve para mostrar mensajes en las vista
            bool st = false;
            #endregion

            try
            {

                #region Asignar valores de la caracteristica actual a las variables
                if (!ori)
                {
                    string[] cad = HttpContext.Session.GetString("actCar").Split('-');
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                }
                #endregion

                #region obtiene la lista de inventario de la tabla caracteristicas
                DBCInventory inv = new DBCInventory();
                List<recursos> lstRes = inv.getInventoryList(keym, idCar, idUsu);
                #endregion

                #region Asigna la lista a una variable ViewBag para se usada desde la vista

                if (lstRes.Count == 0)
                    st = false;
                else
                    st = true;


                ViewBag.lstRes = lstRes;
                ViewBag.st = st;
                #endregion
            }
            catch
            {
                ViewBag.st = st;
            }
            return View();
        }

        /// <summary>
        /// Llama a la vista que muestra los objetivos correspondiente al proyecto o actividades
        /// </summary>
        /// <returns></returns>
        public IActionResult Goals(long keym, long idCar, long idUsu, bool ori)
        {

            #region variables de estado de la vista, sirve para mostrar mensajes en las vista
            bool st = false;
            #endregion

            try
            {
                #region Asignar valores de la caracteristica actual a las variables
                long usuAct = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                if (!ori)
                {
                    string[] cad = HttpContext.Session.GetString("actCar").Split('-');
                    keym = Convert.ToInt64(cad[0]);
                    idCar = Convert.ToInt64(cad[1]);
                    idUsu = Convert.ToInt64(cad[2]);
                }
                #endregion

                #region obtiene la lista de inventario de la tabla caracteristicas
                DBCGoals rep = new DBCGoals();
                string x = rep.getGoals(usuAct, keym, idCar, idUsu);

                if (x.Length > 0)
                    st = true;
                x = "http://docs.google.com/gview?url=" + x + "&embedded=true";
                ViewBag.doc = x;
                #endregion

                #region Asigna la lista a una variable ViewBag para ser usada desde la vista


                ViewBag.st = st;
                #endregion
            }
            catch
            {
            }
            return View();

        }


        //=====================================   METODOS/FUNCIONES AUXILIARES   ==========================================//
        /// <summary>
        /// Realiza la consulta a un archivo JSON que posee las opciones que tendra el usuario al momento de ingresar a la plataforma
        /// </summary>
        /// <returns></returns>
        private JObject webOptions()
        {
            /*
            rol
            1-  default
            2-  Admin
            */

            WebData wd = new WebData();
            return wd.defaultUser();
        }

        /// <summary>
        /// Obtiene los links (1 nivel) asociados a una caracteristicas segun publicacion web
        /// </summary>
        /// <param name="key"></param>
        /// <param name="idcar"></param>
        /// <param name="usu"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> getLinks(long key, long idcar, long usu)
        {
            HttpContext.Session.SetString("carAct", key + "-" + idcar + "-" + usu);
            try
            {
                //dynamic dat = Request.Form;
                //long keym = Convert.ToInt64(dat["key"]);
                //long idCar = Convert.ToInt64(dat["id_caracteristica"]);
                //long idUsu = Convert.ToInt64(dat["usu"]);

                DBCActivities act = new DBCActivities();
                List<string> lst = act.getLinksFromDBActivity(key, idcar, usu);
                return lst;
            }
            catch
            {
                return null;
            }

        }

    }
}
