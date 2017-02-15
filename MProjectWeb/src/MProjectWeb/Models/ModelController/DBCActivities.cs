using MProjectWeb.Models.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MProjectWeb.ViewModels;
using System.Data.Common;

namespace MProjectWeb.Models.ModelController
{
    public class DBCActivities
    {

        MProjectContext db;
        public List<string> llink = null;
        public DBCActivities()
        {

            db = new MProjectContext();
        }

        /*##################################################################################################################*/

        #region Metodos para el manejo de las actividades

        /// <summary>
        /// Obtiene el ID del usuario asignado a una actividad (caracteristica) espesifica
        /// </summary>
        /// <param name="keym"></param>
        /// <param name="usu"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        public long getIdOwnActivity(long keym, long usu, long car)
        {
            caracteristicas ax = db.caracteristicas.Where(x =>
                x.keym == keym &&
                x.id_caracteristica == car &&
                x.id_usuario == usu
            //p//x.usuario_asignado == usu
            ).First();
            long idUsu = ax.id_usuario;
            //p//long idUsu =(long) ax.usuario_asignado;
            try
            {
                while (ax.usuario_asignado == null)
                {
                    ax = db.caracteristicas.Where(x => x.id_caracteristica == ax.id_caracteristica_padre).First();
                }
                return (int)ax.usuario_asignado;
            }
            catch { return idUsu; }
        }

