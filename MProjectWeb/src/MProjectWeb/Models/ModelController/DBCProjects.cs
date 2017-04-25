using MProjectWeb.Models.Postgres;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.Models.ModelController
{
    public class DBCProjects
    {
        MProjectContext db;
        public DBCProjects()
        {
            db = new MProjectContext();
        }
        /// <summary>
        /// trae todos los proyectos correspondientes a un usuario
        /// </summary>
        /// <param name="id_usu"></param>
        /// <returns></returns>
        public List<ProjectsUsers> listProjectsUsers(long id_usu)
        {
            try
            {
                //var dat = from pr in db.proyectos
                //          join car in db.caracteristicas on pr.id_caracteristica equals car.id_caracteristica

                //          where pr.id_usuario == id_usu

                //          select new ProjectsUsers()
                //          {
                //              id_pro_car=pr.id_caracteristica,
                //              id_usu = pr.id_usuario,
                //              id_pro = pr.id_proyecto,
                //              keym=car.keym,
                //              desc =pr.descripcion
                //          };
                var dat = db.proyectos.Where(x => x.id_usuario == id_usu).Select(x =>
                         new ProjectsUsers()
                         {
                             id_car = x.id_caracteristica,
                             id_usu = x.id_usuario_car,
                             id_pro = x.id_proyecto,
                             keym = x.keym_car,
                             
                             desc = x.nombre
                         }
                    );

                if (dat.Count() > 0)
                    return dat.ToList<ProjectsUsers>();
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// trae todos los proyectos publicos existentes  y activos hasta el momento
        /// </summary>
        /// <returns></returns>
        public List<ProjectsUsers> listPublicProjectsUsers()
        {
            try
            {
                var dat = db.proyectos.Where(x => x.caracteristicas.publicacion_web == true && x.ir_proyecto == true).Select(x =>
                    new ProjectsUsers()
                    {
                        id_car = x.id_caracteristica,
                        id_usu = x.id_usuario_car,
                        keym = x.keym,
                        desc = x.descripcion,
                        //ruta = x.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio +
                        //x.nombre
                        //+ ".html"
                        ruta = x.id_usuarioNavigation.repositorios_usuarios.ruta_repositorio +"Web"+
                        x.keym+"-"+
                        x.id_caracteristica+ "-" +
                        x.id_usuario_car+".html"
                    }
                    );
                //var dat = db.proyectos.Where(x =>x.caracteristicas.publicacion_web==true).Select(x =>
                //         new ProjectsUsers()
                //         {
                //             id_car = x.id_caracteristica,
                //             id_usu = x.id_usuario_car,
                //             id_pro = x.id_proyecto,
                //             keym = x.keym,
                //             desc = x.descripcion
                //         }
                //    );

                if (dat.Count() > 0)
                    return dat.ToList<ProjectsUsers>();
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Genera la lista de los prpyectos y actividades encontrados que poseen pagina web publica
        /// </summary>
        /// <param name="txt"></param>
        public List<ListWebPage> seachWebPage(string txt)
        {
            try
            {
                // Connect to a PostgreSQL database
                NpgsqlConnection conn = new NpgsqlConnection("Server=190.254.4.6; User Id=postgres; " +
                   "Password=NJpost2016;Database=MProjectPru;");
                conn.Open();

                string query = @"
                    select t.keym,t.car,t.usu,t.asig,t.nombre,t.tipo
                    from (
                       (
                       SELECT caracteristicas.keym keym, caracteristicas.id_caracteristica car, caracteristicas.id_usuario usu, caracteristicas.usuario_asignado asig,
                       actividades.nombre nombre, caracteristicas.tipo_caracteristica tipo FROM caracteristicas join actividades
                       on  caracteristicas.keym = actividades.keym_car and
                       caracteristicas.id_caracteristica = actividades.id_caracteristica and
                       caracteristicas.id_usuario = actividades.id_usuario_car
                       WHERE to_tsvector('spanish', actividades.nombre) @@ to_tsquery('" + txt + @"')
                       and caracteristicas.publicacion_web = true
                    )

                    UNION

                    (
                        SELECT
                        caracteristicas.keym keym, caracteristicas.id_caracteristica car, caracteristicas.id_usuario usu, caracteristicas.usuario_asignado asig,
                        proyectos.nombre nombre, caracteristicas.tipo_caracteristica tipo FROM caracteristicas join proyectos
                        on  caracteristicas.keym = proyectos.keym_car and
                        caracteristicas.id_caracteristica = proyectos.id_caracteristica and
                        caracteristicas.id_usuario = proyectos.id_usuario_car
                        WHERE to_tsvector('spanish', proyectos.nombre) @@ to_tsquery('" + txt + @"')
                        and caracteristicas.publicacion_web = true
                    )
                ) t order by t.tipo;";

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand(query, conn);

                // Execute the query and obtain the value of the first column of the first row
                //Int64 count = (Int64)command.ExecuteScalar();

                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();

                // Output rows

                List<ListWebPage> lst = new List<ListWebPage>();

                while (dr.Read())
                {
                    string ruta = db.configuracion_inicial.Where(x => x.id == 3).First().val_configuracion.ToString()
                        + "/Projects/publicprojects?p=" + dr[0] + "-" + dr[1] + "-" + dr[2];
                    lst.Add(new ListWebPage()
                    {
                        keym = dr[0] + "",
                        car = dr[1] + "",
                        usu = dr[2] + "",
                        asig = dr[3] + "",
                        nombre = dr[4] + "",
                        tipo = dr[5] + "",
                        ruta = ruta
                    });
                }
                conn.Close();
                return lst;
            }
            catch { return null; }
        }

        public List<ListWebPage> seachWebPage()
        {
            try
            {
                // Connect to a PostgreSQL database
                NpgsqlConnection conn = new NpgsqlConnection("Server=190.254.4.6; User Id=postgres; " +
                   "Password=NJpost2016;Database=MProjectPru;");
                conn.Open();

                string query = @"
                    select t.keym,t.car,t.usu,t.asig,t.nombre,t.tipo
                    from (
                       (
                       SELECT caracteristicas.keym keym, caracteristicas.id_caracteristica car, caracteristicas.id_usuario usu, caracteristicas.usuario_asignado asig,
                       actividades.nombre nombre, caracteristicas.tipo_caracteristica tipo FROM caracteristicas join actividades
                       on  caracteristicas.keym = actividades.keym_car and
                       caracteristicas.id_caracteristica = actividades.id_caracteristica and
                       caracteristicas.id_usuario = actividades.id_usuario_car
                       WHERE caracteristicas.publicacion_web = true
                    )

                    UNION

                    (
                        SELECT
                        caracteristicas.keym keym, caracteristicas.id_caracteristica car, caracteristicas.id_usuario usu, caracteristicas.usuario_asignado asig,
                        proyectos.nombre nombre, caracteristicas.tipo_caracteristica tipo FROM caracteristicas join proyectos
                        on  caracteristicas.keym = proyectos.keym_car and
                        caracteristicas.id_caracteristica = proyectos.id_caracteristica and
                        caracteristicas.id_usuario = proyectos.id_usuario_car
                        WHERE  caracteristicas.publicacion_web = true
                    )
                ) t order by t.tipo desc;";

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand(query, conn);

                // Execute the query and obtain the value of the first column of the first row
                //Int64 count = (Int64)command.ExecuteScalar();

                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();

                // Output rows

                List<ListWebPage> lst = new List<ListWebPage>();

                while (dr.Read())
                {
                    string ruta = db.configuracion_inicial.Where(x => x.id == 3).First().val_configuracion.ToString()
                        + "/Projects/publicprojects?p=" + dr[0] + "-" + dr[1] + "-" + dr[2];
                    lst.Add(new ListWebPage()
                    {
                        keym = dr[0] + "",
                        car = dr[1] + "",
                        usu = dr[2] + "",
                        asig = dr[3] + "",
                        nombre = dr[4] + "",
                        tipo = dr[5] + "",
                        ruta = ruta
                    });
                }
                conn.Close();
                return lst;
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// Clase auxiliar para realizar busqueda de las caracteristicas que poseen pagina web publica
    /// </summary>
    public class ListWebPage
    {
        public string keym { get; set; }
        public string car { get; set; }
        public string usu { get; set; }
        public string asig { get; set; }

        public string nombre { set; get; }
        public string ruta { set; get; }
        public string tipo { set; get; }
    }

    /// <summary>
    /// Clase auxiliar para el mejor manejo de los proyectos
    /// </summary>
    public class ProjectsUsers
    {
        public long? id_pro { get; set; }
        public long? id_car { get; set; }
        public long? id_usu { get; set; }
        public long keym { get; set; }
        public string desc { get; set; }
        public string valor { get; set; }
        public string ruta{ get; set; }

    }
}

