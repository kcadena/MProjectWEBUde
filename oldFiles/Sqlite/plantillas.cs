using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class plantillas
    {
        public plantillas()
        {
            plantillas_meta_datos = new HashSet<plantillas_meta_datos>();
            proyectos = new HashSet<proyectos>();
        }

        public long id_plantilla { get; set; }
        public string descripcion { get; set; }
        public string nombre { get; set; }

        public virtual ICollection<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual ICollection<proyectos> proyectos { get; set; }
    }
}
