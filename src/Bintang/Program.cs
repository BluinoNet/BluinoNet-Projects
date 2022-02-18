using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace Bintang
{
    public class Program
    {
        public const int MaxStars = 50;
        public const int MaxHistory = 3;
        public const double PI2 = 6.283185307179586476925286766559;

        public static Random Rnd = new Random();

        static ESP32StarterKit board = board = new ESP32StarterKit();
        static void ClearPart(SSD1306Imp screen, int x, int y, int width, int height)
        {
            for (var ax = x; ax < x + width; ax++)
                for (var ay = y; ay < y + height; ay++)
                    screen.SetPixel(ax, ay, colorWhite);
        }
        static uint colorBlue = 0;
        static uint colorWhite = 0;
        public const int MaxX = 128;
        public const int MaxY = 64;
        public static void Main()
        {
            colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            board.SetupDisplay();
            var screen = board.BoardDisplay;
            //128 x 64
             
            Star[] stars = new Star[MaxStars];

            for (int i = 0; i < MaxStars; i++)
            {
                stars[i] = new Star();
            }

            while (true)
            {
                for (int i = 0; i < MaxStars; i++)
                {
                    var star = stars[i];

                    if (star.X < 0 || star.X > MaxX || star.Y < 0 || star.Y > MaxY)
                    {
                        for (int j = 0; j <= MaxHistory; j++)
                        {
                            screen.SetPixel(star.X - j * star.Dx, star.Y - j * star.Dy, colorWhite);
                        }

                        star.Initialize();
                    }
                    else
                    {
                        screen.SetPixel(star.X, star.Y, colorBlue);
                        star.X += star.Dx;
                        star.Y += star.Dy;

                        if (star.History < MaxHistory)
                        {
                            star.History++;
                        }
                        else
                        {
                            screen.SetPixel(star.X - (MaxHistory + 1) * star.Dx, star.Y - (MaxHistory + 1) * star.Dy, colorWhite);
                        }
                    }
                }
                screen.Flush();
            }
        }
    }

    class Star
    {
        public int X;
        public int Y;
        public int Dx;
        public int Dy;
        public int History;

        public Star()
        {
            Initialize();
        }

        public void Initialize()
        {
            do
            {
                var angle = Program.PI2 * Program.Rnd.NextDouble();
                var speed = Program.Rnd.Next(5) + 1;

                Dx = (int)System.Math.Round(System.Math.Sin(angle) * speed);
                Dy = (int)System.Math.Round(System.Math.Cos(angle) * speed);
            } while (Dx == 0 || Dy == 0);

            X = 8 * Dx + (Program.MaxX/2);
            Y = 8 * Dy + (Program.MaxY/2);
            History = 0;
        }
    }
}
