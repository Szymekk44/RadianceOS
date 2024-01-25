using Cosmos.System;
using Cosmos.System.Graphics;
using RadianceOSInstaller.System.Managment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using Console = System.Console;

namespace RadianceOSInstaller
{
    public static class PreInstaller
    {
		public static int Stat = 0;
		public static bool canFormat;
		public static string CurrString = "";
		public static string Error = "";
		public static void Render()
        {
			Graphic.CanvasMain.DrawImage(Kernel.Wallpaper, 0, 0);
			if (Kernel.WindowDark == null)
			{
				Window.Window.GetTempImageDark(360, 190, 1200, 700, "Dark", 0.6f);
				Kernel.WindowDark = Window.Window.tempBitmap;
			}
			if (Kernel.WindowText == null)
			{
				Graphic.CanvasMain.DrawImage(Kernel.WindowDark, 360, 190);
				StringsAcitons.DrawCenteredTTFString("RadianceOS " + Kernel.RadianceOSver + " - " + Kernel.RadianceOSsubVer + " Installer (v. " + Kernel.version + ")", 1000, 460, 215 + 20, 30, Color.White, "UMB", 40);

				switch(Stat)
				{
					case 0:
						{
							Graphic.CanvasMain.DrawImageAlpha(new Bitmap(Kernel.Logo2), 360 + 414, 260);
							StringsAcitons.DrawCenteredTTFString("Welcome to RadianceOS installer\nTo start the installation process, please click Install\n\nWarning!\nThis version of the RadianceOS installer does not have the ability to test the system.\nIf you want to run RadianceOS on REAL hardware, please download a different version of the installer.\nOtherwise, your disk may be damaged during formatting.\nPlease do not install RadianceOS on functional computers. This may permanently damage your data.", 1000, 460, 215 + 125, 30, Color.White, "UMR", 22);
						}
						break;
					case 1:
						{
							Graphic.CanvasMain.DrawImageAlpha(new Bitmap(Kernel.Logo2), 360 + 414, 260);
							StringsAcitons.DrawCenteredTTFString("About RadianceOS\n\nRa# version: " + Kernel.RasVersion + "\nRam: " + Cosmos.Core.GCImplementation.GetAvailableRAM() + "MB (Using " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "MB)" + "\n\nRadianceOS is an open source operating system created by Szymekk using Cosmos\nC# Open Source Managed Operating System\n\nSzymekk.pl\nGithub: Szymekk.pl/RadianceOS\nYouTube.com/Szymekk\ngocosmos.org", 1000, 460, 215 + 125, 30, Color.White, "UMR", 22);
						}
						break;
					case 2:
						{
							StringsAcitons.DrawCenteredTTFString("Disk formatting\n\nDisk size: " + Kernel.fs.Disks[0].Size / (1024 * 1024) + " MB" + "\nFS type: FAT32\nWARNING: Please do not try this on actual hardware!\nIt may cause IRREPARBLE DAMAGE to your data.\nIt is recommended to use a virtual machine like VMware, Hyper-V, just to name a few.\nMore info: https://cosmosos.github.io/articles/Kernel/VFS.html", 1000, 460, 215 + 50, 30, Color.White, "UMR", 22);
						}
						break;
					case 3:
						{
							StringsAcitons.DrawCenteredTTFString("Disk formatting\n\nPlease enter custom patrition size in MB (Min. 32MB!)\nFS type: FAT32\nWARNING: Please do not try this on actual hardware!\nIt may cause IRREPARBLE DAMAGE to your data.\nIt is recommended to use a virtual machine like VMware, Hyper-V, just to name a few.\nMore info: https://cosmosos.github.io/articles/Kernel/VFS.html", 1000, 460, 215 + 50, 30, Color.White, "UMR", 22);
						}
						break;
					case 4:
						{
							StringsAcitons.DrawCenteredTTFString("Disk formatting\n\nDisk formatting completed successfully.\nPlease restart your computer to install the system", 1000, 460, 215 + 50, 30, Color.White, "UMR", 22);
						}
						break;
					case 5:
						{
							StringsAcitons.DrawCenteredTTFString("Disk formatting\n\nError: " + Error, 1000, 460, 215 + 50, 30, Color.White, "UMR", 22);
						}
						break;
				}


				Window.Window.GetTempImage(360, 190, 1200, 700, "Image Text");
				Kernel.WindowText = Window.Window.tempBitmap;
			}
			Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
			int MX = (int)MouseManager.X;
			int MY = (int)MouseManager.Y;
			switch (Stat)
			{
				case 0:
					{
						if(MX > 760 && MX < 1160)
						{
							if(MY > 215 + 550 && MY < 215 + 550 + 50)
							{
								DrawButton("About", 215 + 550, true);
								if(MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 1;
								}
							}
							else
								DrawButton("About", 215 + 550, false);

							if (MY > 215 + 610 && MY < 215 + 610 + 50)
							{
								DrawButton("Install", 215 + 610, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									if(Kernel.fs.Disks[0].Size != int.MaxValue && Kernel.fs.Disks[0].Size != 0) 
									Stat = 2;
									else
										Stat = 3;
								}
							}
							else
								DrawButton("Install", 215 + 610, false);
						}
						else
						{
							DrawButton("About", 215 + 550, false);
							DrawButton("Install", 215 + 610, false);
						}
							
					}
					break;
				case 1:
					{
						if (MX > 760 && MX < 1160)
						{
	

							if (MY > 215 + 610 && MY < 215 + 610 + 50)
							{
								DrawButton("Back", 215 + 610, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 0;
								}
							}
							else
								DrawButton("Back", 215 + 610, false);
						}
						else
						{
							DrawButton("Back", 215 + 610, false);
						}
					}
					break;
				case 2:
					{
						if (MX > 760 && MX < 1160)
						{
							if (MY > 215 + 490 && MY < 215 + 490 + 50)
							{
								DrawButton("Back", 215 + 490, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 0;
								}
							}
							else
								DrawButton("Back", 215 + 550, false);


							if (MY > 215 + 550 && MY < 215 + 550 + 50)
							{
								DrawButton("Enter Custom Size", 215 + 550, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 3;
								}
							}
							else
								DrawButton("Enter Custom Size", 215 + 550, false);

							if (MY > 215 + 610 && MY < 215 + 610 + 50)
							{
								DrawButton("Continue", 215 + 610, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									try
									{
										if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
											Kernel.fs.Disks[0].DeletePartition(0);
										Kernel.fs.Disks[0].Clear();
										int diskSize = 0;
										if (CurrString == "")
										{
											diskSize = Kernel.fs.Disks[0].Size / 1048576;
										}
										else
										{
											diskSize = int.Parse(CurrString);
										}

										if (Kernel.fs.Disks[0].Size > 1)
										{
											Kernel.fs.Disks[0].CreatePartition(diskSize);
										}
										else
											Kernel.fs.Disks[0].CreatePartition(diskSize);

										Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
										Stat = 4;
									}
									catch(Exception e)
									{
										Error = e.Message;
										Stat = 5;
									}
								}
							}
							else
								DrawButton("Continue", 215 + 610, false);
						}
						else
						{
							DrawButton("Back", 215 + 490, false);
							DrawButton("Enter Custom Size", 215 + 550, false);
							DrawButton("Continue", 215 + 610, false);
						}

					}
					break;
				case 3:
					{
						
						while (Console.KeyAvailable)
						{
							ConsoleKeyInfo key = Console.ReadKey(true);
							switch (key.Key)
							{
								case ConsoleKey.D0:
									if (CurrString.Length > 0 && CurrString.Length < 18)
										CurrString += "0";
									break;
								case ConsoleKey.D1:
									if (CurrString.Length < 18)
										CurrString += "1";
									break;
								case ConsoleKey.D2:
									if (CurrString.Length < 18)
										CurrString += "2";
									break;
								case ConsoleKey.D3:
									if (CurrString.Length < 18)
										CurrString += "3";
									break;
								case ConsoleKey.D4:
									if (CurrString.Length < 18)
										CurrString += "4";
									break;
								case ConsoleKey.D5:
									if (CurrString.Length < 18)
										CurrString += "5";
									break;
								case ConsoleKey.D6:
									if (CurrString.Length < 18)
										CurrString += "6";
									break;
								case ConsoleKey.D7:
									if (CurrString.Length < 18)
										CurrString += "7";
									break;
								case ConsoleKey.D8:
									if (CurrString.Length < 18)
										CurrString += "8";
									break;
								case ConsoleKey.D9:
									if (CurrString.Length < 18)
										CurrString += "9";
									break;
								case ConsoleKey.Backspace:
									if (CurrString.Length > 0)
										CurrString = CurrString.Remove(CurrString.Length - 1, 1);
									break;
							}
						}

						StringsAcitons.DrawCenteredTTFString(CurrString + "|", 1000, 460, 215 + 350, 30, Color.White, "UMR", 24);
						if(CurrString.Length > 0)
						{
							if (int.Parse(CurrString) > 32)
								canFormat = true;
							else
								canFormat = false;
						}
						else
						{
							canFormat = false;
						}

						if (MX > 760 && MX < 1160)
						{
							if (MY > 215 + 550 && MY < 215 + 550 + 50)
							{
								DrawButton("Back", 215 + 550, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 0;
								}
							}
							else
								DrawButton("Back", 215 + 550, false);
							if(canFormat)
							{
								if (MY > 215 + 610 && MY < 215 + 610 + 50)
								{
									DrawButton("Continue", 215 + 610, true);
									if (MouseManager.MouseState == MouseState.Left)
									{
										Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
										Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
										Kernel.WindowText = null;
									
											try
											{
												if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
													Kernel.fs.Disks[0].DeletePartition(0);
												Kernel.fs.Disks[0].Clear();
												int diskSize = 0;
												if (CurrString == "")
												{
													diskSize = Kernel.fs.Disks[0].Size / 1048576;
												}
												else
												{
													diskSize = int.Parse(CurrString);
												}

												if (Kernel.fs.Disks[0].Size > 1)
												{
													Kernel.fs.Disks[0].CreatePartition(diskSize);
												}
												else
													Kernel.fs.Disks[0].CreatePartition(diskSize);

												Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);


												Stat = 4;
											}
											catch(Exception ex)
											{
												Stat = 5;
												Error = ex.Message;
											}
										
									}
								}
								else
									DrawButton("Continue", 215 + 610, false);
							}
							else
							{
								DrawButton("Enter patrition size...", 215 + 610, false);
							}
						
						}
						else
						{
							DrawButton("Back", 215 + 550, false);
							if(canFormat)
							DrawButton("Continue", 215 + 610, false);
							else
								DrawButton("Enter patrition size...", 215 + 610, false);
						}

					}
					break;
				case 4:
					{
						if (MX > 760 && MX < 1160)
						{


							if (MY > 215 + 610 && MY < 215 + 610 + 50)
							{
								DrawButton("Restart", 215 + 610, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									StringsAcitons.DrawCenteredTTFString("Restarting", (int)Graphic.screenSizeX, (int)Graphic.screenSizeX / 2, (int)Graphic.screenSizeY / 2, 20, Color.White, "UMB", 36);
									Thread.Sleep(500);
									Cosmos.System.Power.Reboot();
								}
							}
							else
								DrawButton("Restart", 215 + 610, false);
						}
						else
						{
							DrawButton("Restart", 215 + 610, false);
						}
					}
					break;
				case 5:
					{
						if (MX > 760 && MX < 1160)
						{


							if (MY > 215 + 610 && MY < 215 + 610 + 50)
							{
								DrawButton("Back", 215 + 610, true);
								if (MouseManager.MouseState == MouseState.Left)
								{
									Graphic.CanvasMain.DrawImage(Kernel.WallpaperL, 0, 0);
									Graphic.CanvasMain.DrawImage(Kernel.WindowText, 360, 190);
									Kernel.WindowText = null;
									Stat = 0;
								}
							}
							else
								DrawButton("Back", 215 + 610, false);
						}
						else
						{
							DrawButton("Back", 215 + 610, false);
						}
					}
					break;

			}
			Graphic.CanvasMain.DrawImageAlpha(Kernel.cursor, MX, MY);
			Graphic.CanvasMain.Display();
		}

		public static void DrawButton(string text,int y ,bool selected)
		{
			if(!selected)
			Graphic.CanvasMain.DrawFilledRectangle(Kernel.main, 760, y, 400, 50);
			else
				Graphic.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, 760, y, 400, 50);
			StringsAcitons.DrawCenteredTTFString(text, 400, 360 + 400, y + 30, 20, Color.White, "UMR", 20);
		}
    }
}
