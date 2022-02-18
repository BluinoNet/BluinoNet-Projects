// Copyright (c) GHI Electronics, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace Bola {

    class Program {

        SplashScreen open = new SplashScreen();

        public void BoardSetup() {
            open.Splash(" Football");
        }

        public void AppLoop() {

            switch (Menu.Show(new string[] { "Football", "No Sound" })) {
                case 1:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    Football.Run(true);

                    break;
                case 2:
                    StarterKit.Display.Clear();

                    StarterKit.Display.Flush();

                    Football.Run(false);

                    break;
            }
        }
    }
}

