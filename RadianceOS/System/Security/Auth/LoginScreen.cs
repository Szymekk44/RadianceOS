using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RadianceOS.System.Graphic;
using CosmosTTF;
using System.Dynamic;
using Cosmos.System.Graphics;
using System.IO;

namespace RadianceOS.System.Security.Auth
{
    public static class LoginScreen
    {
        public static bool Clicked;
        public static bool Closed = true; // Whether or not to display the clock or the login screen
        public static bool LoggingIn = false;
        public static bool Doing = false;

        // 0 = None (Displays "Logging in..."
        // 1 = Incorrect password
        // 2 = Needs Force (UAC popup)
        // 3 = Access Denied (Can't log into the system, use console mode)
        // 4 = Doesn't exist
        public static int LoggingInMode = 0;

        public static bool IsPowerClicked = false;

        /// <summary>
        /// The Render function for rendering the login screen.
        /// </summary>
        /// <param name="X">Not needed (Set to 0)</param>
        /// <param name="Y">Not needed (Set to 0)</param>
        /// <param name="SizeX">The width of the screen</param>
        /// <param name="SizeY">The height of the screen</param>
        /// <param name="i">The process ID</param>
        /// 
        public static bool set;
        public static Bitmap shadowLogo;
        static int width, height;
        public static void Render(int X, int Y, int SizeX, int SizeY, int i)
        {
            try
            {
                InputSystem.Monitore(0, Process.Processes[i].CurrChar, i);
               
                // TODO: Fix the below code (Won't blur correctly)
                //Explorer.CanvasMain.DrawImageAlpha(Kernel.Wallpaper1, 0, 0);
                if(!set)
                {
                    set = true;
					if (File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper6.bmp"))
						Kernel.Wallpaper1 = new Bitmap(File.ReadAllBytes(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper6.bmp"));
					else
					{
                        Kernel.WallpaperNotFound(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper6.bmp");
					}
				}
                if(shadowLogo == null)
                {
                   shadowLogo = new Bitmap(Files.RadianceOSIconShadow);
                    height = (int)shadowLogo.Height;
                    width = (int)shadowLogo.Width;
				}

                int x = (int)(SizeX - Kernel.standbysmall.Width - 5);
                int y = (int)(Kernel.standbysmall.Height + 5);
                int radius = (int)(Kernel.standbysmall.Width);

              //  Explorer.CanvasMain.DrawFilledCircle(Kernel.lightMain, x, y, radius);
                if(IsCursorInArea(Explorer.MX, Explorer.MY, x, y, radius, radius))
                {
                  //  Explorer.CanvasMain.DrawFilledCircle(Kernel.dark, x, y, radius);
                    if(Explorer.Clicked && !IsPowerClicked)
                    {
                        IsPowerClicked = true;

                        Processes processes = new Processes()
                        {
                            Name = "Power options",
                            ID = 99,
                            X = (int)((Explorer.screenSizeX / 2) - 600),
                            Y = (int)((Explorer.screenSizeY / 2) - 200),
                            SizeX = 600,
                            SizeY = 200,
                        };
                        Process.Processes.Add(processes);
                    }
                } else if(IsPowerClicked)
                {
                    IsPowerClicked = false;
                }
                Explorer.CanvasMain.DrawImageAlpha(Kernel.standbysmall, x - 12, y - 12);

                // (int)(SizeX - Kernel.standbysmall.Width - (6 + 24))
                //Explorer.CanvasMain.DrawFilledCircle(Kernel.lightMain, (int)(SizeX - Kernel.standbysmall.Width - 7 - 24) + 12, 6 + 24, (int)Kernel.standbysmall.Width);
                //Explorer.CanvasMain.DrawImageAlpha(Kernel.standbysmall, (int)(SizeX - Kernel.standbysmall.Width - 7 - 24) - 5, 7 + 5);

                if (Clicked)
                {
                    if(Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
                    {
                        Clicked = false;
                    }
                }

                switch (Closed)
                {
                    case true:
                        // Was CR now UMR
                        //Explorer.CanvasMain.DrawStringTTF(FetchTime(), "UMB", Color.White, 163, (SizeX / 2) - (163 / 2 * (FetchTime().Length / 2)), (SizeY / 2) - 163 / 2 - 100);
                        //Explorer.CanvasMain.DrawStringTTF(FetchDate(), "UMR", Color.White, 82, (SizeX / 2) - (82 / 2 * (FetchDate().Length / 2)), (SizeY / 2) - 82 / 2 - 19);
                        StringsAcitons.DrawCenteredTTFString(FetchTime(), SizeX, 0, (SizeY / 2) - 163 / 2 - 100, 1, Color.White, "UMB", 163);
                        StringsAcitons.DrawCenteredTTFString(FetchDate(), SizeX, 0, (SizeY / 2) - 41 / 2 - 59, 1, Color.White, "UMR", 41);

                        StringsAcitons.DrawCenteredTTFString("Press any key or click to unlock", SizeX, 0, (SizeY / 2) - 21 / 2 + 159, 1, Color.Gray, "UMR", 21);

                        Explorer.CanvasMain.DrawImageAlpha(shadowLogo, (SizeX / 2) - (int)width / 2 + 5, (int)(SizeY - height - 15));

                        if (!string.IsNullOrEmpty(InputSystem.CurrentString) || Explorer.Clicked) Closed = false; InputSystem.CurrentString = "";
                        break;
                    case false:
                        if(LoggingIn)
                        {
                            switch (LoggingInMode)
                            {
                                case 0:
                                    StringsAcitons.DrawCenteredTTFString("Logging in...", SizeX, 0, (SizeY / 2) - 163 / 3 / 4, 1, Color.White, "UMB", (163 / 3));
                                    break;
                                case 1:
                                    LoggingIn = false;
                                    // Not needed (May be in the future), just a popup for now.
                                    break;
                                case 2:
                                    StringsAcitons.DrawCenteredTTFString("Someone else is already logged in, would you like to log them out?", SizeX, 0, SizeY / 2, 1, Color.White, "UMR", 163 / 6);
                                    
                                    int inputX = SizeX / 2 - (250 / 2);
                                    int inputY = SizeY / 2 - (25 / 2);
                                    int inputW = 250;
                                    int inputH = 25;
                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, inputX, inputY + 20, inputW, inputH);
                                    Bitmap UACShield = new Bitmap(Files.UACShield16);
                                    Explorer.CanvasMain.DrawImageAlpha(UACShield, inputX, inputY + 20);
                                    StringsAcitons.DrawCenteredTTFString("Login", SizeX, 0, (int)(inputY + 37 + UACShield.Width), 1, Color.White, "UMR", 17);

                                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, inputX, inputY + 40, inputW, inputH);
                                    StringsAcitons.DrawCenteredTTFString("Cancel", SizeX, 0, inputY + 57, 1, Color.White, "UMR", 17);

                                    if (Explorer.MY > SizeY / 2 - (25 / 2) + 17 && Explorer.MY < SizeY / 2 - (25 / 2) + 17 + 25)
                                    {
                                        if (Explorer.MX > SizeX / 2 - (250 / 2) && Explorer.MX < SizeX / 2 - (250 / 2) + 250)
                                        {
                                            if(Explorer.Clicked && !Doing)
                                            {
                                                Doing = true;
                                                Session.ElevateUser(() =>
                                                {
                                                    Session.Authenticate(Kernel.loggedUser, InputSystem.CurrentString, true);
                                                });
                                            }
                                        }
                                    }

                                    if (Explorer.MY > SizeY / 2 - (25 / 2) + 17 && Explorer.MY < SizeY / 2 - (25 / 2) + 17 + 25)
                                    {
                                        if (Explorer.MX > SizeX / 2 - (250 / 2) && Explorer.MX < SizeX / 2 - (250 / 2) + 250)
                                        {
                                        }
                                    }

                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                default:
                                    LoggingIn = false;
                                    break;
                            }
                        } else
                        {
                            RenderCurrentUser(SizeX, SizeY);
                            RenderOtherUsers(SizeX, SizeY);
                        }

                        break;
                }
            }
            catch (Exception e)
            {

                MessageBoxCreator.CreateMessageBox("Fatal Error", "Please open RadianceOS in console mode!\n" + e.Message, MessageBoxCreator.MessageBoxIcon.error, 600, 175);
                Kernel.Crash("Login failed!\nTry to login in console mode!\nException: " + e.Message, 100, 0);
            }
        }

        public static void Reinitialise()
        {
            Clicked = false;
            Closed = true;
            LoggingIn = false;
            Doing = false;
            LoggingInMode = 0;
        }

        public static string FetchTime()
        {
            // There may be more in the future such as different formats, etc...
            // Possible formats:
            // * HH:mm:ss - 24 hour format with seconds
            // * hh:mm:ss - 12 hour format (Without AM/PM)
            // * HH:mm    - 24 hour format without seconds (Default)
            // * hh:mm    - 12 hour format (Without AM/PM) without seconds

            return DateTime.Now.ToString("HH:mm");
        }

        public static string FetchDate()
        {
            // There may be more in the future such as different formats, etc...
            // Possible formats:
            // * dd/MM/yyyy - Self explanatory (Default)
            // * MM/dd/yyyy - Still self explanatory
            // * yyyy/MM/dd - I don't know why

            return DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private static void RenderCurrentUser(int SizeX, int SizeY)
        {
            if (Explorer.MY > SizeY / 2 - (25 / 2) + 17 && Explorer.MY < SizeY / 2 - (25 / 2) + 17 + 25)
            {
                if(Explorer.MX > SizeX / 2 - (250 / 2) && Explorer.MX < SizeX / 2 - (250 / 2) + 250)
                {
                    if(Explorer.Clicked && !LoggingIn)
                    {
                        Explorer.Clicked = false;
                        LoggingIn = true;
                        if(string.IsNullOrEmpty(InputSystem.CurrentString))
                        {
                            MessageBoxCreator.CreateMessageBox("Invalid input", "Please input a password to login.", MessageBoxCreator.MessageBoxIcon.warning, 400);
                            LoggingIn = false;
                        } else
                        {
                            int output = Session.Authenticate(Kernel.loggedUser, InputSystem.CurrentString);
                            if(output == 0)
                            {
                                //MessageBoxCreator.CreateMessageBox("Logged in successfully", "?");
                            } else if(output == 1)
                            {
                                MessageBoxCreator.CreateMessageBox("Incorrect password", "The password you input is incorrect", MessageBoxCreator.MessageBoxIcon.error, 500);
                                LoggingInMode = 1;
                            } else if(output == 2)
                            {
                                MessageBoxCreator.CreateMessageBox("User not found", "The user you are attempting to log into does not exist", MessageBoxCreator.MessageBoxIcon.error, 600);
                            } else if(output == 3)
                            {
                                LoggingInMode = 2;
                                //Session.Authenticate(Kernel.loggedUser, InputSystem.CurrentString, true);
                            } else
                            {
                                MessageBoxCreator.CreateMessageBox("Something happened", "?");
                            }
                        }
                    }
                }
            }

            StringsAcitons.DrawCenteredTTFString(Kernel.loggedUser, SizeX, 0, (SizeY / 2) - 50, 1, Color.White, "UMR", 50);
            string result;
            if(InputSystem.CurrentString.Length > 0)
            {
                result = InputSystem.CurrentString;
            }
            else
            {
                result = "";
            }
            int inputX = SizeX / 2 - (250 / 2);
            int inputY = SizeY / 2 - (25 / 2);
            int inputW = 250;
            int inputH = 25;
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, inputX, inputY, inputW, inputH);
            if(string.IsNullOrEmpty(result))
            {
                Explorer.CanvasMain.DrawStringTTF("Password", "UMR", Color.Gray, 17, inputX + 7, inputY + 17);
            } else
            {
                Explorer.CanvasMain.DrawStringTTF(new string('•', result.Length) + "|", "UMR", Color.White, 17, inputX + 7, inputY + 17);
            }

            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, inputX, inputY + 20, inputW, inputH);
            StringsAcitons.DrawCenteredTTFString("Login", SizeX, 0, inputY + 37, 1, Color.White, "UMR", 17);
        }

        private static void RenderOtherUsers(int SizeX, int SizeY)
        {

        }

        // By ChatGPT, makes it easier to detect if the cursor is in the area.
        public static bool IsCursorInArea(int cursorX, int cursorY, int areaLeft, int areaTop, int areaWidth, int areaHeight)
        {
            return cursorX >= areaLeft &&
                   cursorX <= areaLeft + areaWidth &&
                   cursorY >= areaTop &&
                   cursorY <= areaTop + areaHeight;
        }
    }
}
