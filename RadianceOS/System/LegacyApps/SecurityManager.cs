using CosmosTTF;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RadianceOS.System.Security.Auth;
using System.Diagnostics;

namespace RadianceOS.System.Apps
{
    public static class SecurityManager
    {
        public static int SelectedPage = 0;
        private static bool Initialised = false;
        private static int InitLoop = 0;
        private static bool RequestRerender = false;
        public static void Render(int X, int Y, int SizeX, int SizeY, int i)
        {
            Window.DrawTop(i, X, Y, SizeX, "Security Manager", false, true, true);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 3, Y + 28, SizeX, SizeY - 25);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 25, 200, SizeY - 25);

            Process.Processes[i].metaData = "Security Manager";

            if (Process.Processes[i].bitmap == null || !Initialised || RequestRerender)
            {
                RenderPage(SelectedPage, X, Y, SizeX, SizeY, i);
                Window.GetImage(X, Y, SizeX, SizeY, i, "Security Manager");
            }
            else
                Explorer.CanvasMain.DrawImage(Process.Processes[i].bitmap, X, Y + 25);

            RenderButtons(SelectedPage, X, Y, SizeX, SizeY, i, Explorer.Clicked);

            if (!Initialised)
            {
                InitLoop++;
                SelectedPage = 1;
                if(Session.CurrentUserLevel == Session.UserLevel.User && InitLoop == 1)
                {
                    MessageBoxCreator.CreateMessageBox("Security Manager", "You do not have permission to access Security Manager, attempting to elevate your permissions...", MessageBoxCreator.MessageBoxIcon.info, 875);
                    Process.Processes[i].ProcessSecurityInfo = new Security.ProcessSecurityInfo("Security Manager", "RadianceOS Security Manager", "RadianceOS", true);
                    Process.Processes[i].ProcessSecurityInfo.AttemptElevation(Process.Processes[i], (Security.UAC.UACResult? result) =>
                    {

                    }, (Security.UAC.UACResult? result) => { Process.Processes.RemoveAt(i); });
                    Session.ElevateUser(() =>
                    {
                        Initialised = true;
                    });
                } else
                {
                    Initialised = true;
                }
                return;
            }
        }

        private static void Rerender()
        {
            RequestRerender = true;
        }

        public static void RenderPage(int page, int X, int Y, int SizeX, int SizeY, int i)
        {
            switch (page)
            {
                case 1:
                    {
                        Explorer.CanvasMain.DrawStringTTF("User/Session Status", "UMB", Color.White, 35, X + 210, Y + 20 + 35);
                        Explorer.CanvasMain.DrawStringTTF("Session Username: " + Session.UserName, "UMB", Color.White, 20, X + 210, Y + 20 + 35 + (15 * 2));
                        Explorer.CanvasMain.DrawStringTTF("Session Authenticated At: " + Session.AuthenticatedAt, "UMB", Color.White, 20, X + 210, Y + 20 + 35 + (15 * 3));
                        Explorer.CanvasMain.DrawStringTTF("Session User Level: " + (Session.CurrentUserLevel == Session.UserLevel.User ? "User" : Session.CurrentUserLevel == Session.UserLevel.Administrator ? "Administrator" : "Root"), "UMB", Color.White, 20, X + 210, Y + 20 + 35 + (15 * 4));
                        Explorer.CanvasMain.DrawStringTTF("Kernel Username: " + Kernel.loggedUser, "UMB", Color.White, 20, X + 210, Y + 20 + 35 + (15 * 5));
                    }
                    break;
            }
        }

        public static void RenderButtons(int id, int X, int Y, int SizeX, int SizeY, int i, bool clicked)
        {
            switch(id)
            {
                case 1:
                    // Logout
                    Window.DrawFullRoundedRectangle(X + 210, Y + 20 + 35 + (15 * 6), 100, 20, 5, Kernel.lightMain);
                    if(IsCursorInArea(Explorer.MX, Explorer.MY, X + 210, Y + 20 + 35 + (15 * 6), 100, 20))
                    {
                        Window.DrawFullRoundedRectangle(X + 210, Y + 20 + 35 + (15 * 6), 100, 20, 5, Kernel.middark);
                        if(Explorer.Clicked)
                        {
                            Session.Logout();
                        }
                    }
                    StringsAcitons.DrawCenteredString("Logout", 100, X + 210, Y + 20 + 35 + (15 * 6) + 3, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                    // Shut down
                    Window.DrawFullRoundedRectangle(X + 210 + 110, Y + 20 + 35 + (15 * 6), 100, 20, 5, Kernel.lightMain);
                    if (IsCursorInArea(Explorer.MX, Explorer.MY, X + 210 + 110, Y + 20 + 35 + (15 * 6), 100, 20))
                    {
                        Window.DrawFullRoundedRectangle(X + 210 + 110, Y + 20 + 35 + (15 * 6), 100, 20, 5, Kernel.middark);
                        if (Explorer.Clicked)
                        {
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
                    }
                    StringsAcitons.DrawCenteredString("Power options", 100, X + 210 + 100, Y + 20 + 35 + (15 * 6) + 3, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);
                    break;
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
