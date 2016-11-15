using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class usuarios
    {
        public usuarios()
        {
            actividades = new HashSet<actividades>();
            archivos = new HashSet<archivos>();
            caracteristicas = new HashSet<caracteristicas>();
            proyectos = new HashSet<proyectos>();
            usuarios_tipo_usuarios = new HashSet<usuarios_tipo_usuarios>();
        }

        public long id_usuario { get; set; }
        public string apellido { get; set; }
        public string e_mail { get; set; }
        public string nombre { get; set; }
        public string pass { get; set; }

        public virtual ICollection<actividades> actividades { get; set; }
        public virtual ICollection<archivos> archivos { get; set; }
        public virtual ICollection<caracteristicas> caracteristicas { get; set; }
        public virtual ICollection<proyectos> proyectos { get; set; }
        public virtual ICollection<usuarios_tipo_usuarios> usuarios_tipo_usuarios { get; set; }
    }
}
