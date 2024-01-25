using Cosmos.System.Graphics;
using LunarLabs.Fonts;
using System.Collections.Generic;
using System;
using System.Drawing;
using RadianceOS.System.Apps;
using Cosmos.System;
using RadianceOS;

namespace CosmosTTF
{
	public static class TTFManager
	{
		private static CustomDictString<Font> fonts = new();
		private static CustomDictString<GlyphResult> glyphCache = new();
		private static List<string> glyphCacheKeys = new();

		public static int GlyphCacheSize { get; set; } = 512;
		private static Canvas prevCanv;

		public static void RegisterFont(string name, byte[] byteArray)
		{
			fonts.Add(name, new Font(byteArray));
		}

		/// <summary>
		/// Render a glyph
		/// </summary>
		/// <param name="font">Registered font name</param>
		/// <param name="glyph">The character to render</param>
		/// <param name="color">The color to make the text (ignores alpha)</param>
		/// <param name="scalePx">The scale in pixels</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static GlyphResult RenderGlyphAsBitmap(string font, char glyph, Color color, float scalePx = 16)
		{
			var rgbOffset = ((color.R & 0xFF) << 16) + ((color.G & 0xFF) << 8) + color.B;

			if (!fonts.TryGet(font, out Font f))
			{
				throw new Exception("Font is not registered");
			}

			string key = font + glyph + scalePx + color;

			if (glyphCache.TryGet(key, out GlyphResult cached))
			{
				return cached;
			}

			float scale = f.ScaleInPixels(scalePx);
			var glyphRendered = f.RenderGlyph(glyph, scale);
			var image = glyphRendered.Image;

			var bmp = new Bitmap((uint)image.Width, (uint)image.Height, ColorDepth.ColorDepth32);

			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					byte alpha = image.Pixels[i + j * image.Width];

					if (alpha > 0)
					{
						bmp.RawData[i + j * image.Width] = ((int)alpha << 24) + rgbOffset;
					}
				}
			}

			// Dodajemy do pamięci podręcznej
			glyphCache[key] = new GlyphResult(bmp, glyphRendered.xAdvance, glyphRendered.yOfs);

			return new GlyphResult(bmp, glyphRendered.xAdvance, glyphRendered.yOfs);
		}



		/// <summary>
		/// Draws a string using the registered TTF font provided under the font parameter. Alpha in pen color will be ignored. Do NOT forget to run Canvas.Display, or else it, well, wont display!
		/// </summary>
		/// <param name="cv"></param>
		/// <param name="pen"></param>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="px"></param>
		/// <param name="point"></param>
		public static void DrawStringTTF(this Canvas cv, string text, string font, Color color, float px, int x, int y, float spacingMultiplier = 1f)
		{
			
				prevCanv = cv;
				float offX = 0;
				float offY = 0;

				foreach (char c in text)
				{
					if (c == '\n')
					{
						offY += px;
						continue;
					}

					GlyphResult g = RenderGlyphAsBitmap(font, c, color, px);
					Explorer.CanvasMain.DrawImageAlpha(g.bmp, x + (int)offX, y + g.offY);
					offX += g.offX;
				}
			
		

		}

		public static int GetTTFWidth(this string text, string font, float px)
		{
			if (!fonts.TryGet(font, out Font f))
			{
				throw new Exception("Font is not registered");
			}

			float scale = f.ScaleInPixels(px);
			int totalWidth = 0;

			foreach (char c in text)
			{
				f.GetCodepointHMetrics(c, out int advWidth, out int lsb);
				totalWidth += advWidth;
			}

			return (int)(totalWidth * scale);
		}

		internal static void DebugUIPrint(string txt, int offY = 0)
		{
			prevCanv.DrawFilledRectangle(Color.Black, 0,offY, 1000, 16);
			prevCanv.DrawString(txt, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, Color.White, 16,offY);
			prevCanv.Display();
		}
	}

	public struct GlyphResult
	{
		public Bitmap bmp;
		public int offY = 0;
		public int offX = 0;

		public GlyphResult(Bitmap bmp, int offX, int offY)
		{
			this.bmp = bmp;
			this.offX = offX;
			this.offY = offY;
		}
	}
}