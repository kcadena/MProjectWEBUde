using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class repositorios_usuarios
    {
        public long id_usuario { get; set; }
        public string ruta_repositorio { get; set; }

        public virtual usuarios id_usuarioNavigation { get; set; }
    }
}
