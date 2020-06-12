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
using RpiApp.Model;
using System.Windows.Controls;

namespace RpiApp.ViewModels
{
    using RpiApp.Properties;
    using Model;
    using System.Windows.Controls;
    using System.Windows.Media;
    using RpiApp.Views;

    public class LEDViewModel 
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
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion

        /* BEGIN Colors */

        // Maybe I can use Color here immediatelly 

        int ledActiveColorA; // Active color Alpha components
        int ledActiveColorR = 0x00; // Active color Red components
        int ledActiveColorG = 0x00; // Active color Green components
        int ledActiveColorB = 0x00; // Active color Blue components

        Slider redsliker, blueslider, greenslider;
        Button colorview;


        Color ledOffColor = (Color)ColorConverter.ConvertFromString("#FFAAAAAA"); // LED-is-off color in Int ARGB format
        Color ledActiveColor => ledOffColor; // Active color in Int ARGB format


        List<Color> ledOffColorLst = ledOffColor; // LED-is-off color in Int ARGB format //FIX


        public LEDViewModel()
        {
            ipAddress = config.IpAddress;
            Server = new IoTServer(IpAddress);

            

            
            /* END Colors */

            /* BEGIN Request */
            Dictionary<String, String> paramsClear = new Dictionary<String, String>(); // HTTP POST data: clear display command

            for(int i= 0; i<8; i++)
            {
                for(int j=0; i<8; i++)
                {
                    String data = "[" + Convert.ToString(i) + "," + Convert.ToString(j) + ",0,0,0]";
                    paramsClear.Add(ledIndexToTag(i, j), data);
                }
            }

        }

        int?[,,] ledDisplayModel = new int?[8, 8, 3]; // LED display data model




        String ledIndexToTag(int x, int y)
        {
            return "LED" + Convert.ToString(x) + Convert.ToString(y);
        }

        public int argbToInt(int _a, int _r, int _g, int _b)
        {
            return (_a & 0xff) << 24 | (_r & 0xff) << 16 | (_g & 0xff) << 8 | (_b & 0xff);
        }

        public List<object> intToRGB(int argb)
        {
            int _r = (argb >> 16) & 0xff;
            int _g = (argb >> 8) & 0xff;
            int _b = argb & 0xff;
            List<object> rgb = new List<object>(3);
            rgb.Insert(0, _r);
            rgb.Insert(1, _g);
            rgb.Insert(2, _b);
            return rgb;
        }

        List<object> ledTagToIndex(String tag)
        {
            List<object> lst = new List<object>(2);
            lst.Insert(0, Char.GetNumericValue(tag[3]));
            lst.Insert(1, Char.GetNumericValue(tag[4]));
            return lst;
        }

        String ledIndexToJsonData(int x, int y)
        {
            String _x = Convert.ToString(x);
            String _y = Convert.ToString(y);
            String _r = Convert.ToString(ledDisplayModel[x,y,0]);
            String _g = Convert.ToString(ledDisplayModel[x,y,1]);
            String _b = Convert.ToString(ledDisplayModel[x,y,2]);
            return "[" + _x + "," + _y + "," + _r + "," + _g + "," + _b + "]";
        }

        bool ledColorNotNull(int x, int y)
        {
            return !((ledDisplayModel[x,y,0] == null) || (ledDisplayModel[x,y,1] == null) || (ledDisplayModel[x,y,2] == null));
        }

        public void clearDisplayModel()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ledDisplayModel[i,j,0] = null;
                    ledDisplayModel[i,j,1] = null;
                    ledDisplayModel[i,j,2] = null;
                }
            }
        }

        public void changeLedIndicatorColor(Button b)
        {
            // Set active color as background
            b.Background = ledActiveColor; // FIX 
            // Find element x-y position
            String tag = (String)b.Tag();
            List<object> index = ledTagToIndex(tag);
            int x = (int)index[0];
            int y = (int)index[1];
            //Update LED display data model
            ledDisplayModel[x,y,0] = ledActiveColorR;
            ledDisplayModel[x,y,1] = ledActiveColorG;
            ledDisplayModel[x,y,2] = ledActiveColorB;
        }

        public void clearAllLed(Button b)
        {
            // Clear LED display GUI
            Grid tb = (Grid)(Tag.ledMatrix); // FIX
            Button ledInd;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ledInd = tb.ledIndexToTag(i, j); // FIX
                    ledInd.Background = ledOffColor; // FIX
                }
            }
            // Clear LED display data model
            clearDisplayModel();

            sendClearRequest();
        }

        public Dictionary<String, String> getDisplayControlParams()
        {
            String led;
            String color;
            Dictionary<String, String> paramss = new Dictionary<String, String>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ledColorNotNull(i, j))
                    {
                        led = ledIndexToTag(i, j);
                        color = ledIndexToJsonData(i, j);
                        paramss.Add(led, color);
                    }
                }
            }
            return paramss;
        }

        // Send Control Request ?
        public void sendControlRequest(Button b)
        {

        }

        // Send Clear Request ?
        void sendClearRequest()
        {

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
