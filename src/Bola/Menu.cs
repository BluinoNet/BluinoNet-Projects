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

    static class Menu {

        static public int Show(string[] menu) {
            int selection = -1;

            if (menu.Length > 7)
                throw new System.Exception("Max menu size is 6!");

            StarterKit.Display.Clear();

            for (int i = 0; i < menu.Length; i++)
                StarterKit.Display.DrawString(menu[i], StarterKit.colorBlue,12, 9 * i,  1, 1);

            StarterKit.Display.DrawString("R = Select L = Exit", StarterKit.colorBlue,0, 64 - 8, 1, 1);

            StarterKit.Display.Flush();

            while (true) {
                if (StarterKit.Button2.Pressed || selection == -1) {
                    StarterKit.Buzzer.Out(400,50,10);

                    StarterKit.Display.DrawString(" ", StarterKit.colorBlue,0, selection * 9,  2, 1);

                    selection++;

                    if (selection >= menu.Length)
                        selection = 0;

                    StarterKit.Display.DrawString(">", StarterKit.colorBlue,0, selection * 9,  2, 1);

                    StarterKit.Display.Flush();

                    while (StarterKit.Button2.Pressed)
                        Thread.Sleep(20);
                }
                else if (StarterKit.Button1.Pressed || selection == -1) {
                    StarterKit.Buzzer.Out(400,50,10);

                    StarterKit.Display.DrawString(" ", StarterKit.colorBlue,0, selection * 9,  2, 1);

                    selection--;

                    if (selection < 0)
                        selection = menu.Length - 1;

                    StarterKit.Display.DrawString(">", StarterKit.colorBlue,0, selection * 9,  2, 1);

                    StarterKit.Display.Flush();

                    while (StarterKit.Button1.Pressed)
                        Thread.Sleep(20);
                }

                if (StarterKit.Button3.IsTouched())
                    return selection + 1;

                Thread.Sleep(20);

                int width = StarterKit.Display.Width;
            }
        }
    }
}