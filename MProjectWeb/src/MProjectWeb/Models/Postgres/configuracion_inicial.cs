using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class configuracion_inicial
    {
        public int id { get; set; }
        public string configuracion { get; set; }
        public string descripcion { get; set; }
        public string val_configuracion { get; set; }
    }
}
