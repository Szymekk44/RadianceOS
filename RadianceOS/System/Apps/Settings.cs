using Cosmos.System;
using CosmosTTF;
using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using System.IO;
using RadianceOS.System.Graphic;
using static Cosmos.HAL.Drivers.Video.VGADriver;
using RadianceOS.Render;
using System.Text.Json;

namespace RadianceOS.System.Apps
{
	public static class Settings
	{
		public static int resouresLoaded;
		public static bool SettingAllowTTF;


		public static void Render(int X, int Y, int SizeX, int SizeY, int index)
		{
			Window.DrawTop(index, X, Y, SizeX, "Settings", false, true, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + 3, Y + 28, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 25, 200, SizeY - 25);
			//	Explorer.CanvasMain.DrawFilledRectangle(Color.Black, X + 2, Y + 27, SizeX - 4, SizeY - 29);
			bool mouseClicked = false;
			if (Process.Processes[index].tempBool)
			{
				if (MouseManager.MouseState == MouseState.None)
				{
					Process.Processes[index].tempBool = false;
				}
			}

			if (MouseManager.MouseState == MouseState.Left && !Process.Processes[index].tempBool)
			{
				mouseClicked = true;
				Process.Processes[index].tempBool = true;
			}

			DrawButton(0, Y + 25, X, SizeX, index, mouseClicked);
			DrawButton(1, Y + 25, X, SizeX, index, mouseClicked);


			switch (Process.Processes[index].tempInt)
			{
				case 0:
					{
						RenderMain(X, Y, SizeX, SizeY, index);

					}
					break;
				case 1:
					{
						RenderApperance(X, Y, SizeX, SizeY, index, mouseClicked);
						if (Kernel.Wallpaper1small == null)
							Settings.LoadData();
					}
					break;
			}


		}

