using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace ColorMixer
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
        static void Beep()
        {
            board.BoardBuzzer.Out(100, 50, 50);
        }
        public static void Main()
        {
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            board.SetupTouchSensor(ESP32Pins.IO13);
            board.SetupLedRgb(ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05);
            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupDisplay();
            int selection = 0;
            int[] LightBulbColor = new int[3];
            var screen = board.BoardDisplay;
            screen.Clear();
            screen.DrawString("Up/Down to select", colorBlue, 0, 45,  1, 1);
            screen.DrawString("Right/Left to level", colorBlue, 0, 55,  1, 1);
            screen.DrawString("Red  : 0.0", colorBlue, 12, 9 * 0,  1, 1);
            screen.DrawString("Green: 0.0", colorBlue,  12, 9 * 1,  1, 1);
            screen.DrawString("Blue : 0.0", colorBlue, 12, 9 * 2,  1, 1);

            while (true)
            {
                // The selectino pointer
                screen.DrawString(" ", colorBlue, 0, selection * 9,  2, 1);

                if (board.BoardButton2.Pressed || selection == -1)
                {
                    Beep();
                    selection++;
                    if (selection >= 3)
                        selection = 0;
                    while (board.BoardButton2.Pressed)
                        Thread.Sleep(20);
                }

                if (board.BoardButton1.Pressed || selection == -1)
                {
                    Beep();
                    selection--;
                    if (selection < 0)
                        selection = 2;
                    while (board.BoardButton1.Pressed)
                        Thread.Sleep(20);
                }
                screen.DrawString(">", colorBlue, 0, selection * 9,  2, 1);

                // Change the color level
                if (board.BoardTouchSensor.IsTouched())
                {
                    LightBulbColor[selection] += 1;
                    if (LightBulbColor[selection] > 255) LightBulbColor[selection] = 0;
                    ClearPart(screen, 60, 9 * selection, 18, 8);
                    screen.DrawString(LightBulbColor[selection].ToString("N1"), colorBlue, 54, 9 * selection,  1, 1);
                }
                /*
                if (BrainPad.Buttons.IsLeftPressed())
                {
                    LightBulbColor[selection] -= 0.3;
                    if (LightBulbColor[selection] < 0) LightBulbColor[selection] = 0;
                    screen.ClearPart(60, 9 * selection, 18, 8);
                    screen.DrawString(54, 9 * selection, LightBulbColor[selection].ToString("N1"), 1, 1);
                }*/

                // Set the color and show the screen
                board.BoardLedRgb.SetColorRGB(LightBulbColor[0], LightBulbColor[1], LightBulbColor[2]);
                screen.Flush();
                Thread.Sleep(20);
            }
        }
    }
}
