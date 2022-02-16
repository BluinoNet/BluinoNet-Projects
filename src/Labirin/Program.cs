
using BluinoNet;

namespace Labirin {
    public class Program {

        SplashScreen open = new SplashScreen();
        static ESP32StarterKit board;
        public void BoardSetup() {
            StarterKit.Setup(board);
            open.Splash("Tilty Maze");
        }

        public void ProgramLoop() {
            //Put your program code here. It runs repeatedly after the board starts up.

            switch (Menu.Show(new string[] { "Tilt Beginner", "Tilt Expert", "Button Beginner", "Button Expert" })) {
                case 1:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    LabirinOne.Run();
                    break;
                case 2:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    LabirinTwo.Run();
                    break;
                case 3:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    LabirinThree.Run();
                    break;
                case 4:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    LabirinFour.Run();
                    break;
            }
        }
    }
}



