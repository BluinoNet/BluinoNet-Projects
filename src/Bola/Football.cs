// Copyright GHI Electronics, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Threading;

namespace Bola {
    class Football {

        static public void Run(bool sound) {

            int player1Score = 0;
            int player2Score = 0;
            int down = 1;
            int yardLine = 20;
            int YTG = 10;
            int milliseconds = 0;
            int seconds = 0;
            int minutes = 5;
            bool pastMidField = false;
            //Which player has the ball
            int possession = 1; 

            Player player = new Player(13, 42);

            DefensePlayer def1 = new DefensePlayer(46, 32);

            DefensePlayer def2 = new DefensePlayer(46, 42);

            DefensePlayer def3 = new DefensePlayer(46, 52);

            DefensePlayer def4 = new DefensePlayer(68, 42);

            DefensePlayer def5 = new DefensePlayer(101, 42);

            drawField();

            drawScoreBoard();

            StarterKit.Display.DrawCircle(StarterKit.colorBlue, 13, player.getY(), 2);

            if (sound) {
                touchDownSong();
            }

            //The offset is used in Clear player circle from the radius center.
            int offset = 2;

            while (true) {

                if (StarterKit.Button1.Pressed) {
                    StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                    StarterKit.Display.Flush();

                    player.setY(player.getY() - 10);

                    if (player.getY() <= 32) {
                        player.setY(32);
                    }

                    drawPlayerPosition();
                }

                if (StarterKit.Button2.Pressed) {
                    StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                    StarterKit.Display.Flush();

                    player.setY(player.getY() + 10);

                    if (player.getY() >= 52) {
                        player.setY(52);
                    }

                    drawPlayerPosition();
                }

                if (StarterKit.Button3.IsTouched()) {
                    StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                    player.setX(player.getX() + 11);

                    if (pastMidField) {
                        yardLine = yardLine - 1;

                        drawPlayerPosition();

                        checkForTouchDown();
                    }
                    else {
                        yardLine = yardLine + 1;

                        drawPlayerPosition();
                    }

                    YTG = YTG - 1;
                    if (YTG <= 0) {
                        YTG = 0;
                    }
                    drawScoreBoard();

                    //Returns the Player to the otherside of the of the field
                    if (player.getX() >= 110) {
                        StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                        StarterKit.Display.Flush();

                        player.setX(13);

                        drawPlayerPosition();
                    }
                }

                if (StarterKit.Buttons.IsLeftPressed()) {
                    StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                    StarterKit.Display.Flush();

                    player.setX(player.getX() - 11);

                    if (pastMidField) {
                        yardLine = yardLine + 1;

                        drawPlayerPosition();

                        checkForTouchDown();
                    }
                    else {
                        yardLine = yardLine - 1;

                        drawPlayerPosition();
                    }

                    YTG = YTG + 1;

                    if (YTG <= 0) {

                        YTG = 0;
                    }

                    drawScoreBoard();

                    if (player.getX() <= 13) {
                        yardLine = yardLine + 1;
                        YTG = YTG + 1;

                        player.setX(13);
                    }

                    drawPlayerPosition();
                }
                isPlayerTackled();

                drawDefensePositions();

                isPlayerTackled();

                drawGameClock();

                milliseconds++;
            }

            void drawPlayerPosition()
            {
                StarterKit.Display.DrawCircle(StarterKit.colorBlue, player.getX(), player.getY(), 2);

                StarterKit.Display.Flush();
            }

            void drawDefensePositions()
            {
                Thread.Sleep(10);

                Random rndDefensePlayer = new Random();

                Random defenseMove = new Random();


                if (sound) {
                    StarterKit.Buzzer.Out(36,50,200);
                }

                int DefensePlayer = rndDefensePlayer.Next(5);

                int defMove = defenseMove.Next(4); // 1 = UP. 2 = RIGHT, 3 = DOWN, 4 = LEFT

                switch (DefensePlayer) {
                    case 0:

                        if (defMove == 0) {

                            def1.setY(def1.getY() - 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def1.getX(), def1.getY() + 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def1.getY() <= 32) {
                                    def1.setY(32);
                                }
                            }
                            else {
                                def1.setY(def1.getY() + 10);

                                break;
                            }
                        }
                        else if (defMove == 1) {

                            def1.setX(def1.getX() + 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def1.getX() - 11, def1.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def1.getX() >= 112) {
                                    def1.setX(112);
                                }
                            }
                            else {
                                def1.setX(def1.getX() - 11);
                                break;
                            }
                        }
                        else if (defMove == 2) {

                            def1.setY(def1.getY() + 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def1.getX(), def1.getY() - 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def1.getY() >= 52) {
                                    def1.setY(52);
                                }
                            }
                            else {
                                def1.setY(def1.getY() - 10);
                                break;
                            }
                        }
                        else if (defMove == 3) {

                            def1.setX(def1.getX() - 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def1.getX() + 11, def1.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def1.getX() <= 13) {
                                    def1.setX(13);
                                }
                            }
                            else {
                                def1.setX(def1.getX() + 11);
                                break;
                            }
                        }
                        break;

