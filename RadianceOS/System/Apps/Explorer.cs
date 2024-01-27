using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Cosmos.Core.Memory;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp;
using RadianceOS.System.Radiance;
using CosmosTTF;
using RadianceOS.System.Apps.Games;
using RadianceOS.System.Graphic;
using RadianceOS.System.Apps.RadianceOSwebBrowser;
using RadianceOS.System.Apps.NewInstaller;
using Cosmos.System;
using RadianceOS.System.Programming.RaSharp2;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using RadianceOS.System.Security.Auth;
using System.IO;
using RadianceOS.System.NewApps;

namespace RadianceOS.System.Apps
{
	public static class Explorer
	{
		public static Canvas CanvasMain;

		public static bool Clicked;
		public static bool SingleClick; // Only pulses when the user clicks
		private static bool WasSingleClick;

		public static bool scalingX, scalingXleft, scalingY;
		public static int ClickedIndex;
		public static bool ClickedOnWindow;
		static int OldX, OldY;
		static List<App> NewApps = new();


		public static bool DrawTaskbar, DrawMenu;


		public static uint screenSizeX = 1920, screenSizeY = 1080;

		static int _frames;
		public static int _fps = 0;
		public static int _deltaT = 0;

		public static int MY, MX;

		public static bool drawIcons;
		public static int Wallpaper = 0;
		public static int fail = 0;

		public static int TaskBarHeight = 40;

		public static bool DrawCursor = true;


		public static void Start()
		{
			if (Kernel.workingAudio)
			{
				/*var mixer = new AudioMixer();
				var audioStream = MemoryAudioStream.FromWave(Files.startupAduio);
				var driver = AC97.Initialize(bufferSize: 4096);
				mixer.Streams.Add(audioStream);

				var audioManager = new AudioManager()
				{
					Stream = mixer,
					Output = driver
				};
				audioManager.Enable();*/
			}
			BootScreen.BootImage = null;
			Heap.Collect();
            Cosmos.System.MouseManager.ScreenWidth = screenSizeX;
			Cosmos.System.MouseManager.ScreenHeight = screenSizeY;
			Cosmos.System.MouseManager.X = screenSizeX / 2; Cosmos.System.MouseManager.Y = screenSizeY / 2;




			CanvasMain = FullScreenCanvas.GetFullScreenCanvas(new Mode(screenSizeX, screenSizeY, ColorDepth.ColorDepth32));
			
			// Initialise Radiance Security (It'll start everything up)
			Security.Service.Initialise();

		}

