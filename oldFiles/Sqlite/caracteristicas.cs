using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class caracteristicas
    {
        public caracteristicas()
        {
            archivos = new HashSet<archivos>();
        }

        public long id_caracteristica { get; set; }
        public string estado { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_inicio { get; set; }
        public long? id_actividad { get; set; }
        public long? id_proyecto { get; set; }
        public long? id_usuario { get; set; }
        public string objetivo { get; set; }
        public long? padre_caracteristica { get; set; }
        public long? porcentaje_asignado { get; set; }
        public long? porcentaje_cumplimido { get; set; }
        public long? proyecto_padre { get; set; }

        public virtual ICollection<archivos> archivos { get; set; }
        public virtual actividades id_actividadNavigation { get; set; }
        public virtual proyectos id_proyectoNavigation { get; set; }
        public virtual usuarios id_usuarioNavigation { get; set; }
        public virtual caracteristicas padre_caracteristicaNavigation { get; set; }
        public virtual ICollection<caracteristicas> Inversepadre_caracteristicaNavigation { get; set; }
        public virtual proyectos proyecto_padreNavigation { get; set; }
    }
}
