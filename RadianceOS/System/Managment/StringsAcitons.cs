using Cosmos.System.Graphics.Fonts;
using CosmosTTF;
using RadianceOS.System.Apps;
using RadianceOS.System.Radiance;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment
{
    public static class StringsAcitons
	{
		public static void DrawCenteredString(string myString, int WinLengh, int WinPosX, int WinPosY, int space, Color color, Font font)
		{
			string[] strings = myString.Split(new string[] { "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
			for (int i = 0; i < strings.Length; i++)
			{
				int lengh = 0;
				if (font == Kernel.font18 || font == Kernel.fontRuscii)
				 lengh = strings[i].Length * 8;
				else
					 lengh = strings[i].Length * 6;
				int posX = (WinLengh - lengh) / 2;
				Explorer.CanvasMain.DrawString(strings[i], font, color, posX + WinPosX, WinPosY + i * space);
			}
		}

		public static void DrawCenteredTTFString(string myString, int WinLengh, int WinPosX, int WinPosY, int space, Color color, string fontName, int fontSize)
		{
			string[] strings = myString.Split(new string[] { "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
			for (int i = 0; i < strings.Length; i++)
			{
				int lengh = TTFManager.GetTTFWidth(strings[i], fontName, fontSize);

				int posX = (WinLengh - lengh) / 2;
				Explorer.CanvasMain.DrawStringTTF(strings[i], fontName, color, fontSize, posX + WinPosX, WinPosY + i * space);
			}
		}

		public static void DrawCenteredStringAlt(string myString, int WinLengh, int WinPosX, int WinPosY, int space, Color color, Font font)
		{
			string[] strings = myString.Split(new string[] { "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
			for (int i = 0; i < strings.Length; i++)
			{
				int lengh = 0;
				if (font == Kernel.font18)
					lengh = strings[i].Length * 8;
				else
					lengh = strings[i].Length * 6;
				int posX = (WinLengh - lengh) / 2;
				Radiance.Security.canvas.DrawString(strings[i], font, color, posX + WinPosX, WinPosY + i * space);
			}
		}
	}
}