		public static void ResizeWallpaper(int SizeX, int SizeY)
		{
			if(Kernel.Wallpaper1.Width != SizeX || Kernel.Wallpaper1.Height != SizeY)
			{
				CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0, (int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
				Window.GetTempImage(0, 0, (int)Explorer.screenSizeX, (int)Explorer.screenSizeY, "Wallpaper");
				Kernel.Wallpaper1 = Window.tempBitmap;
			}
		}



		public static void Update()
		{
            // Update the internal RS Service
            Security.Service.UpdateInternal();

            MX = (int)Cosmos.System.MouseManager.X;
			MY = (int)Cosmos.System.MouseManager.Y;

			CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
			if (Kernel.Wallpaper1.Width != Explorer.screenSizeX || Kernel.Wallpaper1.Height != Explorer.screenSizeY)
			{
				Explorer.CanvasMain.Clear(Color.Black);
				Explorer.CanvasMain.DrawImage(new Bitmap(Files.RadianceOSIcon), (int)(Explorer.screenSizeX - 456) / 2, (int)(Explorer.screenSizeY) - 250);
				StringsAcitons.DrawCenteredTTFString("FINISHING", (int)Explorer.screenSizeX, 0, (int)(Explorer.screenSizeY) - 125, 25, Color.White, "UMB", 20);
				StringsAcitons.DrawCenteredTTFString("Loading RadianceOS configuration", (int)Explorer.screenSizeX, 0, (int)(Explorer.screenSizeY) - 100, 25, Color.White, "UMR", 15);
				CanvasMain.Display();
				Explorer.ResizeWallpaper((int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
				Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
			}

			if (Kernel.TaskBar1 == null && Radiance.Security.Logged)
			{
				Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);

				Kernel.TaskBar1 = Window.tempBitmap;
				if (Kernel.Wallpaper1.Width != screenSizeX || Kernel.Wallpaper1.Height != screenSizeY)
				{
					ResizeWallpaper((int)screenSizeX, (int)screenSizeY);

				}

				Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);
				Kernel.TaskBar1 = Window.tempBitmap;
				
			}
			
			if (drawIcons)
				DrawDesktopApps.Render();

			// Render UAC
			Security.Service.UpdateUAC();

			try
			{
				if (!Kernel.diskReady)
					CanvasMain.DrawString("RadianceOS is not fully installed on this PC!", Kernel.font16, Color.Gray, (int)Explorer.screenSizeX - 275, (int)Explorer.screenSizeY - 20);
				for (int i = 0; i < Apps.Process.Processes.Count; i++)
				{
					if (i >= Apps.Process.Processes.Count)
						break;
					if (Apps.Process.Processes[i].hidden)
						continue;
					switch (Apps.Process.Processes[i].ID)
					{
						case 0:
							string button1 = "OK", button2 = null;
							if(Apps.Process.Processes[i].defaultLines.Count > 1)
							{
								button1 = Apps.Process.Processes[i].defaultLines[0];
								button2 = Apps.Process.Processes[i].defaultLines[1];
							}
							else if (Apps.Process.Processes[i].defaultLines.Count > 0)
								button1 = Apps.Process.Processes[i].defaultLines[0];
							MessageBox.Render(Apps.Process.Processes[i].Name, Apps.Process.Processes[i].texts, Apps.Process.Processes[i].metaData, Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i, button1, button2);
							break;
						case 1:
							if (Apps.Process.Processes[i].lines.Count == 0)
							{
								TextColor empty = new TextColor
								{
									text = "",
									color = Color.White,
								};

								Apps.Process.Processes[i].lines.Add(empty);

							}
							Terminal.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i, Apps.Process.Processes[i].lines);

							break;
						case 2:
							Notepad.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i, Apps.Process.Processes[i].defaultLines);
							break;
						case 3:
							RasRender.Render(i);
							break;
						case 4:
							// Rendering the new Login window, keeping the old one, just in case.
							//Login.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							Security.Auth.LoginScreen.Render(0, 0, (int)screenSizeX, (int)screenSizeY, i);
							break;
						case 5:
							Settings.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 6:
							Snake.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i, Apps.Process.Processes[i].fragments, Apps.Process.Processes[i].fragments2);
							break;
						case 7:
							SystemInfo.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i, Apps.Process.Processes[i].tempBool);
							break;
						case 8:
							RadiantWave.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 9:
							Welcome.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 10:
							FileExplorer.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 11:
							Information.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 12:
							RasExecuter.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 13:
							SecurityManager.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 99:
							PowerOptions.Render(Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, Apps.Process.Processes[i].SizeX, Apps.Process.Processes[i].SizeY, i);
							break;
						case 100:
							NewInstallator.Render(i, Apps.Process.Processes[i].tempInt, Apps.Process.Processes[i].X, Apps.Process.Processes[i].Y, 800, 500, Apps.Process.Processes[i].tempBool);
							break;
						case 101:
							Security.Service.Update(i);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				fail++;
				if (fail > 2)
					Kernel.Crash("Explorer.Render: " + ex.Message, 4);
			}
			finally
			{
				fail = 0;
			}






