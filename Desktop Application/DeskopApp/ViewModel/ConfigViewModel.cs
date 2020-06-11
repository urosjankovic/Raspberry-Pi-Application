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
    using RpiApp.Views;

    public class ConfigViewModel : INotifyPropertyChanged
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

        private int maxSampleNumber;
        public string MaxSampleNumber
        {
            get
            {
                return maxSampleNumber.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (maxSampleNumber != st)
                    {
                        maxSampleNumber = st;
                        OnPropertyChanged("MaxSampleNumber");
                    }
                }
            }
        }

        public ButtonCommand UpdateConfigButton { get; set; }
        public ButtonCommand DefaultConfigButton { get; set; }
        #endregion

        #region Fields
        private ConfigParams config = new ConfigParams();
        private IoTServer Server;
        #endregion

        public ConfigViewModel()
        {
            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);
            
            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;
            maxSampleNumber = config.MaxSampleNumber;

            Server = new IoTServer(IpAddress);
        }

        /**
          * @brief Configuration parameters update
          */
        #region ButtonCommands
        private void UpdateConfig()
        {

            config = new ConfigParams(ipAddress, sampleTime, maxSampleNumber);
            Server = new IoTServer(IpAddress);

        }


        /**
          * @brief Configuration parameters defualt values
          */
        private void DefaultConfig()
        {

            config = new ConfigParams();
            IpAddress = config.IpAddress;
            SampleTime = config.SampleTime.ToString();
            MaxSampleNumber = config.MaxSampleNumber.ToString();
            Server = new IoTServer(IpAddress);

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
