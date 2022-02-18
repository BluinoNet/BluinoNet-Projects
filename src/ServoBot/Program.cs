using BluinoNet;
using System;
using System.Diagnostics;
using System.Threading;

namespace ServoBot
{
    class Program
    {
        static ESP32StarterKit board;
        public static void Main()
        {
            StarterKit.Setup(board);
            StarterKit.Servo.ConfigureAsPositional(false);
            StarterKit.Servo.ConfigurePulseParameters(0.5, 2.5);

            while (true)
            {
                StarterKit.Servo.Set(110);
                StarterKit.Display.Clear();
                StarterKit.Display.DrawString("Place ball and press",StarterKit.colorBlue,5, 10);
                StarterKit.Display.DrawString("down button", StarterKit.colorBlue,30, 25);
                StarterKit.Display.Flush();

                while (!StarterKit.Button2.Pressed)
                {
                    Thread.Sleep(20);
                }

                StarterKit.Servo.Set(50);
                Thread.Sleep(200);

                for (int i = 50; i < 111; i++)
                {
                    StarterKit.Servo.Set(i);
                    Thread.Sleep(14);
                }
            }
        }
    }
}