			if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
			{


				if (!Clicked)
				{

					ClickedOnWindow = false;
					for (int i = 0; i < Apps.Process.Processes.Count; i++)
					{
						if (i >= Apps.Process.Processes.Count)
							return;
						if (!Apps.Process.Processes[i].moveAble)
							continue;
						if (Apps.Process.Processes[i].hidden)
							continue;
						if (MX - 3 >= Apps.Process.Processes[i].X && MX + 3 <= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX)
						{
							if (MY - 3 >= Apps.Process.Processes[i].Y && MY + 3 <= Apps.Process.Processes[i].Y + 25)
							{
								ClickedIndex = i;
								ClickedOnWindow = true;
								OldX = MX - Apps.Process.Processes[i].X;
								OldY = MY - Apps.Process.Processes[i].Y;
								//CLOSING
								if (MX >= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 38 && MX <= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 8 && Apps.Process.Processes[i].closeAble)
								{

									if (Apps.Process.Processes[i].ID == 5)
									{
										if (Apps.Process.Processes[i].tempInt == 1)
										{
											Settings.resouresLoaded--;
											if (Settings.resouresLoaded <= 0)
											{
												Settings.UnloadData();
											}
										}
									}
									int scanId = 0;

									switch (Apps.Process.Processes[i].ID)
									{
										case 6:
											scanId = 1;
											break;
										case 7:
											scanId = 1;
											break;
									}

									Apps.Process.Processes.RemoveAt(i);
									bool found = false;
									if (scanId != 0)
									{
										switch (scanId)
										{
											case 1:
												{
													for (int j = 0; j < Apps.Process.Processes.Count; j++)
													{
														switch (Apps.Process.Processes[j].ID)
														{
															case 6:
																found = true;
																break;
															case 7:
																found = true;
																break;
														}
													}
												}
												break;
										}
									}
									if (!found)
									{
										switch (scanId)
										{
											case 1:
												{
													Kernel.countFPS = false;
												}
												break;
										}
									}
									return;
								}
								else if (MX >= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 68 && MX <= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 40)
								{
									if (Apps.Process.Processes[i].sizeAble)
									{
										if (!Apps.Process.Processes[i].maximized)
										{
											Apps.Process.Processes[i].maximized = true;
											Apps.Process.Processes[i].notMaxX = Apps.Process.Processes[i].SizeX;
											Apps.Process.Processes[i].notMaxY = Apps.Process.Processes[i].SizeY;
											Apps.Process.Processes[i].X = 0;
											Apps.Process.Processes[i].Y = 0;
											Apps.Process.Processes[i].SizeX = (int)screenSizeX;
											Apps.Process.Processes[i].SizeY = (int)screenSizeY - TaskBarHeight;
											ClickedOnWindow = false;
										}
										else
										{
											Apps.Process.Processes[i].maximized = false;
											Apps.Process.Processes[i].SizeX = Apps.Process.Processes[i].notMaxX;
											Apps.Process.Processes[i].SizeY = Apps.Process.Processes[i].notMaxY;
											Apps.Process.Processes[i].X = (int)screenSizeX - Apps.Process.Processes[i].SizeX;
											Apps.Process.Processes[i].Y = 0;
											ClickedOnWindow = false;
										}
									}
									
								}
								if (Apps.Process.Processes[i].hideAble)
								{
									if (Apps.Process.Processes[i].sizeAble)
									{
										if (MX >= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 96 && MX <= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 68)
										{
											Apps.Process.Processes[i].hidden = true;
										}
									}
									else
									{
										if (MX >= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 68 && MX <= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 40)
										{
											Apps.Process.Processes[i].hidden = true;
										}
									}
								}


							}




							//SELECTING
							switch (Apps.Process.Processes[i].ID)
							{
								case 1:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												InputSystem.CurrentString = Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count - 1].text;
											}

										}
									}
									break;
								case 2:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												InputSystem.CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];
											}

										}
									}
									break;
								case 3:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												InputSystem.CurrentString = RasPerformer.Data[Apps.Process.Processes[i].tempInt].output[RasPerformer.Data[Apps.Process.Processes[i].tempInt].output.Count - 1].text;
											}

										}
									}
									break;
								case 6:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												Apps.Process.Processes[i].tempInt2 = 0;
											}

										}
									}
									break;
								case 8:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												InputSystem.CurrentString = Apps.Process.Processes[i].texts[0];
											}

										}
									}
									break;
								case 12:
									{
										if (MY >= Apps.Process.Processes[i].Y + 25 && MY <= Apps.Process.Processes[i].Y + 25 + Apps.Process.Processes[i].SizeY)
										{
											if (!Apps.Process.Processes[i].selected)
											{
												for (int j = 0; j < Apps.Process.Processes.Count; j++)
												{
													Apps.Process.Processes[j].selected = false;
												}
												Apps.Process.Processes[i].selected = true;
												//InputSystem.CurrentString = Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count - 1].text;
												InputSystem.CurrentString = Apps.Process.Processes[i].RasData.lines[Apps.Process.Processes[i].RasData.lines.Count - 1].text;
											}

										}
									}
									break;
							}
						}

						if (Apps.Process.Processes[i].sizeAble)
						{
							if (MY + 5 > Apps.Process.Processes[i].Y && MY - 5 < Apps.Process.Processes[i].Y + Apps.Process.Processes[i].SizeY)
							{
								if (MX >= Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX - 5 && MX < Apps.Process.Processes[i].X + Apps.Process.Processes[i].SizeX + 5)
								{
									scalingX = true;
									scalingXleft = false;
									scalingY = false;
									ClickedIndex = i;
								}
								else if (MX >= Apps.Process.Processes[i].X - 5 && MX < Apps.Process.Processes[i].X + 5)
								{
									scalingXleft = true;
									scalingX = false;
									scalingY = false;
									ClickedIndex = i;
								}
								else if (MY >= Apps.Process.Processes[i].Y + Apps.Process.Processes[i].SizeY - 5 && MY < Apps.Process.Processes[i].Y + Apps.Process.Processes[i].SizeY + 5)
								{
									scalingX = false;
									scalingXleft = false;
									scalingY = true;
									ClickedIndex = i;
								}
								else
									Reset();
							}
							else
								Reset();
						}
						else
							Reset();


					}


					//	Kernel.Crash("Application: Explorer performed an illegal operation\nMore info: Mouse down event", 4);

				}
				else if (ClickedOnWindow)
				{
					if (Apps.Process.Processes[ClickedIndex].maximized)
					{
						Apps.Process.Processes[ClickedIndex].maximized = false;
						Apps.Process.Processes[ClickedIndex].SizeX = Apps.Process.Processes[ClickedIndex].notMaxX;
						Apps.Process.Processes[ClickedIndex].SizeY = Apps.Process.Processes[ClickedIndex].notMaxY;

						if (MX - (short)OldX > 0 && MX - (short)OldX < Apps.Process.Processes[ClickedIndex].SizeX)
							Apps.Process.Processes[ClickedIndex].X = MX - (short)OldX;
						else
						{
							Apps.Process.Processes[ClickedIndex].X = MX - Apps.Process.Processes[ClickedIndex].SizeX / 2;
							OldX = MX - Apps.Process.Processes[ClickedIndex].X;
							return;
						}

						Apps.Process.Processes[ClickedIndex].Y = 0;


					}
					if (MX - (short)OldX > 0)
						Apps.Process.Processes[ClickedIndex].X = MX - (short)OldX;
					else
						Apps.Process.Processes[ClickedIndex].X = 0;
					if (MY - (short)OldY > 0)
						Apps.Process.Processes[ClickedIndex].Y = MY - (short)OldY;
					else
						Apps.Process.Processes[ClickedIndex].Y = 0;


					if (Apps.Process.Processes.Count - 1 > ClickedIndex)
					{
						Apps.Process.Processes.Add(Apps.Process.Processes[ClickedIndex]);
						Apps.Process.Processes.RemoveAt(ClickedIndex);
						ClickedIndex = Apps.Process.Processes.Count - 1;
					}
				}
				else if (scalingX)
				{
					if (MX - Apps.Process.Processes[ClickedIndex].X > Apps.Process.Processes[ClickedIndex].MinX)
						Apps.Process.Processes[ClickedIndex].SizeX = MX - Apps.Process.Processes[ClickedIndex].X;
					else
						Apps.Process.Processes[ClickedIndex].SizeX = Apps.Process.Processes[ClickedIndex].MinX;
				}
				else if (scalingY)
				{
					if (MY - Apps.Process.Processes[ClickedIndex].Y > Apps.Process.Processes[ClickedIndex].MinY)
						Apps.Process.Processes[ClickedIndex].SizeY = MY - Apps.Process.Processes[ClickedIndex].Y;
					else
						Apps.Process.Processes[ClickedIndex].SizeY = Apps.Process.Processes[ClickedIndex].MinY;
				}

			}
			if (Clicked)
			{
				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
				{
					Clicked = false;
					ClickedOnWindow = false;
					Reset();
				}

			}

			if (DrawMenu)
			{
				StartMenu.Render();
				if (StartMenu.state != 0)
					StartMenu.Move();

				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
				{
					bool clicked = false;

					if (MY >= screenSizeY - 640 && MY <= screenSizeY)
					{
						if (MX >= 5 && MX <= 705)
						{
							clicked = true;
						}
					}
					if (!clicked)
						StartMenu.state = 2;
				}
			}


			//Kernel.Crash("Application: Explorer performed an illegal operation\nMore info: Start Menu error", 2);



			if (DrawTaskbar)
			{
				CanvasMain.DrawImage(Kernel.TaskBar1, 0, (int)Explorer.screenSizeY - 40);
				//FIX LAGS!
				/*int minute = DateTime.Now.Minute;
					string formattedMinute = minute.ToString().PadLeft(2, '0');

				int hour = DateTime.Now.Hour;
				string formattedHour = hour.ToString().PadLeft(2, '0');

				CanvasMain.DrawString(formattedHour + ":" + formattedMinute, Kernel.fontRuscii, Color.White, 1860, 1054);*/

				if (MY >= (int)Explorer.screenSizeY - 35 && MY <= (int)Explorer.screenSizeY - 5)
				{
					if (MX >= 5 && MX <= 95)
					{
						CanvasMain.DrawImage(Kernel.lightButton, 5, (int)Explorer.screenSizeY - 35);
						if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !Clicked)
						{
							if (!DrawMenu)
							{
								DrawMenu = true;
								StartMenu.y = (int)Explorer.screenSizeY - 35;
								StartMenu.state = 1;
							}
							else
							{
								StartMenu.state = 2;
							}

						}
					}
					else
						CanvasMain.DrawImage(Kernel.DarkButton, 5, (int)Explorer.screenSizeY - 35);
				}
				else
					CanvasMain.DrawImage(Kernel.DarkButton, 5, (int)Explorer.screenSizeY - 35);

				TaskBar.Render();
			}

			if (MouseManager.MouseState == MouseState.Left)
			{
                Clicked = true;
				if(Clicked == true && !SingleClick && !WasSingleClick)
				{
					SingleClick = true;
				} else if(Clicked == true && SingleClick && !WasSingleClick)
				{
					SingleClick = false;
					WasSingleClick = true;
				}
            } else
			{
				WasSingleClick = false;
			}

		/*	foreach (var app in NewApps)
			{
				app.Update();
			}

			if (NewApps.Count == 0)
			{
				NewApps.Add(new Testapp(new Rectangle(100, 100, 400, 400)));
			}*/

			if(DrawCursor) Render.Canvas.DrawImageAlpha(Kernel.Cursor1, MX, MY);//CURSOR

			Screens.Shutdown.Render();

            CanvasMain.Display();
		}

		public static void Reset()
		{
			scalingX = false;
			scalingXleft = false;
			scalingY = false;
		}

		public static void RenderIconx()
		{
			CanvasMain.DrawFilledRectangle(Kernel.shadow, 0, 0, 30, 25);
			Window.DrawFullRoundedRectangle(0, 0, 30, 25, 8, Kernel.dark);
			TTFManager.DrawStringTTF(CanvasMain, "x", "UMR", Kernel.fontColor, 24, 9, 18);
			Window.GetTempImage(0, 0, 30, 25, "x");
			Kernel.Xicon = Window.tempBitmap;
		}

		public static void RenderIconSquare()
		{
			CanvasMain.DrawFilledRectangle(Kernel.shadow, 30, 0, 28, 25);
			Window.DrawFullRoundedRectangle(30, 0, 28, 25, 8, Kernel.dark);
			CanvasMain.DrawRectangle(Kernel.fontColor, 38, 7, 11, 11);
			CanvasMain.DrawRectangle(Kernel.fontColor, 39, 8, 9, 9);
			CanvasMain.DrawPoint(Kernel.fontColor, 48, 17);
			CanvasMain.DrawPoint(Kernel.fontColor, 49, 18);
			Window.GetTempImage(30, 0, 28, 25, "max");
			Kernel.maxIcon = Window.tempBitmap;
		}

		public static void RenderIconminus()
		{
			CanvasMain.DrawFilledRectangle(Kernel.shadow, 58, 0, 28, 25);
			Window.DrawFullRoundedRectangle(58, 0, 28, 25, 8, Kernel.dark);
			TTFManager.DrawStringTTF(CanvasMain, "_", "UMR", Kernel.fontColor, 24, 66, 10);
			Window.GetTempImage(58, 0, 28, 25, "-");
			Kernel.MinusIcon = Window.tempBitmap;
		}
		public static void RenderIconInfo()
		{
			CanvasMain.DrawFilledRectangle(Kernel.main, 86, 0, 48, 48);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.info), 86, 0);
			Window.GetTempImage(86, 0, 48, 48, "info");
			Kernel.Info = Window.tempBitmap;
		}
		public static void RenderIconError()
		{
			CanvasMain.DrawFilledRectangle(Kernel.main, 134, 0, 48, 48);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.stop), 134, 0);
			Window.GetTempImage(134, 0, 48, 48, "stop");
			Kernel.Stop = Window.tempBitmap;
		}
		public static void RenderIconWarning()
		{
			CanvasMain.DrawFilledRectangle(Kernel.main, 182, 0, 48, 48);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.warning), 182, 0);
			Window.GetTempImage(182, 0, 48, 48, "warning");
			Kernel.Error = Window.tempBitmap;
		}
		public static void RenderIconError2()
		{
			CanvasMain.DrawFilledRectangle(Kernel.main, 134, 0, 48, 48);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.criticalStop), 134, 0);
			Window.GetTempImage(134, 0, 48, 48, "stop2");
			Kernel.CriticalStop = Window.tempBitmap;
		}
		public static void RenderSmallIcons()
		{
			CanvasMain.DrawFilledRectangle(Kernel.main, 0, 25, 16, 16);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.text16), 0, 25);
			Window.GetTempImage(0, 25, 16, 16, "text16");
			Kernel.text16 = Window.tempBitmap;

			CanvasMain.DrawFilledRectangle(Kernel.main, 0, 25, 16, 16);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.document16), 0, 25);
			Window.GetTempImage(0, 25, 16, 16, "document16");
			Kernel.docuent16 = Window.tempBitmap;

			CanvasMain.DrawFilledRectangle(Kernel.main, 0, 25, 16, 16);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.folder16), 0, 25);
			Window.GetTempImage(0, 25, 16, 16, "folder16");
			Kernel.folder16 = Window.tempBitmap;

			CanvasMain.DrawFilledRectangle(Kernel.main, 0, 25, 16, 16);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.data16), 0, 25);
			Window.GetTempImage(0, 25, 16, 16, "data16");
			Kernel.data16 = Window.tempBitmap;

			CanvasMain.DrawFilledRectangle(Kernel.main, 0, 25, 16, 16);
			CanvasMain.DrawImageAlpha(new Bitmap(Files.sysData16), 0, 25);
			Window.GetTempImage(0, 25, 16, 16, "sysData16");
			Kernel.sysData16 = Window.tempBitmap;
		}

		public static void RenderIconUser()
		{
			
			
				CanvasMain.DrawFilledRectangle(Kernel.shadow, 134, 0, 60, 73);
				if (Kernel.fontColor == Color.White)
					CanvasMain.DrawImageAlpha(new Bitmap(Files.udt), 134, 0);
				else
				CanvasMain.DrawImageAlpha(new Bitmap(Files.ult), 134, 0);
			Window.GetTempImage(134, 0, 60, 73, "usesIcon");
				Kernel.userIcon = Window.tempBitmap;
			

		}

		public static void UpdateIcons()
		{
			Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0,0);
			Explorer.CanvasMain.Display();
			RenderIconx();
			RenderIconSquare();
			RenderIconminus();
			RenderIconInfo();
			RenderIconError();
			RenderIconError2();
			RenderIconWarning();
			RenderSmallIcons();
			RenderIconUser();
		}

	}
}
