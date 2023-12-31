using Cosmos.System.Graphics;
using CosmosTTF;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace RadianceOS.System.Radiance
{
	public static class BootScreen
	{
		public static Bitmap BootImage;
		public static int ToLoad = 22, Loaded;
		public static bool on = true;
		public static Bitmap bit;
		public static void Start()
		{
			if (!on)
				return;
			BootImage = new Bitmap(Files.RadianceOSIcon);
			Explorer.CanvasMain = (SVGAIICanvas)FullScreenCanvas.GetFullScreenCanvas(new Mode(Explorer.screenSizeX, Explorer.screenSizeY, ColorDepth.ColorDepth32));
			
				Explorer.CanvasMain.DrawImage(BootImage, (int)(Explorer.screenSizeX - 456) / 2, (int)(Explorer.screenSizeY) - 250);
			




			Explorer.CanvasMain.Display();
		}
		public static void Render(string state, string action, Color Color = default, bool countAsLoaded = true)
		{
			if (!on)
				return;
			Explorer.CanvasMain.Clear(Color.Black);
			if (Color == default)
			{
				Color = Color.White;
			}
			if (countAsLoaded)
				Loaded++;
			while (Console.KeyAvailable)
			{
				ConsoleKeyInfo key = Console.ReadKey(true);
				switch (key.Key)
				{
					case ConsoleKey.Escape:
						Explorer.CanvasMain.Disable();
						on = false;
						return;
				}
			}


			

				


				Explorer.CanvasMain.DrawImage(BootImage, (int)(Explorer.screenSizeX - 456) / 2, (int)(Explorer.screenSizeY) - 250);
				StringsAcitons.DrawCenteredTTFString("Press ESC to close gui", (int)Explorer.screenSizeX, 0, (int)((Explorer.screenSizeY - 70)) + 50, 25, Color.White, "UMR", 16);
				StringsAcitons.DrawCenteredTTFString(state, (int)Explorer.screenSizeX, 0, (int)(Explorer.screenSizeY) - 125, 25, Color.White, "UMB", 20);
				StringsAcitons.DrawCenteredTTFString(action, (int)Explorer.screenSizeX, 0, (int)(Explorer.screenSizeY) - 100, 25, Color.White, "UMR", 15);
				//	StringsAcitons.DrawCenteredTTFString("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed et convallis augue, vel dignissim turpis. Curabitur et nisl in nulla ", 1920, 0, 720, 10, Color.White, "RMM", 20);

				int ProgressBarLenght = (int)(((double)Loaded / ToLoad) * 300);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, (int)Explorer.screenSizeX/2 - 153, (int)(Explorer.screenSizeY) - 84, 306, 25);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, (int)Explorer.screenSizeX / 2 - 150, (int)(Explorer.screenSizeY) - 82, ProgressBarLenght, 21);
			

			Explorer.CanvasMain.Display(); 
			

		}
	}
}
