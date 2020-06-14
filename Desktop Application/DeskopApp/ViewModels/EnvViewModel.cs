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

    public class EnvViewModel
    {
        #region Properties
        private string ipAddress;

        private int sampleTime;

        private int maxSampleNumber;


        public PlotModel Temp { get; set; }
        public PlotModel Press { get; set; }
        public PlotModel Humid { get; set; }

        public ButtonCommand StartButtonTemp { get; set; }
        public ButtonCommand StopButtonTemp { get; set; }

        public ButtonCommand StartButtonPress { get; set; }
        public ButtonCommand StopButtonPress { get; set; }

        public ButtonCommand StartButtonHumid { get; set; }
        public ButtonCommand StopButtonHumid { get; set; }

        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimerTemp;
        private Timer RequestTimerPress;
        private Timer RequestTimerHumid;
        private IoTServer Server;

        
        #endregion

        public EnvViewModel()
        {
            Temp = new PlotModel { Title = "Temperature" };
            Press = new PlotModel { Title = "Pressure" };
            Humid = new PlotModel { Title = "Humidity" };


            Temp.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            Temp.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -40,
                Maximum = 150,
                Key = "Vertical",
                Unit = "*C",
                Title = "Temperature"
            });

            Press.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            Press.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 200,
                Maximum = 1600,
                Key = "Vertical",
                Unit = "mbar",
                Title = "Pressure"
            });

            Humid.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            Humid.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 100,
                Key = "Vertical",
                Unit = "%",
                Title = "Humidity"
            });



            Temp.Series.Add(new LineSeries() { Title = "Temperature measurements", Color = OxyColor.Parse("#FFFF0000") });
            Press.Series.Add(new LineSeries() { Title = "Pressure measurements", Color = OxyColor.Parse("#FFFF0000") });
            Humid.Series.Add(new LineSeries() { Title = "Humidity measurements", Color = OxyColor.Parse("#FFFF0000") });
    

            StartButtonTemp = new ButtonCommand(StartTimerTemp);
            StopButtonTemp = new ButtonCommand(StopTimerTemp);

            StartButtonPress = new ButtonCommand(StartTimerPress);
            StopButtonPress = new ButtonCommand(StopTimerPress);

            StartButtonHumid = new ButtonCommand(StartTimerHumid);
            StopButtonHumid = new ButtonCommand(StopTimerHumid);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;
            maxSampleNumber = config.MaxSampleNumber;

            Server = new IoTServer(ipAddress);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePlotTemp(double t, double d)
        {
            LineSeries lineSeries = Temp.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                Temp.Axes[0].Minimum = (t - config.XAxisMax);
                Temp.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            Temp.InvalidatePlot(true);
        }

        private void UpdatePlotPress(double t, double d)
        {
            LineSeries lineSeries = Press.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                Press.Axes[0].Minimum = (t - config.XAxisMax);
                Press.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            Press.InvalidatePlot(true);
        }

        private void UpdatePlotHumid(double t, double d)
        {
            LineSeries lineSeries = Humid.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                Humid.Axes[0].Minimum = (t - config.XAxisMax);
                Humid.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            Humid.InvalidatePlot(true);
        }


        /**
          * @brief Asynchronous chart update procedure with
          *        data obtained from IoT server responses.
          * @param ip IoT server IP address.
          */
        private async void UpdatePlotWithServerResponseTemp()
        {
#if CLIENT
#if GET
            string responseText = await Server.GETwithClient();

#else
            string responseText = await Server.POSTwithClient();
           
#endif
#else
#if GET
            string responseText = await Server.GETwithRequest();
            
#else
            string responseText = await Server.POSTwithRequest();
            
#endif
#endif
            try
            {
#if DYNAMIC
                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotTemp(timeStamp / 1000.0, (double)responseJson.data);


#else
                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                Temp.DataGridTemp = responseJson.data;
                UpdatePlotTemp(timeStamp / 1000.0, responseJson.data);

#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);

                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }


        private async void UpdatePlotWithServerResponsePress()
        {
#if CLIENT
#if GET
            string responseText = await Server.GETwithClientPress();


#else
            string responseText = await Server.POSTwithClientPress();

#endif
#else
#if GET

            string responseText = await Server.GETwithRequestPress();

#else

            string responseText = await Server.POSTwithRequestPress();

#endif
#endif
            try
            {
#if DYNAMIC

                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotPress(timeStamp / 1000.0, (double)responseJson.data1);
#else

                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);

                UpdatePlotPress(timeStamp / 1000.0, responseJson.data1);

#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);

                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }

        private async void UpdatePlotWithServerResponseHumid()
        {
#if CLIENT
#if GET

            string responseText = await Server.GETwithClientHumid();

#else
            string responseText = await Server.POSTwithClientHumid();
#endif
#else
#if GET

            string responseText = await Server.GETwithREquestHumid();
#else

            string responseText = await Server.POSTwithRequestHumid();
#endif
#endif
            try
            {
#if DYNAMIC

                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotHumid(timeStamp / 1000.0, (double)responseJson.data2);

#else

                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlotHumid(timeStamp / 1000.0, responseJson.data2);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);
                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }


        /**
          * @brief Synchronous procedure for request queries to the IoT server.
          * @param sender Source of the event: RequestTimer.
          * @param e An System.Timers.ElapsedEventArgs object that contains the event data.
          */
        private void RequestTimerElapsedTemp(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseTemp();   
        }
        private void RequestTimerElapsedPress(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponsePress();
        }
        private void RequestTimerElapsedHumid(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseHumid();

        }
        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimerTemp()
        {
            if (RequestTimerTemp == null)
            {
                RequestTimerTemp = new Timer(config.SampleTime);
                RequestTimerTemp.Elapsed += new ElapsedEventHandler(RequestTimerElapsedTemp);
                RequestTimerTemp.Enabled = true;

                Temp.ResetAllAxes();

            }
        }

        private void StartTimerPress()
        {
            if (RequestTimerPress == null)
            {
                RequestTimerPress = new Timer(config.SampleTime);
                RequestTimerPress.Elapsed += new ElapsedEventHandler(RequestTimerElapsedPress);
                RequestTimerPress.Enabled = true;

                Press.ResetAllAxes();

            }
        }

        private void StartTimerHumid()
        {
            if (RequestTimerHumid == null)
            {
                RequestTimerHumid = new Timer(config.SampleTime);
                RequestTimerHumid.Elapsed += new ElapsedEventHandler(RequestTimerElapsedHumid);
                RequestTimerHumid.Enabled = true;

                Humid.ResetAllAxes();

            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimerTemp()
        {
            if (RequestTimerTemp != null)
            {
                RequestTimerTemp.Enabled = false;
                RequestTimerTemp = null;
            }
        }

        private void StopTimerPress()
        {
            if (RequestTimerPress != null)
            {
                RequestTimerPress.Enabled = false;
                RequestTimerPress = null;
            }
        }

        private void StopTimerHumid()
        {
            if (RequestTimerHumid != null)
            {
                RequestTimerHumid.Enabled = false;
                RequestTimerHumid = null;
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

