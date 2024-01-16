using Cosmos.System.Graphics;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System.Drawing;

namespace RadianceOS.System.Apps
{
	public static class Welcome
	{
		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			Window.DrawTop(i,X, Y, SizeX, "Welcome - RadianceOS", false, true, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
			if (Process.Processes[i].bitmap == null)
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
				Bitmap logo = new Bitmap(Files.RadianceOSIconTransparent);
				RadianceOS.Render.Canvas.DrawImageAlpha(logo, X + (500 - 456) / 2, Y + 40);
				StringsAcitons.DrawCenteredTTFString("Welcome to RadianceOS", 500, X, Y + 125, 42, Kernel.fontColor, "UMB", 24);
				StringsAcitons.DrawCenteredTTFString("Thanks for installing!\nPlease note that RadianceOS is still in development!", 500, X, Y + 150, 19, Kernel.fontColor, "UMR", 18);
				StringsAcitons.DrawCenteredTTFString("Version: " + Kernel.version + " (" + Kernel.subversion + ")\nRaSharp Version: " + Kernel.RasVersion, 500, X, Y + 200, 17, Color.LightGray, "UMR", 16);

				StringsAcitons.DrawCenteredTTFString("Created by Szymekk\nSzymekk.pl\nhttps://youtube.com/Szymekk", 500, X, Y + 250, 19, Kernel.fontColor, "UMR", 18);
				Window.GetImage(X, Y, SizeX, SizeY, i, "Welcome");
			}
			else
			{
				Explorer.CanvasMain.DrawImage(Process.Processes[i].bitmap, X, Y + 25);
			}
		}
	}
}
