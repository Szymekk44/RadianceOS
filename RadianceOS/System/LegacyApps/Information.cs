using Cosmos.System.Graphics;
using CosmosTTF;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class Information
	{
		public static void Render(int X, int Y, int SizeX, int SizeY, int i)
		{
			Window.DrawTop(i, X, Y, SizeX, "RadianceOS - information");
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);
			if (Process.Processes[i].bitmap == null)
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
				Bitmap logo = new Bitmap(Files.RadianceOSIconTransparent);
				RadianceOS.Render.Canvas.DrawImageAlpha(logo, X + (500 - 456) / 2, Y + 40);
				StringsAcitons.DrawCenteredTTFString("RadianceOS " + Kernel.version, SizeX, X, Y + 125, 42, Kernel.fontColor, "UMB", 24);
				StringsAcitons.DrawCenteredTTFString("System version: " + Kernel.subversion + "\nRa# version: " + Kernel.RasVersion, SizeX, X, Y + 145, 18, Kernel.fontColor, "UMR", 18);

				StringsAcitons.DrawCenteredTTFString("Created by Szymekk\nSzymekk.pl\nYoutube.com/Szymekk", SizeX, X, Y + 185, 18, Kernel.fontColor, "UMR", 18);

				StringsAcitons.DrawCenteredTTFString("RadianceOS was created with COSMOS\nC# Open Source Managed Operating System", SizeX, X, Y + 245, 18, Kernel.fontColor, "UMR", 18);

				Window.GetImage(X, Y, SizeX, SizeY, i, "Information");
			}
			else
			{
				Explorer.CanvasMain.DrawImage(Process.Processes[i].bitmap, X, Y + 25);
			}
		}
	}
}
