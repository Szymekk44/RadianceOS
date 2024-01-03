using Cosmos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cosmos.System;
using System.Linq.Expressions;
using RadianceOS.System.Apps.Games;
using RadianceOS.System.Apps.RadianceOSwebBrowser;
using Cosmos.HAL;
using RadianceOS.Render;

namespace RadianceOS.System.Apps
{
	public static class StartMenu
	{
		public static int state;
		public static int y;
		public static void Render()
		{
	//		Explorer.CanvasMain.DrawImage(Kernel.StartMenu, 5, y);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, 5,y,250,700);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, 255, y, 450, 700);
			DrawStartButton(0);
			DrawStartButton(1);
			DrawStartButton(2);
			DrawStartButton(3);
			DrawStartButton(4);
			DrawStartButton(5);
			DrawStartButton(6);
		}

		public static void DrawStartButton(int id)
		{

			bool selected = false;
			if(state == 0)
			{
				if (Explorer.MY >= y + (id * 40) && Explorer.MY <= y + (id * 40) + 40)
				{
					if (Explorer.MX >= 5 && Explorer.MX <= 255)
					{
						selected = true;
						if (MouseManager.MouseState == MouseState.Left)
						{
							
								state = 2;

								switch(id)
							{
								case 0:
									{
										Processes MessageBox2 = new Processes
										{
											ID = 1,
											Name = "Terminal",
											Description = "CosmosVFS is working!",
											metaData = @"0:\",
											X = 100,
											Y = 100,
											SizeX = 800,
											SizeY = 500,
											sizeAble = true,
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										Process.UpdateProcess(Process.Processes.Count - 1);
									}
									break;

								case 1:
									{
										Processes MessageBox2 = new Processes
										{
											ID = 2,
											Name = "Untitled",
											Description = "CosmosVFS is working!",
											metaData = @"0:\",
											X = 100,
											Y = 100,
											SizeX = 1000,
											SizeY = 700,
											saved = true,
											sizeAble = true,
											temp = @"0:\Users\" + Kernel.loggedUser + @"\Desktop\",
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										Process.UpdateProcess(Process.Processes.Count - 1);
									}
									break;
								case 2:
									{
									

											Processes MessageBox2 = new Processes
										{
											ID = 5,
											Name = "Settings",
											X = 100,
											Y = 100,
											SizeX = 1000,
											SizeY = 700,
											tempInt = 0,
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										Process.UpdateProcess(Process.Processes.Count - 1);
									
									}
									break;
								case 3:
									{


										Processes FE = new Processes
										{
											ID = 10,
											Name = "File Explorer",
											X = 300,
											Y = 200,
											SizeX = 900,
											MinX = 500,
											SizeY = 550,
											sizeAble = true,
											moveAble = true
										};
										Process.Processes.Add(FE);
										FileExplorer.UpdateList(Process.Processes.Count - 1, @"0:\");


									}
									break;
								case 4:
									{


										Processes MessageBox2 = new Processes
										{
											ID = 6,
											Name = "Snake",
											X = 100,
											Y = 70,
											SizeX = 900,
											SizeY = 950,
											tempInt = 0,
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										Snake.Start(Process.Processes.Count - 1);

									}
									break;
								case 5:
									{


										Processes MessageBox2 = new Processes
										{
											ID = 7,
											Name = "Performance Info",
											X = 100,
											Y = 70,
											SizeX = 300,
											SizeY = 142,
											tempInt = 0,
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										Kernel.countFPS = true;

									}
									break;
								case 6:
									{


										Processes MessageBox2 = new Processes
										{
											ID = 8,
											Name = "RadiantWave",
											X = 100,
											Y = 70,
											SizeX = 1200,
											SizeY = 700,
											tempInt = 0,
											texts = new string[2] { "szymekk.pl/RadianceOS.html", "" },
											CurrChar = "szymekk.pl/RadianceOS.html".Length,
											moveAble = true
										};
										Process.Processes.Add(MessageBox2);
										RadiantWave.LoadWebsite(Process.Processes.Count - 1);

									}
									break;


							}


						}
					}
				}
			}
		
			if(!selected)
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.startDefault, 5, y + (id * 40), 250, 40);
			else
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.startLight, 5, y + (id * 40), 250, 40);
			switch (id)
			{
				case 0:
					Canvas.DrawImageAlpha(Kernel.cmd, 10, y+4 + (id * 32));
					Explorer.CanvasMain.DrawString("Terminal",Kernel.font18, Kernel.fontColor, 47, y+12 + (id * 32));
					break;
				case 1:
					Canvas.DrawImageAlpha(Kernel.notepad, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("Notepad", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
				case 2:
					Canvas.DrawImageAlpha(Kernel.settingIcon, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("Settings", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
				case 3:
					Canvas.DrawImageAlpha(Kernel.fileExplorer, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("File explorer", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
				case 4:
					Canvas.DrawImageAlpha(Kernel.gamepadIcon, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("Snake", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
				case 5:
					Canvas.DrawImageAlpha(Kernel.sysinfoIcon, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("Performance", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
				case 6:
					Canvas.DrawImageAlpha(Kernel.RadiantWave, 10, y + 4 + (id * 40));
					Explorer.CanvasMain.DrawString("RadiantWave Web Browser", Kernel.font18, Kernel.fontColor, 47, y + 12 + (id * 40));
					break;
			}
		}

		public static void Move()
		{
			switch(state)
			{
				case 1:
					if (y > Explorer.screenSizeY-600)
					{
						y -= 20;
					}
					else
						state = 0;
					break;
					case 2:
					{
						if (y < Explorer.screenSizeY-40)
						{
							y += 20;
						}
						else
						{
							state = 0;
							Explorer.DrawMenu = false;
						}
							
					}
					break;
			}
			
		}
	}
}
