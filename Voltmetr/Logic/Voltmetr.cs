using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO.Pipes;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace Voltmetr.Logic
{
    public class VoltmetrEMU
    {
        ArrowManager arrow;
        bool isConnected = false;
        public bool Repeat = true;
        public VoltmetrEMU(RotateTransform angle)
        {
            arrow = new ArrowManager(angle);
        }
        public VoltmetrEMU(Canvas canvas, string voltmeter_img, string arrow_img)
        {
            CreateMeter(canvas, voltmeter_img, arrow_img);
        }
        public List<UIElement> CreateMeter(Canvas canvas, string voltmeter_img, string arrow_img)
        {
            const double base_height = 327;
            double mult = canvas.Height / base_height;
            List<UIElement> elements = new List<UIElement>();

            // Создаем изображение вольтметра
            Image voltmeterImage = new Image();
            voltmeterImage.Source = new BitmapImage(new Uri(voltmeter_img, UriKind.Relative));
            voltmeterImage.Width = 453*mult;
            voltmeterImage.Height = 327 * mult;
            Canvas.SetLeft(voltmeterImage, -10 * mult);
            voltmeterImage.HorizontalAlignment = HorizontalAlignment.Left;
            voltmeterImage.VerticalAlignment = VerticalAlignment.Center;
            elements.Add(voltmeterImage);

            // Создаем изображение стрелки
            Image arrowImage = new Image();
            arrowImage.Source = new BitmapImage(new Uri(arrow_img, UriKind.Relative));
            arrowImage.Width = 48 * mult;
            arrowImage.Height = 380 * mult;
            Canvas.SetLeft(arrowImage, 190 * mult);
            Canvas.SetTop(arrowImage, 30 * mult);
            arrowImage.HorizontalAlignment = HorizontalAlignment.Center;
            arrowImage.VerticalAlignment = VerticalAlignment.Top;

            // Создаем и настраиваем RenderTransform для поворота стрелки
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = -81;
            rotateTransform.CenterX = arrowImage.Width/2;
            rotateTransform.CenterY = arrowImage.Height/2;
            arrowImage.RenderTransform = rotateTransform;

            elements.Add(arrowImage);

            // Добавляем созданные элементы на холст
            foreach (UIElement element in elements)
            {
                canvas.Children.Add(element);
            }

            this.arrow = new ArrowManager(rotateTransform);
            return elements;
        }
    public void SetVoltage(double volt)
        {
            arrow.value = volt;
        }
        public async void SetMultiply(double val)
        {
            arrow.Multiply = val;
            await SetUnit(1 / val);
        }
        public async Task UpdateVoltage()
        {
            if (!isConnected)
            {
                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("voltmeterPipe", PipeDirection.In))
                {
                    isConnected = true;
                    await pipeServer.WaitForConnectionAsync(); // Ожидаем подключения внешнего приложения

                    // Читаем данные о напряжении
                    byte[] buffer = new byte[100];
                    int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);
                    string voltageData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Обновляем значение напряжения в вольтметре
                    arrow.value = Convert.ToDouble(voltageData);
                    isConnected = false;
                }
                if(Repeat)
                await UpdateVoltage();
            }
        }
        static async Task SetUnit(double unit)
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "voltmeterUnitPipe", PipeDirection.Out))
            {
                await pipeClient.ConnectAsync(); // Подключаемся к именованному каналу

                // Отправляем данные о напряжении
                string voltageData = unit.ToString(); // Пример значения напряжения
                byte[] buffer = Encoding.UTF8.GetBytes(voltageData);
                pipeClient.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
