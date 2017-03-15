using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MProjectWeb.Models.Postgres;
using Microsoft.AspNet.Http;
using MProjectWeb.Models.ModelController;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MProjectWeb.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("stFile", "Y");
            HttpContext.Session.SetString("carAct","");
            //MProjectDeskSQLITEContext dbMP = new MProjectDeskSQLITEContext();
            ViewBag.errLogin = false;
            try
            {
                bool st = (bool)TempData["err"];
                ViewBag.errLogin = st;
            }
            catch { }
            try
            {
                bool st = (bool)TempData["errReg"];
                ViewBag.errRegister = st;
            }
            catch { }


            ViewData["Title"] = "Mproject";

            //if (project==1)
            //    ViewBag.Pagina = "http://localhost:60395/prueba%20web/principal1.html";
            //else if (project == 2)
            //    ViewBag.Pagina = "http://localhost:60395/prueba%20web/principal2.html";
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("carAct", "");
            return RedirectToAction("Index", "Index");
        }
        /// <summary>
        /// Consigue todos los links de todos los proyectos y actividades
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        public IActionResult Links(string cad)
        {
            bool st = false;
            try
            {
                ViewBag.ids = cad;
                if (cad != null)
                {
                    st = true;
                }
            }
            catch { }
            List<string> lst = getAllLinks();


            #region Creacion de la estructura JSON para Generar TreeView con javascript desde la vista
            string json = getCadLinks(lst);
            try
            {
                ViewBag.json = JsonConvert.DeserializeObject(json);
            }
            catch { }
            //ViewBag.json = json;
            #endregion


            ViewBag.lst = lst;
            ViewBag.st = st;
            DBCConfiguracion conf = new DBCConfiguracion();
            try
            {
                ViewBag.ipPlatServer = conf.getIpPlatServer();
            }
            catch
            {

            }
            return View();
        }

        public IActionResult Help()
        {
            HttpContext.Session.SetString("stFile", "Y");
            HttpContext.Session.SetString("carAct", "");
            return View();
        }

        /// <summary>
        /// Genera y devuelve la cadena de todos los link en formato JSON para crear el arbol de links
        /// </summary>
        /// <returns></returns>
        private string getCadLinks(List<string> lst)
        {
            DBCConfiguracion conf = new DBCConfiguracion();
            string json = "";
            string src = "#";
            try
            {
                src = conf.getIpPlatServer() + "Projects/publicprojects?p=" + lst.First().Split('|')[1].Replace(',', '-');
            }
            catch { }
            //string json = "[{text: \"" + lst.First().Split('-')[3]+"\" , href:\""+@src+ "\", backColor: \"#ff6a00\" }]";

            if (lst.Count == 1)
            {
                json = "[{text: \"" + lst.First().Split('|')[3] + "\" , selectable: false, highlightSelected:false, multiSelect:false  ";

                #region Asigna la direccion y color del link
                if (lst.First().Split('|')[0].Equals("Y"))
                {
                    string car = lst.First().Split('|')[1].Replace(',', '-');
                    string xcar = HttpContext.Session.GetString("carAct");
                    try
                    {
                        if (xcar.Equals(car) && xcar != null)
                        {
                            json = json + ", backColor: \"#ff6a00\" , color: \"#ffffff\"  ";
                        }
                        else
                        {
                            json = json + ", color: \"#1ba3fe\"";
                        }
                    }
                    catch
                    {
                    }
                    json = json + ", href:\"" + src + "\"  ";
                }
                else
                {
                    json = json + ",  state:{ selected: false  } ";
                }
                #endregion

               

                json = json + "}]";

            }
            if (lst.Count > 1)
            {
                json = "[{text: \"" + lst.First().Split('|')[3] + "\" ,  selectable: false, highlightSelected:false, multiSelect:false ";
                #region Asigna la direccion y color del link
                if (lst.First().Split('|')[0].Equals("Y"))
                {
                    string ycar = lst.First().Split('|')[1].Replace(',', '-');
                    string wcar = HttpContext.Session.GetString("carAct");
                    try
                    {
                        if (wcar.Equals(ycar) && wcar != null)
                        {
                            json = json + ", color: \"#ffffff\"  ";
                        }
                        else
                        {
                            json = json + ", color: \"#1ba3fe\"";
                        }
                    }
                    catch
                    {
                    }
                    json = json + ", href:\"" + src + "\" ";
                }
                else
                {
                    json = json + ",  state:{ selected: false  } ";
                }
                #endregion

                string car = lst.First().Split('|')[1].Replace(',', '-');
                string xcar = HttpContext.Session.GetString("carAct");

                try
                {
                    if (xcar.Equals(car))
                    {
                        json = json + ", backColor: \"#ff6a00\" ";
                    }
                }
                catch { }



                if (Convert.ToInt32(lst[0].Split('|')[5]) < Convert.ToInt32(lst[1].Split('|')[5]))
                    json = json + ",nodes:[";
                else
                    json = json + "}";


                try
                {
                    for (int i = 1; i < lst.Count; i++)
                    {
                        if (i == 41)
                        {
                            int f = 0;
                        }
                        #region Variables de la caracteristica
                        string[] xcad = lst[i].Split('|');
                        string idCarStr = xcad[1].Replace(',','-');
                        int n = Convert.ToInt32(xcad[5]), nx = -1;
                        src = conf.getIpPlatServer() + "Projects/publicprojects?p=" + xcad[1].Replace(',', '-');
                        string text = lst[i].Split('|')[3];
                        string srcSta = lst[i].Split('|')[0];
                        #endregion

                     

                        try
                        {
                            nx = Convert.ToInt32(lst[i + 1].Split('|')[5]);
                        }
                        catch
                        {
                            char c = json.Last();
                            if (c != '[')
                                json = json + ",";

                            json = json + getInfNode(text, src, srcSta, idCarStr) + "}";
                            for (int x = 0; x < n; x++)
                            {
                                json = json + "]}";
                            }
                            json = json += "]";
                        }
                        if (nx >= 0)
                        {
                            int df = n - nx;
                            char c = json.Last();

                            if (c != '[')
                                json = json + ",";

                            json = json + getInfNode(text, src, srcSta, idCarStr);

                            if (n < nx)
                                json = json + ", nodes:[";
                            else if (n == nx)
                                json = json + "}";
                            else if (n > nx)
                            {
                                json = json + "}";
                                if (df > 0)
                                {
                                    for (int x = 0; x < df; x++)
                                    {
                                        json = json + "]}";
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return json;
        }

        /// <summary>
        /// Genera parte del JSON correspondiente a la informacion del nodo
        /// </summary>
        /// <param name="text"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        private string getInfNode(string text,string src,string srcSta, string car)
        {
            string cad = "";
            cad = "{text: \"" + text + "\" , selectable: false, highlightSelected:false, multiSelect:false ";
            string act = HttpContext.Session.GetString("infAct");

            

            #region Asigna la direccion y color del link
            if (srcSta.Equals("Y"))
            {
                #region Asigna color para indicar la caracteristica actual
                try
                {
                    string xcad = HttpContext.Session.GetString("carAct");
                    if (xcad.Equals(car))
                    {
                        cad = cad + ", backColor: \"#ff6a00\" , color: \"#ffffff\"";
                    }
                    else
                    {
                        cad = cad + ", color: \"#1ba3fe\"";
                    }
                }
                catch
                {
                    cad = cad + ", color: \"#1ba3fe\"";
                }
                #endregion
                cad = cad + ", href:\"" + src + "\" ";
            }
            else
            {
                cad = cad + ",  state:{ selected: false  } ";
            }
            #endregion

            return cad;
        }

        /// <summary>
        /// Obtiene todos los links que existen en la base de datos
        /// </summary>
        /// <returns></returns>
        public List<string> getAllLinks()
        {
            try
            {
                DBCActivities act = new DBCActivities();
                List<string> lst = act.getAllLinks();
                return lst;
            }
            catch
            {
                return null;
            }

        }

    }
}