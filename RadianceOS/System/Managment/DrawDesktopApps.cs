using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using RadianceOS.System.Programming.RaSharp;
using Cosmos.System.Graphics;
using RadianceOS.System.Graphic;

namespace RadianceOS.System.Managment
{
	public static class DrawDesktopApps
	{
		public static List<DesktopIcon> Icons = new List<DesktopIcon>();

		public static bool clicked;
		public static bool stopperGoing;
		static DateTime startTime;
		static bool clickedOn;
		static bool mainClick;
		
		public static void UpdateIcons()
		{
			Icons.Clear();
			var files_list = Directory.GetFiles(@"0:\Users\" + Kernel.loggedUser + @"\Desktop");
			int pos = 0;
			for (int i = 0; i < files_list.Length; i++)
			{
				int id = 0;
				int lastDotIndex = files_list[i].LastIndexOf('.');
				if (lastDotIndex != -1 && lastDotIndex < files_list[i].Length - 1)
				{
					string extension = files_list[i].Substring(lastDotIndex + 1);
					if (extension == "txt")
						id = 1;
					if (extension == "ras")
						id = 2;
					if (extension == "SysData")
						continue;
				}



				DesktopIcon newicon = new DesktopIcon
				{
					ID = id,
					x = 20,
					y = 20 + (pos * 85),
					dateTime = DateTime.Now,
					path = @"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + files_list[i],
					Name = files_list[i]

				};
				pos++;
				Icons.Add(newicon);
			}
		}



		public static void RenderMenu(int index)
		{
			bool ready = false;
			switch(Icons[index].ID)
			{
				case 2:
					{
						ready = true;
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, Icons[index].x + 26, Icons[index].y + 26, 200, 100);
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, Icons[index].x + 24, Icons[index].y + 24, 200, 100);
						if (Cosmos.System.MouseManager.X > Icons[index].x + 24 && Cosmos.System.MouseManager.X < Icons[index].x + 224)
						{
							if (Cosmos.System.MouseManager.Y > Icons[index].y + 28 && Cosmos.System.MouseManager.Y < Icons[index].y + 46)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + 24, Icons[index].y + 28, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{
								
									RasPerformer.RunScript(Icons[index].path);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
						}

						if (Cosmos.System.MouseManager.X > Icons[index].x + 24 && Cosmos.System.MouseManager.X < Icons[index].x + 224)
						{
							if (Cosmos.System.MouseManager.Y > Icons[index].y + 46 && Cosmos.System.MouseManager.Y < Icons[index].y + 64)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + 24, Icons[index].y + 46, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{
									Notepad.OpenFile(Icons[index].path);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
						}


						Explorer.CanvasMain.DrawString("Run", Kernel.font18, Color.White, Icons[index].x + 30, Icons[index].y + 28);
						Explorer.CanvasMain.DrawString("Edit", Kernel.font18, Color.White, Icons[index].x + 30, Icons[index].y + 46);
					}
					break;
					

			}
			if(!ready)
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, Icons[index].x + 26, Icons[index].y + 26, 200, 100);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, Icons[index].x + 24, Icons[index].y + 24, 200, 100);
				if (Cosmos.System.MouseManager.X > Icons[index].x + 24 && Cosmos.System.MouseManager.X < Icons[index].x + 224)
				{
					if (Cosmos.System.MouseManager.Y > Icons[index].y + 28 && Cosmos.System.MouseManager.Y < Icons[index].y + 46)
					{
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + 24, Icons[index].y + 28, 200, 18);
						if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
						{
							Notepad.OpenFile(Icons[index].path);
							clicked = true;
							Icons[index].selected = false;
							Icons[index].showMenu = false;
						}
					}
				}
				Explorer.CanvasMain.DrawString("Open", Kernel.font18, Color.White, Icons[index].x + 30, Icons[index].y + 28);

			}


		}

		public static void Render()
		{
			clickedOn = false;
			int menuIndex = -1;
			for (int i = 0; i < Icons.Count; i++)
			{
				if (Icons[i].FinaleName == "")
				{
					StringsAcitons.DrawCenteredString("REFRESHING", 1920, 0, 540, 10, Color.White, Kernel.font18);
					string[] commands = Icons[i].Name.Split(' ');
					string finale = "";

					foreach (string command in commands)
					{
						if (finale.Length + command.Length <= 10)
						{
							finale += (string.IsNullOrEmpty(finale) ? "" : " ") + command;
						}
						else
						{
							if (!string.IsNullOrEmpty(finale))
							{
								Icons[i].FinaleName += (string.IsNullOrEmpty(Icons[i].FinaleName) ? "" : "\n") + finale;
							}
							finale = command;
						}
					}

					if (!string.IsNullOrEmpty(finale))
					{
						Icons[i].FinaleName += (string.IsNullOrEmpty(Icons[i].FinaleName) ? "" : "\n") + finale;
					}
				}

				if(Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
					mainClick = true;
				//Explorer.CanvasMain.DrawString(Icons[i].Name, Kernel.font18, Color.White, Icons[i].x + 48, Icons[i].y + 48);
				if (!Icons[i].selected)
				StringsAcitons.DrawCenteredString(Icons[i].FinaleName, 48, Icons[i].x, Icons[i].y + 50, 15, Color.White, Kernel.font16);
				else
					StringsAcitons.DrawCenteredString(Icons[i].FinaleName, 48, Icons[i].x, Icons[i].y + 50, 15, Color.FromArgb(200,200,200), Kernel.font16);

				switch (Icons[i].ID)
				{
					case 0://NONE
						{
                            if (Icons[i].alphaIcon != null)
                            {
                                Explorer.CanvasMain.DrawImage(Icons[i].alphaIcon, Icons[i].x, Icons[i].y);
                            }
                            else
                            {
                                Explorer.CanvasMain.DrawImageAlpha(Kernel.unknownIcon, Icons[i].x, Icons[i].y);
                                Window.GetTempImage(Icons[i].x, Icons[i].y, 48, 48, "File icon");
                                Icons[i].alphaIcon = Window.tempBitmap;
                            }


                            if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												Notepad.OpenFile(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}
							else if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Right && !Icons[i].showMenu)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{

										for (int j = 0; j < Icons.Count; j++)
										{
											Icons[j].showMenu = false;
										}
										Icons[i].showMenu = true;
									}
										
								}

							}

						}
						break;
					case 1://TXT
						{
							if (Icons[i].alphaIcon !=null)
							{
								Explorer.CanvasMain.DrawImage(Icons[i].alphaIcon, Icons[i].x, Icons[i].y);
                            }
							else
							{
                                Explorer.CanvasMain.DrawImageAlpha(Kernel.txtIcon, Icons[i].x, Icons[i].y);
								Window.GetTempImage(Icons[i].x, Icons[i].y, 48, 48, "File icon");
								Icons[i].alphaIcon = Window.tempBitmap;
                            }

							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												// Obsługa double click
												//	Explorer.CanvasMain.DrawString("Double click! " + (Icons[i].dateTime - DateTime.Now).TotalMilliseconds, Kernel.font18, Color.White, 500, 500);
												Notepad.OpenFile(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}
							else if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Right && !Icons[i].showMenu)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{

										for (int j = 0; j < Icons.Count; j++)
										{
											Icons[j].showMenu = false;
										}
										Icons[i].showMenu = true;
									}

								}

							}
						}
						break;

					case 2://Ra#
						{
							Explorer.CanvasMain.DrawImage(Kernel.rasIcon, Icons[i].x + 6, Icons[i].y);

							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												// Obsługa double click
												//	Explorer.CanvasMain.DrawString("Double click! " + (Icons[i].dateTime - DateTime.Now).TotalMilliseconds, Kernel.font18, Color.White, 500, 500);
												Notepad.OpenFile(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}
							else if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Right && !Icons[i].showMenu)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{

										for (int j = 0; j < Icons.Count; j++)
										{
											Icons[j].showMenu = false;
										}
										Icons[i].showMenu = true;
									}

								}

							}
						}
						break;
				}
				if (Icons[i].showMenu)
					menuIndex = i;
				
			}
			if(menuIndex != -1)
			{
				RenderMenu(menuIndex);
			}
			if(clicked)
			{
				if(Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
				{
					clicked = false;
				}
				
			}
			if(mainClick)
			{
				mainClick = false;
				if(!clickedOn)
				{

					for (int i = 0; i < Icons.Count; i++)
					{
						Icons[i].selected = false;
						Icons[i].showMenu = false;
					}
				}
			}

		}
	}

	public class DesktopIcon
	{
		public int ID;
		public string Name;
		public string path;
		public string FinaleName = "";
		public bool selected;
		public int x, y;
		public DateTime dateTime;
		public bool showMenu;
		public Bitmap alphaIcon;
	}
}
