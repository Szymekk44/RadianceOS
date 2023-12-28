using Cosmos.System.Graphics;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using System.Reflection.Metadata;
using RadianceOS.System.Managment;
using Cosmos.System.Graphics.Fonts;
using Cosmos.HAL.Drivers.Video.SVGAII;
using RadianceOS.System.Apps;

namespace RadianceOS.System.Radiance
{
    public static class Security
    {
        public static Canvas canvas;
        public static int State;
        public static string reason;
        public static uint screenSizeX = 1920, screenSizeY = 1080;

        public static Bitmap crash;

        public static bool Logged;
        public static void LogIn()
        {
            Logged = true;
            var folder_list = Directory.GetDirectories(@"0:\Users\");
            Kernel.loggedUser = folder_list[0];
            if(Kernel.render)
            {
                Process.Processes.RemoveAt(1);
				Explorer.drawIcons = true;
				DrawDesktopApps.UpdateIcons();
				Explorer.DrawTaskbar = true;
			}

			

        }

        public static void StartGui()
        {
            Kernel.Cursor1 = new Bitmap(Files.cursor1);
            Cosmos.System.MouseManager.ScreenWidth = screenSizeX;
            Cosmos.System.MouseManager.ScreenHeight = screenSizeY;
            Cosmos.System.MouseManager.X = screenSizeX / 2; Cosmos.System.MouseManager.Y = screenSizeY / 2;
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(screenSizeX, screenSizeY, ColorDepth.ColorDepth32));
            Kernel.font18 = PCScreenFont.LoadFont(Files.Font18);
            crash = new Bitmap(Files.crash);
            Render();
        }
        public static void Render()
        {
            switch (State)
            {
                case 0:
                    {
                        canvas.Clear(Color.FromArgb(132, 0, 0));
                        canvas.DrawImage(crash, 860, 400);
                        StringsAcitons.DrawCenteredStringAlt("RadianceOS did not start correctly.", 1920, 0, 600, 18, Color.White, Kernel.font18);

                        DrawButton(0);
                        DrawButton(1);
                        DrawButton(2);
                        DrawButton(3);
                        DrawButton(4);
                        canvas.DrawImageAlpha(Kernel.Cursor1, (int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);//CURSOR
                        canvas.Display();
                    }
                    break;
                case 1:
                    {
                        canvas.Clear(Color.FromArgb(132, 0, 0));
                        canvas.DrawImage(crash, 860, 400);
                        StringsAcitons.DrawCenteredStringAlt("An error occurred while running RadianceOS\n" + reason, 1920, 0, 600, 18, Color.White, Kernel.font18);

                        DrawButton(1);
                        DrawButton(2);
                        DrawButton(3);
                        DrawButton(4);
                        DrawButton(5);
                        canvas.DrawImageAlpha(Kernel.Cursor1, (int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);//CURSOR
                        canvas.Display();
                    }
                    break;
            }

        }
        public static void DrawButton(int id)
        {
            if (Cosmos.System.MouseManager.X > 810 && Cosmos.System.MouseManager.X < 1110)
            {
                if (Cosmos.System.MouseManager.Y > 645 + id * 30 && Cosmos.System.MouseManager.Y < 675 + id * 30)
                {
                    canvas.DrawFilledRectangle(Color.FromArgb(90, 0, 0), 810, 645 + id * 30, 300, 30);
                    if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
                    {
                        switch (State)
                        {
                            case 0:
                                {
                                    switch (id)
                                    {
                                        case 0:

                                            break;
                                        case 1:

                                            break;
                                        case 2:
                                            Kernel.LoadFiles();
                                            Kernel.diskReady = false;
                                            Kernel.Repair = false;
                                            ConsoleCommands.RunCommand("guir");
                                            break;
                                        case 3:
                                            Cosmos.System.Power.Reboot();
                                            break;
                                        case 4:
                                            Cosmos.System.Power.Shutdown();
                                            break;
                                    }
                                }
                                break;

                            case 1:
                                {
                                    switch (id)
                                    {
                                        case 1:
                                            Cosmos.System.Power.Reboot();
                                            break;
                                        case 2:
                                            if (Process.Processes.Count > 1)
                                            {
                                                int toKill = Process.Processes.Count - 1;

                                                for (int i = 0; i < Process.Processes.Count; i++)
                                                {
                                                    if (Process.Processes[i].selected)
                                                        toKill = i;
                                                }
                                                Process.Processes.RemoveAt(toKill);
                                                Kernel.Repair = false;
                                                Kernel.render = true;
                                                Explorer.DrawTaskbar = true;
                                            }

                                            break;
                                        case 3:
                                            Process.Processes.RemoveAt(Process.Processes.Count - 1);
                                            Kernel.Repair = false;
                                            Kernel.render = true;
                                            Explorer.DrawTaskbar = true;
                                            break;
                                        case 4:
                                            Process.Processes.Clear();
                                            Processes start = new Processes
                                            {
                                                ID = -1,
                                            };
                                            Process.Processes.Add(start);
                                            Kernel.Repair = false;
                                            Kernel.render = true;
                                            Explorer.DrawTaskbar = true;
                                            break;
                                        case 5:
                                            Kernel.Repair = false;
                                            Kernel.render = true;
                                            Explorer.DrawTaskbar = true;
                                            break;

                                    }
                                }
                                break;
                        }

                    }
                }
                else
                    canvas.DrawFilledRectangle(Color.FromArgb(110, 0, 0), 810, 645 + id * 30, 300, 30);
            }
            else
                canvas.DrawFilledRectangle(Color.FromArgb(110, 0, 0), 810, 645 + id * 30, 300, 30);
            switch (State)
            {
                case 0:
                    {
                        switch (id)
                        {
                            case 0:
                                StringsAcitons.DrawCenteredStringAlt("Start troubleshooting", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 1:
                                StringsAcitons.DrawCenteredStringAlt("Open console with system admin", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 2:
                                StringsAcitons.DrawCenteredStringAlt("Reinstall RadianceOS", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 3:
                                StringsAcitons.DrawCenteredStringAlt("Restart", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 4:
                                StringsAcitons.DrawCenteredStringAlt("Shutdown", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (id)
                        {
                            case 1:
                                StringsAcitons.DrawCenteredStringAlt("Restart", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 2:
                                StringsAcitons.DrawCenteredStringAlt("Kill selected task", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 3:
                                StringsAcitons.DrawCenteredStringAlt("Kill last task", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 4:
                                StringsAcitons.DrawCenteredStringAlt("Kill all tasks", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 5:
                                StringsAcitons.DrawCenteredStringAlt("Ignore", 1920, 0, 650 + id * 30, 18, Color.White, Kernel.font18);
                                break;

                        }
                    }
                    break;
            }





        }

    }
}
