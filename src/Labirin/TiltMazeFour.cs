
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
            //Put your program code here. It runs repeatedly after the board starts up.
            while (true) {
                //if (board.Buttons.IsLeftPressed() && board.Button3.IsTouched())
                //    return;

                if (StarterKit.Button2.Pressed) {
                    StarterKit.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setY(ball.getY() - 1);

                    StarterKit.Display.DrawRectangle(StarterKit.colorBlue, ball.getX(), ball.getY(), 3, 3);

                    StarterKit.Display.Flush();
                }
                   
                if (StarterKit.Button1.Pressed) {
                    StarterKit.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setY(ball.getY() + 1);

                    StarterKit.Display.DrawRectangle(StarterKit.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    StarterKit.Display.Flush();
                }
                   
                if (StarterKit.Button3.IsTouched()) {
                    StarterKit.ClearPart(ball.getX(), ball.getY(), 3, 3);

                    ball.setX(ball.getX() + 1);

                    StarterKit.Display.DrawRectangle(StarterKit.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    StarterKit.Display.Flush();
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
                    StarterKit.Display.DrawString("0",StarterKit.colorBlue, 115, 53);

                    StarterKit.Display.DrawString(seconds.ToString(),StarterKit.colorBlue, 121, 53);
                }
                else {
                    StarterKit.Display.DrawString(seconds.ToString(),StarterKit.colorBlue, 115, 53);
                }

                StarterKit.Display.DrawString(":", StarterKit.colorBlue, 108, 53);

                StarterKit.Display.DrawString(minutes.ToString(),StarterKit.colorBlue, 101, 53);

                StarterKit.Display.Flush();

                if (ball.getX() == 115 && ball.getY() <= 59 && ball.getY() >= 51) {
                    StarterKit.Display.DrawString(minutes.ToString(),StarterKit.colorBlue, 45, 25);

                    StarterKit.Display.DrawString(":",StarterKit.colorBlue, 55, 25);

                    StarterKit.Display.DrawString(seconds.ToString(), StarterKit.colorBlue, 65, 25 );

                    StarterKit.Display.Flush();

                    Thread.Sleep(5000);

                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    drawMaze();

                    ball.setY(21);

                    ball.setX(23);

                    StarterKit.Display.DrawRectangle(StarterKit.colorBlue,ball.getX(), ball.getY(), 3, 3);

                    StarterKit.Display.Flush();

                    milliseconds = 0;
                    seconds = 0;
                    minutes = 0;
                }
                else
                    milliseconds++;
            }

            void drawMaze() {
                //Outer parameter
                StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 0, 128, 0);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,1, 1, 128, 1);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,127, 0, 127, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,128, 0, 128, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 63, 128, 63);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 64, 128, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 0, 0, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,1, 1, 1, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,39, 0, 39, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,75, 0, 75, 12);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 15, 26, 15);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,52, 15, 64, 15);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,89, 15, 116, 15);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 15, 18, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 15, 64, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,89, 15, 89, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,102, 15, 102, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 27, 51, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 27, 75, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,115, 27, 128, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 27, 51, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 27, 75, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,115, 27, 128, 27);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 27, 30, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,51, 27, 51, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,75, 27, 75, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,115, 27, 115, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,0, 38, 18, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,39, 38, 51, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,75, 38, 102, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,39, 38, 39, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 41, 64, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,89, 38, 89, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 51, 39, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 51, 89, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,102, 51, 128, 51);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,51, 51, 51, 64);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,75, 51, 75, 64);

                StarterKit.Display.Flush();
            }
        }      
    }
}

