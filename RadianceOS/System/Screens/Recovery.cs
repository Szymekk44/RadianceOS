using Cosmos.HAL;
using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RadianceOS.System.Screens
{
    public static class Recovery
    {
        public static bool InRecovery = false;
        public static int RecoveryState = 0;
        public static void StartRecovery()
        {
            Apps.Process.Processes.Clear();

            Explorer.CanvasMain.Clear();
            Explorer.drawIcons = false;
            Explorer.DrawMenu = false;
            Explorer.DrawTaskbar = false;
            Explorer.DrawCursor = false;

            InRecovery = true;

            Security.Auth.Session.Logout();

            Thread.Sleep(100);

            Explorer.CanvasMain.Clear();
        }

        public static void Render()
        {
            if (!InRecovery) return;

            Explorer.CanvasMain.Clear();

            if(RecoveryState == 0)
            {
                StringsAcitons.DrawCenteredString("RadianceOS", (int)Explorer.screenSizeX, 0, 0, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
                StringsAcitons.DrawCenteredString("Starting recovery...", (int)Explorer.screenSizeX, 0, 15, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                Security.Auth.Session.BruteAuthenticate("Recovery/" + Guid.NewGuid().ToString(), Security.Auth.Session.UserLevel.Root);
                RecoveryState = 1;
            } else if(RecoveryState == 1)
            {
                StringsAcitons.DrawCenteredString("RadianceOS", (int)Explorer.screenSizeX, 0, 0, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
                StringsAcitons.DrawCenteredString("Starting recovery...", (int)Explorer.screenSizeX, 0, 15, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
                StringsAcitons.DrawCenteredString("Authenticated as " + Security.Auth.Session.UserName, (int)Explorer.screenSizeX, 0, 30, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                Explorer.CanvasMain.Display();
                Thread.Sleep(100);

                RecoveryState = 2;
            } else if(RecoveryState == 2)
            {
                Explorer.CanvasMain.Clear(Kernel.main);
                Explorer.CanvasMain.Display();

                Explorer.DrawCursor = true;

                Thread.Sleep(1500);
                RecoveryState = 3;
            } else if(RecoveryState == 3)
            {
                Explorer.CanvasMain.Clear(Kernel.main);

                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, 10, 10, (int)(Explorer.screenSizeX - 20), 25);
                Explorer.CanvasMain.DrawString("RadianceOS Recovery - " + Security.Auth.Session.UserName, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, Color.White, 15, 15);
                Explorer.CanvasMain.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Cosmos.System.Graphics.Fonts.PCScreenFont.Default, Color.White,
                    (int)(Explorer.screenSizeX - (DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Length * 9)), 15);

                int tempA_w = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Length * 9;

                // Shutdown
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5), 10, 150, 25);
                if(IsCursorInArea(Explorer.MX, Explorer.MY, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5), 10, 150, 25))
                {
                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5), 10, 150, 25);
                    if (Explorer.SingleClick) Power.ACPIShutdown();
                }
                StringsAcitons.DrawCenteredString("Shutdown", 150, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5), 15, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                // Reboot
                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5 - 150 - 5), 10, 150, 25);
                if (IsCursorInArea(Explorer.MX, Explorer.MY, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5 - 150 - 5), 10, 150, 25))
                {
                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5 - 150 - 5), 10, 150, 25);
                    if (Explorer.SingleClick) Power.CPUReboot();
                }
                StringsAcitons.DrawCenteredString("Restart", 150, (int)(Explorer.screenSizeX - 20 - tempA_w - 150 - 5 - 150 - 5), 15, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, 10, 35 + 10, 500, 150);
                if(IsCursorInArea(Explorer.MX, Explorer.MY, 10, 35 + 10, 500, 150))
                {
                    Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, 10, 35 + 10, 500, 150);

                    if(Explorer.SingleClick)
                    {
                        Thread.Sleep(100);

                        Explorer.CanvasMain.Clear(Kernel.main);
                        Explorer.CanvasMain.Display();

                        Thread.Sleep(100);

                        Explorer.CanvasMain.Clear();

                        StringsAcitons.DrawCenteredString("RadianceOS", (int)Explorer.screenSizeX, 0, 0, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
                        StringsAcitons.DrawCenteredString("Restarting...", (int)Explorer.screenSizeX, 0, 15, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                        Explorer.CanvasMain.Display();

                        Thread.Sleep(1500);

                        Explorer.DrawCursor = true;

                        Power.CPUReboot();
                    }
                }
                StringsAcitons.DrawCenteredTTFString("Continue to RadianceOS", 500 / 2, 10 + 15, 35 + (10 + (150 / 2)), 1, Color.White, "UMR", 20);
            }
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
