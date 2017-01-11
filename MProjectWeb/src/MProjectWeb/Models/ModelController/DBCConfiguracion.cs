using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Formatters.Json;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Hosting;
using MProjectWeb.Models.Postgres;

namespace MProjectWeb.Models.ModelController
{
    public class DBCConfiguracion
    {
        MProjectContext db;
        public DBCConfiguracion()
        {
            db = new MProjectContext();
        }

        /// <summary>
        /// Trae la ip del repositorio del servidor 
        /// </summary>
        /// <returns></returns>
        public string getIpRepoServer()
        {
            try
            {
                string ip = db.configuracion_inicial.Where(x => x.id == 1).First().val_configuracion;
                return ip;
            }
            catch { return ""; }
        }

        /// <summary>
        /// Trae la ip de la plataforma en el servidor 
        /// </summary>
        /// <returns></returns>
        public string getIpPlatServer()
        {
            try
            {
                string ip = db.configuracion_inicial.Where(x => x.id == 3).First().val_configuracion;
                return ip;
            }
            catch { return ""; }
        }

        /// <summary>
        /// Trae la ruta fisica donde se encuentran los archivos en el servidor
        /// </summary>
        /// <returns></returns>
        public string getPathServer()
        {
            try
            {
                string path = db.configuracion_inicial.Where(x => x.id == 2).First().val_configuracion;
                return path;
            }
            catch { return ""; }
        }
    }
}

