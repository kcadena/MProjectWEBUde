using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCInventory
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCInventory()
        {
            db = new MProjectContext();
        }
     
    }
}
