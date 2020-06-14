using RpiApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Diagnostics;


namespace RpiApp.Views
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RpiApp.Model;
    using RpiApp.ViewModel;
    using RpiApp.ViewModels;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Interaction logic for EnvVieww.xaml
    /// </summary>
    public partial class EnvVieww : UserControl
    {
        public EnvVieww()
        {
            InitializeComponent();
        }

        public void EnvTable()
        {
            DataTable dt = new DataTable();
            DataColumn tempp = new DataColumn("Temperature", typeof(string));
            DataColumn presss = new DataColumn("Pressure", typeof(string));
            DataColumn humidd = new DataColumn("Humidity", typeof(string));

            string url = "http://192.168.1.26/web_app/measurements/tempValues.json";
         
            dt.Columns.Add(tempp);
            dt.Columns.Add(presss);
            dt.Columns.Add(humidd);

            DataRow firstRow = dt.NewRow();
            firstRow[0] = "*C";
            firstRow[1] = "mmHg";
            firstRow[2] = "%";

            dt.Rows.Add(firstRow);

            DataGridEnv.ItemsSource = dt.DefaultView;
        }

        private void DataGridEnv_Loaded(object sender, RoutedEventArgs e)
        {
            this.EnvTable();
        }
    }
}
