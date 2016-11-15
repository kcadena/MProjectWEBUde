using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class plantillas_meta_datos
    {
        public plantillas_meta_datos()
        {
            proyectos_meta_datos = new HashSet<proyectos_meta_datos>();
        }

        public long id_plantilla_meta_dato { get; set; }
        public long id_meta_datos { get; set; }
        public long id_plantilla { get; set; }
        public string requerido { get; set; }

        public virtual ICollection<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual meta_datos id_meta_datosNavigation { get; set; }
        public virtual plantillas id_plantillaNavigation { get; set; }
    }
}
