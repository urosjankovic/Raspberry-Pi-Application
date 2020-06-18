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
        public string Unit { get; set; }
        public double data { get; set; }

    }
    

}
