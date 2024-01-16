using RadianceOS.System.Graphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
    public static class PowerOptions
    {
        public static void Render(int X, int Y, int SizeX, int SizeY, int i)
        {
            Window.DrawTop(i, X, Y, SizeX, "Power options", false, true, false, false);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 3, Y + 28, SizeX, SizeY - 25);
            Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

            // Shutdown
            // I split it like this so it's easier to look at
            Window.DrawFullRoundedRectangle((X + (SizeX / 3) + 5 + 25) - (SizeX / 3) - 5 + 25,
                Y + 5 + 25,
                (SizeX / 3) - 5 - 25,
                SizeY - 5 - 25 - 5, 5, Kernel.lightMain);

            // Restart
            Window.DrawFullRoundedRectangle((X + (SizeX / 3) + 5 + 25),
                Y + 5 + 25,
                (SizeX / 3) - 5 - 25,
                SizeY - 5 - 25 - 5, 5, Kernel.lightMain);

            // Logout
            Window.DrawFullRoundedRectangle((X + (SizeX / 3) + 5 + 25) + (SizeX / 3) - 5 - 25,
                Y + 5 + 25,
                (SizeX / 3) - 5 - 25,
                SizeY - 5 - 25 - 5, 5, Kernel.lightMain);
        }

        public static bool IsCursorInArea(int cursorX, int cursorY, int areaLeft, int areaTop, int areaWidth, int areaHeight)
        {
            return cursorX >= areaLeft &&
                   cursorX <= areaLeft + areaWidth &&
                   cursorY >= areaTop &&
                   cursorY <= areaTop + areaHeight;
        }
    }
}
