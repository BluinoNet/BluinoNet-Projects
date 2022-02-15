using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace Pong
{
    public class Program
    {
        static ESP32StarterKit board = board = new ESP32StarterKit();

        static void Beep()
        {
            board.BoardBuzzer.Out(100, 50, 50);
        }

        static void ClearPart(SSD1306Imp screen, int x,int y, int width, int height)
        {
            var colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);
            for (var ax = x; ax < x+width; ax++)
                for (var ay = y; ay < y+height; ay++)
                    screen.SetPixel(ax, ay, colorWhite);
        }
        public static void Main()
        {
            //button up and down
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            //speaker
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupDisplay();
            var screen = board.BoardDisplay;
            var colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            var colorWhite = BasicGraphics.ColorFromRgb(0, 0, 0);

            screen.Clear();
            screen.DrawString("BluinoNet", colorBlue, 1, 1, 1, 1);
            screen.DrawString("Pong", colorBlue, 1, 20, 1, 1);
            screen.DrawString("--By BMC--", colorBlue, 1, 40, 1, 1);


            screen.Flush();


            Thread.Sleep(2000);
            screen.Clear();

            double BallX = 10, BallY = 10, BallDX = 2.3, BallDY = 2.8;
            int ScoreL = 0, ScoreR = 0;
            int PlayerPos = 30;
            int CompPos = 30;

            screen.DrawRectangle(colorBlue, 0, 0, 128, 64);

            while (true)
            {
                // The Ball
                screen.DrawCircle(colorWhite, (int)BallX, (int)BallY, 4);
                BallX += BallDX;
                BallY += BallDY;

                if (BallX < 10)
                {
                    BallDX *= -1;

                    if (BallY >= CompPos - 1 && BallY <= CompPos + 12)
                    {
                        // hit back
                        Beep();
                    }
                    else
                    {
                        //win
                        for (int i = 0; i < 3; i++)
                        {
                            for (int f = 1000; f < 6000; f += 500)
                            {
                                board.BoardBuzzer.Out(f, 50, 10);
                            }
                        }
                        ScoreR++;

                        Thread.Sleep(500);
                    }
                }

                if (BallX > 115)
                {
                    BallDX *= -1;

                    if (BallY >= PlayerPos - 1 && BallY <= PlayerPos + 12)
                    {
                        // hit back
                        Beep();
                    }
                    else
                    {
                        // Loss
                        for (int f = 2000; f > 200; f -= 200)
                        {
                            board.BoardBuzzer.Out(f, 50, 10);
                        }
                        ScoreL++;
                        Thread.Sleep(500);
                    }
                }

                if (BallY < 5 || BallY > 55)
                {
                    BallDY *= -1;
                    Beep();
                }

                screen.DrawCircle(colorBlue, (int)BallX, (int)BallY, 4);
                // The Field
                for (var y = 0; y < 64; y += 10)
                {
                    // net
                    screen.DrawLine(colorBlue, 64, y, 64, y + 5);
                }

                // Player
                screen.DrawRectangle(colorWhite, 120, PlayerPos, 2, 10);
                if (board.BoardButton1.Pressed) PlayerPos -= 4;
                if (board.BoardButton2.Pressed) PlayerPos += 4;
                if (PlayerPos < 5) PlayerPos = 5;
                if (PlayerPos > 50) PlayerPos = 50;
                screen.DrawRectangle(colorBlue, 120, PlayerPos, 2, 10);

                // Computer
                screen.DrawRectangle(colorWhite, 10, CompPos, 2, 10);
                if (BallY > CompPos + 10) CompPos += 2;
                if (BallY < CompPos) CompPos -= 2;
                if (CompPos < 5) CompPos = 5;
                if (CompPos > 50) CompPos = 50;
                screen.DrawRectangle(colorBlue, 10, CompPos, 2, 10);

                // Score
                ClearPart(screen, 50, 5,10,10);
                ClearPart(screen, 74, 5, 10,10);
                screen.DrawString(ScoreL.ToString(), colorBlue, 50, 5);
                screen.DrawString(ScoreR.ToString(), colorBlue, 74, 5);

                if (ScoreL >= 5)
                {
                    screen.DrawString("You Lose!", colorBlue, 0, 40, 3, 1);
                    screen.Flush();
                    Thread.Sleep(-1);
                }

                if (ScoreR >= 5)
                {
                    screen.DrawString("You Win!", colorBlue, 0, 40, 3, 1);
                    screen.Flush();
                    while (true) Thread.Sleep(1000);
                }

                screen.Flush();
                Thread.Sleep(20);
            }
        }
    }
}
