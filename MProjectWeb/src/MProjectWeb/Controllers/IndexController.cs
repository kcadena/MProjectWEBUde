using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MProjectWeb.Models.Postgres;
using Microsoft.AspNet.Http;
using MProjectWeb.Models.ModelController;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MProjectWeb.Controllers
{
    public class IndexController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
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
            return RedirectToAction("Index","Index");
        }

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
            ViewBag.lst = lst;
            ViewBag.st = st;
            return View();
        }

        //Obtiene todos los links que existen en la base de datos
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