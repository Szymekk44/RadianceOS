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

namespace RadianceOS.System.Apps
{
	public static class Explorer
	{
		public static Canvas CanvasMain;

		public static bool Clicked;

		public static bool scalingX, scalingXleft, scalingY;
		public static int ClickedIndex;
		public static bool ClickedOnWindow;
		static int OldX, OldY;



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
		}

		public static void ResizeWallpaper(int SizeX, int SizeY)
		{
			CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0, (int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
			Window.GetTempImage(0, 0, (int)Explorer.screenSizeX, (int)Explorer.screenSizeY, "Wallpaper");
			Kernel.Wallpaper1 = Window.tempBitmap;
		}



		public static void Update()
		{
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

			if (Kernel.TaskBar1 == null)
			{
				Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);

				Kernel.TaskBar1 = Window.tempBitmap;
				if (Kernel.Wallpaper1.Width != screenSizeX || Kernel.Wallpaper1.Height != screenSizeY)
				{
					ResizeWallpaper((int)screenSizeX, (int)screenSizeY);

				}

				Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);
				Kernel.TaskBar1 = Window.tempBitmap;
				UpdateIcons();
			}

			if (drawIcons)
				DrawDesktopApps.Render();

			try
			{
				if (!Kernel.diskReady)
					CanvasMain.DrawString("RadianceOS is not fully installed on this PC!", Kernel.font16, Color.Gray, (int)Explorer.screenSizeX - 275, (int)Explorer.screenSizeY - 20);
				for (int i = 0; i < Process.Processes.Count; i++)
				{
					if (i >= Process.Processes.Count)
						break;
					if (Process.Processes[i].hidden)
						continue;
					switch (Process.Processes[i].ID)
					{
						case 0:
							MessageBox.Render(Process.Processes[i].Name, Process.Processes[i].texts, Process.Processes[i].metaData, Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i, "OK");
							break;
						case 1:
							if (Process.Processes[i].lines.Count == 0)
							{
								TextColor empty = new TextColor
								{
									text = "",
									color = Color.White,
								};

								Process.Processes[i].lines.Add(empty);

							}
							Terminal.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i, Process.Processes[i].lines);

							break;
						case 2:
							Notepad.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i, Process.Processes[i].defaultLines);
							break;
						case 3:
							RasRender.Render(i);
							break;
						case 4:
							// Rendering the new Login window, keeping the old one, just in case.
							//Login.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							Security.Auth.LoginScreen.Render(0, 0, (int)screenSizeX, (int)screenSizeY, i);
							break;
						case 5:
							Settings.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 6:
							Snake.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i, Process.Processes[i].fragments, Process.Processes[i].fragments2);
							break;
						case 7:
							SystemInfo.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i, Process.Processes[i].tempBool);
							break;
						case 8:
							RadiantWave.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 9:
							Welcome.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 10:
							FileExplorer.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 11:
							Information.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 12:
							RasExecuter.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 13:
							SecurityManager.Render(Process.Processes[i].X, Process.Processes[i].Y, Process.Processes[i].SizeX, Process.Processes[i].SizeY, i);
							break;
						case 100:
							NewInstallator.Render(i, Process.Processes[i].tempInt, Process.Processes[i].X, Process.Processes[i].Y, 800, 500, Process.Processes[i].tempBool);
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
					for (int i = 0; i < Process.Processes.Count; i++)
					{
						if (i >= Process.Processes.Count)
							return;
						if (!Process.Processes[i].moveAble)
							continue;
						if (Process.Processes[i].hidden)
							continue;
						if (MX - 3 >= Process.Processes[i].X && MX + 3 <= Process.Processes[i].X + Process.Processes[i].SizeX)
						{
							if (MY - 3 >= Process.Processes[i].Y && MY + 3 <= Process.Processes[i].Y + 25)
							{
								ClickedIndex = i;
								ClickedOnWindow = true;
								OldX = MX - Process.Processes[i].X;
								OldY = MY - Process.Processes[i].Y;
								//CLOSING
								if (MX >= Process.Processes[i].X + Process.Processes[i].SizeX - 38 && MX <= Process.Processes[i].X + Process.Processes[i].SizeX - 8 && Process.Processes[i].closeAble)
								{

									if (Process.Processes[i].ID == 5)
									{
										if (Process.Processes[i].tempInt == 1)
										{
											Settings.resouresLoaded--;
											if (Settings.resouresLoaded <= 0)
											{
												Settings.UnloadData();
											}
										}
									}
									int scanId = 0;

									switch (Process.Processes[i].ID)
									{
										case 6:
											scanId = 1;
											break;
										case 7:
											scanId = 1;
											break;
									}

									Process.Processes.RemoveAt(i);
									bool found = false;
									if (scanId != 0)
									{
										switch (scanId)
										{
											case 1:
												{
													for (int j = 0; j < Process.Processes.Count; j++)
													{
														switch (Process.Processes[j].ID)
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
								else if (MX >= Process.Processes[i].X + Process.Processes[i].SizeX - 68 && MX <= Process.Processes[i].X + Process.Processes[i].SizeX - 40)
								{
									if (Process.Processes[i].sizeAble)
									{
										if (!Process.Processes[i].maximized)
										{
											Process.Processes[i].maximized = true;
											Process.Processes[i].notMaxX = Process.Processes[i].SizeX;
											Process.Processes[i].notMaxY = Process.Processes[i].SizeY;
											Process.Processes[i].X = 0;
											Process.Processes[i].Y = 0;
											Process.Processes[i].SizeX = (int)screenSizeX;
											Process.Processes[i].SizeY = (int)screenSizeY - TaskBarHeight;
											ClickedOnWindow = false;
										}
										else
										{
											Process.Processes[i].maximized = false;
											Process.Processes[i].SizeX = Process.Processes[i].notMaxX;
											Process.Processes[i].SizeY = Process.Processes[i].notMaxY;
											Process.Processes[i].X = (int)screenSizeX - Process.Processes[i].SizeX;
											Process.Processes[i].Y = 0;
											ClickedOnWindow = false;
										}
									}
									if (Process.Processes[i].hideAble)
									{
										if (Process.Processes[i].sizeAble)
										{
											if (MX >= Process.Processes[i].X + Process.Processes[i].SizeX - 96 && MX <= Process.Processes[i].X + Process.Processes[i].SizeX - 68)
											{
												Process.Processes[i].hidden = true;
											}
										}
										else
										{
											if (MX >= Process.Processes[i].X + Process.Processes[i].SizeX - 68 && MX <= Process.Processes[i].X + Process.Processes[i].SizeX - 40)
											{
												Process.Processes[i].hidden = true;
											}
										}
									}
								}
							
							
							
						}




							//SELECTING
							switch (Process.Processes[i].ID)
							{
								case 1:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												InputSystem.CurrentString = Process.Processes[i].lines[Process.Processes[i].lines.Count - 1].text;
											}

										}
									}
									break;
								case 2:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												InputSystem.CurrentString = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine];
											}

										}
									}
									break;
								case 3:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												InputSystem.CurrentString = RasPerformer.Data[Process.Processes[i].tempInt].output[RasPerformer.Data[Process.Processes[i].tempInt].output.Count - 1].text;
											}

										}
									}
									break;
								case 6:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												Process.Processes[i].tempInt2 = 0;
											}

										}
									}
									break;
								case 8:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												InputSystem.CurrentString = Process.Processes[i].texts[0];
											}

										}
									}
									break;
								case 12:
									{
										if (MY >= Process.Processes[i].Y + 25 && MY <= Process.Processes[i].Y + 25 + Process.Processes[i].SizeY)
										{
											if (!Process.Processes[i].selected)
											{
												for (int j = 0; j < Process.Processes.Count; j++)
												{
													Process.Processes[j].selected = false;
												}
												Process.Processes[i].selected = true;
												//InputSystem.CurrentString = Process.Processes[i].lines[Process.Processes[i].lines.Count - 1].text;
												InputSystem.CurrentString = Process.Processes[i].RasData.lines[Process.Processes[i].RasData.lines.Count - 1].text;
											}

										}
									}
									break;
							}
						}

						if (Process.Processes[i].sizeAble)
						{
							if (MY + 5 > Process.Processes[i].Y && MY - 5 < Process.Processes[i].Y + Process.Processes[i].SizeY)
							{
								if (MX >= Process.Processes[i].X + Process.Processes[i].SizeX - 5 && MX < Process.Processes[i].X + Process.Processes[i].SizeX + 5)
								{
									scalingX = true;
									scalingXleft = false;
									scalingY = false;
									ClickedIndex = i;
								}
								else if (MX >= Process.Processes[i].X - 5 && MX < Process.Processes[i].X + 5)
								{
									scalingXleft = true;
									scalingX = false;
									scalingY = false;
									ClickedIndex = i;
								}
								else if (MY >= Process.Processes[i].Y + Process.Processes[i].SizeY - 5 && MY < Process.Processes[i].Y + Process.Processes[i].SizeY + 5)
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
					if (Process.Processes[ClickedIndex].maximized)
					{
						Process.Processes[ClickedIndex].maximized = false;
						Process.Processes[ClickedIndex].SizeX = Process.Processes[ClickedIndex].notMaxX;
						Process.Processes[ClickedIndex].SizeY = Process.Processes[ClickedIndex].notMaxY;

						if (MX - (short)OldX > 0 && MX - (short)OldX < Process.Processes[ClickedIndex].SizeX)
							Process.Processes[ClickedIndex].X = MX - (short)OldX;
						else
						{
							Process.Processes[ClickedIndex].X = MX - Process.Processes[ClickedIndex].SizeX / 2;
							OldX = MX - Process.Processes[ClickedIndex].X;
							return;
						}

						Process.Processes[ClickedIndex].Y = 0;


					}
					if (MX - (short)OldX > 0)
						Process.Processes[ClickedIndex].X = MX - (short)OldX;
					else
						Process.Processes[ClickedIndex].X = 0;
					if (MY - (short)OldY > 0)
						Process.Processes[ClickedIndex].Y = MY - (short)OldY;
					else
						Process.Processes[ClickedIndex].Y = 0;


					if (Process.Processes.Count - 1 > ClickedIndex)
					{
						Process.Processes.Add(Process.Processes[ClickedIndex]);
						Process.Processes.RemoveAt(ClickedIndex);
						ClickedIndex = Process.Processes.Count - 1;
					}
				}
				else if (scalingX)
				{
					if (MX - Process.Processes[ClickedIndex].X > Process.Processes[ClickedIndex].MinX)
						Process.Processes[ClickedIndex].SizeX = MX - Process.Processes[ClickedIndex].X;
					else
						Process.Processes[ClickedIndex].SizeX = Process.Processes[ClickedIndex].MinX;
				}
				else if (scalingY)
				{
					if (MY - Process.Processes[ClickedIndex].Y > Process.Processes[ClickedIndex].MinY)
						Process.Processes[ClickedIndex].SizeY = MY - Process.Processes[ClickedIndex].Y;
					else
						Process.Processes[ClickedIndex].SizeY = Process.Processes[ClickedIndex].MinY;
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
				Clicked = true;



			Render.Canvas.DrawImageAlpha(Kernel.Cursor1, MX, MY);//CURSOR

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

		public static void UpdateIcons()
		{
			RenderIconx();
			RenderIconSquare();
			RenderIconminus();
			RenderIconInfo();
			RenderIconError();
			RenderIconError2();
			RenderIconWarning();
			RenderSmallIcons();
		}

	}
}
