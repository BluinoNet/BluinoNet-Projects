
using System;
using System.Collections;
//using System.Text;
using System.Threading;

namespace Labirin {
    class LabirinFour {

        static public void Run() {
            int milliseconds = 0;
            int seconds = 0;
            int minutes = 0;

            ExpertPlayer ball = new ExpertPlayer(23, 21);

            drawMaze();
            //Put your program code here. It runs repeatedly after the BrainPad starts up.
            while (true) {
                //if (BrainPad.Buttons.IsLeftPressed() && BrainPad.Button3.IsTouched())
                //    return;

                if (BrainPad.Button2.Pressed) {
                    BrainPad.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setY(ball.getY() - 1);

                    BrainPad.Display.DrawRectangle(BrainPad.colorBlue, ball.getX(), ball.getY(), 3, 3);

                    BrainPad.Display.Flush();
                }
                   
                if (BrainPad.Button1.Pressed) {
                    BrainPad.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setY(ball.getY() + 1);

                    BrainPad.Display.DrawRectangle(BrainPad.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    BrainPad.Display.Flush();
                }
                   
                if (BrainPad.Button3.IsTouched()) {
                    BrainPad.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setX(ball.getX() + 1);

                    BrainPad.Display.DrawRectangle(BrainPad.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    BrainPad.Display.Flush();
                }
                /*    
                if (BrainPad.Buttons.IsLeftPressed()) {
                    BrainPad.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setX(ball.getX() - 1);

                    BrainPad.Display.DrawRectangle(BrainPad.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    BrainPad.Display.Flush();
                }*/
                
                ball.checkWall();

                if (milliseconds > 10) {
                    milliseconds = 0;
                    seconds++;
                }
                if (seconds >= 59) {
                    seconds = 0;
                    minutes++;
                }
                if (seconds < 10) {
                    BrainPad.Display.DrawString("0",BrainPad.colorBlue, 115, 53);

                    BrainPad.Display.DrawString(seconds.ToString(),BrainPad.colorBlue, 121, 53);
                }
                else {
                    BrainPad.Display.DrawString(seconds.ToString(),BrainPad.colorBlue, 115, 53);
                }

                BrainPad.Display.DrawString(":", BrainPad.colorBlue, 108, 53);

                BrainPad.Display.DrawString(minutes.ToString(),BrainPad.colorBlue, 101, 53);

                BrainPad.Display.Flush();

                if (ball.getX() == 115 && ball.getY() <= 59 && ball.getY() >= 51) {
                    BrainPad.Display.DrawString(minutes.ToString(),BrainPad.colorBlue, 45, 25);

                    BrainPad.Display.DrawString(":",BrainPad.colorBlue, 55, 25);

                    BrainPad.Display.DrawString(seconds.ToString(), BrainPad.colorBlue, 65, 25 );

                    BrainPad.Display.Flush();

                    Thread.Sleep(5000);

                    BrainPad.Display.Clear();

                    BrainPad.Display.Flush();

                    drawMaze();

                    ball.setY(21);

                    ball.setX(23);

                    BrainPad.Display.DrawRectangle(BrainPad.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    BrainPad.Display.Flush();

                    milliseconds = 0;
                    seconds = 0;
                    minutes = 0;
                }
                else
                    milliseconds++;
            }

            void drawMaze() {
                //Outer parameter
                BrainPad.Display.DrawLine(BrainPad.colorBlue,0, 0, 128, 0);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,1, 1, 128, 1);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,127, 0, 127, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,128, 0, 128, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,0, 63, 128, 63);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,0, 64, 128, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,0, 0, 0, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,1, 1, 1, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,39, 0, 39, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,75, 0, 75, 12);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,18, 15, 26, 15);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,52, 15, 64, 15);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,89, 15, 116, 15);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,18, 15, 18, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,64, 15, 64, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,89, 15, 89, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,102, 15, 102, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,18, 27, 51, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,64, 27, 75, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,115, 27, 128, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,18, 27, 51, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,64, 27, 75, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,115, 27, 128, 27);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,30, 27, 30, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,51, 27, 51, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,75, 27, 75, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,115, 27, 115, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,0, 38, 18, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,39, 38, 51, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,75, 38, 102, 38);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,39, 38, 39, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,64, 41, 64, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,89, 38, 89, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,18, 51, 39, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,64, 51, 89, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,102, 51, 128, 51);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,51, 51, 51, 64);

                BrainPad.Display.DrawLine(BrainPad.colorBlue,75, 51, 75, 64);

                BrainPad.Display.Flush();
            }
        }      
    }
}

