using Cosmos.System.FileSystem;
using System;
using Sys = Cosmos.System;
using Cosmos.HAL;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using System.Threading;
using Cosmos.Core.Memory;
using System.IO;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.HAL.Audio;
using RadianceOS.System.Radiance;
using CosmosTTF;
using RadianceOS.System.ConsoleMode;

namespace RadianceOS
{
	public class Kernel : Sys.Kernel
	{
		public static CosmosVFS fs;
		public static Canvas canvas;
		public static Font font16;
		public static Font font18, fontRuscii, fontLat;
		public static Bitmap Wallpaper1;
		public static Bitmap Wallpaper1small, Wallpaper2small;
		public static Bitmap TaskBar1, lightButton, DarkButton, StartMenu;
		public static Bitmap Cursor1;
		public static Bitmap Error, Stop, Info, CriticalStop,DiskError, cmd, notepad, padlockIcon, settingIcon, gamepadIcon, sysinfoIcon, RadiantWave, fileExplorer, userIcon;
		public static Bitmap Xicon, maxIcon, MinusIcon;
		public static Bitmap txtIcon, unknownIcon, rasIcon;
		public static Bitmap text16, docuent16, folder16, data16, sysData16;

		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Websites.Test.skk.bmp")]
		public static byte[] skk;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Websites.Test.cc.bmp")]
		public static byte[] cc;
		[ManifestResourceStream(ResourceName = "RadianceOS.Resources.Websites.Test.ok.bmp")]
		public static byte[] ok;

		public static Color main = Color.FromArgb(34, 32, 48);
		public static Color lightMain = Color.FromArgb(54, 51, 79), lightlightMain = Color.FromArgb(63, 59, 99);
		public static Color shadow = Color.FromArgb(26, 24, 36);
		public static Color middark = Color.FromArgb(19, 18, 26);
		public static Color dark = Color.FromArgb(16, 16, 20);
		public static Color fontColor = Color.White;
		public static Color terminalColor = Color.Black;
		public static Color startDefault = Color.FromArgb(47, 44, 66), startLight = Color.FromArgb(56, 51, 82), startDefaultSelected = Color.FromArgb(41, 36, 66), startLightSelected = Color.FromArgb(53, 48, 84);


		public static bool render;

		public static bool diskReady = true;

		public static string path = @"0:\";

		public static AudioMixer mixer;
		public static AudioDriver driver;

		public static string loggedUser;

		public static bool workingAudio = true;
		public static bool Repair;
		public static int collect = 0;

		public static bool countFPS;
		public static int _frames;
		public static int _fps = 200;
		public static int _deltaT = 0;
		public static ulong MaxRam;

		public static string version = "1.0";
		public static string subversion = "0.0.1";
		public static string RasVersion = "alpha 1.0.0";
		public static bool loggedAsAdmin = false;

		public static int Theme = 0;
		public static string htmlcode = "";

		public static bool AllLoaded = false;

		protected override void BeforeRun()
		{
		


			Console.OutputEncoding = Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance.GetEncoding(437);
			Console.SetWindowSize(90, 30);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Running RadianceOS " + version);
			Console.ForegroundColor = ConsoleColor.White;

		

			fs = new Sys.FileSystem.CosmosVFS();
			Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);


			RAT.MinFreePages = 24;

