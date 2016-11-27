using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class usuarios
    {
        public usuarios()
        {
            actividades = new HashSet<actividades>();
            caracteristicas = new HashSet<caracteristicas>();
            caracteristicasNavigation = new HashSet<caracteristicas>();
            costos = new HashSet<costos>();
            meta_datos = new HashSet<meta_datos>();
            plantillas = new HashSet<plantillas>();
            plantillas_meta_datos = new HashSet<plantillas_meta_datos>();
            presupuesto = new HashSet<presupuesto>();
            proyectos = new HashSet<proyectos>();
            proyectos_meta_datos = new HashSet<proyectos_meta_datos>();
            recursos = new HashSet<recursos>();
        }

        public long id_usuario { get; set; }
        public bool administrador { get; set; }
        public string apellido { get; set; }
        public string cargo { get; set; }
        public bool disponible { get; set; }
        public string e_mail { get; set; }
        public string entidad { get; set; }
        public string genero { get; set; }
        public string imagen { get; set; }
        public string nombre { get; set; }
        public string pass { get; set; }
        public string telefono { get; set; }

        public virtual ICollection<actividades> actividades { get; set; }
        public virtual ICollection<caracteristicas> caracteristicas { get; set; }
        public virtual ICollection<caracteristicas> caracteristicasNavigation { get; set; }
        public virtual ICollection<costos> costos { get; set; }
        public virtual ICollection<meta_datos> meta_datos { get; set; }
        public virtual ICollection<plantillas> plantillas { get; set; }
        public virtual ICollection<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual ICollection<presupuesto> presupuesto { get; set; }
        public virtual ICollection<proyectos> proyectos { get; set; }
        public virtual ICollection<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual ICollection<recursos> recursos { get; set; }
        public virtual repositorios_usuarios repositorios_usuarios { get; set; }
    }
}
