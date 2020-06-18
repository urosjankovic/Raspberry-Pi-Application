#define CLIENT
#define GET
#define DYNAMIC

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Net.Http;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RpiApp.ViewModels
{
    using RpiApp.Properties;
    using Model;
    using System.Windows.Controls;
    using RpiApp.ViewModel;
    using System.Net.Http.Headers;
    using System.Data;
    using OxyPlot.Reporting;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public class JoyViewModel
    {
        #region Properties
        private string ipAddress;
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                if (ipAddress != value)
                {
                    ipAddress = value;
                    OnPropertyChanged("IpAddress");
                }
            }
        }
        private int sampleTime;
        private int maxSampleNumber;
        #endregion

        

        public ButtonCommand Reset { get; set; }

        public ButtonCommand UpdateConfigJoy { get; set; }
        public ButtonCommand DefaultConfigJoy { get; set; }
        public PlotModel Joy { get; set; }
        
        private ConfigParams config = new ConfigParams();

        
        #region Fields;
        private IoTServer Server;
        private Timer RequestTimerJoy;
        int yMax = 10, xMax = 10;

        
        #endregion

        public JoyViewModel()
        {

            
            Joy = new PlotModel { Title = "Joystick Data" };

            Joy.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -xMax,
                Maximum = xMax,
                Key = "Horizontal",
                MajorGridlineStyle = LineStyle.Dot,
                //Unit = "sec",
                //Title = "Time"
            }); 
            Joy.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -yMax,
                Maximum = yMax,
                Key = "Vertical",
                MajorGridlineStyle = LineStyle.Dot
                //Unit = "*C",
                //Title = "Temperature"
            }); 

            Reset = new ButtonCommand(ResetGraph);

            UpdateConfigJoy = new ButtonCommand(UpdateJoy);
            DefaultConfigJoy = new ButtonCommand(DefaultJoy);

            Server = new IoTServer(ipAddress);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;
            maxSampleNumber = config.MaxSampleNumber;

            RequestTimerJoy = new Timer(100);
            RequestTimerJoy.Elapsed += new ElapsedEventHandler(RequestTimerElapsedJoy);
            RequestTimerJoy.Enabled = true;
        }
        private void UpdatePlotJoy(int x, int y)
        {
            //Joy.Series.Clear();
            ScatterSeries scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };
            //LineSeries lineSeries = Joy.Series[0] as LineSeries;

            scatterSeries.Points.Add(new ScatterPoint(x, y));

            Joy.Series.Add(scatterSeries);

            if (x >= xMax)
            {
                xMax = x;
                Joy.Axes[0].Minimum = xMax - 21;
                Joy.Axes[0].Maximum = xMax + 1;
            }

            if (x <= -xMax)
            {
                xMax = -x;
                Joy.Axes[0].Minimum = -xMax - 1;
                Joy.Axes[0].Maximum = -xMax + 21;
            }

            if (y >= yMax)
            {
                yMax = y;
                Joy.Axes[1].Minimum = yMax - 21;
                Joy.Axes[1].Maximum = yMax + 1;
            }

            if (y <= -yMax)
            {
                yMax = -y;
                Joy.Axes[1].Minimum = -yMax - 1;
                Joy.Axes[1].Maximum = -yMax + 21;
            }
            bool btnPressed = false;


            Joy.InvalidatePlot(true);
        }

        private async void UpdatePlotWithServerResponseJoy()
        {

            string responseText = await Server.GETwithClientJoy();

            ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
            if(responseJson != null) { UpdatePlotJoy(responseJson.X, responseJson.Y); }
            else { UpdatePlotJoy(0,0); }
               
        }


        private void RequestTimerElapsedJoy(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseJoy();
        }


        #region Button Comands
        public void ResetGraph()
        {
            string json = new WebClient().DownloadString("http://192.168.1.26/web_app/server/joystick_via_deamon.php?id=rst");

            dynamic clean = JObject.Parse(json);

            UpdatePlotJoy((int)clean.X, (int)clean.Y);
            Joy.Series.Clear();
            Joy.Axes[0].Minimum = -xMax;
            Joy.Axes[0].Maximum = xMax;
            Joy.Axes[1].Minimum = -yMax;
            Joy.Axes[1].Maximum = yMax;
            
        }

        private void UpdateJoy()
        {
            config = new ConfigParams(ipAddress, sampleTime, maxSampleNumber);
            Server = new IoTServer(IpAddress);
        }
        private void DefaultJoy()
        {
            config = new ConfigParams();
            IpAddress = config.IpAddress;
            Server = new IoTServer(IpAddress);
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
