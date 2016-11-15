using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class tipos_usuarios
    {
        public tipos_usuarios()
        {
            usuarios_tipo_usuarios = new HashSet<usuarios_tipo_usuarios>();
        }

        public long id_tipo_usu { get; set; }
        public string nombre { get; set; }

        public virtual ICollection<usuarios_tipo_usuarios> usuarios_tipo_usuarios { get; set; }
    }
}
