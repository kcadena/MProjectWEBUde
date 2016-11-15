using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Formatters.Json;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Hosting;

namespace MProjectWeb.Models.ModelController
{
    public class WebData
    {
        private readonly IHostingEnvironment _hostEnvironment;
        public JObject defaultUser()
        {
            JObject o1;
            try
            {
                o1 = JObject.Parse(File.ReadAllText(_hostEnvironment + "json/WebData.json"));
            }
            catch
            {
                o1 = JObject.Parse(File.ReadAllText(_hostEnvironment + "json/WebData.json"));
            }
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
