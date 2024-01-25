using Cosmos.Core;
using Cosmos.Core.Memory;
using RadianceOSInstaller.System.Managment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOSInstaller.System.ConsoleMode
{
	public static class DisplaySizeSelector
	{
		public static bool Finished = false;
		public static int Curr, Last = -1;
		public static int state;
		public static void SelectMode()
		{
			while(!Finished)
			{
				Heap.Collect();
				if(Last != Curr)
				{
					Console.BackgroundColor = ConsoleColor.White;
					Console.Clear();
					DrawInfo("Display Settings");
					switch (state)
					{
						case 0:
							{
								DrawButton(0, "1920x1080");
								DrawButton(1, "More");
								DrawInfo("Troubleshooting");
								DrawButton(2, "RadianceOS Console Mode");
							}
							break;
							case 1:
							{
								DrawButton(0, "1920x1080");
								DrawInfo("Better HD-Ready Resolutions");
								DrawButton(1, "1680x1050");
								DrawButton(2, "1600x1200");
								DrawButton(3, "1400x1200");
								DrawButton(4, "1440x900");
								DrawInfo("Old HD-Ready Resolutions");
								DrawButton(5, "1360x768");
								DrawButton(6, "1280x1024");
								DrawButton(7, "1280x800");
								DrawButton(8, "1280x768");
								DrawButton(9, "1280x720");
								DrawInfo("SD Resolutions");
								DrawButton(10, "1152x768");
								DrawButton(11, "1024x768");
								DrawButton(12, "800x600");
								DrawInfo("Other");
								DrawButton(13, "1920x1200");
								DrawButton(14, "2048x1536");
								DrawButton(15, "2560x1080");
								DrawInfo("Troubleshooting");
								DrawButton(16, "RadianceOS Console Mode");
							}
							break;
					}
					Last = Curr;
				
					
				}

				while (Console.KeyAvailable)
				{
	
					ConsoleKeyInfo key = Console.ReadKey(true);
					switch (key.Key)
					{
						case ConsoleKey.DownArrow:
							{
								switch(state)
								{
									case 0:
										{
											if (Curr < 2)
												Curr++;
										}
										break;
									case 1:
										{
											if (Curr < 16)
												Curr++;
										}
										break;
								}
							
							}
							break;
						case ConsoleKey.UpArrow:
							{
								if (Curr > 0)
									Curr--;
							}
							break;
						case ConsoleKey.Enter:
							{
								switch (state)
								{
									case 0:
										{
											switch (Curr)
											{
												case 0:
													{
														SaveMode(0);
														Finished = true;
													}
													break;
												case 1:
													{
														state = 1;
														Curr = 0;
													}
													break;
												case 2:
													{
														Console.BackgroundColor = ConsoleColor.Black;
														Console.Clear();
														Finished = true;
														Kernel.render = false;
														 
													}
													break;

											}
											
										}
										break;
									case 1:
										{
											switch (Curr)
											{
												
												case 16:
													{
														Console.BackgroundColor = ConsoleColor.Black;
														Console.Clear();
														Finished = true;
														Kernel.render = false;
														  
														return;
													}
													break;
													

											}
											Console.BackgroundColor = ConsoleColor.Black;
											Console.Clear();
											SaveMode(Curr);
											Finished = true;
										}
										break;


								}

							}
								break;
					}
				}





			}

			

		}
		public static void SaveMode(int mode)
		{
		
			switch (mode)
			{
				case 0:
					{
						Graphic.screenSizeX = 1920; Graphic.screenSizeY = 1080;
					}
					break;
				case 1:
					{
						Graphic.screenSizeX = 1680; Graphic.screenSizeY = 1050;
					}
					break;
				case 2:
					{
						Graphic.screenSizeX = 1600; Graphic.screenSizeY = 1200;
					}
					break;
				case 3:
					{
						Graphic.screenSizeX = 1400; Graphic.screenSizeY = 1200;
					}
					break;
				case 4:
					{
						Graphic.screenSizeX = 1440; Graphic.screenSizeY = 900;
					}
					break;
				case 5:
					{
						Graphic.screenSizeX = 1360; Graphic.screenSizeY = 768;
					}
					break;
				case 6:
					{
						Graphic.screenSizeX = 1280; Graphic.screenSizeY = 1024;
					}
					break;
				case 7:
					{
						Graphic.screenSizeX = 1280; Graphic.screenSizeY = 800;
					}
					break;
				case 8:
					{
						Graphic.screenSizeX = 1280; Graphic.screenSizeY = 768;
					}
					break;
				case 9:
					{
						Graphic.screenSizeX = 1280; Graphic.screenSizeY = 720;
					}
					break;
				case 10:
					{
						Graphic.screenSizeX = 1152; Graphic.screenSizeY = 768;
					}
					break;
				case 11:
					{
						Graphic.screenSizeX = 1024; Graphic.screenSizeY = 768;
					}
					break;
				case 12:
					{
						Graphic.screenSizeX = 800; Graphic.screenSizeY = 600;
					}
					break;
				case 13:
					{
						Graphic.screenSizeX = 1920; Graphic.screenSizeY = 1200;
					}
					break;
				case 14:
					{
						Graphic.screenSizeX = 2048; Graphic.screenSizeY = 1536;
					}
					break;
				case 15:
					{
						Graphic.screenSizeX = 2560; Graphic.screenSizeY = 1080;
					}
					break;
		

			}
			try
			{
				if (Kernel.diskReady)
				{
					File.Create(@"0:\RadianceOS\Settings\DisplayMode.dat");
					File.WriteAllText(@"0:\RadianceOS\Settings\DisplayMode.dat", mode.ToString());
				}
		
			}
			catch
			{

			}
		
		}
		public static void DrawButton(int id, string text)
		{
			if(id == Curr)
			{
				Console.BackgroundColor = ConsoleColor.DarkCyan;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(DrawConsole.CenterText(text));
			}
            else
            {
				Console.BackgroundColor = ConsoleColor.White;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write(DrawConsole.CenterText(text));
			}
        }

		public static void DrawInfo(string info)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(DrawConsole.CenterText(info));
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
		}
	}
}
