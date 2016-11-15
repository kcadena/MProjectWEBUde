using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Sqlite
{
    public partial class tipos_archivos
    {
        public tipos_archivos()
        {
            archivos = new HashSet<archivos>();
        }

        public long id_tipo_archivo { get; set; }
        public string nombre { get; set; }

        public virtual ICollection<archivos> archivos { get; set; }
    }
}
