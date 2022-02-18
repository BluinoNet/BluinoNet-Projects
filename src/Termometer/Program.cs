using BluinoNet;
using System;
using System.Diagnostics;
using System.Threading;

namespace Termometer
{
    public class Program
    {
        static double CurrentTemperature, MaximumTemperature, MinimumTemperature, TemperaturePosition;
        static ESP32StarterKit board;
        public static void Main()
        {
            StarterKit.Setup(board);
            CurrentTemperature = StarterKit.TemperatureSensor.ReadTemperature();
            MinimumTemperature = CurrentTemperature;
            MaximumTemperature = CurrentTemperature;

            while (true)
            {
                CurrentTemperature = StarterKit.TemperatureSensor.ReadTemperature();
                if (CurrentTemperature > MaximumTemperature)
                    MaximumTemperature = CurrentTemperature;
                if (CurrentTemperature < MinimumTemperature)
                    MinimumTemperature = CurrentTemperature;

                StarterKit.Display.Clear();

                StarterKit.Display.DrawString("Current",StarterKit.colorBlue,39, 0);
                StarterKit.Display.DrawString(CurrentTemperature.ToString("F1"),StarterKit.colorBlue,37, 12);

                StarterKit.Display.DrawString("Minimum",StarterKit.colorBlue,2, 34);
                StarterKit.Display.DrawString(MinimumTemperature.ToString("F1"),StarterKit.colorBlue,0, 46);

                StarterKit.Display.DrawString("Maximum",StarterKit.colorBlue,72, 34);
                StarterKit.Display.DrawString(MaximumTemperature.ToString("F1"),StarterKit.colorBlue,70, 46);

                if ((MaximumTemperature - MinimumTemperature) > 0)
                {
                    TemperaturePosition = (CurrentTemperature - MinimumTemperature) / (MaximumTemperature - MinimumTemperature) * 127;
                    StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 63, (int)TemperaturePosition, 63);
                    StarterKit.Display.SetPixel( 0, 62,StarterKit.colorBlue);
                    StarterKit.Display.SetPixel(127, 62,StarterKit.colorBlue);

                    StarterKit.LightBulb.SetColorRGB((int)(TemperaturePosition / 32), 0, (int)((127 - TemperaturePosition) / 16));
                }

                StarterKit.Display.Flush();
                Thread.Sleep(250);
            }
        }
    }
}