        /// <summary>
        /// Obtiene los links de las actividades incluido el boton regresar
        /// </summary>
        /// <param name="keym">key o id de la maquina</param>
        /// <param name="idCar">ID de la caracteristica</param>
        /// <param name="idUsu">ID del Usuario</param>
        /// <param name="op">Opcion 1=> Abre carpeta de actividades   2=> boton Atras    </param>
        /// <returns></returns>
        public List<ActivityList> getActivityList(long keym, long idCar, long idUsu, int op,long usuAct)
        {
            //op opcion  1=>actividades  2=>back
            try
            {
                #region Realiza la busqueda de las actividades ya sean de caracteristica de proyecto o ingresar en una actividades
                if (op == 1)
                {
                    List<ActivityList> dat = search(idCar, idUsu, keym,usuAct);
                    return dat;
                }
                #endregion
                #region Realiza la busqueda de las actividades cuando se presiona el boton ATRAS
                else if (op == 2)
                {
                    var idPar = db.caracteristicas.Where(x =>
                        x.id_caracteristica == idCar &&
                        x.keym == keym &&
                        x.id_usuario == idUsu
                    //p//x.usuario_asignado == idUsu
                    ).First();
                    List<ActivityList> dat = search((long)idPar.id_caracteristica_padre, (long)idPar.id_usuario_padre, (long)idPar.keym_padre,usuAct);
                    return dat;
                }
                #endregion
            }
            catch (Exception err)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// realiza la busqueda de las subactividades en la base de datos
        /// </summary>
        /// <param name="idCar"></param>
        /// <param name="idUsu"></param>
        /// <param name="keym"></param>
        /// <returns></returns>
        private List<ActivityList> search(long idCar, long idUsu, long keym,long usuAct)
        {
            try
            {
                long ownAct = getIdOwnActivity(keym, idUsu, idCar);

                List<ActivityList> dat = db.actividades.Where(y =>
                    y.caracteristicas.id_caracteristica_padre == idCar &&
                    y.caracteristicas.keym_padre == keym &&
                    y.caracteristicas.id_usuario_padre == idUsu
                    && (
                    
                    (
                    y.caracteristicas.id_usuario == usuAct ||
                    y.caracteristicas.usuario_asignado == usuAct
                    )
                    
                    ||
                    
                    (
                    y.caracteristicas.id_usuario != usuAct
                    && y.caracteristicas.visualizar_superior == true
                    )
                    
                    )
                ).OrderBy(x => x.pos).Select(x => new ActivityList()
                {
                    keym = x.caracteristicas.keym.ToString(),
                    idCar = x.caracteristicas.id_caracteristica,
                    usuCar = x.caracteristicas.id_usuario, //p//usuCar = x.caracteristicas.usuario_asignado,

                    parKeym = x.caracteristicas.keym_padre.ToString(),
                    parCar = x.caracteristicas.id_caracteristica_padre,
                    parUsu = x.caracteristicas.id_usuario_padre,

                    usuAsign = x.caracteristicas.usuario_asignado,

                    idAct = x.id_actividad,

                    desc = x.descripcion,
                    folder = (x.folder == 0 ? 1 : 0),

                    nom = x.nombre,
                    pos = x.pos,
                    sta = x.caracteristicas.estado
                }).ToList();
                return dat;
            }
            catch (Exception err)
            {
                return null;
            }
        }

        #endregion

        /*##################################################################################################################*/

        #region Generacion de links para la navegacion entre los proyectos y actividades publicas

        #region genera los links por publicacion web => 1 nivel

        /// <summary>
        /// trae todos los proyectos y actividades de la base de datos para generar la lista del 1 priemr nivel de los links
        /// </summary>
        public List<string> getLinks(long key, long idcar, long usu)
        {
            llink = new List<string>();
            try
            {
                caracteristicas car = (from x in db.caracteristicas
                                       where x.keym == key && x.id_caracteristica == idcar && x.id_usuario == usu
                                       //p// x.usuario_asignado == usu
                                       select x).First();
                caracteristicas cx = car;
                //Crea opcion para retroceder en la navegacion de las caracteristicas que poseen paginaWeb
                try
                {
                    bool st = true;
                    #region segun actividades
                    while (st)
                    {
                        var carPar = (from x in db.caracteristicas
                                      join y in db.actividades on new
                                      {
                                          A = x.keym,
                                          B = x.id_caracteristica,
                                          C = x.id_usuario
                                          //p// C = x.usuario_asignado
                                      } equals new { A = y.keym, B = y.id_caracteristica, C = y.id_usuario }

                                      where x.keym == car.keym_padre &&
                                              x.id_caracteristica == car.id_caracteristica_padre &&
                                              x.id_usuario == car.id_usuario_padre
                                      select new
                                      {
                                          x,
                                          x.keym,
                                          x.id_caracteristica,

                                          //p// Nota: especial atencion
                                          x.id_usuario,
                                          //

                                          y.nombre,
                                          y.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                                      }).First();

                        st = (bool)carPar.x.publicacion_web;
                        if (st)
                        {
                            string idPadre = carPar.keym + "," + carPar.id_caracteristica + "," + carPar.id_usuario;
                            //p//string rutaPadre = carPar.ruta_repositorio + carPar.nombre + ".html";
                            string rutaPadre = carPar.ruta_repositorio + "Web" + idPadre + ".html";

                            llink.Add(idPadre + "|" + "Atras" + "|" + rutaPadre);
                            st = false;

                        }
                        else
                        {
                            car = carPar.x;
                            st = true;
                        }
                    }
                    #endregion
                }
                catch
                {
                    #region segun proyectos
                    try
                    {
                        car = cx;
                        var carPar = (from x in db.caracteristicas
                                      join pro in db.proyectos on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = pro.keym, B = pro.id_caracteristica, C = pro.id_usuario }
                                      //p// C = x.usuario_asignado
                                      where x.keym == car.keym_padre &&
                                              x.id_caracteristica == car.id_caracteristica_padre &&
                                              x.id_usuario == car.id_usuario_padre
                                      select new
                                      {
                                          x.keym,
                                          x.id_caracteristica,

                                          //p// Nota: especial atencion
                                          x.id_usuario,
                                          //

                                          pro.nombre,
                                          pro.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                                      }).First();




                        string idPadre = carPar.keym + "," + carPar.id_caracteristica + "," + carPar.id_usuario;
                        //p//string rutaPadre = carPar.ruta_repositorio + carPar.nombre + ".html";
                        string rutaPadre = carPar.ruta_repositorio + "Web" + idPadre + ".html";

                        llink.Add(idPadre + "|" + "Atras" + "|" + rutaPadre);
                    }
                    catch
                    {

                    }
                    #endregion
                }
                //metodo recursivo para obtener los links publicos
                getLinksRecursive(cx);


            }
            catch (Exception err) { llink.Add(err.Message); }

            return llink;
        }

