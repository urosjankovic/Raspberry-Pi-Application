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
using System.Windows.Media;
using System.Net.Http;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RpiApp.Model;
using System.Windows.Controls;
using System.Web;
using System.Drawing;




namespace RpiApp.ViewModels
{
    using RpiApp.Properties;
    using Model;
    using System.Windows.Controls;
    using System.Windows.Media;
    using RpiApp.Views;
    using RpiApp.ViewModel;
    using System.Security.Cryptography.X509Certificates;
    using Xceed.Wpf.Toolkit;

    public class LEDViewModel : INotifyPropertyChanged
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

        private ConfigParams config = new ConfigParams();
        private IoTServer Server;

        private int sampleTime;
        private int maxSampleNumber;
        #endregion

        
        /* BEGIN Colors */

        public ButtonCommand SendData { get; set; }

        public ButtonCommand UpdateConfigLED { get; set; }
        public ButtonCommand DefaultConfigLED { get; set; }

        public ButtonCommand ClearButton { get; set; }

        public LEDViewModel()
        {
            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;
            maxSampleNumber = config.MaxSampleNumber;

            UpdateConfigLED = new ButtonCommand(UpdateLED);
            DefaultConfigLED = new ButtonCommand(DefaultLED);

            Server = new IoTServer(IpAddress);

            SendData = new ButtonCommand(sendControlRequest);

        }


        SolidColorBrush colorBrush;

        byte R, G, B;
        Color cc;

        Dictionary<String, List<int>> paramsLED = new Dictionary<String, List<int>>();

        public void colorChangesNEW(ColorPicker cLED, Button button)
        {

            List<int> ledcolorled = new List<int>();

            R = cLED.SelectedColor.Value.R;
            G = cLED.SelectedColor.Value.G;
            B = cLED.SelectedColor.Value.B;

            cc = Color.FromRgb(R, G, B);
            //Create object of Color class.
            colorBrush = new SolidColorBrush(cc); //Creating object of SolidColorBruch class.
            button.Background = colorBrush; //Setting background of a button.

            ledcolorled.Add(R);
            ledcolorled.Add(G);
            ledcolorled.Add(B);            

            paramsLED.Add(button.Name, ledcolorled);    

        }

        public void offColor()
        {
            paramsLED.Clear();
        }

        public void sendControlRequest()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.26/web_app/ledControl/ledmatrix.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(paramsLED);

                streamWriter.Write(json);

                Console.WriteLine(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        private void UpdateLED()
        {
            config = new ConfigParams(ipAddress, sampleTime, maxSampleNumber);
            Server = new IoTServer(IpAddress);
        }
        private void DefaultLED()
        {
            config = new ConfigParams();
            IpAddress = config.IpAddress;
            Server = new IoTServer(IpAddress);
        }

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
