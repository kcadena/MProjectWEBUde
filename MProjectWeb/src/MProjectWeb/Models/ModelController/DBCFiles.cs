using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCFiles
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCFiles()
        {
            db = new MProjectContext();
        }
        /// <summary>
        /// actualizar informacion georreferenciable
        /// </summary>
        /// <param name="dat"></param>
        public void updatePointGEO(Dictionary<string, string> dat)
        {
       
            try
            {
                long keym_arc, id_archivo, id_usuario_arc;
                keym_arc = Convert.ToInt64(dat["keym_arc"]);
                id_archivo = Convert.ToInt64(dat["id_archivo"]);
                id_usuario_arc = Convert.ToInt64(dat["id_usuario_arc"]);
                var fil = db.archivos.Where(x => x.keym_arc == keym_arc && x.id_archivo == id_archivo && x.id_usuario_arc == id_usuario_arc).Single();
                fil.localizacion = Convert.ToDecimal(dat["localizacion"].ToString().Replace(".",","));
                fil.longitud = Convert.ToDecimal(dat["longitud"].ToString().Replace(".", ","));
                db.SaveChanges();
            }
            catch { }
        }
    }
}
