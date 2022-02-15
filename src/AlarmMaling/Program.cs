using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace AlarmMaling
{
    public class Program
    {
        static ESP32StarterKit board = board = new ESP32StarterKit();
        public static void Main()
        {
            double x=0, y=0, z=0, sensitivity;

            //rgb light
            board.SetupLedRgb(ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05);

            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            //button left and right
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);


            board.SetupDisplay();
            var screen = board.BoardDisplay;
            var colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);

            screen.Clear();
            screen.DrawString("BluinoNet", colorBlue, 1, 1, 1, 1);
            screen.DrawString("Alarm Maling", colorBlue, 1, 20, 1, 1);
            screen.DrawString("--By BMC--", colorBlue, 1, 40, 1, 1);


            screen.Flush();

            Thread.Sleep(2000);

            screen.DrawString("Alarm",colorBlue, 16, 0,2,2);
            screen.DrawString( "Maling",colorBlue, 34, 18,2,2);
            screen.DrawString( "To arm alarm place on", colorBlue, 0, 37);
            screen.DrawString( "surface and press the",colorBlue, 0, 47);
            screen.DrawString("left button.",colorBlue, 0, 57);
            screen.Flush();

            while (!board.BoardButton1.Pressed)
                Thread.Sleep(20);

            for (var CountDown = 10; CountDown > 0; CountDown -= 1)
            {
                screen.Clear();
                screen.DrawString( "Arming...",colorBlue, 10, 0);
                screen.DrawString( CountDown.ToString(),colorBlue, 59, 30,2,2);
                screen.Flush();
                Thread.Sleep(500);
            }
            board.SetupMpu6050();
            board.BoardMpu6050.StartUpdating();
            double accelX = 0;
            double accelY = 0;
            double accelZ = 0;

            board.BoardMpu6050.SensorInterruptEvent += (a, e) =>
            {
                //screen.Clear();
                for (int i = 0; i < e.Values.Length; i++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        x = e.Values[i].AccelerationX;
                        y = e.Values[i].AccelerationY;
                        z = e.Values[i].AccelerationZ;
                    }
                    else
                    {
                        accelX = e.Values[i].AccelerationX;
                        accelY = e.Values[i].AccelerationY;
                        accelZ = e.Values[i].AccelerationZ;
                    }
                    break;
                }
            };

            
            sensitivity = 0.035;

            screen.Clear();
            screen.DrawString( "Armed",colorBlue, 35, 22);
            screen.Flush();

            while ((Math.Abs(accelX - x) < sensitivity & Math.Abs(accelY - y) < sensitivity & Math.Abs(accelZ - z) < sensitivity))
                Thread.Sleep(20);

            screen.Clear();
            screen.DrawString("ALARM", colorBlue, 35, 22);
            screen.Flush();
         

            while (true)
            {
                for (var frequency = 1200; frequency <= 3200; frequency += 160)
                {
                    board.BoardBuzzer.Out(frequency,50,8);

                    LightColorChanger();
                }

                for (var frequency = 3200; frequency >= 1200; frequency += -160)
                {
                    board.BoardBuzzer.Out(frequency, 50, 8);
                    

                    LightColorChanger();
                }
            }
        }

        public static void LightColorChanger()
        {
            var lightTimer = 0;

            lightTimer += 1;

            if (lightTimer == 15)
                board.BoardLedRgb.SetColorRGB(0,0,200);

            if (lightTimer == 30)
            {
                board.BoardLedRgb.SetColorRGB(200, 0, 0);
                lightTimer = 0;
            }
        }
    }
}
