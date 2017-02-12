using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCFinancial
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCFinancial()
        {
            db = new MProjectContext();
        }
        


    }
}
