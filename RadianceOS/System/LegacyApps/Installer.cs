/*

---OLD INSTALLER CODE!
---DO NOT DELETE/EDIT!



using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RadianceOS.System.Managment;
using Sys = Cosmos.System;
using System.Threading;
using System.IO;
using RadianceOS.System.Radiance;

namespace RadianceOS.System.Apps
{
    public static class Installer
	{
		public static int Progress = 0;
		public static Bitmap Window;
		public static Bitmap Button;
		public static Bitmap Button2;
		static Color col;
		static Color col2;
		public static bool cantClick;

		public static string progressText;

		public static bool canContinue;

		public static string AccountName;
		public static string AccountPassword;

		public static string ErrorMessage;

		public static bool readyToFormat;
		public static string errorToFormat;
		public static void Run()
		{
			InputSystem.AllowArrows = false;
			Window = new Bitmap(Files.BlurWindow);
			Button = new Bitmap(Files.BlurButton);
			Button2 = new Bitmap(Files.BlurButton2);
			col = Color.White;
			col2 = Color.FromArgb(255, 176, 171);
			Progress = 3;
			try
			{
				Directory.CreateDirectory(@"0:\RadianceOS");
			}
			catch
			{
				Progress = 0;
				
			}
		}
		public static void Render(int ProcessID)
		{
			switch (Progress)
			{
				case 0:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						Explorer.CanvasMain.DrawImage(Button2, 1920 / 4 + 322, (1080 / 4) + 367);
						StringsAcitons.DrawCenteredString("Welcome to RadianceOS Installer\n \nTo begin the installation process, please click 'Install'", 905, 480, 325, 15, Color.White, Kernel.font18);
						StringsAcitons.DrawCenteredString("Install\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
						StringsAcitons.DrawCenteredString("Skip\n", 905, 480, 270 + 383, 15, col2, Kernel.font18);
					}
					break;
				case 1:
					{
						InputSystem.SpecialCharracters = false;
						InputSystem.AllowUpDown = false;
						InputSystem.AllowArrows = false;
						InputSystem.allowDots = false;
						InputSystem.onlyNumbers = true;
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						if(Kernel.fs.Disks[0].Size > 0)
						{
							StringsAcitons.DrawCenteredString("Disk Formatting\n \nBefore we can install RadianceOS, we need to format your hard drive.\n\nTotal disk size: " + (Kernel.fs.Disks[0].Size), 905, 480, 325, 15, Color.White, Kernel.font18);
							canContinue = true;
							StringsAcitons.DrawCenteredString("Format and continue\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
						}
						else
						{
							StringsAcitons.DrawCenteredString("Disk Formatting\n \nBefore we can install RadianceOS, we need to format your hard drive.\n\nTotal disk size unknown!\nPlease enter custom disk size in MB\n> " + InputSystem.CurrentString, 905, 480, 325, 15, Color.White, Kernel.font18);
							if (int.Parse(InputSystem.CurrentString) < 32)
							{
								canContinue = false;
								StringsAcitons.DrawCenteredString("Min disk size is 32MB.", 905, 480, 270 + 438, 15, Color.Red, Kernel.font18);
							}
							else
							{
								canContinue = true;
								StringsAcitons.DrawCenteredString("Format and continue\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
							}
							InputSystem.Monitore(0, 0, 0);
						}


							
					}
					break;
				case 2:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						StringsAcitons.DrawCenteredString("Disk Formatting\n\nPlease wait for the format to complete...", 905, 480, 325, 15, Color.White, Kernel.font18);
					}
					break;
				case 3:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						StringsAcitons.DrawCenteredString("User Account Setup\nCreate your RadianceOS user account\n\nEnter main account name\n> " + InputSystem.CurrentString, 905, 480, 325, 15, Color.White, Kernel.font18);
						if(InputSystem.CurrentString.Length >= 2)
						{
							StringsAcitons.DrawCenteredString("Continue\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
							canContinue = true;
						}
						else if(InputSystem.CurrentString.Length < 2)
						{
							StringsAcitons.DrawCenteredString("Enter account name...\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
							canContinue = false;
						}
						InputSystem.SpecialCharracters = false;
						InputSystem.AllowUpDown = false;
						InputSystem.AllowArrows = false;
						InputSystem.allowDots = false;
						InputSystem.onlyNumbers = false;
						InputSystem.Monitore(0,0,0);
					}
					break;
				case 4:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						StringsAcitons.DrawCenteredString("User Account Setup\nCreate your RadianceOS user account\n\nEnter main account password\n> " + InputSystem.CurrentString, 905, 480, 325, 15, Color.White, Kernel.font18);
						if (InputSystem.CurrentString.Length >= 4)
						{
							StringsAcitons.DrawCenteredString("Continue\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
							canContinue = true;
						}
						else if (InputSystem.CurrentString.Length < 1)
						{
							StringsAcitons.DrawCenteredString("Enter account password...\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
							canContinue = false;
						}
						else
						{
							StringsAcitons.DrawCenteredString("Your password is too short", 905, 480, 270 + 438, 15, col, Kernel.font18);
							canContinue = false;
						}
						InputSystem.AllowUpDown = false;
						InputSystem.SpecialCharracters = true;
						InputSystem.AllowArrows = false;
						InputSystem.allowDots = false;
						InputSystem.Monitore(0, 0,0);
					}
					break;
				case 5:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						StringsAcitons.DrawCenteredString("Installing RadianceOS\n\nCreating system files...\n" + progressText, 905, 480, 325, 15, Color.White, Kernel.font18);
					}
					break;
				case 6:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						StringsAcitons.DrawCenteredString("Disk Formatting - Success\n \nTo continue, please restart your computer.", 905, 480, 325, 15, Color.White, Kernel.font18);
						StringsAcitons.DrawCenteredString("Restart\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
					}
					break;
				case 7:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						Explorer.CanvasMain.DrawImage(Button2, 1920 / 4 + 322, (1080 / 4) + 367);
						StringsAcitons.DrawCenteredString("Skip RadianceOS Installation\nIf you select this option, the system will not have access to the disk!\n\nYou will not be able to save files\nSome apps will not work\nRadianceOS will not be able to save any settings\n\n\r\nUse this function only if you plan to temporarily test the system.\n\nIf you restart RadianceOS, you will be able to install it again.", 905, 480, 325, 15, Color.White, Kernel.font18);
						StringsAcitons.DrawCenteredString("Return\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
						StringsAcitons.DrawCenteredString("Skip anyway\n", 905, 480, 270 + 383, 15, col2, Kernel.font18);
					}
					break;
				case 8:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						StringsAcitons.DrawCenteredString("Everything is ready!\n \nRadianceOS has been successfully installed on your computer", 905, 480, 325, 15, Color.White, Kernel.font18);
						StringsAcitons.DrawCenteredString("Close setup\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
					}
					break;
				case 9:
					{
						Explorer.CanvasMain.DrawImage(Window, 1920 / 4, 1080 / 4);
						Explorer.CanvasMain.DrawImage(Button, 1920 / 4 + 322, (1080 / 4) + 422);
						StringsAcitons.DrawCenteredString("RadianceOS has not been installed correctly \nFatal: " + ErrorMessage, 905, 480, 325, 15, Color.White, Kernel.font18);
						StringsAcitons.DrawCenteredString("Restart\n", 905, 480, 270 + 438, 15, col, Kernel.font18);
					}
					break;
			}



			int MX = (int)Cosmos.System.MouseManager.X;
			int MY = (int)Cosmos.System.MouseManager.Y;
			#region Buttons
			if (MX >= 1920 / 4 + 322 && MX <= 1920 / 4 + 578)
			{
				if (MY >= (1080 / 4) + 422 && MY <= (1080 / 4) + 472)
				{
					if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !cantClick)
					{
						col = Color.FromArgb(151, 45, 166);
					switch(Progress)
						{
							case 0:
								{
									Progress++;
									InputSystem.CurrentString = "";
								}
								break;
							case 1:
								{
									if (!canContinue)
										return;
									Progress++;
									Explorer.Update();

									try
									{
										if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
											Kernel.fs.Disks[0].DeletePartition(0);
										Kernel.fs.Disks[0].Clear();

										//	Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / 1048576);
										if (Kernel.fs.Disks[0].Size > 1)
										{
											Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / 1048576);
											//Kernel.fs.Disks[0].CreatePartition(512);
										}
										else
											Kernel.fs.Disks[0].CreatePartition(int.Parse(InputSystem.CurrentString));
										Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
									}
									catch
									(Exception ex)
									{
										Progress = 9;
										ErrorMessage = ex.Message;
										Kernel.DiskError = new Bitmap(Files.disk);
										string errorDet;
										if(ErrorMessage == "size")
										{
											errorDet = "Invalid partition size! (code: 0)\n";
										}
										else
											errorDet = "Unknown error";
										Processes MessageBox = new Processes
										{
											ID = 0,
											Name = "Fatal disk format error",
											Description = "RadianceOS was unable to format your drive properly.\n\nError: " + ErrorMessage + "\n" + errorDet,
											metaData = "diskError",
											X = 100,
											Y = 100,
											SizeX = 600,
											SizeY = 200,
											moveAble = true
										};
										Process.Processes.Add(MessageBox);
										Process.UpdateProcess(Process.Processes.Count - 1);
										return;

									}
								

									Thread.Sleep(1000);
									InputSystem.CurrentString = "";
					
									Progress = 6;
								
								}
								break;
							case 3:
								{
									if(canContinue)
									{
										AccountName = InputSystem.CurrentString;
										InputSystem.CurrentString = "";
										InputSystem.SpecialCharracters = true;
										Progress++;
									}
								}
								break;
							case 4:
								{
									if (canContinue)
									{
										AccountPassword = InputSystem.CurrentString;
										InputSystem.CurrentString = "";
										Progress++;


										progressText = @"0:\RadianceOS";
										Explorer.Update();
										try
										{
											Directory.CreateDirectory(@"0:\RadianceOS");
										}
										catch (Exception ex)
										{
											if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
												Kernel.fs.Disks[0].DeletePartition(0);
											Kernel.fs.Disks[0].Clear();
											if(Kernel.fs.Disks[0].Size < 4294967296)
											{
												Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / 1048576);
											}
										else
												Kernel.fs.Disks[0].CreatePartition(4096);
											Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
										}
										try
										{
											Directory.CreateDirectory(@"0:\RadianceOS");
										}
										catch 
										{
											Progress = 9;
											return;
										}
								
										Directory.CreateDirectory(@"0:\RadianceOS\System");
										Directory.CreateDirectory(@"0:\RadianceOS\Settings");
										
										progressText = @"0:\Users";
										Explorer.Update();
										Directory.CreateDirectory(@"0:\Users");
										
										progressText = @"0:\Users\" + AccountName + @"\AccountInfo";
										Explorer.Update();
										Directory.CreateDirectory(@"0:\Users\" + AccountName);
										string myUser = @"0:\Users\" + AccountName + @"\";
										Directory.CreateDirectory(myUser + "AccountInfo");
										File.Create(myUser + @"AccountInfo\Password.SysData");
										File.WriteAllText(myUser + @"AccountInfo\Password.SysData", AccountPassword);
										
										progressText = @"0:\Users\" + AccountName + @"\Desktop";
										Explorer.Update();


										Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Desktop");
										File.Create(@"0:\Users\" + AccountName + @"\Desktop\desktop.SysData");
										
										progressText = @"0:\Users\" + AccountName + @"\Documents";
										Explorer.Update();
										Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Documents");
										progressText = @"0:\Users\" + AccountName + @"\Desktop";
										Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Settings");
										Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Saved");
										progressText = @"0:\Users\" + AccountName + @"\Settings";
										File.Create(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat");
										File.WriteAllText(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat", "0");
										File.Create(@"0:\Users\" + AccountName + @"\Settings\Theme.dat");
										File.WriteAllText(@"0:\Users\" + AccountName + @"\Settings\Theme.dat", "0");

										Explorer.Update();
						
										Progress = 8 ;
									}
								}
								break;
					
							case 6:
								{
									Sys.Power.Reboot();
								}
								break;
							case 7:
								{
									Progress = 0;
								}
								break;
							case 9:
								{
									Sys.Power.Reboot();
								}
								break;
							case 8:
								{

									Process.Processes.RemoveAt(ProcessID);
										Kernel.diskReady = true;
									Processes welcome = new Processes
									{
										ID = 9,
										Name = "Welcome",
										X = 200,
										Y = 100,
										SizeX = 500,
										SizeY = 305,
										moveAble = true
									};
									Process.Processes.Add(welcome);
									Explorer.DrawTaskbar = true;
									Security.LogIn();

								}
								break;
						}
						cantClick = true;
					}
					else
						col = Color.FromArgb(242, 145, 255);

				}
				else
					col = Color.White;

				if(Progress == 0 || Progress == 7)
				{
					if(MY >= (1080 / 4) + 367 && MY <= (1080 / 4) + 417)
					{
						col2 = Color.FromArgb(255, 82, 71);
						if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !cantClick)
						{
							cantClick = true;
							if(Progress == 0)
							{
								Progress = 7;
							}
							else if(Progress == 7)
							{
								Process.Processes.Clear();
								Kernel.diskReady = false;
								Kernel.DiskError = new Bitmap(Files.disk);
								Processes start = new Processes
								{
									ID = -1,
								};
								Process.Processes.Add(start);
								Processes MessageBox = new Processes
								{
									ID = 0,
									Name = "Drive error",
									Description = "The drive has not been configured!\nRadianceOS cannot use VFS\n\nMost applications may not work.",
									metaData = "diskError",
									X = 100,
									Y = 100,
									SizeX = 400,
									SizeY = 200,
									moveAble = true
								};
								Process.Processes.Add(MessageBox);
									Process.UpdateProcess(Process.Processes.Count - 1);
								Processes MessageBox2 = new Processes
								{
									ID = 1,
									Name = "Disk info",
									Description = "CosmosVFS is working!",
									metaData = @"0:\",
									X = 100,
									Y = 100,
									SizeX = 800,
									SizeY = 500,
									moveAble = true
								};
								Process.Processes.Add(MessageBox2);
								Process.UpdateProcess(Process.Processes.Count - 1);
								Explorer.DrawTaskbar = true;
							}
						}
					}
					else
						col2 = Color.FromArgb(255, 176, 171);
				}

				
			}
			else
			{
				col2 = Color.FromArgb(255, 176, 171);
				col = Color.White;
			}
				

			if(cantClick)
			{
				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
					cantClick = false;
			}
			#endregion
		}


	}
}
*/