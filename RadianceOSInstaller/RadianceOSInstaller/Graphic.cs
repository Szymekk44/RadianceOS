using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using RadianceOSInstaller.System.Managment;
using RadianceOSInstaller.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOSInstaller
{
	public static class Graphic
	{
		public static uint screenSizeX = 1920, screenSizeY = 1080;
		public static Canvas CanvasMain;

		public static void Start()
		{
			Heap.Collect();
			Cosmos.System.MouseManager.ScreenWidth = screenSizeX;
			Cosmos.System.MouseManager.ScreenHeight = screenSizeY;
			Cosmos.System.MouseManager.X = screenSizeX / 2; Cosmos.System.MouseManager.Y = screenSizeY / 2;

			CanvasMain = FullScreenCanvas.GetFullScreenCanvas(new Mode(screenSizeX, screenSizeY, ColorDepth.ColorDepth32));
			CanvasMain.DrawImage(Kernel.Wallpaper, 0, 0);
			ResizeWallpaper((int)screenSizeX, (int)screenSizeY);
		}
		public static void ResizeWallpaper(int SizeX, int SizeY)
		{
			if (Kernel.Wallpaper.Width != SizeX || Kernel.Wallpaper.Height != SizeY)
			{
				CanvasMain.DrawImage(Kernel.Wallpaper, 0, 0, (int)Graphic.screenSizeX, (int)Graphic.screenSizeY);
				Window.Window.GetTempImage(0, 0, (int)Graphic.screenSizeX, (int)Graphic.screenSizeY, "Wallpaper");
				Kernel.Wallpaper = Window.Window.tempBitmap;
			}
		}

		public static void Render()
		{
			CanvasMain.DrawImage(Kernel.Wallpaper, 0, 0);
			if (Kernel.WindowDark == null)
			{
				Window.Window.GetTempImageDark(460, 215, 1000, 650, "Dark", 0.75f);
				Kernel.WindowDark = Window.Window.tempBitmap;
			}
			if (Kernel.WindowText == null)
			{
				CanvasMain.DrawImage(Kernel.WindowDark, 460, 215);
				StringsAcitons.DrawCenteredTTFString("RadianceOS packages installer", 1000, 460, 215 + 45, 30, Color.White, "UMB", 40);
				


				Window.Window.GetTempImage(460, 215, 1000, 650, "Image");
				Kernel.WindowText = Window.Window.tempBitmap;
			}
			CanvasMain.DrawImage(Kernel.WindowText, 460, 215);
			int ProgressBarLenght = (int)(((double)Kernel.status / 8) * 900);
			StringsAcitons.DrawCenteredTTFString(Kernel.statusString, 1000, 460, 215 + 70, 30, Color.White, "UMR", 24);
			CanvasMain.DrawFilledRectangle(Kernel.shadow, 510, 775, 900, 50);
			if(!Kernel.procent)
			CanvasMain.DrawFilledRectangle(Kernel.lightMain, 510, 775, ProgressBarLenght, 50);
			else
				CanvasMain.DrawFilledRectangle(Kernel.lightMain, 510, 775, 90 * Kernel.Progress, 50);
			if (!Kernel.procent)
				StringsAcitons.DrawCenteredTTFString( Kernel.status * (100/8) + "%", 900, 510, 775+10+24, 30, Color.White, "UMB", 24);
			else
				StringsAcitons.DrawCenteredTTFString(Kernel.Progress * 10 + "%", 900, 510, 775 + 10 + 24, 30, Color.White, "UMB", 24);
			CanvasMain.Display();
		}
	}
}
