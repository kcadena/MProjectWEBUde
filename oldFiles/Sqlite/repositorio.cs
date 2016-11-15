using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class repositorio
    {
        public repositorio()
        {
            proyectos = new HashSet<proyectos>();
        }

        public long id_repositorio { get; set; }
        public string descripcion { get; set; }
        public string ruta_proyecto { get; set; }

        public virtual ICollection<proyectos> proyectos { get; set; }
    }
}
