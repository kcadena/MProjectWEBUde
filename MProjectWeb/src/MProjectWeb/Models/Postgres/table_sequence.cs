using System;
using System.Collections.Generic;

namespace MProjectWeb.Models.Postgres
{
    public partial class table_sequence
    {
        public long key { get; set; }
        public long actividades { get; set; }
        public long archivos { get; set; }
        public long caracteristicas { get; set; }
        public long costos { get; set; }
        public long presuspuesto { get; set; }
        public long proyectos { get; set; }
        public long proyectos_meta_datos { get; set; }
        public long recursos { get; set; }
    }
}
