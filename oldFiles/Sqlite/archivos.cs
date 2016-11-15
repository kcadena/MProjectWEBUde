using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class archivos
    {
        public long id_archivo { get; set; }
        public string contenido { get; set; }
        public string fecha_carga { get; set; }
        public long id_caracteristica { get; set; }
        public long id_tipo_archivo { get; set; }
        public long? id_usuario { get; set; }
        public string nombre { get; set; }
        public long? publicacion { get; set; }

        public virtual caracteristicas id_caracteristicaNavigation { get; set; }
        public virtual tipos_archivos id_tipo_archivoNavigation { get; set; }
        public virtual usuarios id_usuarioNavigation { get; set; }
    }
}
