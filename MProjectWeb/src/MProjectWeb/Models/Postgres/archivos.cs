using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class archivos
    {
        public long keym_arc { get; set; }
        public long id_archivo { get; set; }
        public long id_usuario_arc { get; set; }
        public string contenido { get; set; }
        public string descripcion { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_ultima_modificacion { get; set; }
        public long id_caracteristica { get; set; }
        public long id_usuario_car { get; set; }
        public long keym_car { get; set; }
        public decimal? localizacion { get; set; }
        public decimal? longitud { get; set; }
        public string nombre_archivo { get; set; }
        public int? publicacion { get; set; }
        public string srcGifServ { get; set; }
        public string srcServ { get; set; }
        public string subtitulo { get; set; }
        public string tipo { get; set; }
        public string titulo { get; set; }

        public virtual caracteristicas caracteristicas { get; set; }
    }
}
