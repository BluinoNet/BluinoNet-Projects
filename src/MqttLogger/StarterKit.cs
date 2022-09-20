using BluinoNet;
using BluinoNet.Modules;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;

namespace MqttLogger
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

            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupDisplay();
            board.SetupBMP180();
            board.SetupLedRgb(ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05);

            Display = board.BoardDisplay;
            Buzzer = board.BoardBuzzer;
            TemperatureSensor = board.BoardBMP180;
            LightBulb = board.BoardLedRgb;
        }

        public static void ClearPart(int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    Display.SetPixel(ax, ay, colorWhite);
        }

        public static RgbLedPwm LightBulb { get; set; }
        public static Bmp180Module TemperatureSensor { get; set; }
        public static Tunes Buzzer { get; set; }
        public static SSD1306Imp Display { get; set; }

    }
}
