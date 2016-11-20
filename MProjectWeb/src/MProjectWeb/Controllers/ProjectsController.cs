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
        public IActionResult Projects()
        {
            HttpContext.Session.Remove("id_prj");
            DBCProjects h = new DBCProjects();
            long user = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
            ViewBag.projects = h.listProjectsUsers(user);
            HttpContext.Session.SetString("infAct", "");
            return View();
        }

        public IActionResult PublicProjects(string p)
        {
            MProjectContext dbMP = new MProjectContext();
            try
            {
                string[] pro = p.Split('-');
                //caracteristicas car = (from x in dbMP.caracteristicas
                //                       where x.keym == Convert.ToInt64(pro[0]) &&
                //                       x.id_usuario == Convert.ToInt64(pro[2]) &&
                //                       x.id_caracteristica == Convert.ToInt64(pro[1])
                //                       select x).First();

                try
                {
                    var car = (from x in dbMP.caracteristicas
                               join y in dbMP.proyectos on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym, B = y.id_caracteristica, C = y.id_usuario }

                               where x.keym == Convert.ToInt64(pro[0]) &&
                                       x.id_caracteristica == Convert.ToInt64(pro[1]) &&
                                       x.id_usuario == Convert.ToInt64(pro[2])
                               select new
                               {
                                   x.keym,
                                   x.id_caracteristica,
                                   x.id_usuario,
                                   y.nombre,
                                   y.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                               }).First();


                    ViewBag.key = car.keym;
                    ViewBag.idCar = car.id_caracteristica;
                    ViewBag.idUsu = car.id_usuario;
                    ViewBag.Pagina = car.ruta_repositorio + "Web"+ car.keym +"-"+car.id_caracteristica+"-"+car.id_usuario + ".html";//ruta 
                    //ViewBag.Pagina = car.ruta_repositorio + car.nombre.ToLower().Replace(" ", "_") + ".html";//ruta 
                    return View();
                }
                catch
                {
                    var car = (from x in dbMP.caracteristicas
                                  join y in dbMP.actividades on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym, B = y.id_caracteristica, C = y.id_usuario }

                                  where x.keym == Convert.ToInt64(pro[0]) &&
                                       x.id_caracteristica == Convert.ToInt64(pro[1]) &&
                                       x.id_usuario == Convert.ToInt64(pro[2])
                                  select new
                                  {
                                      x.keym,
                                      x.id_caracteristica,
                                      x.id_usuario,
                                      y.nombre,
                                      y.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                                  }).First();

                    ViewBag.key = car.keym;
                    ViewBag.idCar = car.id_caracteristica;
                    ViewBag.idUsu = car.id_usuario;
                    ViewBag.Pagina = car.ruta_repositorio + "Web" + car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario + ".html";//ruta 
                    //ViewBag.Pagina = car.ruta_repositorio + car.nombre.Replace(" ", "_") + ".html";//ruta 
                    return View();

                }
                //ViewBag.Pagina = "http://172.16.10.248/prueba%20web/principal1.html";
            }
            catch
            {

            }
            DBCProjects h = new DBCProjects();
            ViewBag.projects = h.listPublicProjectsUsers();
            return View();
        }


        //==========================================       VIEWS HELP       ===============================================//
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

        [HttpPost]
        public IActionResult ActMoreInfo()
        {
            dynamic dat = Request.Form;
            long id = Convert.ToInt64(dat["id_car"]);
            ViewBag.id_car = id;
            
            return View();
        }

        //==========================================   VISTAS SUBOPCIONES   ===============================================//
        public IActionResult Activity()
        {
            try
            {
                ViewBag.usuAct = Convert.ToInt64(HttpContext.Session.GetString("idUsu"));
                string ax="";
                dynamic dat;
                try
                {
                    dat = Request.Form;
                    ax = dat["id_prj"];
                    HttpContext.Session.SetString("id_prj",ax);
                }
                catch
                {
                    ax = HttpContext.Session.GetString("id_prj");
                }
                string[] prj = ax.Split('-'); //[0]=>keym   [1]=>idCarProject     [2]=>idUsuCar
                long idUsu = Convert.ToInt64(prj[2]);
                long keym = Convert.ToInt64(prj[0]);
                ViewBag.id_prj = ax;

                string[] cad = null;
                long idCar=0;
                int op=0;
                long usu=0;

                try
                {
                    cad = HttpContext.Session.GetString("infAct").Split('-');
                }
                catch { }

                try
                {
                    if (cad != null && cad.Count() >= 2 )
                    {
                        idCar =Convert.ToInt64(cad[1]);
                        HttpContext.Session.SetString("par_car", idCar.ToString());
                        op = 1;
                        usu = Convert.ToInt64(cad[2]);
                        keym = Convert.ToInt64(cad[0]);
                    }

                    try
                    {
                        //dynamic dat = new JsonArrayAttribute();
                        dat = Request.Form;
                        idCar = Convert.ToInt64(dat["id_car"]);
                        HttpContext.Session.SetString("par_car", idCar.ToString());
                        op = Convert.ToInt32(dat["opt"]);
                        usu = Convert.ToInt32(dat["usu"]);
                    }
                    catch
                    {
                        if (cad == null || cad.Count() <= 2)
                        {
                            idCar = Convert.ToInt64(prj[1]);// prj   -->   [0]=>keym   [1]=>idCarProject
                            DBCActivities actx = new DBCActivities();
                            List<ActivityList> act_lstx = actx.getActivityList(idCar, idUsu, keym, 1);
                            HttpContext.Session.SetString("infAct", act_lstx.First().keyM + "-" + act_lstx.First().parCar + "-" + act_lstx.First().parUsu);
                            ViewBag.act_lst = act_lstx;
                            return View();
                        }
                    }
                    

                    

                    DBCActivities act = new DBCActivities();
                    List<ActivityList> act_lst = act.getActivityList(idCar, usu, keym, op);
                    ViewBag.act_lst = act_lst;
                    try
                    {
                        ViewBag.idCar = act_lst.First().parCar;
                        ViewBag.usuCar = act_lst.First().parUsu;
                        HttpContext.Session.SetString("infAct", act_lst.First().keyM+"-"+ act_lst.First().parCar+"-"+ act_lst.First().parUsu);
                    }
                    catch
                    {
                        return Content("0");
                    }
                    ViewBag.prj = Convert.ToInt64(prj[1]);
                }
                catch
                {
                   
                }
            }
            catch
            {
                ViewBag.id_prj = null;
            }
            return View();
        }

        [HttpGet]
        public IActionResult PublicFiles(string type,string text)
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
                }
            }
            catch { }
            
            return View();
        }

        [HttpPost]
        public IActionResult Files()
        {
            try
            {
                long keym;
                long idCar;
                long idUsu;
                string type;
                string text="";
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
                        cad = HttpContext.Session.GetString("infAct").Split('-');
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
                
                string car="",usr="";
                try
                {
                    car = arc.getCaracteriscaChildren(keym, idUsu, idCar);
                    usr = arc.getUsersCaracteristicas(keym, idUsu, idCar);
                }
                catch (Exception err){ }
                

                LuceneAct lc = new LuceneAct();

                
                ViewBag.pubFil = pubFil;
                List<Lucene.Net.Search.ScoreDoc> lstDoc;
                if (!pubFil)
                {
                    Dictionary<string, string> dt = new Dictionary<string, string>();
                    dt["usuAct"] = HttpContext.Session.GetString("idUsu");

                    lstDoc = lc.search(dt, text, type, car,usr);

                }
                else
                {
                    lstDoc = lc.publicSearch( text, type, car);
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
                    }
                }
                catch { }
                ViewBag.lstDoc = lstDoc;
                ViewBag.searcher = lc.searcher;
                if(lc.totSear>0)
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
        public IActionResult Georeference()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Charts()
        {
            long keym;
            long idCar;
            long idUsu;

            try
            {
                dynamic dat = Request.Form;//Obtiene los datos que llegan desde la llamada AJAX
                keym = Convert.ToInt64(dat["keym"]);
                idCar = Convert.ToInt64(dat["idCar"]);
                idUsu = Convert.ToInt64(dat["idUsu"]);

                ViewBag.keym = keym;
                ViewBag.idCar = idCar;
                ViewBag.idUsu = idUsu;
            }
            catch
            {
                string[] cad = null;
                try
                {
                    cad = HttpContext.Session.GetString("infAct").Split('-');
                }
                catch { }
                keym = Convert.ToInt64(cad[0]);
                idCar = Convert.ToInt64(cad[1]);
                idUsu = Convert.ToInt64(cad[2]);

                ViewBag.keym = keym;
                ViewBag.idCar = idCar;
                ViewBag.idUsu = idUsu;
            }

            DBCActivities dbAct = new DBCActivities();
            try
            {
                Dictionary<string,string> x = dbAct.getChildPieChart(keym,idUsu,idCar);
                ViewBag.percentAsign = x["perAsig"];
                ViewBag.percentComplete = x["perComp"];
                ViewBag.complete = x["comp"];
            }
            catch { }

            return View();
        }
       


        //=====================================   METODOS/FUNCIONES AUXILIARES   ==========================================//
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
        [HttpGet]
        //Obtiene los links (1 nivel) asociados a una caracteristicas segun publicacion web
        public List<string> getLinks(long key,long idcar,long usu)
        {
            try
            {
                //dynamic dat = Request.Form;
                //long keym = Convert.ToInt64(dat["key"]);
                //long idCar = Convert.ToInt64(dat["id_caracteristica"]);
                //long idUsu = Convert.ToInt64(dat["usu"]);

                DBCActivities act = new DBCActivities();
                List<string> lst = act.getLinks(key,idcar,usu);
                return lst;
            }catch
            {
                return null;
            }
            
        }
       
    }
}
