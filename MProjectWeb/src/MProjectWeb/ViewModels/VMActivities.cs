using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.ViewModels
{
    public class ActivityList
    {
        public long idCar { get; set; }
        public long idAct { get; set; }
        public long usuCar { get; set; }
        public long parUsu { get; set; }
        public long parCar { get; set; }

        public string keyM { get; set; }
        public string nom { get; set; }
        public string desc { get; set; }
        public string sta { get; set; }

        public int pos { get; set; }
        public int folder { get; set; }

    }

    public class ActivityInfo
    {
        ulong id_act { get; set; }
        ulong id_characteristic { get; set; }
        string name { get; set; }
        string description { get; set; }
        ulong id_fol { get; set; }
        string folder { get; set; }
        string state { get; set; }
        float asign_percent { get; set; }
        float execute_percent { get; set; }
        int time { get; set; }
        string type_time { get; set; }
        DateTime start_date { get; set; }
        uint sub_act_1_lev { get; set; }
        uint sub_act_all_lev { get; set; }
        bool prj_prj { get; set; }

    }
}
