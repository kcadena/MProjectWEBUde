using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.ViewModels
{
    public class ActivityList
    {
        public string keym { get; set; }
        public long idCar { get; set; }
        public long usuCar { get; set; }
      
        public string parKeym { get; set; }
        public long? parCar { get; set; }
        public long? parUsu { get; set; }

        public long? usuAsign { get; set; }

        public long idAct { get; set; }

        public string nom { get; set; }
        public string desc { get; set; }
        public string sta { get; set; }

        public int pos { get; set; }
        public int folder { get; set; }

    }

   
}
