using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cosmos.System.Graphics;
using Cosmos.HAL;

namespace RadianceOS.System.Screens
{
    public static class Shutdown
    {
        public static bool InShutdown = false;
        public static int ShutdownCode = 0;
        private static int ShutdownState = 0;

        public static char[] SpinnerChars = "|/-\\".ToCharArray();

        public static int Frame = 0;
        public static int FrameI = 0;
        public static int FrameS = 0;

        public static Bitmap LoginBitmap = new Bitmap(Files.wallpaperL);
        public static void StartShutdown(int? code = 0)
        {
            Apps.Process.Processes.Clear();

            Explorer.CanvasMain.Clear();
            Explorer.drawIcons = false;
            Explorer.DrawMenu = false;
            Explorer.DrawTaskbar = false;
            Explorer.DrawCursor = false;

            InShutdown = true;
            ShutdownCode = (int)(code != null ? code : 0);

            Render();
        }

        public static void Render()
        {
            if (!InShutdown) return;

            Frame++;

            if(Frame == 25)
            {
                Frame = 0;
                FrameI++;
                if(FrameI >= SpinnerChars.Length)
                {
                    FrameI = 0;
                }

                FrameS++;
            }

            //Explorer.CanvasMain.Clear(Kernel.main);
            Explorer.CanvasMain.DrawImage(LoginBitmap, 0, 0);

            StringsAcitons.DrawCenteredTTFString("Shutting down...", (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - ("Shutting down...".Length * 20 / 2)), 1, Color.White, "UMB", 20);

            if(ShutdownState == 0)
            {
                StringsAcitons.DrawCenteredTTFString(SpinnerChars[FrameI].ToString(), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - 10) + 30, 1, Color.White, "UMB", 20);
            } else if(ShutdownState == 1)
            {
                StringsAcitons.DrawCenteredTTFString("Waiting on ACPI shutdown... " + SpinnerChars[FrameI].ToString(), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - 10) + 30, 1, Color.White, "UMB", 20);
            }

            if(FrameS == 25)
            {
                ShutdownState = 1;
            } else if(FrameS == 30)
            {
                Power.ACPIShutdown();
            }
        }
    }
}
