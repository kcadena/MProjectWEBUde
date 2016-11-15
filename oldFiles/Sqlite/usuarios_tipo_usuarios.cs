using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class usuarios_tipo_usuarios
    {
        public long id_usuario { get; set; }
        public long id_tipo_usu { get; set; }

        public virtual tipos_usuarios id_tipo_usuNavigation { get; set; }
        public virtual usuarios id_usuarioNavigation { get; set; }
    }
}
