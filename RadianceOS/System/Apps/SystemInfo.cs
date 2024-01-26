using Cosmos.System;
using RadianceOS.System.Graphic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class SystemInfo
	{
		public static void Render(int X, int Y, int SizeX, int SizeY, int i, bool enabledRam)
		{
			Window.DrawTop(i, X, Y, SizeX, "System Performance", false, true, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 3, Y + 28, SizeX, SizeY-25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y+25, SizeX, SizeY-25);

			Explorer.CanvasMain.DrawString("RadianceOS " + Kernel.version + " - " + Kernel.subversion, Kernel.fontRuscii, Kernel.fontColor, X + 5, Y + 28);
			Explorer.CanvasMain.DrawString("FPS: " + Kernel._fps, Kernel.font18, Kernel.fontColor, X + 5, Y + 61);
			
			if(enabledRam)
			{
				uint usedRam = Cosmos.Core.GCImplementation.GetUsedRAM();
				Explorer.CanvasMain.DrawString("Memory usage: " + (usedRam / 1024) + "/" + Kernel.MaxRam * 1024 + "KB", Kernel.font18, Kernel.fontColor, X + 5, Y + 79);
				Explorer.CanvasMain.DrawString("Memory usage: " + (usedRam / 1048576) + "/" + Kernel.MaxRam + "MB", Kernel.font18, Kernel.fontColor, X + 5, Y + 97);
			}
			else
			{
				Explorer.CanvasMain.DrawString("Memory usage: disabled/" + Kernel.MaxRam * 1024 + "KB", Kernel.font18, Kernel.fontColor, X + 5, Y + 79);
				Explorer.CanvasMain.DrawString("Memory usage: disabled/" + Kernel.MaxRam + "MB", Kernel.font18, Kernel.fontColor, X + 5, Y + 97);
			}
			
			Explorer.CanvasMain.DrawString("Processes: " + Apps.Process.Processes.Count, Kernel.font18, Kernel.fontColor, X + 5, Y + 115);
		}
	}
}
