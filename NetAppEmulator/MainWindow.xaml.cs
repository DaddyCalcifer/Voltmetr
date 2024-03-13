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
using System.IO.Pipes;

namespace NetAppEmulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static async Task SetVoltage(double volt)
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "voltmeterPipe", PipeDirection.Out))
            {
                await pipeClient.ConnectAsync(); // Подключаемся к именованному каналу

                // Отправляем данные о напряжении
                string voltageData = volt.ToString(); // Пример значения напряжения
                byte[] buffer = Encoding.UTF8.GetBytes(voltageData);
                pipeClient.Write(buffer, 0, buffer.Length);
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Если не число, отменяем ввод
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SetVoltage(Convert.ToDouble(voltageText.Text));
        }
    }
}
