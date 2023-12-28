using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment
{
	public static class sysStatus
	{
		public static void DrawBusy(string reason)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, (int)Explorer.screenSizeX / 2 - 100, (int)Explorer.screenSizeY / 2 - 50, 200, 100);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, (int)Explorer.screenSizeX / 2 - 103, (int)Explorer.screenSizeY / 2 - 53, 200, 100);
			StringsAcitons.DrawCenteredString("RadianceOS is busy\n" + reason + "\nPlease Wait...", (int)Explorer.screenSizeX, 0, (int)Explorer.screenSizeY / 2 - 26, 18,Kernel.fontColor, Kernel.fontRuscii);
			Explorer.CanvasMain.Display();
		}
	}
}
