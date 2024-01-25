using Cosmos.Core.Memory;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CosmosTTF;
using IL2CPU.API.Attribs;
using RadianceOSInstaller.System.ConsoleMode;
using RadianceOSInstaller.System.Managment;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using Sys = Cosmos.System;

namespace RadianceOSInstaller
{
	public class Kernel : Sys.Kernel
	{
		public static CosmosVFS fs;
		public static string version = "1.0.0";
		public static string RadianceOSver = "1.0", RadianceOSsubVer = "0.0.1";
		public static string RasVersion = "alpha 1.0.0";
		public static Bitmap WindowDark, WindowText, Wallpaper, WallpaperL, cursor;
		public static bool render;
		public static bool diskReady;
		public static bool procent;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Fonts.zap-ext-light16.psf")]
		public static byte[] Font16;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Fonts.zap-ext-light18.psf")]
		public static byte[] Font18;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Fonts.lat9w-16.psf")]
		public static byte[] FontLat;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Fonts.ruscii_8x16.psf")]
		public static byte[] FontRuscii;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Fonts.tis-ptlight.f16.psf")]
		public static byte[] FontTis;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.TTF.Fonts.UbuntuMono-Regular.ttf")]
		public static byte[] UbuntuMonoRegular;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.TTF.Fonts.UbuntuMono-Bold.ttf")]
		public static byte[] UbuntuMonoBold;

		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.wll1.bmp")]
		public static byte[] Wallpaper1;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.wll2.bmp")]
		public static byte[] Wallpaper2;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.wallpaper-tree.bmp")]
		public static byte[] Wallpaper3;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.wallpaper-tree2.bmp")]
		public static byte[] Wallpaper4;

		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.wallpaper-tree3.bmp")]
		public static byte[] Wallpaper01;
		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.login.bmp")]
		public static byte[] Wallpaper02;



		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.Packages.RadianceOS iconShadow.bmp")]
		public static byte[] Logo2;


		[ManifestResourceStream(ResourceName = "RadianceOSInstaller.Resources.cursor.bmp")]
		public static byte[] Cursor;

		public static Font font16;
		public static Font font18, fontRuscii, fontLat, fontTis, fontDefault;

