using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using MProjectWeb.ViewModels;
using Newtonsoft.Json;


using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using MProjectWeb.LuceneIR;
using MProjectWeb.Controllers;

namespace MProjectWeb.Controllers
{
    public class MultiMediaController : Controller
    {
        public IActionResult Pictures()
        {
            return View();
        }
        public IActionResult Videos()
        {
            return View();
        }
        public IActionResult Maps()
        {
            LuceneAct lc = new LuceneAct();
            try
            {
                List<Lucene.Net.Search.ScoreDoc> lstDoc = lc.publicSearchPosition();
                ViewBag.lstDoc = lstDoc;
                ViewBag.searcher = lc.searcher;
                ViewBag.stFind = true;
            }
            catch
            {
                ViewBag.stFind = false;
            }

            return View();
        }
        public IActionResult load()
        {
            return View();
        }

        public IActionResult regMap()
        {
            return View();
        }

        public bool UpdateRegPosition()
        {
            dynamic dat = Request.Form;
            Dictionary<string, string> inf = new Dictionary<string, string>();
            inf["keym"] = dat["keym"];
            inf["idCar"] = dat["idCar"];
            inf["usuCar"] = dat["usuCar"];
            inf["idFile"] = dat["idFile"];
            LuceneAct lc = new LuceneAct();
            Lucene.Net.Search.ScoreDoc dc=null;
            try
            {
                dc = lc.searchFile(inf);
            }
            catch { }
            if (dc != null)
            {
                Lucene.Net.Documents.Document doc = lc.searcher.Doc(dc.Doc);

                try { inf["usuAct"] = doc.Get("usuAct"); } catch {}
                try { inf["usuCar"] = doc.Get("usuCar"); } catch {}
                try { inf["usuOwn"] = doc.Get("usuOwn"); } catch {}

                try { inf["keym"] = doc.Get("keym"); } catch {}
                try { inf["idFile"] = doc.Get("idFile"); } catch {}
                try { inf["idCar"] = doc.Get("idCar"); } catch {}

                try { inf["nom"] = doc.Get("nom"); } catch {}
                try { inf["nom2"] = doc.Get("nom2"); } catch {}
                try { inf["desc"] = doc.Get("desc"); } catch {}
                try { inf["cont"] = doc.Get("cont"); } catch {}

                try { inf["src"] = doc.Get("src"); } catch {}
                try { inf["srcServ"] = doc.Get("srcServ"); } catch {}
                try { inf["srcGif"] = doc.Get("srcGif"); } catch {}
                try { inf["srcGifServ"] = doc.Get("srcGifServ"); } catch {}
                try { inf["type"] = doc.Get("type"); } catch {}
                try { inf["vis"] = doc.Get("vis"); } catch {}

                try {
                    inf["location"] = dat["loc"];
                    inf["longitude"] = dat["lng"];

                    bool st = lc.luceneUpdate(inf);
                    if (st)
                    {
                        //Actualizar los datos en Postgres
                        
                    }
                    return true;
                } catch { }


            }
            return false;
        }
      
    }
}
