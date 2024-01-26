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
        public static uint screenSizeX, screenSizeY;

        public static Bitmap crash;

        public static bool Logged;
        public static void LogIn()
        {
            Logged = true;
            var folder_list = Directory.GetDirectories(@"0:\Users\");
            Kernel.loggedUser = folder_list[0];
            if(Kernel.render)
            {
                Apps.Process.Processes.RemoveAt(1);
				Explorer.drawIcons = true;
				DrawDesktopApps.UpdateIcons();
				Explorer.DrawTaskbar = true;
			}

			

        }

        public static void StartGui()
        {
            screenSizeX = Explorer.screenSizeX;
            screenSizeY = Explorer.screenSizeY;
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
                         canvas.DrawImage(crash, ((int)Explorer.screenSizeX-200)/2, 300);
						StringsAcitons.DrawCenteredStringAlt("An error occurred while running RadianceOS\nThe main system process has been stopped to prevent damage to you computer.\nRadianceOS " + Kernel.version + " - " + Kernel.subversion + "\n" + reason + "\nFor more information please visit szymekk.pl/RadianceOS/stop", (int)Explorer.screenSizeX, 0, 500, 18, Color.White, Kernel.font18);

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
                        canvas.DrawImage(crash, ((int)Explorer.screenSizeX-200)/2, 300);
                        StringsAcitons.DrawCenteredStringAlt("An error occurred while running RadianceOS\nThe main system process has been stopped to prevent damage to you computer.\nRadianceOS " + Kernel.version + " - " + Kernel.subversion + "\n" + reason + "\nFor more information please visit szymekk.pl/RadianceOS/stop", (int)Explorer.screenSizeX, 0, 500, 18, Color.White, Kernel.font18);

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
            if (Cosmos.System.MouseManager.X > ((int)Explorer.screenSizeX-300)/2 && Cosmos.System.MouseManager.X < ((int)Explorer.screenSizeX + 300) / 2)
            {
                if (Cosmos.System.MouseManager.Y > 745 + id * 30 && Cosmos.System.MouseManager.Y < 775 + id * 30)
                {
                    canvas.DrawFilledRectangle(Color.FromArgb(90, 0, 0), ((int)Explorer.screenSizeX - 300) / 2, 745 + id * 30, 300, 30);
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
                                            Kernel.render = false;
                                            Kernel.Repair = false;
                                            canvas.Disable();
                                            while(!System.Radiance.Security.Logged)
                                            {
												Console.ForegroundColor = ConsoleColor.Cyan;
												Console.Write("Password> ");
												Console.ForegroundColor = ConsoleColor.White;
												var input = Console.ReadLine();
												string myUser = @"0:\Users\" + Kernel.loggedUser + @"\";
												if (input == File.ReadAllText(myUser + @"AccountInfo\Password.SysData"))
												{
													System.Radiance.Security.Logged = true;
													Kernel.WriteLineOK("Logged as " + Kernel.loggedUser);
												}
												else
												{
													Kernel.WriteLineERROR("Incorrect password!");
												}
											}
                                            while(!Kernel.render && Kernel.Repair)
                                            {

												Console.Write(Kernel.path + ">");
												var input = Console.ReadLine();
												ConsoleCommands.RunCommand(input);
											}
                                            break;
                                        case 2:
                                            if(Kernel.Error == null)
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
                                            if (Apps.Process.Processes.Count > 1)
                                            {
                                                int toKill = Apps.Process.Processes.Count - 1;

                                                for (int i = 0; i < Apps.Process.Processes.Count; i++)
                                                {
                                                    if (Apps.Process.Processes[i].selected)
                                                        toKill = i;
                                                }
                                                Apps.Process.Processes.RemoveAt(toKill);
                                                Kernel.Repair = false;
                                                Kernel.render = true;
                                                Explorer.DrawTaskbar = true;
                                            }

                                            break;
                                        case 3:
                                            Apps.Process.Processes.RemoveAt(Apps.Process.Processes.Count - 1);
                                            Kernel.Repair = false;
                                            Kernel.render = true;
                                            Explorer.DrawTaskbar = true;
                                            break;
                                        case 4:
                                            Apps.Process.Processes.Clear();
                                            Processes start = new Processes
                                            {
                                                ID = -1,
                                            };
                                            Apps.Process.Processes.Add(start);
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
                    canvas.DrawFilledRectangle(Color.FromArgb(110, 0, 0), ((int)Explorer.screenSizeX - 300) / 2, 745 + id * 30, 300, 30);
            }
            else
                canvas.DrawFilledRectangle(Color.FromArgb(110, 0, 0), ((int)Explorer.screenSizeX - 300) / 2, 745 + id * 30, 300, 30);
            switch (State)
            {
                case 0:
                    {
                        switch (id)
                        {
                            case 0:
                                StringsAcitons.DrawCenteredStringAlt("Start troubleshooting", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 1:
                                StringsAcitons.DrawCenteredStringAlt("Open console mode", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 2:
                                StringsAcitons.DrawCenteredStringAlt("Reinstall RadianceOS", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 3:
                                StringsAcitons.DrawCenteredStringAlt("Restart", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 4:
                                StringsAcitons.DrawCenteredStringAlt("Shutdown", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (id)
                        {
                            case 1:
                                StringsAcitons.DrawCenteredStringAlt("Restart", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 2:
                                StringsAcitons.DrawCenteredStringAlt("Kill selected task", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 3:
                                StringsAcitons.DrawCenteredStringAlt("Kill last task", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 4:
                                StringsAcitons.DrawCenteredStringAlt("Kill all tasks", (int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;
                            case 5:
                                StringsAcitons.DrawCenteredStringAlt("Ignore",(int)Explorer.screenSizeX, 0, 750 + id * 30, 18, Color.White, Kernel.font18);
                                break;

                        }
                    }
                    break;
            }





        }

    }
}
