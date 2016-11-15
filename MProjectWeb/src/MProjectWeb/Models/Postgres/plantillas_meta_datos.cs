using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class plantillas_meta_datos
    {
        public long keym { get; set; }
        public long id_plantilla_meta_dato { get; set; }
        public long id_usuario { get; set; }
        public string fecha_ultima_modificacion { get; set; }
        public long id_meta_dato { get; set; }
        public long id_plantilla { get; set; }
        public long id_usuario_met { get; set; }
        public long id_usuario_pla { get; set; }
        public long keym_met { get; set; }
        public long keym_pla { get; set; }
        public bool requerido { get; set; }

        public virtual usuarios id_usuarioNavigation { get; set; }
        public virtual meta_datos meta_datos { get; set; }
        public virtual plantillas plantillas { get; set; }
    }
}
