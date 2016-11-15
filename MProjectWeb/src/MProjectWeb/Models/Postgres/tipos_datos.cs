using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class tipos_datos
    {
        public tipos_datos()
        {
            meta_datos = new HashSet<meta_datos>();
        }

        public long id_tipo_dato { get; set; }
        public string descripcion { get; set; }

        public virtual ICollection<meta_datos> meta_datos { get; set; }
    }
}
