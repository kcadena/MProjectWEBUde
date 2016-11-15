using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class archivos
    {
        public long keym { get; set; }
        public long id_archivo { get; set; }
        public long id_usuario_own { get; set; }
        public string contenido { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_carga { get; set; }
        public DateTime fecha_ultima_modificacion { get; set; }
        public long id_caracteristica { get; set; }
        public long id_usuario_car { get; set; }
        public long keym_car { get; set; }
        public string nombre { get; set; }
        public int? publicacion { get; set; }
        public string subtitulo { get; set; }
        public string tipo { get; set; }
        public string titulo { get; set; }

        public virtual caracteristicas caracteristicas { get; set; }
    }
}
