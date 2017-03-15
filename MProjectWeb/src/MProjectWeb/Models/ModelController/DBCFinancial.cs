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

        /// <summary>
        /// Trae la lista de los recursos financieros con que cuenta una caracteristica ya sea de un proyecto o actividad
        /// </summary>
        /// <param name="keym"></param>
        /// <param name="idCar"></param>
        /// <param name="usu"></param>
        /// <returns></returns>
        public List<presupuesto> getResourcesList(long keym, long idCar, long usu)
        {
            try
            {
                var res = db.presupuesto.Where(x => x.keym_car == keym && x.id_caracteristica == idCar && x.id_usuario_car == usu).ToList();
                return res;
            }
            catch { return null; }
            
        }

        /// <summary>
        /// Trae la lista de los recursos financieros con que cuenta una caracteristica ya sea de un proyecto o actividad
        /// </summary>
        /// <param name="keym"></param>
        /// <param name="idCar"></param>
        /// <param name="usu"></param>
        /// <returns></returns>
        public List<costos> getCostsList(long keym, long idCar, long usu)
        {
            try
            {
                var cos = db.costos.Where(x => x.keym_car == keym && x.id_caracteristica == idCar && x.id_usuario_car == usu).ToList();
                return cos;
            }
            catch { return null; }
        }

    }
}
