using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class meta_datos
    {
        public meta_datos()
        {
            plantillas_meta_datos = new HashSet<plantillas_meta_datos>();
        }

        public long keym { get; set; }
        public long id_meta_dato { get; set; }
        public long id_usuario { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_ultima_modificacion { get; set; }
        public long id_tipo_dato { get; set; }
        public bool meta_dato_ir { get; set; }

        public virtual ICollection<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual tipos_datos id_tipo_datoNavigation { get; set; }
        public virtual usuarios id_usuarioNavigation { get; set; }
    }
}
