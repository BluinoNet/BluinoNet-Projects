
using BluinoNet;

namespace Labirin {
    public class Program {

        SplashScreen open = new SplashScreen();
        static ESP32StarterKit board;
        public void BrainPadSetup() {
            BrainPad.Setup(board);
            open.Splash("Tilty Maze");
        }

        public void BrainPadLoop() {
            //Put your program code here. It runs repeatedly after the BrainPad starts up.

            switch (Menu.Show(new string[] { "Tilt Beginner", "Tilt Expert", "Button Beginner", "Button Expert" })) {
                case 1:
                    BrainPad.Display.Clear();

                    BrainPad.Display.Flush();

                    LabirinOne.Run();
                    break;
                case 2:
                    BrainPad.Display.Clear();

                    BrainPad.Display.Flush();

                    LabirinTwo.Run();
                    break;
                case 3:
                    BrainPad.Display.Clear();

                    BrainPad.Display.Flush();

                    LabirinThree.Run();
                    break;
                case 4:
                    BrainPad.Display.Clear();

                    BrainPad.Display.Flush();

                    LabirinFour.Run();
                    break;
            }
        }
    }
}