                    case 1:
                        if (defMove == 0) {

                            def2.setY(def2.getY() - 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def2.getX(), def2.getY() + 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def2.getY() <= 32) {
                                    def2.setY(32);
                                }
                            }
                            else {
                                def2.setY(def2.getY() + 10);

                                break;
                            }
                        }
                        else if (defMove == 1) {

                            def2.setX(def2.getX() + 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def2.getX() - 11, def2.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def2.getX() >= 112) {
                                    def2.setX(112);
                                }
                            }
                            else {
                                def2.setX(def2.getX() - 11);

                                break;
                            }
                        }
                        else if (defMove == 2) {

                            def2.setY(def2.getY() + 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def2.getX(), def2.getY() - 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def2.getY() >= 52) {
                                    def2.setY(52);
                                }
                            }
                            else {
                                def2.setY(def2.getY() - 10);

                                break;
                            }
                        }
                        else if (defMove == 3) {

                            def2.setX(def2.getX() - 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def2.getX() + 11, def2.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def2.getX() <= 13) {
                                    def2.setX(13);
                                }
                            }
                            else {
                                def2.setX(def2.getX() + 11);
                                break;
                            }
                        }
                        break;

                    case 2:
                        if (defMove == 0) {

                            def3.setY(def3.getY() - 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def3.getX(), def3.getY() + 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def3.getY() <= 32) {
                                    def3.setY(32);
                                }
                            }
                            else {
                                def3.setY(def3.getY() + 10);

                                break;
                            }
                        }
                        else if (defMove == 1) {

                            def3.setX(def3.getX() + 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def3.getX() - 11, def3.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def3.getX() >= 112) {
                                    def3.setX(112);
                                }
                            }
                            else {
                                def3.setX(def3.getX() - 11);

                                break;
                            }
                        }
                        else if (defMove == 2) {

                            def3.setY(def3.getY() + 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def3.getX(), def3.getY() - 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def3.getY() >= 52) {
                                    def3.setY(52);

                                }
                            }
                            else {
                                def3.setY(def3.getY() - 10);

                                break;
                            }
                        }
                        else if (defMove == 3) {

                            def3.setX(def3.getX() - 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def3.getX() + 11, def3.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def3.getX() <= 13) {
                                    def3.setX(13);
                                }
                            }
                            else {
                                def3.setX(def3.getX() + 11);

                                break;
                            }
                        }
                        break;
                    case 3:
                        if (defMove == 0) {

                            def4.setY(def4.getY() - 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def4.getX(), def4.getY() + 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def4.getY() <= 32) {
                                    def4.setY(32);
                                }
                            }
                            else {
                                def4.setY(def4.getY() + 10);

                                break;
                            }
                        }
                        else if (defMove == 1) {

                            def4.setX(def4.getX() + 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def4.getX() - 11, def4.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def4.getX() >= 112) {
                                    def4.setX(112);
                                }
                            }
                            else {
                                def4.setX(def4.getX() - 11);

                                break;
                            }
                        }
                        else if (defMove == 2) {

                            def4.setY(def4.getY() + 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def4.getX(), def4.getY() - 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def4.getY() >= 52) {
                                    def4.setY(52);

                                }
                            }
                            else {
                                def4.setY(def4.getY() - 10);

                                break;
                            }
                        }
                        else if (defMove == 3) {

                            def4.setX(def4.getX() - 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def4.getX() + 11, def4.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def4.getX() <= 13) {
                                    def4.setX(13);
                                }
                            }
                            else {
                                def4.setX(def4.getX() + 11);

                                break;
                            }
                        }
                        break;
                    case 4:
                        if (defMove == 0) {

                            def5.setY(def5.getY() - 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def5.getX(), def5.getY() + 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def5.getY() <= 32) {
                                    def5.setY(32);
                                }
                            }
                            else {
                                def5.setY(def5.getY() + 10);

                                break;
                            }
                        }
                        else if (defMove == 1) {

                            def5.setX(def5.getX() + 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def5.getX() - 11, def5.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def5.getX() >= 112) {
                                    def5.setX(112);
                                }
                            }
                            else {
                                def5.setX(def5.getX() - 11);

                                break;
                            }
                        }
                        else if (defMove == 2) {

                            def5.setY(def5.getY() + 10);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def5.getX(), def5.getY() - 10, 4, 2);

                                StarterKit.Display.Flush();

                                if (def5.getY() >= 52) {
                                    def5.setY(52);

                                }
                            }
                            else {
                                def5.setY(def5.getY() - 10);

                                break;
                            }
                        }
                        else if (defMove == 3) {

                            def5.setX(def5.getX() - 11);

                            if (isSpaceOpen()) {
                                StarterKit.ClearPart(def5.getX() + 11, def5.getY(), 4, 2);

                                StarterKit.Display.Flush();

                                if (def5.getX() <= 13) {
                                    def5.setX(13);
                                }
                            }
                            else {
                                def5.setX(def5.getX() + 11);
                                break;
                            }
                        }
                        break;

                    default:

                        break;
                }

                //StarterKit.Buzzer.StopBuzzing();

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue, def1.getX(), def1.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def2.getX(), def2.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def3.getX(), def3.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def4.getX(), def4.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def5.getX(), def5.getY(), 4, 2);

                StarterKit.Display.Flush();
            }

            void checkForTouchDown()
            {
                if (pastMidField && yardLine <= 0) {

                    if (possession == 1) {
                        player1Score = player1Score + 7;

                        StarterKit.Display.Clear();
                        //Scoreboard
                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 98, 2);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,98, 2, 98, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 30, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 21, 98, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,48, 2, 48, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,80, 2, 80, 21);

                        drawScoreBoard();

                        drawGameClock();

                        StarterKit.Display.DrawString("TouchDown!",StarterKit.colorBlue, 2, 32);

                        StarterKit.Display.DrawString("Any Button to Kick", StarterKit.colorBlue, 6, 50 );

                        StarterKit.Display.Flush();

                        if (sound) {
                            touchDownSong();
                        }

                        possession = 2;

                        while (true) {
                            if (StarterKit.Button3.IsTouched() || StarterKit.Buttons.IsLeftPressed() || StarterKit.Button1.Pressed || StarterKit.Button2.Pressed) {
                                kickOff();

                                return;
                            }
                        }
                    }
                    else {
                        player2Score = player2Score + 7;

                        StarterKit.Display.Clear();
                        //Scoreboard
                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 98, 2);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,98, 2, 98, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 30, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 21, 98, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,48, 2, 48, 21);

                        StarterKit.Display.DrawLine(StarterKit.colorBlue,80, 2, 80, 21);

                        drawScoreBoard();

                        drawGameClock();

                        StarterKit.Display.DrawString("TouchDown!",StarterKit.colorBlue, 2, 32);

                        StarterKit.Display.DrawString("Any Button to Kick",StarterKit.colorBlue,6, 50);

                        StarterKit.Display.Flush();

                        if (sound) {
                            touchDownSong();
                        }

                        possession = 1;

                        while (true) {

                            if (StarterKit.Button3.IsTouched() || StarterKit.Buttons.IsLeftPressed() || StarterKit.Button1.Pressed || StarterKit.Button2.Pressed) {
                                kickOff();

                                return;
                            }
                        }
                    }
                }
            }

            void kickOff()
            {
                Random rnd = new Random();

                int num = rnd.Next(6);

                switch (num) {

                    case 0:
                        yardLine = 5;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 1:
                        yardLine = 10;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 2:
                        yardLine = 15;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 3:
                        yardLine = 20;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 4:
                        yardLine = 25;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 5:
                        yardLine = 30;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                    case 6:
                        yardLine = 35;
                        down = 1;
                        YTG = 10;
                        pastMidField = false;
                        break;

                }

                StarterKit.Display.Clear();

                drawScoreBoard();

                drawGameClock();

                drawField();

                resetPlay();

                drawPlayerPosition();

                drawDefensePositions();
            }

            void tryFieldGoal()
            {

                Random rnd = new Random();

                drawField();

                drawGameClock();

                drawScoreBoard();

                int num = rnd.Next(yardLine);

                if (num <= 10 && pastMidField) {
                    //90%
                    int num2 = rnd.Next(10);

                    if (num2 > 1) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
                else if (num >= 11 && num <= 20 && pastMidField) {
                    //80%
                    int num2 = rnd.Next(10);

                    if (num2 > 2) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
                else if (num >= 21 && num <= 30 && pastMidField) {
                    //70%
                    int num2 = rnd.Next(10);

                    if (num2 > 3) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
                else if (num >= 31 && num <= 40 && pastMidField) {
                    //60%
                    int num2 = rnd.Next(10);

                    if (num2 > 4) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
                else if (num >= 41 && num <= 50 && pastMidField) {
                    //50%
                    int num2 = rnd.Next(10);

                    if (num2 > 5) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
                else {
                    int num2 = rnd.Next(10);

                    if (num2 > 9) {
                        //FieldGoal is good
                        fieldGoalGood();
                    }
                    else {
                        //FieldGoal is nogood
                        fieldGoalNoGood();
                    }
                }
            }

            void fieldGoalGood()
            {
                StarterKit.Display.DrawString("It's Good",StarterKit.colorBlue, 10, 35);

                StarterKit.Display.Flush();

                if (sound) {
                    touchDownSong();
                }

                if (possession == 1) {
                    player1Score = player1Score + 3;
                    possession = 2;
                    down = 1;
                    YTG = 10;

                    kickOff();
                }

                else {
                    player2Score = player2Score + 3;
                    possession = 1;
                    down = 1;
                    YTG = 10;

                    kickOff();
                }
            }

            void fieldGoalNoGood()
            {
                StarterKit.Display.DrawString("No Good!",StarterKit.colorBlue, 28, 35);

                StarterKit.Display.Flush();

                if (sound) {
                    StarterKit.Buzzer.Out(100,50,50);


                    StarterKit.Buzzer.Out(50,50,200);

                }

                Thread.Sleep(2000);

                StarterKit.Display.Clear();

                if (possession == 1)
                    possession = 2;
                else
                    possession = 1;

                if (pastMidField) {
                    pastMidField = false;
                }
                else
                    pastMidField = true;

                down = 1;
                YTG = 10;

                drawField();
            }

            void drawScoreBoard()
            {
                //Clears PlayerOne Score
                StarterKit.ClearPart(6, 4, 10, 10);
                //Clears PlayerTwo Score
                StarterKit.ClearPart(103, 4, 10, 10);

                StarterKit.ClearPart(59, 13, 10, 10);

                StarterKit.ClearPart(35, 8, 10, 10);

                StarterKit.ClearPart(84, 8, 12, 10);
                //Draws Box around Player who controls the ball. 
                if (possession == 1) {
                    StarterKit.Display.DrawLine(StarterKit.colorBlue,1, 1, 28, 1);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,1, 1, 1, 23);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,1, 23, 28, 23);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,28, 23, 28, 1);
                }
                else {
                    StarterKit.Display.DrawLine(StarterKit.colorBlue,100, 1, 127, 1);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,100, 1, 100, 23);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,100, 23, 127, 23);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,127, 23, 127, 1);
                }

                if (player1Score < 10) {
                    //PlayerOne Score 1 digit number
                    StarterKit.Display.DrawString(player1Score.ToString(),StarterKit.colorBlue,11, 5);
                }
                else {
                    //PlayerOne Score 2 digit number
                    StarterKit.Display.DrawString(player1Score.ToString(), StarterKit.colorBlue,4, 4);
                }

                if (player2Score < 10) {
                    //PlayerTwo Score 1 digit number
                    StarterKit.Display.DrawString(player2Score.ToString(), StarterKit.colorBlue,108, 5);
                }
                else {
                    //PlayerTwo Score 2 digit number
                    StarterKit.Display.DrawString(player2Score.ToString(), StarterKit.colorBlue,103, 4);
                }

                //DownMarker
                StarterKit.Display.DrawString("" + down, StarterKit.colorBlue,35, 8, 2, 1);

                if (yardLine >= 50) {
                    // Clears the direction >
                    StarterKit.ClearPart(52, 13, 30, 8);

                    StarterKit.Display.Flush();

                    pastMidField = true;
                }
                else {

                    if (pastMidField) {
                        StarterKit.ClearPart(71, 13, 8, 8);
                        //Position Marker
                        StarterKit.Display.DrawString(">", StarterKit.colorBlue,71, 13,  1, 1);

                        StarterKit.Display.Flush();
                    }
                    else {
                        StarterKit.ClearPart(52, 13, 15, 8);
                        //Position Marker
                        StarterKit.Display.DrawString("<", StarterKit.colorBlue,52, 13,  1, 1);

                        StarterKit.Display.Flush();
                    }
                }

                if (yardLine < 10) {
                    StarterKit.Display.DrawString("0" + yardLine, StarterKit.colorBlue,59, 12,  1, 1);
                }
                else
                    StarterKit.Display.DrawString("" + yardLine, StarterKit.colorBlue,59, 12,  1, 1);

                StarterKit.Display.DrawString("" + down, StarterKit.colorBlue,35, 8,  2, 1);

                //Yards to go
                if (YTG >= 10)
                    StarterKit.Display.DrawString("" + YTG, StarterKit.colorBlue,84, 8,  1, 1);
                else
                    StarterKit.Display.DrawString("" + YTG, StarterKit.colorBlue,87, 8,  1, 1);

                StarterKit.Display.Flush();
            }

            void isPlayerTackled()
            {
                if (player.getX() == def1.getX() && player.getY() == def1.getY()) {
                    endPlay();
                }
                else if (player.getX() == def2.getX() && player.getY() == def2.getY()) {
                    endPlay();
                }
                else if (player.getX() == def3.getX() && player.getY() == def3.getY()) {
                    endPlay();
                }
                else if (player.getX() == def4.getX() && player.getY() == def4.getY()) {
                    endPlay();
                }
                else if (player.getX() == def5.getX() && player.getY() == def5.getY()) {
                    endPlay();
                }
            }
            void endPlay()
            {
                down = down + 1;

                if (sound) {
                    blowWhistle();
                }

                checkDown();

                drawScoreBoard();

                resetPlay();
            }

            void checkDown()
            {
                if (YTG <= 0) {
                    YTG = 10;
                    down = 1;
                }

                if (down < 4) {
                    return;
                }
                else if (down == 4) {
                    StarterKit.Display.Clear();
                    //Scoreboard
                    StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 98, 2);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,98, 2, 98, 21);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 30, 21);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 21, 98, 21);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,48, 2, 48, 21);

                    StarterKit.Display.DrawLine(StarterKit.colorBlue,80, 2, 80, 21);

                    drawScoreBoard();

                    drawGameClock();

                    StarterKit.Display.DrawString("4th Down: " + YTG + " YTG", StarterKit.colorBlue,17, 30);

                    StarterKit.Display.DrawString("Punt/L Kick/U Run/R",StarterKit.colorBlue,5, 50);

                    StarterKit.Display.Flush();

                    while (true) {
                        //Going for it on 4th down -- RUN
                        if (StarterKit.Button3.IsTouched()) {
                            StarterKit.Display.Clear();

                            StarterKit.Display.Flush();

                            drawScoreBoard();

                            drawField();

                            return;
                        }

                        if (StarterKit.Buttons.IsLeftPressed()) {
                            StarterKit.Display.Clear();
                            StarterKit.Display.Flush();

                            kickOff();

                            drawScoreBoard();

                            drawField();

                            return;
                        }

                        if (StarterKit.Button1.Pressed) {
                            StarterKit.Display.Clear();

                            StarterKit.Display.Flush();

                            tryFieldGoal();

                            return;
                        }
                    }
                }
                else if (down > 4) {

                    while (true) {
                        StarterKit.Display.Clear();

                        StarterKit.Display.DrawString("Change",StarterKit.colorBlue, 25, 15);

                        StarterKit.Display.DrawString("Possesion",StarterKit.colorBlue, 20, 40);

                        StarterKit.Display.Flush();

                        Thread.Sleep(3000);

                        StarterKit.Display.Clear();

                        StarterKit.Display.Flush();

                        if (possession == 1)
                            possession = 2;
                        else
                            possession = 1;

                        down = 1;
                        YTG = 10;

                        drawField();

                        return;
                    }
                }

                if (YTG <= 0) {
                    YTG = 10;
                    down = 1;
                }
            }

            void touchDownSong()
            {
                //G 196 - C 261.63 - E  329.63 - G  392 - E  329.63 - G - 392
                StarterKit.Buzzer.Out(196,50,200);//G


                StarterKit.Buzzer.Out(261,50,200);//C


                StarterKit.Buzzer.Out(329,50,200);//E


                StarterKit.Buzzer.Out(392,50,100);//G

   
                StarterKit.Buzzer.Out(329,50,200);//E

   
                StarterKit.Buzzer.Out(392,50,800);//G

            }

            void blowWhistle()
            {
                for (int i = 0; i < 3; i++) {
                    StarterKit.Buzzer.Out(2489,50,15);


                    StarterKit.Buzzer.Out(2217,50,15);

                }

            }

            void drawGameClock()
            {
                if (minutes == 0 && seconds == 0) {

                }
                else {
                    if (milliseconds >= 10) {
                        seconds = seconds - 1;
                        milliseconds = 0;

                    }
                    if (seconds <= 0) {
                        seconds = 59;
                        minutes = minutes - 1;

                    }
                    //GameClock
                    StarterKit.Display.DrawString(minutes.ToString(), StarterKit.colorBlue,53, 4);

                    StarterKit.Display.DrawString(":",StarterKit.colorBlue,60, 4);

                    if (seconds < 10) {
                        StarterKit.Display.DrawString("0",StarterKit.colorBlue,67, 4);

                        StarterKit.Display.DrawString(seconds.ToString(), StarterKit.colorBlue,73, 4);
                    }
                    else {
                        StarterKit.Display.DrawString(seconds.ToString(), StarterKit.colorBlue,67, 4);
                    }
                }

                StarterKit.Display.Flush();
            }

            void resetPlay()
            {
                StarterKit.ClearPart(player.getX() - offset, player.getY() - offset, 6, 5);

                StarterKit.ClearPart(def1.getX(), def1.getY(), 4, 2);

                StarterKit.ClearPart(def2.getX(), def2.getY(), 4, 2);

                StarterKit.ClearPart(def3.getX(), def3.getY(), 4, 2);

                StarterKit.ClearPart(def4.getX(), def4.getY(), 4, 2);

                StarterKit.ClearPart(def5.getX(), def5.getY(), 4, 2);

                player.setX(13);

                player.setY(42);

                def1.setX(46);

                def2.setX(46);

                def3.setX(46);

                def4.setX(68);

                def5.setX(101);

                def1.setY(32);

                def2.setY(42);

                def3.setY(52);

                def4.setY(42);

                def5.setY(42);

                StarterKit.Display.DrawCircle(StarterKit.colorBlue, player.getX(), player.getY(), 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def1.getX(), def1.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def2.getX(), def2.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def3.getX(), def3.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def4.getX(), def4.getY(), 4, 2);

                StarterKit.Display.DrawRectangle(StarterKit.colorBlue,def5.getX(), def5.getY(), 4, 2);

                StarterKit.Display.Flush();

                Thread.Sleep(2000);
            }

            bool isSpaceOpen()
            {
                if (def1.getX() == def2.getX() && def1.getY() == def2.getY()) {
                    return false;
                }
                else if (def1.getX() == def3.getX() && def1.getY() == def3.getY()) {
                    return false;

                }
                else if (def1.getX() == def4.getX() && def1.getY() == def4.getY()) {
                    return false;

                }
                else if (def1.getX() == def5.getX() && def1.getY() == def5.getY()) {
                    return false;

                }
                else if (def2.getX() == def3.getX() && def2.getY() == def3.getY()) {
                    return false;

                }
                else if (def2.getX() == def4.getX() && def2.getY() == def4.getY()) {
                    return false;

                }
                else if (def2.getX() == def5.getX() && def2.getY() == def5.getY()) {
                    return false;

                }

                else if (def3.getX() == def4.getX() && def3.getY() == def4.getY()) {
                    return false;

                }
                else if (def3.getX() == def5.getX() && def3.getY() == def5.getY()) {
                    return false;

                }
                else if (def4.getX() == def5.getX() && def4.getY() == def5.getY()) {
                    return false;
                }

                return true;
            }

            void drawField()
            {
                //OutlineField
                StarterKit.Display.DrawLine(StarterKit.colorBlue,5, 25, 124, 25);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,124, 25, 124, 60);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,124, 60, 5, 60);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,5, 60, 5, 25);

                //Left Goal Post
                StarterKit.Display.DrawLine(StarterKit.colorBlue,5, 42, 3, 42);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,3, 39, 3, 44);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,3, 39, 1, 36);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,3, 44, 1, 46);

                //Right Goal Post
                StarterKit.Display.DrawLine(StarterKit.colorBlue,124, 42, 126, 42);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,126, 40, 126, 44);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,126, 40, 128, 38);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,126, 44, 128, 46);

                //LeftGoalLine
                StarterKit.Display.DrawLine(StarterKit.colorBlue,9, 25, 9, 60);

                //10 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,20, 25, 20, 60);

                //20 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,31, 25, 31, 60);

                //30 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,42, 25, 42, 60);

                //40 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,53, 25, 53, 60);

                //50 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,64, 25, 64, 60);

                //40 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,75, 25, 75, 60);

                //30 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,85, 25, 85, 60);

                //20 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,97, 25, 97, 60);

                //10 Yard 
                StarterKit.Display.DrawLine(StarterKit.colorBlue,108, 25, 108, 60);

                //RightGoalLine
                StarterKit.Display.DrawLine(StarterKit.colorBlue,119, 25, 119, 60);

                //Hashmarks
                StarterKit.Display.DrawLine(StarterKit.colorBlue,9, 37, 11, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,9, 49, 11, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 37, 22, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,18, 49, 22, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,29, 37, 33, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,29, 49, 33, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,40, 37, 44, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,40, 49, 44, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,51, 37, 55, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,51, 49, 55, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,62, 37, 66, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,62, 49, 66, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,73, 37, 77, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,73, 49, 77, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,83, 37, 87, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,83, 49, 87, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,95, 37, 99, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,95, 49, 99, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,106, 37, 110, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,106, 49, 110, 49);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,117, 37, 119, 37);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,117, 49, 119, 49);

                //Scoreboard
                StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 98, 2);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,98, 2, 98, 21);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 2, 30, 21);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,30, 21, 98, 21);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,48, 2, 48, 21);

                StarterKit.Display.DrawLine(StarterKit.colorBlue,80, 2, 80, 21);

                StarterKit.Display.Flush();
            }
        }

        class Player {

            int posX = 0;
            int posY = 0;
            int previousX = 0;
            int previousY = 0;

            public Player(int x, int y) {
                previousX = posX;
                previousY = posY;
                posX = x;
                posY = y;

            }
            public int getX() {
                return posX;
            }
            public int getY() {
                return posY;
            }
            public int getPreviousX() {
                return previousX;
            }
            public int getPreviousY() {
                return previousY;
            }
            public void setX(int x) {
                previousX = posX;
                posX = x;
            }
            public void setY(int y) {
                previousY = posY;
                posY = y;
            }
        }

        class DefensePlayer {

            int posX = 0;
            int posY = 0;
            int previousX = 0;
            int previousY = 0;

            public DefensePlayer(int x, int y) {
                previousX = posX;
                previousY = posY;
                posX = x;
                posY = y;
            }
            public int getX() {
                return posX;
            }
            public int getY() {
                return posY;
            }
            public int getPreviousX() {
                return previousX;
            }
            public int getPreviousY() {
                return previousY;
            }
            public void setX(int x) {
                previousX = posX;
                posX = x;
            }
            public void setY(int y) {
                previousY = posY;
                posY = y;
            }
        }
    }
}

