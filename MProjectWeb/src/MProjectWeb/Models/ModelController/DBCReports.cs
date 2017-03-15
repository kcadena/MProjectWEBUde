using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCReports
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCReports()
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
        public string getReport(long usuAct,long keym, long idCar, long usu)
        {
            try
            {
                var usuAsig = db.caracteristicas.Where(x=>x.keym==keym && x.id_caracteristica==idCar && x.id_usuario == usu).First().usuario_asignado;
                string cad = db.configuracion_inicial.Where(x=>x.id==1).First().val_configuracion + "mp/user" + usuAsig + "/reportes/" + keym + "-" + idCar + "-" + usu + ".docx";
                return cad;
            }
            catch { return ""; }
            
        }
        

    }
}
