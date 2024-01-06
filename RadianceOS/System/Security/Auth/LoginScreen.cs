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

namespace RadianceOS.System.Security.Auth
{
    public static class LoginScreen
    {
        public static bool Clicked;
        public static bool Closed = true; // Whether or not to display the clock or the login screen

        /// <summary>
        /// The Render function for rendering the login screen.
        /// </summary>
        /// <param name="X">Not needed (Set to 0)</param>
        /// <param name="Y">Not needed (Set to 0)</param>
        /// <param name="SizeX">The width of the screen</param>
        /// <param name="SizeY">The height of the screen</param>
        /// <param name="i">The process ID</param>
        public static void Render(int X, int Y, int SizeX, int SizeY, int i)
        {
            try
            {
                InputSystem.Monitore(0, Process.Processes[i].CurrChar, i);

                // TODO: Fix the below code
                //Explorer.CanvasMain.DrawImageAlpha(Kernel.Wallpaper1, 0, 0);
                //Window.GetTempImageDarkAndBlur(0, 0, SizeX, SizeY, "Wallpaper", 0.5f, 3);
                //Kernel.TaskBar1 = Window.tempBitmap;

                // Temp
                // Was middark, not main
                Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(127, Kernel.main), 0, 0, SizeX, SizeY);

                // Testing how to centre properly
                //Explorer.CanvasMain.DrawImage(Kernel.padlockIcon, (SizeX / 2) - (int)Kernel.padlockIcon.Width / 2, (SizeY / 2) - (int)Kernel.padlockIcon.Height / 2);

                Explorer.CanvasMain.DrawImageAlpha(Kernel.standbysmall, (int)(SizeX - Kernel.standbysmall.Width - 3), 3);

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

                        StringsAcitons.DrawCenteredTTFString("Press any key to unlock", SizeX, 0, (SizeY / 2) - 21 / 2 + 159, 1, Color.Gray, "UMR", 21);

                        Explorer.CanvasMain.DrawImageAlpha(Kernel.RadianceOSLogoTransparent, (SizeX / 2) - (int)Kernel.RadianceOSLogoTransparent.Width / 2, (int)(SizeY - Kernel.RadianceOSLogoTransparent.Height - 15));

                        if (string.IsNullOrEmpty(InputSystem.CurrentString)) Closed = false; InputSystem.CurrentString = "";
                        break;
                    case false:
                        RenderCurrentUser(SizeX, SizeY);
                        RenderOtherUsers(SizeX, SizeY);

                        break;
                }
            }
            catch (Exception e)
            {
                MessageBoxCreator.CreateMessageBox("Fatal Error", "Please open RadianceOS console mode!\n" + e.Message, MessageBoxCreator.MessageBoxIcon.error, 600, 175);
            }
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

            return DateTime.Now.ToString("dd/MM/yyyy");
        }

        private static void RenderCurrentUser(int SizeX, int SizeY)
        {
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
        }

        private static void RenderOtherUsers(int SizeX, int SizeY)
        {

        }
    }
}
