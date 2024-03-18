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
using System.Collections.ObjectModel;

namespace Voltmetr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VoltmetrEMU voltmetr;
        public ObservableCollection<string> RadioButtonItems { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            voltmetr = new VoltmetrEMU(ArrowAngle);
            voltmetr.SetMultiply(0f);
            voltmetr.SetVoltage(10);
            DataContext = this;
            RadioButtonItems = new ObservableCollection<string>
            {
                "Option 1",
                "Option 2",
                "Option 3",
                "Option 4"
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await voltmetr.UpdateVoltage();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            voltmetr.SetMultiply(2f);
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            voltmetr.SetMultiply(1f);
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            voltmetr.SetMultiply(0.5f);
        }

        private void RadioButton_Click_3(object sender, RoutedEventArgs e)
        {
            voltmetr.SetMultiply(0.25f);
        }

        private void RadioButton_Click_4(object sender, RoutedEventArgs e)
        {
            voltmetr.SetMultiply(0f);
        }
    }
}
