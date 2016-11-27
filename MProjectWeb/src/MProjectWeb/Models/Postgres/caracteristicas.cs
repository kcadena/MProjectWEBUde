using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class caracteristicas
    {
        public caracteristicas()
        {
            actividades = new HashSet<actividades>();
            archivos = new HashSet<archivos>();
            costosNavigation = new HashSet<costos>();
            presupuestoNavigation = new HashSet<presupuesto>();
            proyectos = new HashSet<proyectos>();
            recursosNavigation = new HashSet<recursos>();
        }

        public long keym { get; set; }
        public long id_caracteristica { get; set; }
        public long id_usuario { get; set; }
        public int? costos { get; set; }
        public string estado { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_ultima_modificacion { get; set; }
        public long? id_caracteristica_padre { get; set; }
        public long? id_usuario_padre { get; set; }
        public long? keym_padre { get; set; }
        public decimal? porcentaje { get; set; }
        public decimal porcentaje_asignado { get; set; }
        public decimal porcentaje_cumplido { get; set; }
        public int? presupuesto { get; set; }
        public bool publicacion_reporte { get; set; }
        public bool? publicacion_web { get; set; }
        public int? recursos { get; set; }
        public int? recursos_restantes { get; set; }
        public string tipo_caracteristica { get; set; }
        public long? usuario_asignado { get; set; }
        public bool visualizar_superior { get; set; }

        public virtual ICollection<actividades> actividades { get; set; }
        public virtual ICollection<archivos> archivos { get; set; }
        public virtual ICollection<costos> costosNavigation { get; set; }
        public virtual ICollection<presupuesto> presupuestoNavigation { get; set; }
        public virtual ICollection<proyectos> proyectos { get; set; }
        public virtual ICollection<recursos> recursosNavigation { get; set; }
        public virtual usuarios id_usuarioNavigation { get; set; }
        public virtual usuarios usuario_asignadoNavigation { get; set; }
        public virtual caracteristicas caracteristicasNavigation { get; set; }
        public virtual ICollection<caracteristicas> InversecaracteristicasNavigation { get; set; }
    }
}
