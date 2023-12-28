using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cosmos.Core.Memory;
using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using RadianceOS.System.Managment;
using Cosmos.HAL.Audio;
using System.Threading;
using Cosmos.HAL;
using Cosmos.System.Graphics.Fonts;
using RadianceOS.System.Programming.RaSharp;
using RadianceOS.System.Radiance;
using RadianceOS.System.Apps;

namespace RadianceOS.Render
{
	public static class Canvas
	{
		public static Cosmos.System.Graphics.Canvas canvas = Explorer.CanvasMain;
		public static void DrawImageAlpha(Cosmos.System.Graphics.Image image, int x, int y)
		{
			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
					if (color.A == 0)
						continue;
					canvas.DrawPoint(color, x + i, y + j);
				}
			}
		}
		public static void DrawString(string str, Font font, Color color, int x, int y)
		{
			byte height = font.Height;
			byte width = font.Width;

			for (int i = 0; i < str.Length; i++)
			{
				char currentChar = str[i];

				if (currentChar == ' ') // Przerwa dla spacji
				{
					x += width;
				}
				else
				{
					DrawChar(currentChar, font, color, x, y);
					x += width;
				}
			}
		}

		public static void DrawChar(char c, Font font, Color color, int x, int y)
		{
			byte height = font.Height;
			byte width = font.Width;
			byte[] data = font.Data;
			int num = height * (byte)c;

			// Przed wejściem do pętli sprawdź, czy znak jest widoczny
	

			for (int i = 0; i < height; i++)
			{
				for (byte b = 0; b < width; b++)
				{
					// Przenieś warunek do pętli
					if (font.ConvertByteToBitAddress(data[num + i], b + 1))
					{
						// Sprawdź, czy punkt jest w granicach
						canvas.DrawPoint(color, (ushort)(x + b), (ushort)(y + i));
						
					}
				}
			}
		}



		// Funkcja rysująca punkty
		static void DrawPoints(Color color, List<Point> points)
		{
			// Implementacja rysowania punktów z bufora
			foreach (Point point in points)
			{
				canvas.DrawPoint(color, (ushort)point.X, (ushort)point.Y);
			}
		}
	}
}
