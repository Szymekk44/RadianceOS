using Cosmos.System.Graphics.Fonts;
using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment.Crash
{
	public static class BootFail
	{
		public static void Render()
		{
			Explorer.CanvasMain.Clear(Color.FromArgb(0, 0, 0));
			Font def = PCScreenFont.Default;
			Explorer.CanvasMain.DrawString("Fatal RadianceOS Error", def, Color.White, 0, 0);
			Explorer.CanvasMain.DrawString("RadianceOS failed to start. System files not found! Please run RadianceOS installer.", def, Color.White, 0, 20);
			FindFiles();
			if(notFoundCritical)
			{
				Explorer.CanvasMain.DrawString("Critical files not found! Restart in: 30 sec", def, Color.White, 0, 550);
				Explorer.CanvasMain.Display();
				Thread.Sleep(30000);
				Cosmos.System.Power.Reboot();
			}
			else
			{
				Explorer.CanvasMain.DrawString("RadianceOS will start in 10 seconds. Some things may not work properly. Please run RadianceOS installer to restore files!", def, Color.White, 0, 550);
				Explorer.CanvasMain.Display();
				Thread.Sleep(10000);
			}

		}
		public static int currY;
		public static bool notFound = false;
		public static bool notFoundCritical = false;
		public static void FindFiles()
		{
		
			bool notFound = false;

			string path = @"0:\RadianceOS\System\Files\";

			find(path + @"Wallpapers\Wallpaper1.bmp", false);
			find(path + @"Wallpapers\Wallpaper2.bmp", false);
			find(path + @"Wallpapers\Wallpaper3.bmp", false);
			find(path + @"Wallpapers\Wallpaper4.bmp", false);
			find(path + @"Wallpapers\Wallpaper5.bmp", false);
			find(path + @"Wallpapers\Wallpaper6.bmp", false);

			find(path + @"Images\ShadowLogo.bmp", false);
		}

		public static void find(string path, bool Critical)
		{
			Font def = PCScreenFont.Default;
		
			if (File.Exists(path))
			{
				Explorer.CanvasMain.DrawString("Found: " + path , def, Color.White, 0, currY);
			
			}
			else
			{
				if(!Critical)
				Explorer.CanvasMain.DrawString("MISSING: " + path, def, Color.FromArgb(240, 56, 56), 0, currY);
				else
				{
					Explorer.CanvasMain.DrawString("CRITICAL FILE MISSING! " + path, def, Color.FromArgb(255, 0, 0), 0, currY);
				}
				notFound = true;
			}
			currY += 18;
		}
	}
}
