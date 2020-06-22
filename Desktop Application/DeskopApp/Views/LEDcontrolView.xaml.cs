using RpiApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using RpiApp.ViewModels;

namespace RpiApp.Views
{
    /// <summary>
    /// Interaction logic for LEDcontrolView.xaml
    /// </summary>
    public partial class LEDcontrolView : UserControl
    {
        public LEDcontrolView()
        {
            InitializeComponent();
        }
        
        public void changeColor(Button button)
        {
            (DataContext as LEDViewModel).colorChangesNEW(LEDchange, button);
        }  
        private void changeLedIndicatiorColor(object sender, RoutedEventArgs e)
        {
            changeColor(sender as Button);
        }
        public void clearAllLed(object sender, RoutedEventArgs e)
        {
            // Clear LED display GUI
            foreach (Button button in ledMatrix.Children.OfType<Button>())
            {
                button.Background = (Brush)new BrushConverter().ConvertFrom("#FFAAAAAA");
            }
            (DataContext as LEDViewModel).offColor();
            (DataContext as LEDViewModel).sendControlRequest();
        }

    }
}
