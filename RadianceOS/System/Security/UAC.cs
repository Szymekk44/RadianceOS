using RadianceOS.System.Apps;
using RadianceOS.System.Graphic;
using RadianceOS.System.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);

            if(!init)
            {
                Explorer.DrawTaskbar = false;
                Explorer.DrawMenu = false;
                Explorer.drawIcons = false;

                Service.PauseAllProcesses();
                init = true;
            }

            int width = 200;
            int height = 450;
            int x = (int)((Explorer.screenSizeX / 2) - width);
            int y = (int)((Explorer.screenSizeY / 2) - height);
            Window.DrawFullRoundedRectangle(x, y, width, height, 5, Kernel.main);
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
        }

        public class UACResult
        {
            public bool Success { get; private set; }
        }

        public class ApplicationElevation : UACRequest
        {
            public ProcessSecurityLevel RequestedLevel { get; private set; }
            public ProcessSecurityLevel CurrentLevel { get; private set; }
            Processes Process;

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