		public static void RenderMain(int X, int Y, int SizeX, int SizeY, int index)
		{

			if (SettingAllowTTF)
			{
				TTFManager.DrawStringTTF(Explorer.CanvasMain, "Performance", "UMB", Kernel.fontColor, 20, X + 220, Y + 50);
				Explorer.CanvasMain.DrawString("- Disabling these features may speed up RadianceOS", Kernel.fontDefault, Kernel.fontColor, X + 335, Y + 37);
			}
			else
			{
				Explorer.CanvasMain.DrawString("Performance", Kernel.fontRuscii, Kernel.fontColor, X + 220, Y + 37);
				Explorer.CanvasMain.DrawString("- Disabling these features may speed up RadianceOS", Kernel.fontDefault, Kernel.fontColor, X + 316, Y + 37);
			}
			
			Explorer.CanvasMain.DrawString("Allow TTF", Kernel.font18, Kernel.fontColor, X + 245, Y + 61);
			if(!SettingAllowTTF)
			{
				Explorer.CanvasMain.DrawString("The remaining TTF text does not negatively impact system performance.", Kernel.font16, Kernel.fontColor, X + 245+78, Y + 63);
			}
			if(Explorer.MX > X+215 && Explorer.MX < X+235)
			{
				if(Explorer.MY > Y+60 &&  Explorer.MY < Y+80)
				{
					UICheckBox.DrawCheckBox(X + 215, Y + 60, SettingAllowTTF, true);
					if(MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
					{
						SettingAllowTTF = !SettingAllowTTF;
						SaveMainSettings();
					}
				}
				else
					UICheckBox.DrawCheckBox(X + 215, Y + 60, SettingAllowTTF, false);
			}
			else
			{
				UICheckBox.DrawCheckBox(X + 215, Y + 60, SettingAllowTTF, false);
			}
		}

	
		public static void RenderApperance(int X, int Y, int SizeX, int SizeY, int index, bool msc)
		{
			DrawWallpaper(0, Y + 50, X + 260, SizeX, index, msc);
			DrawWallpaper(1, Y + 50, X + 260 + 360, SizeX, index, msc);
			DrawWallpaper(2, Y + 270, X + 260, SizeX, index, msc);
			DrawThemeButton(0, Y + 550, X + 310, SizeX, index, msc);
			DrawThemeButton(1, Y + 550, X + 310, SizeX, index, msc);
			DrawThemeButton(2, Y + 550, X + 310, SizeX, index, msc);
		}
		public static void DrawThemeButton(int id, int y, int x, int sizeX, int index, bool clicked)
		{
			int curr = Kernel.Theme;


			bool selected = false;
			if (Explorer.MY >= y - 50 && Explorer.MY <= y + 50)
			{
				if (Explorer.MX >= x + (id * 110) - 60 && Explorer.MX <= x + (id * 110) + 40)
				{
					selected = true;
					if (clicked)
					{
						if (Kernel.Theme != id)
						{
							Kernel.Theme = id;
							if (Kernel.diskReady)
								File.WriteAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Theme.dat", id.ToString());
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Theme settings could not be saved.\nRadianceOS is not installed on this computer.", MessageBoxCreator.MessageBoxIcon.error, 500);
							}
							Design.ChangeTheme(id);

						}

					}
				}
			}



			if (id != curr)
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(47, 44, 66), x + (id * 110) - 10, y + 10, 50);
				else
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(56, 51, 82), x + (id * 110) - 10, y + 10, 50);
			}
			else
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(41, 36, 66), x + (id * 110) - 10, y + 10, 50);
				else
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(53, 48, 84), x + (id * 110) - 10, y + 10, 50);
			}
			switch (id)
			{
				case 0:
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(26, 24, 36), x + (id * 110) - 10, y + 10, 40);
					break;
				case 1:
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(29, 29, 29), x + (id * 110) - 10, y + 10, 40);
					break;
				case 2:
					Explorer.CanvasMain.DrawFilledCircle(Color.FromArgb(230, 230, 230), x + (id * 110) - 10, y + 10, 40);
					break;
			}
		}
		public static void DrawWallpaper(int id, int y, int x, int sizeX, int index, bool clicked)
		{
			int curr = Explorer.Wallpaper;
			bool selected = false;
			if (Explorer.MY >= y && Explorer.MY <= y + 200)
			{
				if (Explorer.MX >= x && Explorer.MX <= x + 340)
				{
					selected = true;
					if (clicked)
					{
						if (Explorer.Wallpaper != id)
						{
							Explorer.Wallpaper = id;
							if (Kernel.diskReady)
								File.WriteAllText(@"0:\Users\" + Kernel.loggedUser + @"\Settings\Wallpaper.dat", id.ToString());
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Wallpaper settings could not be saved.\nRadianceOS is not installed on this computer.", MessageBoxCreator.MessageBoxIcon.error, 500);
							}
							switch (Explorer.Wallpaper)
							{
								case 0:
									{
										Kernel.Wallpaper1 = new Bitmap(Files.wallpaper1);
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);

										if (Kernel.Wallpaper1.Width != Explorer.screenSizeX || Kernel.Wallpaper1.Height != Explorer.screenSizeY)
										{
											Explorer.ResizeWallpaper((int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
											Kernel.Wallpaper1 = Window.tempBitmap;
										}
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
										Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);
										Kernel.TaskBar1 = Window.tempBitmap;
										DrawDesktopApps.clearIcons();
										DrawDesktopApps.UpdateIcons();
									}
									break;
								case 1:
									{
										Kernel.Wallpaper1 = new Bitmap(Files.wallpaper2);
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);

										if (Kernel.Wallpaper1.Width != Explorer.screenSizeX || Kernel.Wallpaper1.Height != Explorer.screenSizeY)
										{
											Explorer.ResizeWallpaper((int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
											Kernel.Wallpaper1 = Window.tempBitmap;
										}
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
										Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);
										Kernel.TaskBar1 = Window.tempBitmap;
										DrawDesktopApps.clearIcons();
										DrawDesktopApps.UpdateIcons();
									}
									break;
								case 2:
									{
										Kernel.Wallpaper1 = new Bitmap(Files.wallpaper3);
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);

										if (Kernel.Wallpaper1.Width != Explorer.screenSizeX || Kernel.Wallpaper1.Height != Explorer.screenSizeY)
										{
											Explorer.ResizeWallpaper((int)Explorer.screenSizeX, (int)Explorer.screenSizeY);
											Kernel.Wallpaper1 = Window.tempBitmap;
										}
										Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
										Window.GetTempImageDarkAndBlur(0, (int)Explorer.screenSizeY - 40, (int)Explorer.screenSizeX, 40, "TaskBar", 0.5f, 3);
										Kernel.TaskBar1 = Window.tempBitmap;
										DrawDesktopApps.clearIcons();
										DrawDesktopApps.UpdateIcons();
									}
									break;
							}

						}

					}
				}
			}

			if (id != curr)
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(47, 44, 66), x - 10, y + 10, 340, 200);
				else
					Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(56, 51, 82), x - 10, y + 10, 340, 200);
			}
			else
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(41, 36, 66), x - 10, y + 10, 340, 200);
				else
					Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(53, 48, 84), x - 10, y + 10, 340, 200);
			}
			switch (id)
			{
				case 0:
					Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1small, x, y + 20);
					break;
				case 1:
					Explorer.CanvasMain.DrawImage(Kernel.Wallpaper2small, x, y + 20);
					break;
				case 2:
					Explorer.CanvasMain.DrawImage(Kernel.Wallpaper3small, x, y + 20);
					break;
			}
		}

		public static void LoadData()
		{
			Kernel.Wallpaper1small = new Bitmap(Files.wallpaper1S);
			Kernel.Wallpaper2small = new Bitmap(Files.wallpaper2S);
			Kernel.Wallpaper3small = new Bitmap(Files.wallpaper3S);
			resouresLoaded++;
		}
		public static void UnloadData()
		{
			Kernel.Wallpaper1small = null;
			Kernel.Wallpaper2small = null;
			Kernel.Wallpaper3small = null;
			resouresLoaded = 0;
		}
		public static void DrawButton(int id, int y, int x, int sizeX, int index, bool clicked)
		{
			int curr = Process.Processes[index].tempInt;
			bool selected = false;
			if (Explorer.MY >= y + (id * 40) && Explorer.MY <= y + (id * 40) + 40)
			{
				if (Explorer.MX >= x && Explorer.MX <= x + 200)
				{
					selected = true;
					if (clicked && id != Process.Processes[index].tempInt)
					{
						Process.Processes[index].tempInt = id;
						switch (id)
						{
							case 0:
								resouresLoaded--;
								if (resouresLoaded <= 0)
								{
									UnloadData();
									resouresLoaded = 0;
								}
								break;
						}
					}
				}
			}
			if (id != curr)
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.startDefault, x, y + (id * 40), 200, 40);
				else
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.startLight, x, y + (id * 40), 200, 40);
			}
			else
			{
				if (!selected)
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.startDefaultSelected, x, y + (id * 40), 200, 40);
				else
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.startLightSelected, x, y + (id * 40), 200, 40);
			}
			switch (id)
			{
				case 0:
					//Explorer.CanvasMain.DrawString("System", Kernel.font18, Color.White, x+10, y + 7 + (id * 32));
					if (SettingAllowTTF)
						Explorer.CanvasMain.DrawStringTTF("System", "UMR", Kernel.fontColor, 20, x + 10, y + 27 + (id * 32));
					else
						Explorer.CanvasMain.DrawString("System", Kernel.fontDefault, Kernel.fontColor, x + 10, y + 13 + (id * 32));
					break;
				case 1:
					//Explorer.CanvasMain.DrawString("System", Kernel.font18, Color.White, x+10, y + 7 + (id * 32));
					if(SettingAllowTTF)
					Explorer.CanvasMain.DrawStringTTF("Appearance", "UMR", Kernel.fontColor, 20, x + 10, y + 27 + (id * 40));
					else
						Explorer.CanvasMain.DrawString("Appearance", Kernel.fontDefault, Kernel.fontColor, x + 10, y + 13 + (id * 40));
					break;
			}
		}

		public static void SaveMainSettings()
		{
			string path1 = @"0:\Users\" + Kernel.loggedUser + @"\Settings\AllowTTF.dat";
			if (!File.Exists(path1))
			{
				File.Create(path1);
			}
			File.WriteAllText(path1, SettingAllowTTF.ToString());
		}

		public static void LoadMainSettings()
		{
			string path1 = @"0:\Users\" + Kernel.loggedUser + @"\Settings\AllowTTF.dat";
			if (File.Exists(path1))
			{
				SettingAllowTTF = bool.Parse(File.ReadAllText(path1));
			}
			else
			{

				if (!File.Exists(path1))
				{
					File.Create(path1);
				}
				File.WriteAllText(path1, "True");
				SettingAllowTTF = true;
			}
		}
	}
}