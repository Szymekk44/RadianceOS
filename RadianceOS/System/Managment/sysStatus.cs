using Cosmos.System;
using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cosmos.HAL.Drivers.Video.VGADriver;

namespace RadianceOS.System.Managment
{
	public static class sysStatus
	{
		public static void DrawBusy(string reason)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, (int)Explorer.screenSizeX / 2 - 100, (int)Explorer.screenSizeY / 2 - 50, 200, 100);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, (int)Explorer.screenSizeX / 2 - 103, (int)Explorer.screenSizeY / 2 - 53, 200, 100);
			StringsAcitons.DrawCenteredTTFString("RadianceOS is busy\n" + reason + "\nPlease Wait...", (int)Explorer.screenSizeX, 0, (int)Explorer.screenSizeY / 2 - 17, 18,Kernel.fontColor, "UMR", 18);

			if (Explorer.DrawMenu)
			{
				StartMenu.Render();
			}

			if (Explorer.DrawTaskbar)
			{
				Explorer.CanvasMain.DrawImage(Kernel.TaskBar1, 0, (int)Explorer.screenSizeY - 40);
				

				TaskBar.Render();
			}



			Render.Canvas.DrawImageAlpha(Kernel.Cursor1, Explorer.MX, Explorer.MY);//CURSOR


			Explorer.CanvasMain.Display();
		}
	}
}
