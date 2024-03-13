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
using Voltmetr.Logic;

namespace Voltmetr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VoltmetrEMU voltmetr;
        public MainWindow()
        {
            InitializeComponent();
            voltmetr = new VoltmetrEMU(ArrowAngle);
            voltmetr.SetVoltage(10);
            DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await voltmetr.UpdateVoltage();
        }
    }
}
