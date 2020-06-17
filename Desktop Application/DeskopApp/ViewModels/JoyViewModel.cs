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
        #region Fields
        private string ipAddress;
        private int maxSampleNumber;
        private int sampleTime;
        private int timeStamp = 0;
        private IoTServer Server;
        private Timer RequestTimerJoy;

        public ButtonCommand Reset { get; set; }

        public PlotModel Joy { get; set; }
        
        private ConfigParams config = new ConfigParams();

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

           

            Server = new IoTServer(ipAddress);

            sampleTime = config.SampleTime;
            ipAddress = config.IpAddress;
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
                Joy.Axes[0].Minimum = xMax - 20;
                Joy.Axes[0].Maximum = xMax + 5;
            }

            if (x <= -xMax)
            {
                xMax = -x;
                Joy.Axes[0].Minimum = -xMax - 5;
                Joy.Axes[0].Maximum = -xMax + 20;
            }

            if (y >= yMax)
            {
                yMax = y;
                Joy.Axes[1].Minimum = yMax - 20;
                Joy.Axes[1].Maximum = yMax + 5;
            }

            if (y <= -yMax)
            {
                yMax = -y;
                Joy.Axes[1].Minimum = -yMax - 5;
                Joy.Axes[1].Maximum = -yMax + 20;
            }

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

        public void ResetGraph()
        { 
            UpdatePlotJoy(0, 0);
            Joy.Series.Clear();
            Joy.Axes.Clear();
            //Joy.Axes[0].Minimum = -xMax;
            //Joy.Axes[0].Maximum = xMax;
            //Joy.Axes[1].Minimum = -yMax;
            //Joy.Axes[1].Maximum = yMax;
        }
    }
}