			bool fileSytstemError = false;
			try
			{
				Directory.GetDirectories(@"0:\RadianceOS\System\");
			}
			catch
			{
				diskReady = false;
				WriteLineERROR("RadianceOS cannot access the disk. The drive was not formatted correctly!");
				fileSytstemError = true;
			}
			if(fileSytstemError)
			{
				DisplaySizeSelector.SelectMode();
			}
			else if(!File.Exists(@"0:\RadianceOS\Settings\DisplayMode.dat"))
			{
				DisplaySizeSelector.SelectMode();
			}
			else
			{
				try
				{
					int DisplayMode = int.Parse(File.ReadAllText(@"0:\RadianceOS\Settings\DisplayMode.dat"));
					DisplaySizeSelector.SaveMode(DisplayMode);
				}
				catch(Exception e)
				{
					Kernel.WriteLineERROR(e.Message);
					Thread.Sleep(3000);
					DisplaySizeSelector.SelectMode();
				}
			}


			if (!fileSytstemError)
			{
				try
				{
					Directory.GetDirectories(@"0:\Users");
				}
				catch
				{
					Cursor1 = new Bitmap(Files.cursor1);
					Security.StartGui();
					diskReady = false;
					WriteLineERROR("Users folder not found!");

					Repair = true;
					render = false;
					return;
				}
				if (diskReady)
				{
					var folder_list = Directory.GetDirectories(@"0:\Users\");
					loggedUser = folder_list[0];
				}
			}
			MaxRam = Cosmos.Core.GCImplementation.GetAvailableRAM();
			try
			{
				BootScreen.Start();
			}
			catch (Exception ex) 
			{
				WriteLineERROR(ex.Message);
				render = false;
			}
			finally{
				try
				{
                    LoadFiles();
                }
               catch(Exception e)
				{
					render = false;
					Explorer.CanvasMain.Disable();
					WriteLineERROR(e.Message);
				}
            }
		}


		public static void LoadFiles()
		{
			if (MaxRam < 32)
			{
				Explorer.CanvasMain.Disable();
				WriteLineERROR("Not enogh memory!");
				Thread.Sleep(100);
				WriteLineERROR("RadianceOS will shut down in 30 seconds!");
				Thread.Sleep(30000);
				Cosmos.System.Power.Shutdown();
			}
			Console.WriteLine("Loading ttf...");
			TTFManager.RegisterFont("UMR", Files.UbuntuMonoRegular);
			TTFManager.RegisterFont("UMB", Files.UbuntuMonoBold);

			TTFManager.RegisterFont("CR", Files.UbuntuMonoRegular);
			TTFManager.RegisterFont("CB", Files.UbuntuMonoBold);


			TTFManager.RegisterFont("STR", Files.STRegualr);

			Console.WriteLine("Loading files...");
			font16 = PCScreenFont.LoadFont(Files.Font16);
			WriteLineOK("Font16 Default");
			font18 = PCScreenFont.LoadFont(Files.Font18);
			WriteLineOK("Font18 Default");
			fontRuscii = PCScreenFont.LoadFont(Files.FontRuscii);
			WriteLineOK("Font16 Ruscii");
			fontLat = PCScreenFont.LoadFont(Files.FontLat);
			WriteLineOK("Font16 Lat");
			if (diskReady)
			{
				if (File.Exists(@"0:\Users\" + loggedUser + @"\Settings\Wallpaper.dat"))
				{
					try
					{
                        Explorer.Wallpaper = int.Parse(File.ReadAllText(@"0:\Users\" + loggedUser + @"\Settings\Wallpaper.dat"));
                    }
					catch
					{
						File.Delete(@"0:\Users\" + loggedUser + @"\Settings\Wallpaper.dat");
                        File.Create(@"0:\Users\" + loggedUser + @"\Settings\Wallpaper.dat");
                        File.WriteAllText(@"0:\Users\" + loggedUser + @"\Settings\Wallpaper.dat", "0");
						MessageBoxCreator.CreateMessageBox("Config Erorr", "Wallpaper config was corrupted!\nRadianceOS has restored default settings.", MessageBoxCreator.MessageBoxIcon.warning, 500, 175);
                    }
					switch (Explorer.Wallpaper)
					{
						case 0:
							Wallpaper1 = new Bitmap(Files.wallpaper1);
							break;
						case 1:
							Wallpaper1 = new Bitmap(Files.wallpaper2);
							break;
					}
				}
				else
				{
					Wallpaper1 = new Bitmap(Files.wallpaper1);
				}
				
			}
			else
			{
				Wallpaper1 = new Bitmap(Files.wallpaper2);
			}
			try
			{
                BootScreen.Render("LOADING SYSTEM FILES", "Wallpaper");
            }
			catch
			{
				render = false;
				Repair = false;
				return;

			}





			if(BootScreen.on)
			Explorer.CanvasMain.Display();

			if (MaxRam - Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576 < 16) //CHECK RAM AFTER LOADING WALLPAPER
			{
				Explorer.CanvasMain.Disable();
				WriteLineERROR("Not enogh memory!");
				Thread.Sleep(100);
				WriteLineERROR("RadianceOS will shut down in 30 seconds!");
				Thread.Sleep(30000);
				Cosmos.System.Power.Shutdown();
			}

			WriteLineOK("Wallpaper");

			BootScreen.Render("LOADING SYSTEM FILES", "Taskbar1");
			WriteLineOK("Taskbar1");
			DarkButton = new Bitmap(Files.DButton);
			BootScreen.Render("LOADING SYSTEM FILES", "StartDark");
			WriteLineOK("StartDark");
			lightButton = new Bitmap(Files.LButton);
			BootScreen.Render("LOADING SYSTEM FILES", "StartLight");
			WriteLineOK("StartLight");
			Error = new Bitmap(Files.warning);
			BootScreen.Render("LOADING SYSTEM FILES", "Warning Icon");
			WriteLineOK("Warning Icon");
			Stop = new Bitmap(Files.stop);
			BootScreen.Render("LOADING SYSTEM FILES", "Stop Icon");
			WriteLineOK("Stop Icon");
			Info = new Bitmap(Files.info);
			BootScreen.Render("LOADING SYSTEM FILES", "Info Icon");
			WriteLineOK("Info Icon");
			CriticalStop = new Bitmap(Files.criticalStop);
			BootScreen.Render("LOADING SYSTEM FILES", "Critical Stop Icon");
			WriteLineOK("Critical Stop Icon");
			cmd = new Bitmap(Files.cmd);
			BootScreen.Render("LOADING SYSTEM FILES", "Terminal Icon Small");
			WriteLineOK("Terminal Icon Small");
			notepad = new Bitmap(Files.notepad);
			BootScreen.Render("LOADING SYSTEM FILES", "Notepad Icon Small");
			WriteLineOK("Notepad Icon Small");
			settingIcon = new Bitmap(Files.setting);
			BootScreen.Render("LOADING SYSTEM FILES", "Settings Icon Small");
			WriteLineOK("Settings Icon Small");
			gamepadIcon = new Bitmap(Files.gamepad);
			BootScreen.Render("LOADING SYSTEM FILES", "Gamepad Icon Small");
			WriteLineOK("Gamepad Icon Small");
			RadiantWave = new Bitmap(Files.RadiantWave);
			BootScreen.Render("LOADING SYSTEM FILES", "RadiantWave Icon Small");
			WriteLineOK("RadiantWave Icon Small");
			sysinfoIcon = new Bitmap(Files.sysinfo);
			BootScreen.Render("LOADING SYSTEM FILES", "Sysinfo Icon Small");
			WriteLineOK("Sysinfo Icon Small");
			txtIcon = new Bitmap(Files.txt);
			fileExplorer = new Bitmap(Files.FileExplorer);
			BootScreen.Render("LOADING SYSTEM FILES", "File Explorer Icon Small");
			WriteLineOK("File Explorer Icon Small");
			BootScreen.Render("LOADING SYSTEM FILES", "Txt Icon");
			WriteLineOK("Txt Icon");
			rasIcon = new Bitmap(Files.ras);
			BootScreen.Render("LOADING SYSTEM FILES", "Ras Icon");
			WriteLineOK("Ras Icon");
			padlockIcon = new Bitmap(Files.padlock);
			BootScreen.Render("LOADING SYSTEM FILES", "Padlock Icon");
			WriteLineOK("Padlock Icon");
			unknownIcon = new Bitmap(Files.unknown);
			BootScreen.Render("LOADING SYSTEM FILES", "Unknown Icon");
			WriteLineOK("Unknown Icon");
			Xicon = new Bitmap(Files.Xicon);
			BootScreen.Render("LOADING SYSTEM FILES", "Close Icon");
			WriteLineOK("Close Icon");
			Cursor1 = new Bitmap(Files.cursor1);
			BootScreen.Render("LOADING SYSTEM FILES", "Cursor");
			WriteLineOK("Cursor1");
			
			BootScreen.Render("LOADING SYSTEM FILES", "File Icons 16px");
			text16 = new Bitmap(Files.text16);
			docuent16 = new Bitmap(Files.document16);
			folder16 = new Bitmap(Files.folder16);
			data16 = new Bitmap(Files.data16);
			sysData16 = new Bitmap(Files.sysData16);
			WriteLineOK("File Icons 16px");
			BootScreen.Render("Audio", "Starting audio");

			AllLoaded = true;
			try
			{
				var mixer = new AudioMixer();
				var memAudioStream = new MemoryAudioStream(new SampleFormat(AudioBitDepth.Bits16, 2, true), 48000, Files.startupAduio);
				var driver = Cosmos.HAL.Drivers.Audio.AC97.Initialize(bufferSize: 4096);
				mixer.Streams.Add(memAudioStream);

				var audioManager = new AudioManager()
				{
					Stream = mixer,
					Output = driver
				};
				audioManager.Enable();

			}
			catch
			{
				workingAudio = false;
				WriteLineERROR("Audio driver AC97 is not supported!");
				BootScreen.Render("Audio", "Audio driver AC97 is not supported!", Color.Red, false);
			}

			Heap.Collect();
			if (diskReady)
			{
				Console.WriteLine();
				Console.Write(DrawConsole.CenterText("Welcome"));
				BootScreen.Render("Welcome", "Connecting to network", Color.White, false);
			
				RadianceOS.System.Networking.NetworkManager.Connect();
				if (!RadianceOS.System.Networking.NetworkManager.Network)
				{
					BootScreen.Render("Connection Failed", "Network connection failed!", Color.White, false);
					Thread.Sleep(500);
				}
					if (BootScreen.on)
					ConsoleCommands.RunCommand("gui");
			}
			else
			{
				BootScreen.Render("Welcome", "Starting installer...", Color.White, false);
				Thread.Sleep(750);
				if (BootScreen.on)
				{
				
						ConsoleCommands.RunCommand("gui");
					
			
				}
					
			}

			Console.WriteLine();
			Console.WriteLine();


			Console.ForegroundColor = ConsoleColor.DarkCyan;
			DrawConsole.DrawLogo();
			Console.ForegroundColor = ConsoleColor.White;
			if (diskReady)
			{
				if (Kernel.loggedUser == null)
				{
					Security.Logged = true;
					return;
				}
				string myUser = @"0:\Users\" + Kernel.loggedUser + @"\";
				if (File.Exists(myUser + @"AccountInfo\Password.SysData"))
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("[LOGIN] ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("Please enter main account password!");
					Console.WriteLine();
				}
				else
				{
					Security.Logged = true;
				}

			}
			else
			{
				Security.Logged = true;
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



		protected override void Run()
		{
			if (render)
			{
			
				Explorer.Update();
			}
			else if (!Repair)
			{
				if (Security.Logged)
				{

					Console.Write(path + ">");
					var input = Console.ReadLine();
					ConsoleCommands.RunCommand(input);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("Password> ");
					Console.ForegroundColor = ConsoleColor.White;
					var input = Console.ReadLine();
					string myUser = @"0:\Users\" + Kernel.loggedUser + @"\";
					if (input == File.ReadAllText(myUser + @"AccountInfo\Password.SysData"))
					{
						Security.Logged = true;
						WriteLineOK("Logged as " + loggedUser);
					}
					else
					{
						WriteLineERROR("Incorrect password!");
					}
				}
			}
			else if (Repair)
			{
				Security.Render();
			}

			if(countFPS)
			{

				if (_deltaT != RTC.Second)
				{
					_fps = _frames;
					_frames = 0;
					_deltaT = RTC.Second;
					
				}
				_frames++;
			}

			if (collect >= 20)
			{
				Heap.Collect();
				collect = 0;
			}
			else
				collect++;

		}
		protected override void AfterRun()
		{
			Crash("Kernel has been stopped! Please restart RadianceOS.", 6);
		}
		public static void Crash(string Error, int id)
		{
			render = false;
			Repair = true;
			Explorer.CanvasMain.Clear(Color.FromArgb(132, 0, 0));
			Explorer.DrawMenu = false;
			Explorer.DrawTaskbar = false;

			Explorer.CanvasMain.Clear(Color.FromArgb(132, 0, 0));
			Explorer.CanvasMain.Display();
			Security.reason = Error + "\nID: " + id;
			Security.State = 1;
			Security.StartGui();
		}
		public static void Write(string s)
		{
			Console.Write(s);
		}

    }
}
