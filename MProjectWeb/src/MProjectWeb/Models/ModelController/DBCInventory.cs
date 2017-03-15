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

        /// <summary>
        /// Trae la lista del inventario de una caracteristica
        /// </summary>
        public List<recursos> getInventoryList(long keym,long idCar, long usu)
        {
            var inv = db.recursos.Where(x => x.keym_car == keym && x.id_caracteristica == idCar && x.id_usuario_car == usu).ToList();
            return inv;
        }
     
    }
}
