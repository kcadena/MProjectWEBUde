using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCGoals
    {
        MProjectContext db;
        public List<string> llink = null;
        public DBCGoals()
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
        public string getGoals(long usuAct, long keym, long idCar, long usu)
        {
            try
            {
                string cad = "";
                string path = "";

                #region realiza la busqueda del proyecto al cual corresponde la caracteristica
                proyectos ax=null;

                ax = db.proyectos.Where(x=>x.keym_car==keym && x.id_caracteristica==idCar && x.id_usuario_car==usu).Single();

                if(ax==null)
                    ax = db.proyectos.Where(x => x.keym == keym && x.id_caracteristica == idCar && x.id_usuario_car == usu).Single();

                #endregion
                #region Asignacion de variables identificadoras para realizar la busqueda de los archivos

                long keym_pry = ax.keym;
                long id_pry = ax.id_proyecto;
                long usu_pry = ax.id_usuario;

                #endregion

                path = db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + "user" + usu_pry + "/proyectos/" + keym_pry + "-" + id_pry + "-" + usu_pry + "/documentos/objetivos" + keym_pry + "-" + id_pry + "-" + usu_pry + ".docx"; ;

                if (System.IO.File.Exists(path))
                {
                    cad = db.configuracion_inicial.Where(x => x.id == 1).First().val_configuracion + "mp/user" + usu_pry + "/proyectos/" + keym_pry + "-" + id_pry + "-" + usu_pry + "/documentos/objetivos" + keym_pry + "-" + id_pry + "-" + usu_pry + ".docx";
                    return cad;
                }
                return "";

            }
            catch(Exception e) { return ""; }

        }


    }
}
