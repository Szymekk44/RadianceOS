using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RadianceOS.System
{
    public static class Sleep
    {
        // Doesn't work, don't use it.

        // This makes use of only needing to render once
        // This will help power-save as it barely uses any power and basically idles the machine.
        public static bool IsAwake = false; // Shows the time and to press any key to start back up
        public static DateTime? WasAwake = null;

        public static int OMX = 0;
        public static int OMY = 0;
        public static void StartSleep()
        {
            return;

            Explorer.CanvasMain.Clear();

            OMX = Explorer.MX;
            OMY = Explorer.MY;

            Kernel.IsSleeping = true;

            IsAwake = true;
            WasAwake = DateTime.Now;
        }

        public static void Render()
        {
            return;

            Explorer.MX = (int)Cosmos.System.MouseManager.X;
            Explorer.MY = (int)Cosmos.System.MouseManager.Y;

            if (IsAwake)
            {
                Explorer.CanvasMain.Clear();
                StringsAcitons.DrawCenteredTTFString(DateTime.Now.ToString("HH:mm"), (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY / 2) - (163 / 2)), 1, Color.White, "UMB", 163);

                if(WasAwake >= WasAwake.Value.AddSeconds(10))
                {
                    //Explorer.CanvasMain.Clear();
                    //IsAwake = false;
                    WasAwake = null;
                }

                if (!string.IsNullOrEmpty(InputSystem.CurrentString))
                {
                    // Wake up from sleep
                    Kernel.IsSleeping = false;
                }

                RadianceOS.Render.Canvas.DrawImageAlpha(Kernel.Cursor1, Explorer.MX, Explorer.MY);
            } else
            {
                if(OMX != Explorer.MX || OMY != Explorer.MY)
                {
                    IsAwake = true;
                    WasAwake = DateTime.Now;
                } else if(!string.IsNullOrEmpty(InputSystem.CurrentString))
                {
                    IsAwake = true;
                    WasAwake = DateTime.Now;
                }

                OMX = Explorer.MX;
                OMY = Explorer.MY;
            }
        }
    }
}