		public static int InstallerProgress = 0;
		public static int status = 0;
		public static int Progress = 0;
		public static Color main = Color.FromArgb(34, 32, 48);
		public static Color lightMain = Color.FromArgb(54, 51, 79), lightlightMain = Color.FromArgb(63, 59, 99);
		public static Color shadow = Color.FromArgb(26, 24, 36);
		public static string statusString = "Wallpaper - Radiance";
		int curr = 0;
		protected override void BeforeRun()
		{
			Console.OutputEncoding = Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance.GetEncoding(437);
			Console.SetWindowSize(90, 30);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Running Package RadianceOS " + version);
			DrawConsole.DrawLogo();
			Console.ForegroundColor = ConsoleColor.White;
			try
			{
				fs = new Sys.FileSystem.CosmosVFS();
				Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
			

			if (!File.Exists(@"0:\RadianceOS\Settings\DisplayMode.dat"))
			{
					DisplaySizeSelector.SelectMode();
			}
			else
			{
				SaveMode(int.Parse(File.ReadAllText(@"0:\RadianceOS\Settings\DisplayMode.dat")));
					diskReady = true;
			}


			Console.WriteLine("Loading ttf...");
			TTFManager.RegisterFont("UMR", UbuntuMonoRegular);
			TTFManager.RegisterFont("UMB", UbuntuMonoBold);

			Console.WriteLine("Loading files...");
			font16 = PCScreenFont.LoadFont(Font16);
			WriteLineOK("Font16 Default");
			font18 = PCScreenFont.LoadFont(Font18);
			WriteLineOK("Font18 Default");
			fontRuscii = PCScreenFont.LoadFont(FontRuscii);
			WriteLineOK("Font16 Ruscii");
			fontLat = PCScreenFont.LoadFont(FontLat);
			WriteLineOK("Font16 Lat");
			fontTis = PCScreenFont.LoadFont(FontTis);
			WriteLineOK("Font16 Tis");
			fontDefault = PCScreenFont.Default;

	
				WallpaperL = new Bitmap(Wallpaper02);
				cursor = new Bitmap(Cursor);
			Graphic.Start();
			}
			catch (Exception ex)
			{
				Kernel.WriteLineERROR(ex.Message);
			}
			diskReady = true;
			try
			{
				File.Create(@"0:\Test.txt");
			}
			catch (Exception ex)
			{
				diskReady = false;
			}
			if(diskReady)
			{
				InstallerProgress = 1;
				Wallpaper = new Bitmap(Wallpaper1);
			}
			else
				Wallpaper = new Bitmap(Wallpaper01);
		}
		protected override void Run()
		{
			switch(InstallerProgress)
			{
				case 0:
					{
						PreInstaller.Render();
					}
					break;
				case 1:
					{
						Graphic.Render();
						switch (status)
						{
							case 0:
								{
									statusString = "Creating Directories";
									procent = true;

									Directory.CreateDirectory(@"0:\RadianceOS");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Wallpapers");
									Progress++;
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Icons");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Audio");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Apps");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\System\Files\Images");
									Progress++;
									Graphic.Render();
									Directory.CreateDirectory(@"0:\RadianceOS\Settings");
									Progress++;
									Graphic.Render();
									status++;

								}
								break;
							case 1:
								{
									statusString = "Saving Image: Wallpaper 1/6";
								
									procent = false;
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper1.bmp"))
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper1.bmp", Wallpaper1, Wallpaper1.Length);
									else
										status++;
								}
								break;
							case 2:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper2.bmp"))
									{
										statusString = "Saving Image: Wallpaper 2/6";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper2.bmp", Wallpaper2, Wallpaper2.Length);
									}
									else
										status++;
								}
								break;
							case 3:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper3.bmp"))
									{
										statusString = "Saving Image: Wallpaper 3/6";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper3.bmp", Wallpaper3, Wallpaper3.Length);
									}
									else
										status++;
								}
								break;
							case 4:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper4.bmp"))
									{
										statusString = "Saving Image: Wallpaper 4/6";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper4.bmp", Wallpaper4, Wallpaper4.Length);
									}
									else
										status++;
								}
								break;
							case 5:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper5.bmp"))
									{
										statusString = "Saving Image: Wallpaper 5/6";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper5.bmp", Wallpaper01, Wallpaper01.Length);
									}
									else
										status++;
								}
								break;
							case 6:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper6.bmp"))
									{
										statusString = "Saving Image: Wallpaper 6/6";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Wallpapers\Wallpaper6.bmp", Wallpaper02, Wallpaper02.Length);
									}
									else
										status++;
								}
								break;
							case 7:
								{
									if (!File.Exists(@"0:\RadianceOS\System\Files\Images\ShadowLogo.bmp"))
									{
										statusString = "Saving Image: Logo - RadianceOS Shadow Logo";
										ZapisywanieFragmentow(@"0:\RadianceOS\System\Files\Images\ShadowLogo.bmp", Logo2, Logo2.Length);
									}
									else
										status++;
								}
								break;
							default:
								{
									statusString = "Installed!";
									Thread.Sleep(1500);
									Cosmos.System.Power.Reboot();
								}
								break;
						}
					}
					break;
			}
			if(curr >= 20)
			{
				Heap.Collect();
				curr = 0;
			}
			else
				curr++;

		}

		static void ZapisywanieFragmentow(string sciezkaPliku, byte[] dane, int rozmiarFragmentu)
		{
			Graphic.Render();
			File.Create(sciezkaPliku);
			File.WriteAllBytes(sciezkaPliku, dane);
			status++;
			Progress = 0;
			Heap.Collect();
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
		}

		public static void WriteLineOK(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[OK] ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.Gray;
			Heap.Collect();
			Console.ForegroundColor = ConsoleColor.White;

		}

		public static void WriteLineERROR(string text)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("[ERROR] ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(text);
		}

		public static void WriteLineWARN(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("[WARN] ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(text);
		}
	}
}
