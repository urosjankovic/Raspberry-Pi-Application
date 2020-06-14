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

    /** 
      * @brief View model for AnglesView.xaml 
      */
    public class AnglesViewModel
    {
        #region Properties
        private string ipAddress;

        private int sampleTime; 

        private int maxSampleNumber;
        

        public PlotModel RPY { get; set; }

        public PlotModel RPY1 { get; set; }

        public PlotModel RPY2 { get; set; }

        public ButtonCommand StartButtonRPY { get; set; }
        public ButtonCommand StopButtonRPY { get; set; }

        public ButtonCommand StartButtonRPY1 { get; set; }
        public ButtonCommand StopButtonRPY1 { get; set; }

        public ButtonCommand StartButtonRPY2 { get; set; }
        public ButtonCommand StopButtonRPY2 { get; set; }



        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimerRPY;
        private Timer RequestTimerRPY1;
        private Timer RequestTimerRPY2;
        private IoTServer Server;
        #endregion

        public AnglesViewModel()
        {
            RPY = new PlotModel { Title = "RPY Accelerometer" };
            RPY1 = new PlotModel { Title = "xyz Magnetic" };
            RPY2 = new PlotModel { Title = "RPY Gyroscope" };

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
                Maximum = 360,
                Key = "Vertical",
                Unit = "degrees",
                Title = "RPY"
            });

            RPY1.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });

            RPY1.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -50,
                Maximum = 50,
                Key = "Vertical",
                Unit = "degrees",
                Title = "RPY1"
            });

            RPY2.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });

            RPY2.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                Key = "Vertical",
                Unit = "degrees",
                Title = "RPY2"
            });

            RPY.Series.Add(new LineSeries() { Title = "R", Color = OxyColor.Parse("#0000FF") });
            RPY.Series.Add(new LineSeries() { Title = "P", Color = OxyColor.Parse("#FFFF0000") });
            RPY.Series.Add(new LineSeries() { Title = "Y", Color = OxyColor.Parse("#00FF00") });

            RPY1.Series.Add(new LineSeries() { Title = "x", Color = OxyColor.Parse("#0000FF") });
            RPY1.Series.Add(new LineSeries() { Title = "y", Color = OxyColor.Parse("#FFFF0000") });
            RPY1.Series.Add(new LineSeries() { Title = "z", Color = OxyColor.Parse("#00FF00") });

            RPY2.Series.Add(new LineSeries() { Title = "R", Color = OxyColor.Parse("#0000FF") });
            RPY2.Series.Add(new LineSeries() { Title = "P", Color = OxyColor.Parse("#FFFF0000") });
            RPY2.Series.Add(new LineSeries() { Title = "Y", Color = OxyColor.Parse("#00FF00") });

            StartButtonRPY = new ButtonCommand(StartTimerRPY);
            StopButtonRPY = new ButtonCommand(StopTimerRPY);

            StartButtonRPY1 = new ButtonCommand(StartTimerRPY1);
            StopButtonRPY1 = new ButtonCommand(StopTimerRPY1);

            StartButtonRPY2 = new ButtonCommand(StartTimerRPY2);
            StopButtonRPY2 = new ButtonCommand(StopTimerRPY2);


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
                RPY.Axes[2].Maximum = t + config.SampleTime / 1000.0; 
            }

            RPY.InvalidatePlot(true);
        }

        private void UpdatePlotR1(double t, double d)
        {
            LineSeries lineSeries = RPY1.Series[0] as LineSeries;


            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY1.Axes[0].Minimum = (t - config.XAxisMax);
                RPY1.Axes[0].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY1.InvalidatePlot(true);
        }

        private void UpdatePlotP1(double t, double d)
        {
            LineSeries lineSeries = RPY1.Series[1] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY1.Axes[1].Minimum = (t - config.XAxisMax);
                RPY1.Axes[1].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY1.InvalidatePlot(true);
        }

        private void UpdatePlotY1(double t, double d)
        {
            LineSeries lineSeries = RPY1.Series[2] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                RPY1.Axes[2].Minimum = (t - config.XAxisMax);
                RPY1.Axes[2].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY1.InvalidatePlot(true);
        }

        private void UpdatePlotR2(double t, double d)
        {
            LineSeries lineSeries = RPY2.Series[0] as LineSeries;


            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY2.Axes[0].Minimum = (t - config.XAxisMax);
                RPY2.Axes[0].Maximum = t + config.SampleTime / 1000.0;

            }

            RPY2.InvalidatePlot(true);
        }

        private void UpdatePlotP2(double t, double d)
        {
            LineSeries lineSeries = RPY2.Series[1] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);


            if (t >= config.XAxisMax)
            {
                RPY2.Axes[1].Minimum = (t - config.XAxisMax);
                RPY2.Axes[1].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY2.InvalidatePlot(true);
        }

        private void UpdatePlotY2(double t, double d)
        {
            LineSeries lineSeries = RPY2.Series[2] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                RPY2.Axes[2].Minimum = (t - config.XAxisMax);
                RPY2.Axes[2].Maximum = t + config.SampleTime / 1000.0;
            }

            RPY2.InvalidatePlot(true);
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

        private async void UpdatePlotWithServerResponseRPY1()
        {
#if CLIENT
#if GET

            string responseText = await Server.GETwithClientRPY1();

#else
            string responseText = await Server.POSTwithClientRPY1();
#endif
#else
#if GET

            string responseText = await Server.GETwithREquestRPY1();
#else

            string responseText = await Server.POSTwithRequestRPY1();
#endif
#endif
            try
            {
#if DYNAMIC

                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotR1(timeStamp / 1000.0, (double)responseJson.x);
                UpdatePlotP1(timeStamp / 1000.0, (double)responseJson.y);
                UpdatePlotY1(timeStamp / 1000.0, (double)responseJson.z);

#else

                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlotR1(timeStamp / 1000.0, responseJson.x);
                UpdatePlotP1(timeStamp / 1000.0, responseJson.y);
                UpdatePlotY1(timeStamp / 1000.0, responseJson.z);
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

        private async void UpdatePlotWithServerResponseRPY2()
        {
#if CLIENT
#if GET

            string responseText = await Server.GETwithClientRPY2();

#else
            string responseText = await Server.POSTwithClientRPY2();
#endif
#else
#if GET

            string responseText = await Server.GETwithREquestRPY2();
#else

            string responseText = await Server.POSTwithRequestRPY2();
#endif
#endif
            try
            {
#if DYNAMIC

                dynamic responseJson = JObject.Parse(responseText);
                UpdatePlotR2(timeStamp / 1000.0, (double)responseJson.roll);
                UpdatePlotP2(timeStamp / 1000.0, (double)responseJson.pitch);
                UpdatePlotY2(timeStamp / 1000.0, (double)responseJson.yaw);

#else

                ServerData responseJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlotR2(timeStamp / 1000.0, responseJson.roll);
                UpdatePlotP2(timeStamp / 1000.0, responseJson.pitch);
                UpdatePlotY2(timeStamp / 1000.0, responseJson.yaw);
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

        private void RequestTimerElapsedRPY1(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseRPY1();
        }

        private void RequestTimerElapsedRPY2(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponseRPY2();
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
        private void StartTimerRPY1()
        {
            if (RequestTimerRPY1 == null)
            {
                RequestTimerRPY1 = new Timer(config.SampleTime);
                RequestTimerRPY1.Elapsed += new ElapsedEventHandler(RequestTimerElapsedRPY1);
                RequestTimerRPY1.Enabled = true;

                RPY1.ResetAllAxes();
            }
        }
        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimerRPY1()
        {
            if (RequestTimerRPY1 != null)
            {
                RequestTimerRPY1.Enabled = false;
                RequestTimerRPY1 = null;
            }
        }

        private void StartTimerRPY2()
        {
            if (RequestTimerRPY2 == null)
            {
                RequestTimerRPY2 = new Timer(config.SampleTime);
                RequestTimerRPY2.Elapsed += new ElapsedEventHandler(RequestTimerElapsedRPY2);
                RequestTimerRPY2.Enabled = true;

                RPY2.ResetAllAxes();
            }
        }
        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimerRPY2()
        {
            if (RequestTimerRPY2 != null)
            {
                RequestTimerRPY2.Enabled = false;
                RequestTimerRPY2 = null;
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