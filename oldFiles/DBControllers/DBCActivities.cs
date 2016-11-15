using MProjectWeb.Models.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MProjectWeb.ViewModels;
using System.Data.Common;

namespace MProjectWeb.Models.DBControllers
{
    public class DBCActivities
    {

        //MProjectDeskSQLITEContext db;
        //public DBCActivities()
        //{
        //    db = new MProjectDeskSQLITEContext();
        //}

        //public List<ActivityList> activityList(long id,int op)
        //{

        //    //op opcion  1=>actividades  2=>back
         
        //    try
        //    {
        //        if (op == 1)
        //        {
        //            var dat = from act in db.actividades
        //                      join car in db.caracteristicas on act.id_actividad equals car.id_actividad
        //                      where car.padre_caracteristica == id
        //                      orderby act.pos, act.folder
        //                      select new ActivityList
        //                      {
        //                          id_act = act.id_actividad,
        //                          name = act.nombre,
        //                          description = act.descripcion,
        //                          state = car.estado,
        //                          id_characteristic = car.id_caracteristica,
        //                          type = 1,
        //                          folder = (long)act.folder,
        //                          par_characteristic = (long)car.padre_caracteristica
        //                      };

                   
        //            if (dat.Count() == 0)
        //            {
        //                caracteristicas cr = (from car in db.caracteristicas
        //                                      where car.padre_caracteristica == id
        //                                      select car).First();
        //                var res = from act in db.actividades
        //                          join car in db.caracteristicas on act.id_actividad equals car.id_actividad
        //                          where car.padre_caracteristica == cr.id_caracteristica && act.compartido == 1
        //                          orderby act.pos, act.folder
        //                          select new ActivityList
        //                          {
        //                              id_act = act.id_actividad,
        //                              name = act.nombre,
        //                              description = act.descripcion,
        //                              state = car.estado,
        //                              id_characteristic = car.id_caracteristica,
        //                              type = 2,
        //                              folder = (long)act.folder,
        //                              par_characteristic = (long)car.padre_caracteristica
        //                          };
        //                List<ActivityList> ds= res.ToList<ActivityList>();
        //                return ds;
        //            }
        //            else {
        //                List<ActivityList> d = dat.ToList<ActivityList>();
        //                return d;
        //            }
        //        }
        //        else if (op == 2)
        //        {
        //            caracteristicas cr = (from car in db.caracteristicas
        //                                  where car.id_caracteristica == id
        //                                  select car).First();
                   
        //            var dat = from act in db.actividades
        //                      join car in db.caracteristicas on act.id_actividad equals car.id_actividad
        //                      where car.padre_caracteristica == cr.padre_caracteristica
        //                      orderby act.pos, act.folder
        //                      select new ActivityList
        //                      {
        //                          id_act = act.id_actividad,
        //                          name = act.nombre,
        //                          description = act.descripcion,
        //                          state = car.estado,
        //                          id_characteristic = car.id_caracteristica,
        //                          type = 1,
        //                          folder = (long)act.folder,
        //                          par_characteristic = (long)car.padre_caracteristica
        //                      };

        //            if (dat.Count() == 0)
        //            {
        //                cr = (from car in db.caracteristicas
        //                      where car.id_caracteristica == cr.padre_caracteristica
        //                      select car).First();
        //                dat = from act in db.actividades
        //                          join car in db.caracteristicas on act.id_actividad equals car.id_actividad
        //                          where car.padre_caracteristica == cr.padre_caracteristica
        //                          orderby act.pos, act.folder
        //                          select new ActivityList
        //                          {
        //                              id_act = act.id_actividad,
        //                              name = act.nombre,
        //                              description = act.descripcion,
        //                              state = car.estado,
        //                              id_characteristic = car.id_caracteristica,
        //                              type = 1,
        //                              folder = (long)act.folder,
        //                              par_characteristic = (long)car.padre_caracteristica
        //                          };

        //            }

        //            List<ActivityList> d = dat.ToList<ActivityList>();
        //            return d;
        //        }

        //    }
        //    catch { }
        //    return null;
        //}
    }
}











/* select 	
  actividades.id_actividad,
  actividades.nombre,
  actividades.descripcion,
  folders.id_folder,
  folders.nombre,
  caracteristicas.estado,
  caracteristicas.id_caracteristica
from 	
  actividades 	join caracteristicas on caracteristicas.id_actividad=actividades.id_actividad 
  left join folders on actividades.id_folder = folders.id_folder
  where caracteristicas.proyecto_padre = 1
  ;


              var dat = from act in db.actividades
                        join car in db.caracteristicas on act.id_actividad equals car.id_actividad
                        join fol in db.folders on act.id_folder equals fol.id_folder into pp

                        from fol in pp.DefaultIfEmpty()

                        select 
                        new ActivityList
                        {
                            id_act = (long)act.id_actividad,
                            //id_fol =  (act.id_folder),
                            id_characteristic = (long)car.id_caracteristica,
                            description = act.descripcion,
                            name = act.nombre,
                            folder = fol == null ? "NULL": "OK",
                            state = car.estado
                        };


  */
