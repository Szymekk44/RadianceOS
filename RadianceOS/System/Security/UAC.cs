using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RadianceOS.System.Managment;
using System.Runtime.CompilerServices;
using Cosmos.System.Graphics;
using Cosmos.Core.Memory;

namespace RadianceOS.System.Security
{
    public static class UAC
    {
        // The User Account Control popup
        // I'll work on this later

        private static List<UACRequest> UACRequestQueue = new List<UACRequest>();

        static bool init = false;

        public static void Render()
        {
            if (UACRequestQueue.Count == 0) return;

            UACRequest currentRequest = UACRequestQueue[0];

            // Explorer.CanvasMain.DrawImage(new Bitmap(Files.wallpaperL), 0, 0); // Temporarily disabled because it causes an extreme lag spike

            if(!init)
            {
                Explorer.DrawTaskbar = false;
                Explorer.DrawMenu = false;
                Explorer.drawIcons = false;

                Service.PauseAllProcesses();
                init = true;
            }

            int width = 400;
            int height = 450;
            int x = (int)((Explorer.screenSizeX / 2) - (width / 2));
            int y = (int)((Explorer.screenSizeY / 2) - (height / 2));

            Window.DrawFullRoundedRectangle(x, y, width, height, 5, Kernel.main);
            //Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, x, y, width, height);

            StringsAcitons.DrawCenteredTTFString("User Access Control", width, x, y + 40, 1, Color.White, "UMB", 25);

            switch(currentRequest.UACRequestType)
            {
                case UACRequestType.ApplicationElevation:
                    ApplicationElevation elevation = (ApplicationElevation)currentRequest;

                    StringsAcitons.DrawCenteredTTFString(elevation.Process.Name + " is requesting elevation", width, x, y + 40 + 25 + 10, 1, Color.White, "UMR", 15);

                    StringsAcitons.DrawCenteredTTFString("If you allow this, " + elevation.Process.Name + " will be able to:", width, x, y + 40 + 25 + 10 + 15, 1, Color.White, "UMR", 15);
                    StringsAcitons.DrawCenteredTTFString("Access system files", width, x, y + 40 + 25 + 10 + (15 * 2), 1, Color.White, "UMR", 15);
                    StringsAcitons.DrawCenteredTTFString("Make changes to the computer", width, x, y + 40 + 25 + 10 + (15 * 3), 1, Color.White, "UMR", 15);

                    // Yes
                    Window.DrawFullRoundedRectangle(x + 10, y + height - 30, width - 20, 20, 5, Kernel.lightMain);
                    if(IsCursorInArea(Explorer.MX, Explorer.MY, x + 10, y + height - 30, width - 20, 20))
                    {
                        Window.DrawFullRoundedRectangle(x + 10, y + height - 30, width - 20, 20, 5, Kernel.middark);
                    }
                    StringsAcitons.DrawCenteredString("Yes", width, x + 10, y + height - 30 + 5, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                    // No
                    Window.DrawFullRoundedRectangle(x + 10, y + height - 30 - 30, width - 20, 20, 5, Kernel.lightMain);
                    if(IsCursorInArea(Explorer.MX, Explorer.MY, x + 10, height - 30 - 30, width - 20, 20))
                    {
                        Window.DrawFullRoundedRectangle(x + 10, y + height - 30 - 30, width - 20, 20, 5, Kernel.middark);
                    }
                    StringsAcitons.DrawCenteredString("No", width, x + 10, y + height - 30 - 30 + 5, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                    // Kill application
                    Window.DrawFullRoundedRectangle(x + 10, y + height - 30 - 60, width - 20, 20, 5, Kernel.lightMain);
                    if(IsCursorInArea(Explorer.MX, Explorer.MY, x + 10, y + height - 30 - 60, width - 20, 20))
                    {
                        Window.DrawFullRoundedRectangle(x + 10, y + height - 30 - 60, width - 20, 20, 5, Kernel.middark);
                    }
                    StringsAcitons.DrawCenteredString("Kill Application", width, x + 10, y + height - 30 - 60 + 5, 1, Color.White, Cosmos.System.Graphics.Fonts.PCScreenFont.Default);

                    break;
                case UACRequestType.UserElevation:

                    break;
                case UACRequestType.UserAdminRequest:

                    break;
            }

            Heap.Collect();
        }

        // By ChatGPT, makes it easier to detect if the cursor is in the area.
        public static bool IsCursorInArea(int cursorX, int cursorY, int areaLeft, int areaTop, int areaWidth, int areaHeight)
        {
            return cursorX >= areaLeft &&
                   cursorX <= areaLeft + areaWidth &&
                   cursorY >= areaTop &&
                   cursorY <= areaTop + areaHeight;
        }

        public static void RequestUAC(UACRequest request)
        {
            UACRequestQueue.Add(request);
        }

        public class UACRequest
        {
            public Action<UACResult> RequestComplete { get; set; }
            public UACResult Result { get; set; }
            private bool Complete = false;
            public UACRequestType UACRequestType { get; private set; }
        }

        public enum UACRequestType
        {
            ApplicationElevation = 0,
            UserElevation = 1,
            UserAdminRequest = 2
        }

        public class UACResult
        {
            public bool Success { get; private set; }
        }

        public class ApplicationElevation : UACRequest
        {
            public ProcessSecurityLevel RequestedLevel { get; private set; }
            public ProcessSecurityLevel CurrentLevel { get; private set; }
            public Processes Process { get; private set; }

            public ApplicationElevation(Processes process, ProcessSecurityLevel requestedLevel)
            {
                Process = process;
                CurrentLevel = process.ProcessSecurityInfo.Level;
                RequestedLevel = requestedLevel;
            }
        }
        public class ApplicationElevationResult : UACResult
        {
            public bool Success { get; private set; } = false;
        }

        public class UserElevation : UACRequest
        {
            public Session.UserLevel RequestingUserLevel { get; private set; }
            public UserElevation(Session.UserLevel requestingUserLevel,
                Action<UACResult> complete = null)
            {
                RequestingUserLevel = requestingUserLevel;
                RequestComplete = complete;
                Result = new UserElevationResult();
            }
        }

        public class UserElevationResult : UACResult
        {

        }

        public class UserAdminRequest : UACRequest
        {
            
        }
        public class UserAdminRequestResult : UACResult
        {

        }
    }
}
