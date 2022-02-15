
using System;
using System.Collections;
//using System.Text;
using System.Threading;

namespace Labirin {
    static class Menu {

        static public int Show(string[] menu) {
            int selection = -1;

            if (menu.Length > 7)
                throw new System.Exception("Max menu size is 6!");

            BrainPad.Display.Clear();

            for (int i = 0; i < menu.Length; i++)
                BrainPad.Display.DrawString(menu[i],BrainPad.colorBlue, 12, 9 * i,  1, 1);

            BrainPad.Display.DrawString("R = Select L = Exit", BrainPad.colorBlue, 0, 64 - 8,  1, 1);

            BrainPad.Display.Flush();

            while (true) {
                if (BrainPad.Button1.Pressed || selection == -1) {
                    BrainPad.Buzzer.Out(400,50,10);

                    BrainPad.Display.DrawString(" ", BrainPad.colorBlue ,0, selection * 9,  2, 1);

                    selection++;

                    if (selection >= menu.Length)
                        selection = 0;

                    BrainPad.Display.DrawString(">", BrainPad.colorBlue, 0, selection * 9,  2, 1);

                    BrainPad.Display.Flush();

                    while (BrainPad.Button1.Pressed)
                        Thread.Sleep(20);
                }
                else if (BrainPad.Button2.Pressed || selection == -1) {
                    BrainPad.Buzzer.Out(400,50,10);

                    BrainPad.Display.DrawString(" ", BrainPad.colorBlue ,0, selection * 9,  2, 1);

                    selection--;

                    if (selection < 0)
                        selection = menu.Length - 1;

                    BrainPad.Display.DrawString(">", BrainPad.colorBlue,0, selection * 9,  2, 1);

                    BrainPad.Display.Flush();

                    while (BrainPad.Button2.Pressed)
                        Thread.Sleep(20);
                }

                if (BrainPad.Button3.IsTouched())
                    return selection + 1;

                Thread.Sleep(20);

                int width = BrainPad.Display.Width;
            }
        }
    }
}