using Cosmos.HAL;
using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Screens
{
    public static class Restart
    {
        public static bool InRestart = false;
        public static int RestartCode = 0;
        private static int RestartState = 0;

        public static char[] SpinnerChars = "|/-\\".ToCharArray();

        public static int Frame = 0;
        public static int FrameI = 0;
        public static int FrameS = 0;

        public static Bitmap LoginBitmap = new Bitmap(Files.wallpaperL);
        public static void StartRestart(int? code = 0)
        {
            Apps.Process.Processes.Clear();

            Explorer.CanvasMain.Clear();
            Explorer.drawIcons = false;
            Explorer.DrawMenu = false;
            Explorer.DrawTaskbar = false;
            Explorer.DrawCursor = false;

            InRestart = true;
            RestartCode = (int)(code != null ? code : 0);

            Render();
        }

        public static void Render()
        {
            if (!InRestart) return;

            Frame++;

            if (Frame == 15)
            {
                Frame = 0;
                FrameI++;
                if (FrameI >= SpinnerChars.Length)
                {
                    FrameI = 0;
                }

                FrameS++;
            }

            //Explorer.CanvasMain.Clear(Kernel.main);
            Explorer.CanvasMain.DrawImage(LoginBitmap, 0, 0);

            StringsAcitons.DrawCenteredTTFString("Restarting...", (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - ("Restarting...".Length * 20 / 2)), 1, Color.White, "UMB", 20);

            if (RestartState == 0)
            {
                StringsAcitons.DrawCenteredTTFString(SpinnerChars[FrameI].ToString(), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - 10) + 30, 1, Color.White, "UMB", 20);
            }
            else if (RestartState == 1)
            {
                StringsAcitons.DrawCenteredTTFString("Waiting on CPU Reboot... " + SpinnerChars[FrameI].ToString(), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - 10) + 30, 1, Color.White, "UMB", 20);
            } else if(RestartState == 2)
            {
                StringsAcitons.DrawCenteredTTFString("Waiting on recovery... " + SpinnerChars[FrameI].ToString(), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - 10) + 30, 1, Color.White, "UMB", 20);
            }

            if (FrameS == 25)
            {
                if(InputSystem.Shift)
                {
                    RestartState = 2;
                }

                RestartState = 1;
            }
            else if (FrameS == 130 && RestartState != 2)
            {
                Power.CPUReboot();
            }
        }
    }
}
