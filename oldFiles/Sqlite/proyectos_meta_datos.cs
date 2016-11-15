using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class proyectos_meta_datos
    {
        public long id_proyecto_meta_dato { get; set; }
        public long id_plantilla_meta_dato { get; set; }
        public long id_proyecto { get; set; }
        public string valor { get; set; }

        public virtual plantillas_meta_datos id_plantilla_meta_datoNavigation { get; set; }
        public virtual proyectos id_proyectoNavigation { get; set; }
    }
}
