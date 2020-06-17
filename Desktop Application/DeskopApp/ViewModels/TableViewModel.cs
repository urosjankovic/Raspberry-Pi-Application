using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace RpiApp.ViewModels
{
    using Model;
    using OxyPlot.Reporting;

    public class TableViewModel : INotifyPropertyChanged
    {

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private double _roll;
        public string Roll
        {
            get
            {
                return _roll.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _roll != tmp)
                {
                    _roll = tmp;
                    OnPropertyChanged("Data");
                }
            }
        }

        private double _pitch;
        public string Pitch
        {
            get
            {
                return _pitch.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _pitch != tmp)
                {
                    _pitch = tmp;
                    OnPropertyChanged("Data");
                }
            }
        }

        private double _yaw;
        public string Yaw
        {

            get
            {
                return _yaw.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _yaw != tmp)
                {
                    _yaw = tmp;
                    OnPropertyChanged("Data");
                }
            }
        }

        private double _data;
        public string Data
        {
            get
            {
                return _data.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _data != tmp)
                {
                    _data = tmp;
                    OnPropertyChanged("Data");
                }
            }
        }

        private string _unit;
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("Unit");
                }
            }
        }

        public TableViewModel(MeasurementModel model)
        {
            UpdateWithModel(model);
        }
        public class DataOri
        {
            public double Roll { get; set; }
            public double Pitch { get; set; }
            public double Yaw { get; set; }
        }

        public void UpdateWithModel(MeasurementModel model)
        {
            _name = model.Name;
            OnPropertyChanged("Name");
            _data = model.Data;
            OnPropertyChanged("Data");
            _unit = model.Unit;
            OnPropertyChanged("Unit");
            _roll = model.Roll;
            OnPropertyChanged("Roll");
            _pitch = model.Pitch;
            OnPropertyChanged("Pitch");
            _yaw = model.Yaw;
            OnPropertyChanged("Yaw");
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