        /// <summary>
        /// Metodo recursivo para obtener los links 1 nivel
        /// </summary>
        /// <param name="car"></param>
        private void getLinksRecursive(caracteristicas car)
        {
            try
            {
                //db.Dispose();
                db = new MProjectContext();
                //lista de hijos de la caracteristica car
                var lstCar = (
                    from x in db.caracteristicas
                    join y in db.actividades
                    on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym_car, B = y.id_caracteristica, C = y.id_usuario_car }
                    //p// C = x.usuario_asignado
                    where (
                        x.keym_padre == car.keym &&
                        x.id_caracteristica_padre == car.id_caracteristica &&
                        x.id_usuario_padre == car.id_usuario)

                    orderby y.pos

                    select new
                    {
                        x,
                        x.keym,
                        y.id_actividad,
                        x.id_caracteristica,
                        //p// Nota: Especia Atencio
                        x.id_usuario,
                        // 

                        y.nombre,
                        y.id_usuarioNavigation.repositorios_usuarios,
                        x.publicacion_web
                    }
                    );
                try
                {
                    foreach (var x in lstCar)
                    {
                        if ((bool)x.publicacion_web)
                        {
                            try
                            {
                                string id = x.keym + "," + x.id_caracteristica + "," + x.id_usuario; //p// Revisar: es necesario?
                                string nombre = x.nombre;
                                //p//string ruta = x.repositorios_usuarios.ruta_repositorio + x.nombre + ".html";
                                string ruta = x.repositorios_usuarios.ruta_repositorio + "Web" + id + ".html";

                                llink.Add(id + "|" + nombre + "|" + ruta);
                            }
                            catch
                            {
                                llink.Add("ERROR");
                            }
                        }
                        else
                        {
                            getLinksRecursive(x.x);
                        }
                    }
                }
                catch (Exception err)
                {
                    string s = err.ToString();
                }
            }
            catch { }
        }

        #endregion

        #region GEnera los links de todas las caracteristicas por publicacion web => todos los niveles
        private List<string> allLink;
        private int pos = 0;


