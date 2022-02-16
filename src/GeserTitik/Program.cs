using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace GeserTitik
{
    public class Program
    {
        static ESP32StarterKit board = board = new ESP32StarterKit();
        static uint colorBlue = 0;
        static uint colorWhite = 0;
        static void ClearPart(SSD1306Imp screen, int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    screen.SetPixel(ax, ay, colorWhite);
        }
        public static void Main()
        {
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            //speaker
            board.SetupDisplay();
            var screen = board.BoardDisplay;
            screen.DrawString("Geser: Gambar - D: Hapus", colorBlue,0, 57);
            screen.DrawLine(colorBlue,0, 55, 127, 55);
            int x = 64, y = 32;
            const double ACC_TOLERANCE = 20;
            board.SetupMpu6050();
            board.BoardMpu6050.StartUpdating();
            double accelX = 0;
            double accelY = 0;

            board.BoardMpu6050.SensorInterruptEvent += (a, e) =>
            {
                for (int i = 0; i < e.Values.Length; i++)
                {
                    accelX = e.Values[i].AccelerationX;
                    accelY = e.Values[i].AccelerationY;
                    break;
                }
                if (accelY > -ACC_TOLERANCE) y--;
                if (accelY < ACC_TOLERANCE) y++;
                if (accelX > ACC_TOLERANCE) x++;
                if (accelX < -ACC_TOLERANCE) x--;

                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x > 127) x = 127;
                if (y > 50) y = 50;

                screen.SetPixel(x, y,colorWhite);
                screen.Flush();

                if (board.BoardButton1.Pressed)
                    ClearPart(screen,0, 0, 128, 55);

                Thread.Sleep(20);

                screen.SetPixel(x, y,colorBlue);
                screen.Flush();
            };
        }
    }
}
