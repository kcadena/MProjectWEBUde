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

        }

    }


}