        /// <summary>
        /// trae todos los proyectos y actividades de la base de datos para generar la lista de todos los links
        /// </summary>
        public List<string> getAllLinks()
        {
            allLink = new List<string>();
            try
            {
                var carPro = from x in db.caracteristicas
                             join pro in db.proyectos on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = pro.keym, B = pro.id_caracteristica, C = pro.id_usuario }
                             //p// C = x.usuario_asignado
                             select new datLinks()
                             {
                                 car = x,
                                 idKey = x.keym,
                                 idCar = x.id_caracteristica,

                                 //p// Nota: especial cuidado
                                 idUsu = x.id_usuario,
                                 //

                                 nom = pro.nombre,
                                 rutaRep = pro.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio,
                                 pubWeb = x.publicacion_web
                             };
                foreach (var x in carPro)
                {
                    string pubWeb = "";
                    if ((bool)x.pubWeb)
                        pubWeb = "Y";
                    else
                        pubWeb = "N";
                    string id = x.idKey + "," + x.idCar + "," + x.idUsu;
                    string idPar = x.car.keym_padre + "," + x.car.id_caracteristica_padre + "," + x.car.id_usuario_padre;
                    string nombre = x.nom;
                    string rutaPadre = x.rutaRep + "Web" + id + ".html";
                    //p//string rutaPadre = x.rutaRep + x.nom + ".html";

                    allLink.Add(pubWeb + "|" + id + "|" + idPar + "|" + nombre + "|" + rutaPadre + "|" + pos);
                    getAllLinksRecursive(x.car);
                }


                return allLink;
            }
            catch { }
            return null;
        }
        /// <summary>
        /// metodo recursivo para traer las actividades que formara todos los links de la BD
        /// </summary>
        private void getAllLinksRecursive(caracteristicas car)
        {
            pos++;
            try
            {
                //db.Dispose();
                db = new MProjectContext();
                //lista de hijos de la caracteristica car
                var lstCar = (
                    from x in db.caracteristicas
                    join y in db.actividades
                    on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym_car, B = y.id_caracteristica, C = y.id_usuario_car }
                    //p// C = x.usuario_asignado

                    where (
                        x.keym_padre == car.keym &&
                        x.id_caracteristica_padre == car.id_caracteristica &&
                        x.id_usuario_padre == car.id_usuario)

                    orderby y.pos ascending

                    select new datLinks()
                    {
                        car = x,
                        idKey = x.keym,
                        idAct = y.id_actividad,
                        idCar = x.id_caracteristica,

                        //p// Nota: especial cuidado
                        idUsu = x.id_usuario,
                        //
                        nom = y.nombre,
                        repoUsu = y.id_usuarioNavigation.repositorios_usuarios,
                        pubWeb = x.publicacion_web
                    }
                    );
                try
                {

                    foreach (datLinks x in lstCar)
                    {

                        string pubWeb = "";
                        if ((bool)x.pubWeb)
                            pubWeb = "Y";
                        else
                            pubWeb = "N";
                        try
                        {
                            string id = x.idKey + "," + x.idCar + "," + x.idUsu;
                            string idPar = x.car.keym_padre + "," + x.car.id_caracteristica_padre + "," + x.car.id_usuario_padre;
                            string nombre = x.nom;
                            string ruta = x.repoUsu.ruta_repositorio + "Web" + id + ".html";
                            //p//string ruta = x.repoUsu.ruta_repositorio + x.nom + ".html";

                            allLink.Add(pubWeb + "|" + id + "|" + idPar + "|" + nombre + "|" + ruta + "|" + pos);
                            getAllLinksRecursive(x.car);
                        }
                        catch
                        {
                            allLink.Add("ERROR");
                        }


                    }


                }
                catch (Exception err)
                {
                    string s = err.ToString();
                    return;
                }
                try
                {
                    foreach (datLinks x in lstCar)
                    {
                        caracteristicas cr = x.car;
                        //getAllLinksRecursive(cr);
                    }
                }
                catch { }
            }
            catch { }
            pos--;
            return;
        }
        #endregion

        #endregion

        /*##################################################################################################################*/

        #region Estadisticas
        /// <summary>
        /// Stadisticas:  obtine los hijos de una caracteristica y devuelve la cadena json para generar graficas
        /// </summary>
        /// <param name="keym"></param>
        /// <param name="usu"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        public Dictionary<string, string> getChildPieChart(long keym, long usu, long car)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                var carAct = db.actividades.Where(x => x.caracteristicas.keym_padre == keym && x.caracteristicas.id_usuario_padre == usu && x.caracteristicas.id_caracteristica_padre == car).Select(
                    x => new datPieChar
                    {
                        label = x.nombre,
                        percent = (double)x.caracteristicas.porcentaje,
                        perAsig = (double)x.caracteristicas.porcentaje,
                        perComp = (double)x.caracteristicas.porcentaje_cumplido
                    }).ToList();

                var carChi = db.actividades.Where(x => x.caracteristicas.keym == keym && x.caracteristicas.id_usuario == usu && x.caracteristicas.id_caracteristica == car).Select(
                    x => new datPieChar
                    {
                        label = x.nombre,
                        percent = (double)x.caracteristicas.porcentaje,
                        perAsig = (double)x.caracteristicas.porcentaje,
                        perComp = (double)x.caracteristicas.porcentaje_cumplido
                    }).First();

                return convertJsonPieChart(carAct,carChi);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// genera la cadena en forma de JSON para luego crear las graficas con MORRIS
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private Dictionary<string, string> convertJsonPieChart(List<datPieChar> lst, datPieChar par)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string cadAsig = "[";
            string cadComp = "[";
            double val = 0;
            double valComp = 0;
            try
            {
                foreach (var x in lst)
                {
                    cadAsig = cadAsig + "{ label:'" + x.label + "' , value:" + x.perAsig.ToString().Replace(',', '.') + "},";
                    cadComp = cadComp + "{ cumplido:'" + x.label + "' , value:" + x.perComp.ToString().Replace(',', '.') + "},";
                    val = val + (double)x.perAsig;
                    valComp = valComp + (x.percent * x.perComp / 100);
                }
            }
            catch
            {

            }
            val = 100 - val;
            cadAsig = cadAsig + "{ label:'Yo', value:" + val.ToString().Replace(',', '.') + "}]";


            cadComp = cadComp.Remove(cadComp.Length - 1) + "]";

            string comp = "[{ label:'SI' , value:" + par.perComp.ToString().Replace(',', '.') + "}," + "{ label: 'NO' , value: " + (100 - par.perComp ).ToString().Replace(',', '.') + "}]";

            dic["perAsig"] = cadAsig;
            dic["perComp"] = cadComp;
            dic["comp"] = comp;

            return dic;
        }
        #endregion

        /*##################################################################################################################*/
        #region Clases auxiliares que ayudan en el funcionamiento de Project
        //clase que tiene informacion necesaria para generar los links desde la vista
        public ActivityInfo getInfoPrj(string[] pro)
        {
            ActivityInfo car = (from x in db.caracteristicas
                                join y in db.proyectos on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym, B = y.id_caracteristica, C = y.id_usuario }

                                where x.keym == Convert.ToInt64(pro[0]) &&
                                        x.id_caracteristica == Convert.ToInt64(pro[1]) &&
                                        x.id_usuario == Convert.ToInt64(pro[2])
                                select new ActivityInfo()
                                {
                                    keym = x.keym.ToString(),
                                    id_caracteristica = x.id_caracteristica,
                                    id_usuario = x.id_usuario,
                                    nombre = y.nombre,
                                    ruta_repositorio = y.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                                }).First();
            return car;
        }
        public ActivityInfo getInfoAct(string[] pro)
        {
            ActivityInfo car = (from x in db.caracteristicas
                                join y in db.actividades on new { A = x.keym, B = x.id_caracteristica, C = x.id_usuario } equals new { A = y.keym, B = y.id_caracteristica, C = y.id_usuario }

                                where x.keym == Convert.ToInt64(pro[0]) &&
                                     x.id_caracteristica == Convert.ToInt64(pro[1]) &&
                                     x.id_usuario == Convert.ToInt64(pro[2])
                                select new ActivityInfo()
                                {
                                    keym = x.keym.ToString(),
                                    id_caracteristica = x.id_caracteristica,
                                    id_usuario = x.id_usuario,
                                    nombre = y.nombre,
                                    ruta_repositorio = y.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio
                                }).First();
            return car;
        }
        /*##################################################################################################################*/
        //clase que tiene informacion necesaria para generar los links desde la vista
        private class datLinks
        {
            public caracteristicas car { get; set; }
            public long? idCar { get; set; }
            public long? idAct { get; set; }
            public long? idKey { get; set; }
            public long? idUsu { get; set; }
            public repositorios_usuarios repoUsu { get; set; }
            public bool? pubWeb { get; set; }
            public string nom { get; set; }
            public string rutaRep { get; set; }
        }
        //clase que tiene informacion necesaria para generar las graficas
        private class datPieChar
        {

            public string label { get; set; }
            public double percent { get; set; }
            public double perAsig { get; set; }
            public double perComp { get; set; }

        }
        //Clase ActivityInfo necesaria para mostrar los proyectos publicos
        public class ActivityInfo
        {
            public string keym { get; set; }
            public long id_caracteristica { get; set; }
            public long id_usuario { get; set; }
            public string nombre { get; set; }
            public string ruta_repositorio { get; set; }
        }
        #endregion
    }
}
