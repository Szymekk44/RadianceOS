using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using CosmosTTF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RadianceOSInstaller.Window
{
	public static class Window
	{
		public static Bitmap tempBitmap;
		

        public static Rectangle DrawTop(int X, int Y, int SizeX, string windowName, bool sizeAble = false, bool closeAble = true, bool ttf = false, bool hideAble = true, int tempSize = -1)
        {
            if (tempSize == -1)
                tempSize = SizeX;
            int cornerSize = 20;
            if (SizeX - tempSize != 0)
            {
                cornerSize = (SizeX - tempSize);
            }

            if (cornerSize < 0)
                cornerSize = 0;


          

            Graphic.CanvasMain.DrawString(windowName, Kernel.fontLat, Color.White, X + 8, Y + 5);

            return new(X,Y,SizeX,25);
            
        }

        public static void DrawRoundedRectangle(int x, int y, int width, int height, int radius, Color col, int modifedX)
		{
			Graphic.CanvasMain.DrawFilledRectangle(col, x + radius, y, width - 2 * radius, height);

			Graphic.CanvasMain.DrawFilledRectangle(col, x, y + radius, width, height - radius);

			Graphic.CanvasMain.DrawFilledCircle(col, x + radius, y + radius, radius);
			if(modifedX > 19)
			Graphic.CanvasMain.DrawFilledCircle(col, x + width - radius - 1, y + radius, radius);
		}

		public static void DrawFullRoundedRectangle(int x, int y, int width, int height, int radius, Color col)
		{
			// Draw top rectangle
			Graphic.CanvasMain.DrawFilledRectangle(col, x + radius, y, width - 2 * radius, height);

			// Draw left and right rectangles
			Graphic.CanvasMain.DrawFilledRectangle(col, x, y + radius, radius, height - 2 * radius);
			Graphic.CanvasMain.DrawFilledRectangle(col, x + width - radius, y + radius, radius, height - 2 * radius);

			// Draw top left, top right, bottom left, and bottom right circles
			Graphic.CanvasMain.DrawFilledCircle(col, x + radius, y + radius, radius);
			Graphic.CanvasMain.DrawFilledCircle(col, x + width - radius - 1, y + radius, radius);
			Graphic.CanvasMain.DrawFilledCircle(col, x + radius, y + height - radius - 1, radius);
			Graphic.CanvasMain.DrawFilledCircle(col, x + width - radius - 1, y + height - radius - 1, radius);
		}
		public static void DrawRoundedTopRightCorner(int x, int y, int width, int height, int radius, Color col)
		{
			// Draw top rectangle (without the right corner)
			Graphic.CanvasMain.DrawFilledRectangle(col, x, y, width - radius, height);
			Graphic.CanvasMain.DrawFilledRectangle(col, x, y+radius, width, height-radius);
			// Draw top right circle
			Graphic.CanvasMain.DrawFilledCircle(col, x + width - radius - 1, y + radius, radius);
		}




		


		public static void GetTempImage(int X, int Y, int SizeX, int SizeY, string operation)
		{

			

				//sysStatus.DrawBusy("Rendering " + operation);
				Heap.Collect();


				int width = SizeX;
				int height = SizeY;

				Color[] colors = new Color[width * height];
				Heap.Collect();

				for (int y = Y , destY = 0; y < Y + SizeY; y++, destY++)
				{
					for (int x = X, destX = 0; x < X + SizeX; x++, destX++)
					{
						colors[(height - 1 - destY) * width + destX] = Graphic.CanvasMain.GetPointColor(x, y);
					}
				}
				byte[] pixelData = new byte[width * height * 4];
				Heap.Collect();
				int index = 0;
				foreach (Color pixelColor in colors)
				{
					pixelData[index++] = pixelColor.B;
					pixelData[index++] = pixelColor.G;
					pixelData[index++] = pixelColor.R;
					pixelData[index++] = pixelColor.A;
				}


				// Utwórz nagłówek pliku BMP
				int fileSize = 54 + pixelData.Length; // 54 bajty to rozmiar nagłówka BMP
				byte[] fileHeader = new byte[54];
				fileHeader[0] = 66; // B
				fileHeader[1] = 77; // M
				BitConverter.GetBytes(fileSize).CopyTo(fileHeader, 2);
				BitConverter.GetBytes(54).CopyTo(fileHeader, 10);
				BitConverter.GetBytes(40).CopyTo(fileHeader, 14);
				BitConverter.GetBytes(width).CopyTo(fileHeader, 18);
				BitConverter.GetBytes(height).CopyTo(fileHeader, 22); // Uwaga: zmieniono wysokość
				BitConverter.GetBytes(1).CopyTo(fileHeader, 26);
				BitConverter.GetBytes(32).CopyTo(fileHeader, 28); // Zmieniono format na 32-bitowy

				Heap.Collect();

				// Utwórz obiekt Bitmap
				try
				{
					byte[] byteArray = fileHeader.Concat(pixelData).ToArray();

					tempBitmap = new Cosmos.System.Graphics.Bitmap(byteArray);
				}
				catch
				{
					
				}

				Heap.Collect();


			
		}




        public static void GetTempImageDark(int X, int Y, int SizeX, int SizeY, string operation, float dark)
        {



            //sysStatus.DrawBusy("Rendering " + operation);
            Heap.Collect();


            int width = SizeX;
            int height = SizeY;

            Color[] colors = new Color[width * height];
            Heap.Collect();

            for (int y = Y, destY = 0; y < Y + SizeY; y++, destY++)
            {
                for (int x = X, destX = 0; x < X + SizeX; x++, destX++)
                {
                    colors[(height - 1 - destY) * width + destX] = Graphic.CanvasMain.GetPointColor(x, y);
                }
            }
            colors = DarkenColors(colors.ToArray(), dark);
            byte[] pixelData = new byte[width * height * 4];
            Heap.Collect();
            int index = 0;
            foreach (Color pixelColor in colors)
            {
                pixelData[index++] = pixelColor.B;
                pixelData[index++] = pixelColor.G;
                pixelData[index++] = pixelColor.R;
                pixelData[index++] = pixelColor.A;
            }
         


            // Utwórz nagłówek pliku BMP
            int fileSize = 54 + pixelData.Length; // 54 bajty to rozmiar nagłówka BMP
            byte[] fileHeader = new byte[54];
            fileHeader[0] = 66; // B
            fileHeader[1] = 77; // M
            BitConverter.GetBytes(fileSize).CopyTo(fileHeader, 2);
            BitConverter.GetBytes(54).CopyTo(fileHeader, 10);
            BitConverter.GetBytes(40).CopyTo(fileHeader, 14);
            BitConverter.GetBytes(width).CopyTo(fileHeader, 18);
            BitConverter.GetBytes(height).CopyTo(fileHeader, 22); // Uwaga: zmieniono wysokość
            BitConverter.GetBytes(1).CopyTo(fileHeader, 26);
            BitConverter.GetBytes(32).CopyTo(fileHeader, 28); // Zmieniono format na 32-bitowy

            Heap.Collect();

            // Utwórz obiekt Bitmap
            try
            {
                byte[] byteArray = fileHeader.Concat(pixelData).ToArray();

                tempBitmap = new Cosmos.System.Graphics.Bitmap(byteArray);
            }
            catch
            {
             
            }

            Heap.Collect();



        }

        static Color[] DarkenColors(Color[] colors, float percent)
        {
            List<Color> darkenedColors = new List<Color>();

            foreach (var color in colors)
            {
                int red = (int)(color.R * (1 - percent));
                int green = (int)(color.G * (1 - percent));
                int blue = (int)(color.B * (1 - percent));

                red = Math.Max(0, red);
                green = Math.Max(0, green);
                blue = Math.Max(0, blue);

                darkenedColors.Add(Color.FromArgb(color.A, red, green, blue));
            }
			

            return darkenedColors.ToArray();
        }



        public static void GetTempImageDarkAndBlur(int X, int Y, int SizeX, int SizeY, string operation, float dark, int blurRadius)
        {
            int width = SizeX;
            int height = SizeY;

            Color[] colors = new Color[width * height];

            for (int y = Y, destY = 0; y < Y + SizeY; y++, destY++)
            {
                for (int x = X, destX = 0; x < X + SizeX; x++, destX++)
                {
                    colors[(height - 1 - destY) * width + destX] = Graphic.CanvasMain.GetPointColor(x, y);
                }
            }

            colors = DarkenAndBlurColors(colors.ToArray(), (int)Graphic.screenSizeX, 40, dark, blurRadius);

            byte[] pixelData = new byte[width * height * 4];
            Heap.Collect();
            int index = 0;
            foreach (Color pixelColor in colors)
            {
                pixelData[index++] = pixelColor.B;
                pixelData[index++] = pixelColor.G;
                pixelData[index++] = pixelColor.R;
                pixelData[index++] = pixelColor.A;
            }



            // Utwórz nagłówek pliku BMP
            int fileSize = 54 + pixelData.Length; // 54 bajty to rozmiar nagłówka BMP
            byte[] fileHeader = new byte[54];
            fileHeader[0] = 66; // B
            fileHeader[1] = 77; // M
            BitConverter.GetBytes(fileSize).CopyTo(fileHeader, 2);
            BitConverter.GetBytes(54).CopyTo(fileHeader, 10);
            BitConverter.GetBytes(40).CopyTo(fileHeader, 14);
            BitConverter.GetBytes(width).CopyTo(fileHeader, 18);
            BitConverter.GetBytes(height).CopyTo(fileHeader, 22); // Uwaga: zmieniono wysokość
            BitConverter.GetBytes(1).CopyTo(fileHeader, 26);
            BitConverter.GetBytes(32).CopyTo(fileHeader, 28); // Zmieniono format na 32-bitowy

            Heap.Collect();

            // Utwórz obiekt Bitmap
            try
            {
                byte[] byteArray = fileHeader.Concat(pixelData).ToArray();

                tempBitmap = new Cosmos.System.Graphics.Bitmap(byteArray);
            }
            catch
            {
                
            }

            Heap.Collect();

        }

        static Color[] DarkenAndBlurColors(Color[] colors, int width, int height, float darkPercent, int blurRadius)
        {
            List<Color> resultColors = new List<Color>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int avgRed = 0, avgGreen = 0, avgBlue = 0;
                    int count = 0;

                    // Uwzględnij piksele z obszaru blurowania
                    for (int i = Math.Max(0, y - blurRadius); i <= Math.Min(height - 1, y + blurRadius); i++)
                    {
                        for (int j = Math.Max(0, x - blurRadius); j <= Math.Min(width - 1, x + blurRadius); j++)
                        {
                            Color pixelColor = colors[i * width + j];
                            avgRed += pixelColor.R;
                            avgGreen += pixelColor.G;
                            avgBlue += pixelColor.B;
                            count++;
                        }
                    }

                    avgRed /= count;
                    avgGreen /= count;
                    avgBlue /= count;

                    int red = (int)(avgRed * (1 - darkPercent));
                    int green = (int)(avgGreen * (1 - darkPercent));
                    int blue = (int)(avgBlue * (1 - darkPercent));

                    red = Math.Max(0, red);
                    green = Math.Max(0, green);
                    blue = Math.Max(0, blue);

                    resultColors.Add(Color.FromArgb(255, red, green, blue));
                }
            }

            return resultColors.ToArray();
        }



    }
}
