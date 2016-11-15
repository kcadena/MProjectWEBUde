using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Formatters.Json;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MProjectWeb.Models.DBControllers
{
    public class WebData
    {
        public JObject defaultUser()
        {
            JObject o1 = JObject.Parse(File.ReadAllText(@"C:\Users\admi\Desktop\Trabajo de grado\PROGRAMMING\Project.Management\MProjectWEB\MProjectWeb\src\MProjectWeb\Models\DBControllers\WebData.json"));
            return o1;
        }
    }
}

/*

JObject o1 = JObject.Parse(File.ReadAllText(@"c:\videogames.json"));

// read JSON directly from a file
using (StreamReader file = File.OpenText(@"c:\videogames.json"))
using (JsonTextReader reader = new JsonTextReader(file))
{
    JObject o2 = (JObject)JToken.ReadFrom(reader);
}

    */
