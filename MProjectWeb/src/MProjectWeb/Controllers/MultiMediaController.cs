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
            inf["keym_arc"] = dat["keym_arc"];
            inf["id_archivo"] = dat["id_archivo"];
            inf["id_usuario_arc"] = dat["id_usuario_arc"];
            
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

                try { inf["keym_car"] = doc.Get("keym_car"); } catch { }
                try { inf["id_caracteristica"] = doc.Get("id_caracteristica"); } catch { }
                try { inf["id_usuario_car"] = doc.Get("id_usuario_car"); } catch {}

                try { inf["keym_arc"] = doc.Get("keym_arc"); } catch { }
                try { inf["id_archivo"] = doc.Get("id_archivo"); } catch { }
                try { inf["id_usuario_arc"] = doc.Get("id_usuario_arc"); } catch { }

                try { inf["tipo"] = doc.Get("tipo"); } catch { }
                try { inf["publicacion"] = doc.Get("publicacion"); } catch { }

                try { inf["titulo"] = doc.Get("titulo"); } catch {}
                try { inf["subtitulo"] = doc.Get("subtitulo"); } catch {}
                try { inf["descripcion"] = doc.Get("descripcion"); } catch {}
                try { inf["contenido"] = doc.Get("contenido"); } catch {}

                try { inf["src"] = doc.Get("src"); } catch {}
                try { inf["srcServ"] = doc.Get("srcServ"); } catch {}
                try { inf["srcGif"] = doc.Get("srcGif"); } catch {}
                try { inf["srcGifServ"] = doc.Get("srcGifServ"); } catch {}
                
                try {
                    inf["localizacion"] = dat["loc"];
                    inf["longitud"] = dat["lng"];

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
