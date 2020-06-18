using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;

namespace RpiApp.ViewModels
{
    using Model;
    public class TableViewModelOri : INotifyPropertyChanged
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
                    OnPropertyChanged("Roll");
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
                    OnPropertyChanged("Pitch");
                }
            }
        }

        private double _z;
        public string z
        {

            get
            {
                return _z.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _z != tmp)
                {
                    _z = tmp;
                    OnPropertyChanged("z");
                }
            }
        }

        private double _x;
        public string x
        {
            get
            {
                return _x.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _x != tmp)
                {
                    _x = tmp;
                    OnPropertyChanged("x");
                }
            }
        }

        private double _y;
        public string y
        {
            get
            {
                return _y.ToString("0.0####", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _y != tmp)
                {
                    _y = tmp;
                    OnPropertyChanged("y");
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
                    OnPropertyChanged("Yaw");
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


        public TableViewModelOri(MeasurementModelOri model)
        {
            UpdateWithModelOri(model);
        }

        public void UpdateWithModelOri(MeasurementModelOri model)
        {
            _name = model.Name;
            OnPropertyChanged("Name");
            _unit = model.Unit;
            OnPropertyChanged("Unit");
            _roll = model.Data.Roll;
            OnPropertyChanged("Roll");
            _pitch = model.Data.Pitch;
            OnPropertyChanged("Pitch");
            _yaw = model.Data.Yaw;
            OnPropertyChanged("Yaw");

            _x = model.Data.x;
            OnPropertyChanged("x");
            _y = model.Data.y;
            OnPropertyChanged("y");
            _z = model.Data.z;
            OnPropertyChanged("z");
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
