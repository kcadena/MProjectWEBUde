using System;
using System.Collections.Generic;
using System.Linq;
using MProjectWeb.Models.Postgres;

namespace MProjectWeb.Models.Lucene
{
    class ArchivosMultimedia
    {
        private MProjectContext db;
        private List<archivos> lstArc;
        private string cadCar;
        public ArchivosMultimedia()
        {
            this.db = new MProjectContext();
        }


        string cadUsr = "";
        public string getUsersCaracteristicas(long keym, long idUsu, long idCar)
        {
            try
            {

                caracteristicas car = db.caracteristicas.Where(x =>
                    x.keym == keym &&
                    x.id_caracteristica == idCar &&
                    x.id_usuario == idUsu
                    ).First();
                try
                {
                    while (car != null)
                    {
                        if (car.usuario_asignado != null)
                            cadUsr = cadUsr + " OR ( usuOwn:" + car.usuario_asignado + " ) ";

                        car = db.caracteristicas.Where(x =>
                        x.keym == car.keym_padre &&
                        x.id_caracteristica == car.id_caracteristica_padre &&
                        x.id_usuario == car.id_usuario_padre
                        ).First();
                    }
                }
                catch
                {
                    if (car.tipo_caracteristica.Equals("p"))
                        cadUsr = cadUsr + " OR ( usuOwn:" + car.id_usuario + " ) ";
                }

                if (cadUsr.Length > 0)
                {
                    cadUsr = cadUsr.Remove(0, 3);
                    return cadUsr;
                }
                else
                    return "";

            }
            catch (Exception err) { return ""; }
        }
        private bool st;
        public string getCaracteriscaChildren(long keym, long usu, long idCar)
        {
            st = false;
            //caracteristicas car = db.caracteristicas.Where(x =>
            //         x.keym ==keym &&
            //         x.id_usuario == usu &&
            //         x.id_caracteristica == idCar
            //    ).First();
            //cadCar = "( idCar:" + car.id_caracteristica;
            getCaracteriscas(keym, usu, idCar);
            //getCaracteriscas(car.keym,car.id_usuario,car.id_caracteristica);
            cadCar = cadCar.Remove(0, 3);
            if (cadCar.Length > 0)
                return cadCar;
            else
                return "";
        }
        private void getCaracteriscas(long keym, long usu, long idCar)
        {
            try
            {
                List<caracteristicas> lstcar = db.caracteristicas.Where(x =>
                    x.keym_padre == keym &&
                    x.id_usuario_padre == usu &&
                    x.id_caracteristica_padre == idCar 
                    //&&  x.visualizar_superior == true
                ).ToList();
                try
                {
                    foreach (var x in lstcar)
                    {
                        getCaracteriscas(x.keym, x.id_usuario, x.id_caracteristica);
                    }
                    st = true;
                }
                catch { }
                cadCar = cadCar + " OR ( idCar:" + idCar + " AND usuCar:" + usu + " ) ";
            }
            catch (Exception err) { return; }
        }
    }
}