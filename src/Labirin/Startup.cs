using BluinoNet;
using BluinoNet.Modules;
using BMC.Drivers.BasicGraphics;
using System.Diagnostics;
using System.Threading;

namespace Labirin {
    [DebuggerNonUserCode]
    public class Startup {
        public static void Main() {
            var p = new Program();

            p.BoardSetup();

            while (true) {
                p.ProgramLoop();

                Thread.Sleep(10);
            }
        }
    }

    [DebuggerNonUserCode]
    public static class StarterKit {
        public static uint colorBlue = 0;
        public static uint colorWhite = 0;
        public static double accelX = 0;
        public static double accelY = 0;

       
public static void Setup(ESP32StarterKit board)
        {
            board = new ESP32StarterKit();
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            board.SetupTouchSensor(ESP32Pins.IO13);
            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupDisplay();
            board.SetupMpu6050();

            board.BoardMpu6050.StartUpdating();
            board.BoardMpu6050.SensorInterruptEvent += (a, e) =>
            {
                for (int i = 0; i < e.Values.Length; i++)
                {
                    accelX = (e.Values[i].AccelerationX );
                    accelY = (e.Values[i].AccelerationY );

                    break;
                }
            };
            Accelerometer = board.BoardMpu6050;
            Button1 = board.BoardButton1;
            Button2 = board.BoardButton2;
            Button3 = board.BoardTouchSensor;
            Display = board.BoardDisplay;
            Buzzer = board.BoardBuzzer;
        }

        public static void ClearPart(int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    Display.SetPixel(ax, ay, colorWhite);
        }

        public static Mpu6050 Accelerometer { get; set; }
        public static Button Button1 { get; set; } 
        public static Button Button2 { get; set; } 
        public static TouchSensor Button3 { get; set; } 
        public static Tunes Buzzer { get; set; } 
        public static SSD1306Imp Display { get; set; }
        //public static LightBulb LightBulb { get; } = new LightBulb();
        //public static LightSensor LightSensor { get; } = new LightSensor();
        //public static ServoMotors ServoMotors { get; } = new ServoMotors();
        //public static TemperatureSensor TemperatureSensor { get; } = new TemperatureSensor();
        //public static Wait Wait { get; } = new Wait();
    }
}
