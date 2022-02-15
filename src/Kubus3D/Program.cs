using BluinoNet;
using BMC.Drivers.BasicGraphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace Kubus3D
{
    public class Program
    {
        static ESP32StarterKit board = board = new ESP32StarterKit();
        class Vector3
        {
            public double X;
            public double Y;
            public double Z;
            public Vector3(double X, double Y, double Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
        }

        class Vector2
        {
            public double X;
            public double Y;
            public Vector2(double X, double Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        static void Translate3Dto2D(Vector3[] Points3D, Vector2[] Points2D, Vector3 Rotate, Vector3 Position)
        {
            int OFFSETX = 64;
            int OFFSETY = 32;
            int OFFSETZ = 50;

            double sinax = Math.Sin(Rotate.X * Math.PI / 180);
            double cosax = Math.Cos(Rotate.X * Math.PI / 180);
            double sinay = Math.Sin(Rotate.Y * Math.PI / 180);
            double cosay = Math.Cos(Rotate.Y * Math.PI / 180);
            double sinaz = Math.Sin(Rotate.Z * Math.PI / 180);
            double cosaz = Math.Cos(Rotate.Z * Math.PI / 180);

            for (int i = 0; i < 8; i++)
            {
                double x = Points3D[i].X;
                double y = Points3D[i].Y;
                double z = Points3D[i].Z;

                double yt = y * cosax - z * sinax;  // rotate around the x axis
                double zt = y * sinax + z * cosax;  // using the Y and Z for the rotation
                y = yt;
                z = zt;

                double xt = x * cosay - z * sinay;  // rotate around the Y axis
                zt = x * sinay + z * cosay;         // using X and Z
                x = xt;
                z = zt;

                xt = x * cosaz - y * sinaz;         // finally rotate around the Z axis
                yt = x * sinaz + y * cosaz;         // using X and Y
                x = xt;
                y = yt;

                x = x + Position.X;                 // add the object position offset
                y = y + Position.Y;                 // for both x and y
                z = z + OFFSETZ - Position.Z;       // as well as Z

                Points2D[i].X = (x * 64 / z) + OFFSETX;
                Points2D[i].Y = (y * 64 / z) + OFFSETY;
                //BrainPad.ImageBuffer.DrawPoint((int)Points2D[i].X, (int)Points2D[i].Y);
            }
        }

        static void Main()
        {
            board.SetupDisplay();
            var screen = board.BoardDisplay;
            var colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            var colorGreen = BasicGraphics.ColorFromRgb(0, 255, 0);
            var colorRed = BasicGraphics.ColorFromRgb(255, 0, 0);
            //var colorWhite = BasicGraphics.ColorFromRgb(255, 255, 255);

            screen.Clear();
            screen.DrawString("BluinoNet", colorGreen, 1, 1, 1, 1);
            screen.DrawString("KUBUS 3D", colorBlue, 1, 20, 1, 1);
            screen.DrawString("--By BMC--", colorRed, 1, 40, 1, 1);


            screen.Flush();

            Thread.Sleep(2000);
            #region Draw Kubus
            // our object in 3D space
            Vector3[] cube_points = new Vector3[8]
            {
            new Vector3(10,10,-10),
            new Vector3(-10,10,-10),
            new Vector3(-10,-10,-10),
            new Vector3(10,-10,-10),
            new Vector3(10,10,10),
            new Vector3(-10,10,10),
            new Vector3(-10,-10,10),
            new Vector3(10,-10,10),
            };

            // what we get back in 2D space!
            Vector2[] cube2 = new Vector2[8]
            {
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            };

            // the connections between the "dots"
            int[] start = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3 };
            int[] end = new int[12] { 1, 2, 3, 0, 5, 6, 7, 4, 4, 5, 6, 7 };

            Vector3 rot = new Vector3(0, 0, 0);
            Vector3 pos = new Vector3(0, 0, 0);
            board.SetupMpu6050();
            board.BoardMpu6050.StartUpdating();
            double accelX = 0;
            double accelY = 0;

            board.BoardMpu6050.SensorInterruptEvent += (a, e) =>
            {
                screen.Clear();
                for (int i = 0; i < e.Values.Length; i++)
                {
                    accelX = e.Values[i].AccelerationX;
                    accelY = e.Values[i].AccelerationY;
                    break;
                }
                //draw cube
               
                screen.Clear();
                //rot.Z += 5;
                //rot.X += 5;
                //pos.X += 1;

                rot.X = 360 - (accelX*100);
                rot.Y = (accelY*100);

                Translate3Dto2D(cube_points, cube2, rot, pos);

                for (int i = 0; i < start.Length; i++)
                {    // draw the lines that make up the object
                    int vertex = start[i];                  // temp = start vertex for this line
                    int sx = (int)cube2[vertex].X;          // set line start x to vertex[i] x position
                    int sy = (int)cube2[vertex].Y;          // set line start y to vertex[i] y position
                    vertex = end[i];                        // temp = end vertex for this line
                    int ex = (int)cube2[vertex].X;          // set line end x to vertex[i+1] x position
                    int ey = (int)cube2[vertex].Y;          // set line end y to vertex[i+1] y position
                    screen.DrawLine(colorBlue, sx, sy, ex, ey);
                }
                screen.DrawString("X: " + (accelX * 100).ToString("F0"), colorBlue, 0, 55);
                screen.DrawString("Y: " + (accelY * 100).ToString("F0"), colorBlue, 80, 55);
                screen.Flush();
            };

            #endregion
            Thread.Sleep(Timeout.Infinite);
            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
