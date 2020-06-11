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

    /** 
      * @brief View model for AnglesView.xaml 
      */
    public class AnglesViewModel
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
        public string SampleTime
        {
            get
            {
                return sampleTime.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (sampleTime != st)
                    {
                        sampleTime = st;
                        OnPropertyChanged("SampleTime");
                    }
                }
            }
        }

        public PlotModel RPY { get; set; }

        public ButtonCommand StartButtonRPY { get; set; }
        public ButtonCommand StopButtonRPY { get; set; }


        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimerRPY;
        private IoTServer Server;
        #endregion

        public AnglesViewModel()
        {
            RPY = new PlotModel { Title = "RPY Angles" };

            RPY.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });

            RPY.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 500,
                Key = "Vertical",
                Unit = "degrees",
                Title = "R"
            });

            RPY.Series.Add(new LineSeries() { Title = "R angle", Color = OxyColor.Parse("#0000FF") });
            RPY.Series.Add(new LineSeries() { Title = "P angle", Color = OxyColor.Parse("#FFFF0000") });
            RPY.Series.Add(new LineSeries() { Title = "Y angle", Color = OxyColor.Parse("#00FF00") });

            StartButtonRPY = new ButtonCommand(StartTimerRPY);
            StopButtonRPY = new ButtonCommand(StopTimerRPY);

            Server = new IoTServer(IpAddress);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePlotR(double t, double d)
        {
            LineSeries lineSeries = RPY.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY.Axes[0].Minimum = (t - config.XAxisMax);
                RPY.Axes[0].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY.InvalidatePlot(true);
        }

        private void UpdatePlotP(double t, double d)
        {
            LineSeries lineSeries = RPY.Series[1] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY.Axes[1].Minimum = (t - config.XAxisMax);
                RPY.Axes[1].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY.InvalidatePlot(true);
        }

        private void UpdatePlotY(double t, double d)
        {
            LineSeries lineSeries = RPY.Series[2] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                RPY.Axes[2].Minimum = (t - config.XAxisMax);
                RPY.Axes[2].Maximum = t + config.SampleTime / 1000.0; ;
            }

            RPY.InvalidatePlot(true);
        }

        private async void UpdatePlotWithServerResponseRPY()
        {
#if CLIENT
#if GET

            string responseText = await Server.GETwithClientRPY();

#else
            string responseText = await Server.POSTwithClientRPY();
#endif
#else
#if GET

            string responseText = await Server.GETwithREquestRPY();
#else

            string responseText = await Server.POSTwithRequestRPY();
#endif
#endif
            try
            {
#if DYNAMIC

                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotR(timeStamp / 1000.0, (double)responseJson.roll);
                UpdatePlotP(timeStamp / 1000.0, (double)responseJson.pitch);
                UpdatePlotY(timeStamp / 1000.0, (double)responseJson.yaw);

#else

                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlotR(timeStamp / 1000.0, responseJson.roll);
                UpdatePlotP(timeStamp / 1000.0, responseJson.pitch);
                UpdatePlotY(timeStamp / 1000.0, responseJson.yaw);
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
        private void RequestTimerElapsedRPY(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseRPY();
        }

        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimerRPY()
        {
            if (RequestTimerRPY == null)
            {
                RequestTimerRPY = new Timer(config.SampleTime);
                RequestTimerRPY.Elapsed += new ElapsedEventHandler(RequestTimerElapsedRPY);
                RequestTimerRPY.Enabled = true;

                RPY.ResetAllAxes();

            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimerRPY()
        {
            if (RequestTimerRPY != null)
            {
                RequestTimerRPY.Enabled = false;
                RequestTimerRPY = null;
            }
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