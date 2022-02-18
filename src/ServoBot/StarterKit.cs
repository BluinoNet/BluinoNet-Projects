using BluinoNet;
using BluinoNet.Modules;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;

namespace ServoBot
{
    [DebuggerNonUserCode]
    public static class StarterKit
    {
        public static uint colorBlue = 0;
        public static uint colorWhite = 0;
        public static double accelX = 0;
        public static double accelY = 0;


        public static void Setup(ESP32StarterKit board)
        {
            board = new ESP32StarterKit();
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            //servo
            board.SetupServo(ESP32Pins.IO05);

            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);

            board.SetupDisplay();

            Display = board.BoardDisplay;
            Button1 = board.BoardButton1;
            Button2 = board.BoardButton2;
        }

        public static void ClearPart(int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    Display.SetPixel(ax, ay, colorWhite);
        }

        public static Button Button1 { get; set; }
        public static Button Button2 { get; set; }

        public static ServoController Servo { get; set; }
        public static SSD1306Imp Display { get; set; }

    }
}
