using RpiApp.ViewModels;
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

namespace RpiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMenuVisible = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnvData_click(object sender, RoutedEventArgs e)
        {
            DataContext = new EnvViewModel();
        }

        private void Angles_click(object sender, RoutedEventArgs e)
        {
            DataContext = new AnglesViewModel();
        }


        private void LEDBtn_click(object sender, RoutedEventArgs e)
        {
            DataContext = new LEDViewModel();
        }

        private void JoyBtn_click(object sender, RoutedEventArgs e)
        {
            DataContext = new JoyViewModel();
        }

    }
}
