using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System.Graphics;
using RadianceOS.System.Graphic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class ImageViewer
	{
		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			Window.DrawTop(i, X, Y, SizeX, "Image Viewer - " + Process.Processes[i].temp, false, true, true, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawImage(Process.Processes[i].bitmap, X, Y + 25);
		}

		public static void OpenImage(string url)
		{
			Bitmap temp = new Bitmap(File.ReadAllBytes(url));
			Processes imgViewer = new Processes
			{
				ID = 14,
				Name = "Image Viewer",
				Description = "Image Viewer " + url,
				X = 100,
				Y = 70,
				SizeX = (int)temp.Width,
				SizeY = (int)temp.Height + 25,
				bitmap = temp,
				temp = url,
				moveAble = true,
				sizeAble = false,
				hideAble = true
			};
			Process.Processes.Add(imgViewer);
		}
	}
}
