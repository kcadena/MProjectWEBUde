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
                
                path = db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + "user" + usuAct + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos" + keym + "-" + idCar + "-" + usu + ".docx"; ;

                if (System.IO.File.Exists(path))
                {
                    cad = db.configuracion_inicial.Where(x => x.id == 1).First().val_configuracion + "mp/user" + usuAct + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos" + keym + "-" + idCar + "-" + usu + ".docx";
                    return cad;
                }
                    
                else
                {
                    try
                    {
                        var dat = db.caracteristicas.Where(x => x.keym == keym && x.id_caracteristica == idCar && x.id_usuario == usu && x.publicacion_reporte == true).First();
                        long? idUAs = dat.usuario_asignado;
                        path = db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + "user" + usu + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos"+ keym + "-" + idCar + "-" + usu + ".docx"; ;

                        if (System.IO.File.Exists(path))
                        {
                            cad = db.configuracion_inicial.Where(x => x.id == 1).First().val_configuracion + "mp/user" + usu + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos" + keym + "-" + idCar + "-" + usu + ".docx";
                            return cad;
                        }
                            
                        else
                        {
                            path = db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion + "user" + idUAs + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos" + keym + "-" + idCar + "-" + usu + ".docx"; ;
                            if (System.IO.File.Exists(path))
                            {
                                cad = db.configuracion_inicial.Where(x => x.id == 1).First().val_configuracion + "mp/user" + idUAs + "/proyectos/" + keym + "-" + idCar + "-" + usu + "/documentos/objetivos" + keym + "-" + idCar + "-" + usu + ".docx";
                                return cad;
                            }
                                
                            else
                                return "";
                        }
                            
                    }
                    catch (Exception e){ return ""; }
                }
                               
            }
            catch { return ""; }

        }


    }
}
