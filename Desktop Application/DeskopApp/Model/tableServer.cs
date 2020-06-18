using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace RpiApp.Model
{
    public class tableServer
    {

        public dynamic getMeasurementsEnv()
        {
            var json = new WebClient().DownloadString("http://192.168.1.26/web_app/server/sensors_via_deamon.php?id=env");

            return JArray.Parse(json);
        }

        public dynamic getMeasurementsOri() 
        {
            string json = new WebClient().DownloadString("http://192.168.1.26/web_app/server/sensors_via_deamon.php?id=ori");

            return JArray.Parse(json);
        }

    }
}
