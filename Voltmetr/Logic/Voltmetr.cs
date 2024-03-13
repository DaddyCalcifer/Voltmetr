using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO.Pipes;

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
        public void SetVoltage(double volt)
        {
            arrow.value = volt;
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
    }
}
