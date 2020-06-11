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

namespace DesktopApp.ViewModel
{
    using DesktopApp.Properties;
    using Model;
    using System.Windows.Controls;
    

    /** 
      * @brief View model for MainWindow.xaml 
      */
    public class MainViewModel : INotifyPropertyChanged
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

        public PlotModel Temp { get; set; }
        public PlotModel Press { get; set; }
        public PlotModel Humid { get; set; }

        public PlotModel RPY { get; set; }

        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }

        public ButtonCommand UpdateConfigButton { get; set; }
        public ButtonCommand DefaultConfigButton { get; set; }
        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion

        public MainViewModel()
        {
            Temp = new PlotModel { Title = "Temperature" };
            Press = new PlotModel { Title = "Pressure" };
            Humid = new PlotModel { Title = "Humidity" };

            RPY = new PlotModel { Title = "RPY Angles" };


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


            Temp.Series.Add(new LineSeries() { Title = "Temperature measurements", Color = OxyColor.Parse("#FFFF0000") });
            Press.Series.Add(new LineSeries() { Title = "Pressure measurements", Color = OxyColor.Parse("#FFFF0000") });
            Humid.Series.Add(new LineSeries() { Title = "Humidity measurements", Color = OxyColor.Parse("#FFFF0000") });

            RPY.Series.Add(new LineSeries() { Title = "R angle", Color = OxyColor.Parse("#0000FF") });
            RPY.Series.Add(new LineSeries() { Title = "P angle", Color = OxyColor.Parse("#FFFF0000") });
            RPY.Series.Add(new LineSeries() { Title = "Y angle", Color = OxyColor.Parse("#00FF00") });

            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);

            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;

            Server = new IoTServer(IpAddress);
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

        /**
          * @brief Asynchronous chart update procedure with
          *        data obtained from IoT server responses.
          * @param ip IoT server IP address.
          */
        private async void UpdatePlotWithServerResponse()
        {
#if CLIENT
#if GET
            string responseText = await Server.GETwithClient();

#else
            string responseText1 = await Server.POSTwithClientPress();
           
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
                ServerData resposneJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlotTemp(timeStamp / 1000.0, resposneJson.data);

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

            string responseText1= await Server.GETwithRequestPress();

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
        private void RequestTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponse();
            UpdatePlotWithServerResponsePress();
            UpdatePlotWithServerResponseHumid();
            UpdatePlotWithServerResponseRPY();
        }
        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimer()
        {
            if (RequestTimer == null)
            {
                RequestTimer = new Timer(config.SampleTime);
                RequestTimer.Elapsed += new ElapsedEventHandler(RequestTimerElapsed);
                RequestTimer.Enabled = true;

                Temp.ResetAllAxes();

            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimer()
        {
            if (RequestTimer != null)
            {
                RequestTimer.Enabled = false;
                RequestTimer = null;
            }
        }
       
        /**
         * @brief Configuration parameters update
         */
        private void UpdateConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();
                

            config = new ConfigParams(ipAddress, sampleTime);
            Server = new IoTServer(IpAddress);

            if (restartTimer)
                StartTimer();
              

        }

        /**
          * @brief Configuration parameters defualt values
          */
        private void DefaultConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();
              

            config = new ConfigParams();
            IpAddress = config.IpAddress;
            SampleTime = config.SampleTime.ToString();
            Server = new IoTServer(IpAddress);

            if (restartTimer)
                StartTimer();
              

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




        ///* BEGIN colors */
        //int ledActiveColorA; /// Active color Alpha components
        //int ledActiveColorR = 0x00; /// Active color Red components
        //int ledActiveColorG = 0x00; /// Active color Green components
        //int ledActiveColorB = 0x00; /// Active color Blue components
        //int ledActiveColor; /// Active color in Int ARGB format
        //                                //FIX
        //int ledOffColor = ResourcesCompat.getColor(getResources(), R.color.ledIndBackground, null); /// LED-is-off color in Int ARGB format
        //                            //FIX
        //List<object> ledOffColorVec intToRGB(ledOffColor); /// LED-is-off color in Int ARGB format

        //                              //FIX
        //int[][][] ledDisplayModel = new int[8][8][3]; /// LED display data model TO BE FIXED
        ///* END colors */

        ///* BEGIN Request */
        //String url = "http://192.168.1.26/led_display.php"; /// Default IoT server script URL

        //private RequestQueue queue; /// HTTP requests queue

        //Dictionary<String, String> paramsClear = new Dictionary<String, String>(); /// HTTP POST data: clear display command

        ///* END Request */

        ////FIX
        //clearDisplayModel();
        ///* END Color data initialization */

        ///* BEGIN widgets initialization */

        ////FIX
        ///* BEGIN Widgets */
        //public Slider redSlider, blueSlider, greenSlider;
        //public Button colorView;
        ///* END Widgets */

        //Slider redSlider = (Slider)findViewbyID(R.id.seekR);

        /*public int argbToInt(int _a, int _r, int _g, int _b)
        {
            return (_a & 0xff) << 24 | (_r & 0xff) << 16 | (_g & 0xff) << 8 | (_b & 0xff);
        }*/

        /*public List<object> intToRGB(int argb)
        {
            int _r = (argb >> 16) & 0xff;
            int _g = (argb >> 8) & 0xff;
            int _b = argb & 0xff;
            List<object> rgb = new List<object>(3);
            rgb.Insert(0, _r);
            rgb.Insert(1, _g);
            rgb.Insert(2, _b);
            return rgb;
        }*/

        /*List<object> ledTagToIndex(string tag)
        {
            List<object> vec = new List<object>(2);
            vec.Insert(0, (int)char.GetNumericValue(tag[3]));
            vec.Insert(1, (int)char.GetNumericValue(tag[4]));
            return vec;
        }*/

        /*string ledindextojsondata(int x, int y)
        {
            string _x = convert.tostring(x);
            string _y = convert.tostring(y);
            string _r = convert.tostring(leddisplaymodel[x][y][0]);
            string _g = convert.tostring(leddisplaymodel[x][y][1]);
            string _b = convert.tostring(leddisplaymodel[x][y][2]);
            return "[" + _x + "," + _y + "," + _r + "," + _g + "," + _b + "]";
        }*/

        /*bool ledColorNotNull(int x, int y)
        {
            return !((ledDisplayModel[x][y][0] == null) || (ledDisplayModel[x][y][1] == null) || (ledDisplayModel[x][y][2] == null));
        }*/

        /*public void clearDisplayModel()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ledDisplayModel[i][j][0] = null;
                    ledDisplayModel[i][j][1] = null;
                    ledDisplayModel[i][j][2] = null;
                }
            }
        }*/

        /*internal string ledIndexToTag(int x, int y)
        {
            return "LED" + Convert.ToString(x) + Convert.ToString(y);
        }*/

        /*int seekBarUpdate(char color, int value)
        {
            switch (color)
            {
                case 'R':
                    ledActiveColorR = value;
                    break;
                case 'G':
                    ledActiveColorG = value;
                    break;
                case 'B':
                    ledActiveColorB = value;
                    break;
                default: // do nothing
                    break;
            }
            ledActiveColorA = (ledActiveColorR + ledActiveColorG + ledActiveColorB) / 3;
            return argbToInt(ledActiveColorA, ledActiveColorR, ledActiveColorG, ledActiveColorB);
        }*/

        /*public void changeLedIndicatorColor(Button v)
        {
            // Set active color as background
            v.Background = ledActiveColor;
            Find element x-y position
            string tag = (string)v.Tag;
            List<object> index = ledTagToIndex(tag);
            int x = (int)index[0];
            int y = (int)index[1];
            //Update LED display data model
            ledDisplayModel[x][y][0] = ledActiveColorR;
            ledDisplayModel[x][y][1] = ledActiveColorG;
            ledDisplayModel[x][y][2] = ledActiveColorB;
        }*/

        // FIX
        /*public void clearAllLed(Button v)
        {
            // Clear LED display GUI
            Grid tb = (Grid)findViewById(R.id.ledTable);
            Button ledInd;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ledInd = tb.findViewWithTag(ledIndexToTag(i, j));
                    ledInd.setBackgroundColor(ledOffColor);
                }
            }
            // Clear LED display data model
            clearDisplayModel();

            sendClearRequest();
        }*/

        /*public Dictionary<string, string> DisplayControlParams
        {
            get
            {
                string led;
                string color;
                Dictionary<string, string> @params = new Dictionary<string, string>();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (ledColorNotNull(i, j))
                        {
                            led = ledIndexToTag(i, j);
                            color = ledIndexToJsonData(i, j);
                            @params[led] = color;
                        }
                    }
                }
                return @params;
            }
        }*/


        #endregion
    }
}