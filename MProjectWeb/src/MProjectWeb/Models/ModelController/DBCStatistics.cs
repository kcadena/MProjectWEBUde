using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCStatistics
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCStatistics()
        {
            db = new MProjectContext();
        }
     
    }
}
