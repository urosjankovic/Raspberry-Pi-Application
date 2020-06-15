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




namespace RpiApp.ViewModels
{
    using RpiApp.Properties;
    using Model;
    using System.Windows.Controls;
    using System.Windows.Media;
    using RpiApp.Views;
    using RpiApp.ViewModel;
    using System.Security.Cryptography.X509Certificates;

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
        #endregion

        
        /* BEGIN Colors */

        public ButtonCommand SendData { get; set; }

        public ButtonCommand ClearButton { get; set; }

        public LEDViewModel()
        {
            ipAddress = config.IpAddress;
            Server = new IoTServer(IpAddress);

            SendData = new ButtonCommand(sendControlRequest);

        }

        
        Dictionary<String, List<int>> paramsss = new Dictionary<String, List<int>>();
        byte rr, gg, bb;
        Color cc;
        SolidColorBrush colorBrush;


        public void colorChanges(Slider seekR, Slider seekG, Slider seekB, Button button)
        {
            List<int> ledColors = new List<int>();
            rr = (byte)seekR.Value;
            gg = (byte)seekG.Value;
            bb = (byte)seekB.Value;

            cc = Color.FromRgb(rr, gg, bb); //Create object of Color class.
            colorBrush = new SolidColorBrush(cc); //Creating object of SolidColorBruch class.
            button.Background = colorBrush; //Setting background of a button.

            ledColors.Add(rr);
            ledColors.Add(gg);
            ledColors.Add(bb);

            paramsss.Add(button.Name, ledColors);

            
        }
        
        public void offColor()
        {
            paramsss.Clear();
        }


//        private async void UpdateDataLED()
//        {

//            string responseText = await Server.POSTwithRequestLED();

//            try
//            {
//#if DYNAMIC
//                dynamic responseJson = JObject.Parse(responseText);

//#else

//                ServerData responseJson = JsonConvert.SerializeObject<ServerData>(responseText);

//#endif
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine("JSON DATA ERROR");
//                Debug.WriteLine(responseText);
//                Debug.WriteLine(e);
//            }
//        }



        public void sendControlRequest()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.26/web_app/ledControl/ledmatrix.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(paramsss);

                streamWriter.Write(json);

                Console.WriteLine(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
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
