using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MProjectWeb.Models.Postgres;

namespace MProjectWeb.Models.ModelController
{
    public class DBCUsuarios
    {
        MProjectContext db;
        public DBCUsuarios()
        {
            db = new MProjectContext();
        }

        /// <summary>
        /// Inicio de cesion del usuario => disponible: identifica si esta activo o no
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public usuarios loginUsuario(Dictionary<string, string> dic)
        {
            try
            {
                usuarios dat = (from x in db.usuarios
                                where x.e_mail == dic["email"] && x.pass == dic["pass"] && x.disponible == true
                                select x).First();
                return dat;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtine el usuario espesificado segun su ID en la clase ViewModels/Usuarios 
        /// </summary>
        /// <param name="idUsu"></param>
        /// <returns></returns>
        public ViewModels.usuarios getUser(long idUsu)
        {
            try
            {
                var usr = db.usuarios.Where(x => x.id_usuario == idUsu).Select(x => new ViewModels.usuarios
                {
                    id_usuario = x.id_usuario,
                    e_mail = x.e_mail,
                    pass = x.pass,
                    nombre = x.nombre,
                    apellido = x.apellido,
                    cargo = x.cargo,
                    entidad = x.entidad,
                    genero = x.genero,
                    telefono = x.telefono,
                    imagen = x.imagen,
                    administrador = x.administrador,
                    path = x.repositorios_usuarios.ruta_repositorio ,
                    disponible = x.disponible
                }).First();
                return usr;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Actualiza informacion del usuario
        /// </summary>
        /// <param name="usu"></param>
        /// <returns></returns>
        public bool updateUserData(usuarios usu)
        {
            try
            {
                var usr = db.usuarios.Where(x => x.id_usuario == usu.id_usuario).First();
                usr.e_mail = usu.e_mail;
                usr.pass = usu.pass;
                usr.nombre = usu.nombre;
                usr.apellido = usu.apellido;
                usr.genero = usu.genero;
                usr.cargo = usu.cargo;
                usr.telefono = usu.telefono;
                usr.entidad = usu.entidad;
                usr.imagen = usu.imagen;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Otiene el id del usuario libre para luego ser asignado al siguiente
        /// </summary>
        /// <returns></returns>
        public long getFreeIdUser()
        {
            try
            {
                long id = db.usuarios.OrderByDescending(x => x.id_usuario).First().id_usuario;
                return id + 1;
            }
            catch
            {
                return 1;
            }
        }
       
        /// <summary>
        /// Realiza el registro del usuario
        /// </summary>
        /// <param name="usu">Clase Usuarios del modelo segun la base de datos Postgres</param>
        /// <returns></returns>
        public long regUser(usuarios usu)
        {
            try
            {
                long id = getFreeIdUser();
                if (id != -1)
                {
                    DBCConfiguracion conf = new DBCConfiguracion();
                    
                    repositorios_usuarios rp = new repositorios_usuarios();
                    rp.id_usuario = id;
                    rp.ruta_repositorio = conf.getIpRepoServer() + "mp/user" + id+"/";

                    usu.id_usuario = id;
                    usu.imagen = "PicProfile-" + id + ".jpg";

                    usu.disponible = true;

                    db.usuarios.Add(usu);
                    db.SaveChanges();
                    db.repositorios_usuarios.Add(rp);
                    db.SaveChanges();
                    return id;
                }
                else
                    return -1;

            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Activa el registro hecho por el usuario => necesario para poder ingresar a la plataforma
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns></returns>
        public usuarios userActivate(long id)
        {
            try
            {
                var usr = db.usuarios.Where(x => x.id_usuario == id).First();
                usr.disponible = true;
                db.SaveChanges();
                return usr;
            }
            catch { return null; }
        }

        /// <summary>
        /// Genera y asigna nueva clave al usuario 
        /// </summary>
        /// <param name="email">Correo electronico del usuario</param>
        /// <returns></returns>
        public string forgetPassword(string email)
        {
            try
            {
                string pass = newRandomPassword();
                var usr = db.usuarios.Where(x => x.e_mail == email && x.disponible == true).First();
                usr.pass = pass;
                db.SaveChanges();
                return pass;
            }
            catch
            {
                return "err";
            }
        }

        /// <summary>
        /// Verifica que el correo electronico no este repido
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool verifyEmail(string email)
        {
            try
            {
                var usu = db.usuarios.Where(x => x.e_mail == email).First();
                if (usu.e_mail.Length > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// crea clave aleatoria
        /// </summary>
        /// <returns></returns>
        private string newRandomPassword()
        {
            Random obj = new Random();
            string opc = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int length = opc.Length;
            char letra;
            int cadLength = 30;
            string newCad = "";
            for (int i = 0; i < cadLength; i++)
            {
                letra = opc[obj.Next(length)];
                newCad += letra.ToString();
            }
            return newCad;
        }

       
    }
}
