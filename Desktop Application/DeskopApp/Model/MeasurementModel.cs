using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpiApp.Model
{
    using RpiApp.ViewModels;
    public class MeasurementModel
    {
        public string Name { get; set; }
        public double Data { get; set; }
        public string Unit { get; set; }

        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }      
       
    }
}
