using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace Dadu
{
    public class Program
    {
        static ESP32StarterKit board = board = new ESP32StarterKit();
        const int Dice_Base_X = 55;
        const int Dice_Base_Y = 10;
        static void ClearPart(SSD1306Imp screen, int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    screen.SetPixel(ax, ay, colorWhite);
        }
        static void Beep()
        {
            board.BoardBuzzer.Out(100, 50, 50);
        }
        static uint colorBlue = 0;
        static uint colorWhite = 0;
        public static void Main()
        {
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            board.SetupTouchSensor(ESP32Pins.IO13);

            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupDisplay();
            var screen = board.BoardDisplay;
            var Rnd = new Random();
            while (true)
            {
                screen.DrawString("Shake or Up to roll", colorBlue, 10, 55);
                screen.DrawRectangle(colorBlue, Dice_Base_X - 5, Dice_Base_Y - 5, 31, 31);

                for (var i = 0; i < 100; i += 5)
                {
                    ShowDice(screen, Rnd.Next(6) + 1);
                    Beep();
                    screen.Flush();
                    Thread.Sleep(i);

                }
                board.SetupMpu6050();
                board.BoardMpu6050.StartUpdating();
                double accelX = 0;

                board.BoardMpu6050.SensorInterruptEvent += (a, e) =>
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        accelX = (e.Values[i].AccelerationX*100);

                        break;
                    }
                };
                while (accelX < 100 && board.BoardButton1.Pressed == false) Thread.Sleep(20);
                Thread.Sleep(20);
            }
        }

        static void ShowDice(SSD1306Imp screen, int num)
        {
            ClearPart(screen, Dice_Base_X + 3, Dice_Base_Y + 3, 16, 16);

            switch (num)
            {
                case 1:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 10, Dice_Base_Y + 10, 2);
                    break;

                case 2:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 15, 2);
                    break;

                case 3:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 10, Dice_Base_Y + 10, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 15, 2);
                    break;

                case 4:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 15, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 15, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 5, 2);
                    break;

                case 5:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 15, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 15, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 10, Dice_Base_Y + 10, 2);
                    break;

                case 6:
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 10, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 5, Dice_Base_Y + 15, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 5, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 10, 2);
                    screen.DrawCircle(colorBlue, Dice_Base_X + 15, Dice_Base_Y + 15, 2);
                    break;
            }
        }
    }
}
