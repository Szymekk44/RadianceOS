using RadianceOS.System.Apps;
using RadianceOS.System.Radiance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cosmos.System.Network.Config;
using Cosmos.HAL;
using Process = RadianceOS.System.Apps.Process;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4;
using System.Net.Sockets;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System.Drawing;
using Cosmos.Core;
using webkerneltest.HTMLRENDERV2;
using RadianceOS.System.ConsoleMode;

namespace RadianceOS.System.Managment
{
	public static class ConsoleCommands
	{
		public static void RunCommand(string input)
		{
			string[] command = input.Split(' ');
			if (command[0] == "dir" || command[0] == "ls")
			{
				string location;
				if (command.Length > 1)
				{
					if (command.Contains(@"0:\"))
						location = command[1];
					else
						location = @"0:\" + command[1];
				}
				else
					location = Kernel.path;


				var folder_list = Directory.GetDirectories(location);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(Kernel.path + " - " + folder_list.Length + " Folder(s)");
				Console.ForegroundColor = ConsoleColor.Gray;

				foreach (var file in folder_list)
				{
					Console.WriteLine(file);
				}
				var files_list = Directory.GetFiles(Kernel.path);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(Kernel.path + " - " + files_list.Length + " File(s)");
				Console.ForegroundColor = ConsoleColor.Gray;

				foreach (var file in files_list)
				{
					Console.WriteLine(file);
				}
				Console.ForegroundColor = ConsoleColor.White;
			}
			if (command[0] == "disk")
			{
				if (command.Length <= 1)
				{
					ReportError(0);
					return;
				}

				if (command[1] == "size")
				{
					Console.WriteLine("Disk size: " + Kernel.fs.Disks[0].Partitions[0].Host.BlockCount * Kernel.fs.Disks[0].Partitions[0].Host.BlockSize / 1024uL / 1024uL);
				}
				else
					ReportError(0);
			}
			else if (command[0] == "cd")
			{
				if (command.Length > 1)
				{
					if (Directory.Exists(Kernel.path + command[1]))
						Kernel.path += command[1] + @"\";
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Kernel.WriteLineERROR("Directory does not exist!");
						Console.ForegroundColor = ConsoleColor.White;
					}
				}
				else
				{
					Kernel.path = @"0:\";
				}
			}
			else if (command[0] == "cd..")
			{
				if (Kernel.path == @"0:\")
				{
					Kernel.WriteLineERROR("Could not exit disk 0!");
				}
				else
				{
					Kernel.path = Kernel.path.Remove(Kernel.path.Length - 1);
					int lastSlashIndex = Kernel.path.LastIndexOf(@"\");
					string result = Kernel.path.Substring(0, lastSlashIndex + 1);
					Kernel.path = result;
				}
			}

			else if (command[0] == "cat")
			{

				if (command.Length <= 1)
				{
					ReportError(0);
					return;
				}
				if (!Kernel.diskReady)
				{
					ReportError(1);
					return;
				}
				string location;

				if (command[1].Contains(@"\"))
				{
					if (command[1].Contains(@"0:\"))
						location = command[1];
					else
						location = @"0:\" + command[1];
				}
				else
					location = Kernel.path + command[1];

				if (File.Exists(location))
				{
					if (location.Contains(".SysData"))
					{

						ReportError(2);
					}
					else
					{
						try
						{

							Console.ForegroundColor = ConsoleColor.Gray;
							Console.WriteLine(File.ReadAllText(location));
							Console.ForegroundColor = ConsoleColor.White;
						}
						catch (Exception e)
						{


							Kernel.WriteLineERROR(e.Message);

						}
					}

				}
				else
				{
					Kernel.WriteLineERROR("File does not exist!");
				}

			}

			else if (command[0] == "del")
			{
				if (!Kernel.diskReady)
				{
					ReportError(1);
					return;
				}
				string location;
				string fullLocation = "";
				if (command.Length > 2)
				{
					for (int i = 1; i < command.Length; i++)
					{
						if (i < command.Length - 1)
							fullLocation += command[i] + " ";
						else
							fullLocation += command[i];
					}
				}
				else
				{
					fullLocation = command[1];
				}

				if (fullLocation.Contains(@"\"))
				{
					if (fullLocation.Contains(@"0:\"))
						location = fullLocation;
					else
						location = @"0:\" + fullLocation;
				}
				else
					location = Kernel.path;

				if (File.Exists(location))
				{
					if (command[1].Contains(".SysData"))
					{
						ReportError(2);
					}
					else
					{
						try
						{
							File.Delete(location);
							Kernel.WriteLineOK("File " + location + " has been deleted");
						}
						catch (Exception e)
						{


							Kernel.WriteLineERROR(e.ToString());
						}
					}

				}

				else
				{
					Kernel.WriteLineERROR("File " + fullLocation + " does not exist!");
				}
			}


			else if (command[0] == "md")
			{
				if (!Kernel.diskReady)
				{
					ReportError(1);
					return;
				}
				string location;
				string fullLocation = "";
				if (command.Length > 2)
				{
					for (int i = 1; i < command.Length; i++)
					{
						if (i < command.Length - 1)
							fullLocation += command[i] + " ";
						else
							fullLocation += command[i];
					}
				}
				else
				{
					fullLocation = Kernel.path + command[1];
				}

				if (fullLocation.Contains(@"\"))
				{
					if (fullLocation.Contains(@"0:\"))
						location = fullLocation;
					else
						location = @"0:\" + fullLocation;
				}
				else
					location = Kernel.path + fullLocation;

				if (!Directory.Exists(location))
				{

					try
					{
						Directory.CreateDirectory(location);

						Kernel.WriteLineOK("Created directory " + location);
					}
					catch (Exception e)
					{
						Kernel.WriteLineERROR(e.ToString());
					}


				}
				else
				{
					Kernel.WriteLineERROR("Directory" + fullLocation + " already exist!");
				}
			}
			else if (command[0] == "echo")
			{
				if (command.Length > 1)
				{
					string finale = "";
					for (int i = 1; i < command.Length; i++)
					{
						finale += command[i] + " ";
					}
					if (finale.Contains(">"))
					{
						if (!Kernel.diskReady)
						{
							ReportError(1);
						}
						string[] parts = finale.Split('>');
						string fileName = parts[parts.Length - 1];
						fileName = fileName.Substring(0, fileName.Length - 1);
						var file_stream = File.Create(Kernel.path + fileName);
						file_stream.Close();
						string content = "";
						for (int i = 0; i < parts.Length - 1; i++)
						{
							content += parts[i];
						}
						File.WriteAllText(Kernel.path + fileName, content);
						Kernel.WriteLineOK("File created in " + Kernel.path);


					}
					else
					{
						Console.WriteLine(finale);
					}

				}
				else
				{
					Console.WriteLine();
				}

			}

			else if (command[0] == "gui")
			{
				Explorer.Start();


				if (!Kernel.diskReady)
				{
					InputSystem.CurrentString = "";
					Processes start = new Processes
					{
						ID = -1,
					};
					Process.Processes.Add(start);
					int status = 8;

					try //Checks if already formatted
					{
						Directory.CreateDirectory(@"0:\Test");
					}
					catch (Exception e)
					{
						status = 0;
					}


					Processes InstallerWin = new Processes
					{
						ID = 100,
						X = (int)(Explorer.screenSizeX - 800) / 2,
						Y = (int)(Explorer.screenSizeY - 500) / 2,
						SizeX = 800,
						SizeY = 500,
						Name = "RadianceOSIinstaller",
						Description = "Installer Window",
						tempInt = status,
						metaData = "error",
						closeAble = false,
						sizeAble = false,
						moveAble = true
					};
					Process.Processes.Add(InstallerWin);
				}
				else
				{
					Processes start = new Processes
					{
						ID = -1,
					};
					Process.Processes.Add(start);
					Processes MessageBox = new Processes
					{
						ID = 0,
						Name = "Disk info",
						Description = "CosmosVFS is working!",
						metaData = "info",
						X = 100,
						Y = 100,
						SizeX = 300,
						SizeY = 175,
						moveAble = true
					};
					//		Process.Processes.Add(MessageBox);
					//	Process.UpdateProcess(Process.Processes.Count - 1);
					if (!Security.Logged)
					{
						Processes MessageBox2 = new Processes
						{
							ID = 4,
							Name = "Log in",
							X = (int)Explorer.screenSizeX / 4,
							Y = (int)Explorer.screenSizeY / 4,
							SizeX = 440,
							SizeY = 225,
							CurrChar = 0,
							tempInt = 0,
							selected = true,
							moveAble = true,
							closeAble = false,
							hideAble = false
						};
						Process.Processes.Add(MessageBox2);
					}
					else
					{
						Explorer.drawIcons = true;
						DrawDesktopApps.UpdateIcons();
						Explorer.DrawTaskbar = true;
					}


				}


				//	Explorer.canvas1.Display();
				Kernel.render = true;
			}
			else if (command[0] == "guir")
			{
				Explorer.Start();


				int status = 8;

				InputSystem.CurrentString = "";

				try //Checks if already formatted
				{
					File.Create(@"0:\test.txt");
				}
				catch (Exception e)
				{
					status = 0;
				}


				Processes InstallerWin = new Processes
				{
					ID = 100,
					X = (int)(Explorer.screenSizeX - 800) / 2,
					Y = (int)(Explorer.screenSizeY - 500) / 2,
					SizeX = 800,
					SizeY = 500,
					Name = "RadianceOSIinstaller",
					Description = "Installer Window",
					tempInt = status,
					metaData = "error",
					closeAble = false,
					sizeAble = false,
					moveAble = true
				};
				Process.Processes.Add(InstallerWin);



				Kernel.render = true;
			}
			else if (command[0] == "guid")
			{
				Explorer.Start();
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
					sizeAble = false,

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
				Kernel.render = true;
			}
			else if (command[0] == "repair")
			{
				Security.StartGui();
				Kernel.render = false;
				Kernel.Repair = true;
			}
			else if (command[0] == "format")
			{
				if (!Kernel.diskReady)
				{
					Kernel.WriteLineWARN("Starting...");
					if (Kernel.fs.Disks[0].Partitions.Count > 0) //FORMAT
						Kernel.fs.Disks[0].DeletePartition(0);
					Kernel.fs.Disks[0].Clear();

					//	Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / 1048576);
					if (Kernel.fs.Disks[0].Size > 1)
					{
						Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / 1048576);
						Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
						//Kernel.fs.Disks[0].CreatePartition(512);
					}
					else
					{
						Kernel.WriteLineERROR("Unknown disk size!");
						Kernel.WriteLineWARN("Enter disk size in MB> ");
						int input2 = int.Parse(Console.ReadLine());
						try
						{
							Kernel.fs.Disks[0].CreatePartition(input2);
							Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
						}
						catch (Exception e)
						{
							Kernel.WriteLineERROR(e.Message);
						}
					}





				}
				else
				{
					Kernel.WriteLineWARN("Disk already formatted!");
					Kernel.WriteLineERROR("Cannot format your disk! Please login as Admin");
				}
			}
			else if (command[0] == "install")
			{
				if (!Kernel.diskReady)
				{
					Kernel.WriteLineWARN("Starting...");


					Explorer.Update();

					try
					{
						Directory.CreateDirectory(@"0:\RadianceOS");
					}
					catch (Exception e)
					{
						Kernel.WriteLineERROR("Installation failed! " + e.Message);
						return;
					}

					Kernel.WriteLineWARN("Enter account name");
					string AccountName = "User";
					AccountName = Console.ReadLine();

					Directory.CreateDirectory(@"0:\RadianceOS\System");
					Directory.CreateDirectory(@"0:\RadianceOS\Settings");

					Kernel.WriteLineOK(@"0:\Users\");

					Directory.CreateDirectory(@"0:\Users");
					Kernel.WriteLineOK(@"0:\Users\" + AccountName + @"\AccountInfo");


					Directory.CreateDirectory(@"0:\Users\" + AccountName);
					string myUser = @"0:\Users\" + AccountName + @"\";
					Directory.CreateDirectory(myUser + "AccountInfo");
					Kernel.WriteLineWARN("Enter account password");
					string AccountPassword = "";
					AccountPassword = Console.ReadLine();
					File.Create(myUser + @"AccountInfo\Password.SysData");
					File.WriteAllText(myUser + @"AccountInfo\Password.SysData", AccountPassword);


					Kernel.WriteLineOK(@"0:\Users\" + AccountName + @"\Desktop");


					Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Desktop");
					File.Create(@"0:\Users\" + AccountName + @"\Desktop\desktop.SysData");


					Kernel.WriteLineOK(@"0:\Users\" + AccountName + @"\Documents");

					Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Documents");

					Kernel.WriteLineOK(@"0:\Users\" + AccountName + @"\Desktop");
					Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Settings");
					Directory.CreateDirectory(@"0:\Users\" + AccountName + @"\Saved");

					Kernel.WriteLineOK(@"0:\Users\" + AccountName + @"\Settings");
					File.Create(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat");
					File.WriteAllText(@"0:\Users\" + AccountName + @"\Settings\Wallpaper.dat", "0");

					Kernel.WriteLineOK("Done!");

				}
				else
				{
					Kernel.WriteLineWARN("System is already installed!");
				}
			}
			else if (command[0] == "network")
			{
				Console.WriteLine("Connecting to network!");
				NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0"); //get network device by name
				IPConfig.Enable(nic, new Address(192, 168, 1, 69), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254)); //enable IPv4 configuration
																																  //Kernel.WriteLineOK("Found network device! Current IP: " + NetworkConfiguration.CurrentAddress);
				using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
				{
					/** Send a DHCP Discover packet **/
					//This will automatically set the IP config after DHCP response
					xClient.SendDiscoverPacket();

				}
				Kernel.WriteLineOK("New ip: " + NetworkConfiguration.CurrentAddress);



				try
				{
					using (TcpClient client = new TcpClient())
					{
						var dnsClient = new DnsClient();

						// DNS
						dnsClient.Connect(DNSConfig.DNSNameservers[0]);
						dnsClient.SendAsk("szymekk.pl");

						// Address from ip
						Address address = dnsClient.Receive();
						dnsClient.Close();
						string serverIp = address.ToString();
						int serverPort = 80;

						client.Connect(serverIp, serverPort);
						NetworkStream stream = client.GetStream();
						string httpget = "GET /RadianceOS.html HTTP/1.1\r\n" +
									 "User-Agent: RadianceOS\r\n" +
									 "Accept: */*\r\n" +
									 "Accept-Encoding: identity\r\n" +
									 "Host: szymekk.pl\r\n" +
									 "Connection: Keep-Alive\r\n\r\n";
						string messageToSend = httpget;
						byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
						stream.Write(dataToSend, 0, dataToSend.Length);

						/** Receive data **/
						byte[] receivedData = new byte[client.ReceiveBufferSize];
						int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
						string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);


						string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

						if (responseParts.Length == 2)
						{
							string headers = responseParts[0];
							string content = responseParts[1];
							Console.WriteLine(content);
							Kernel.htmlcode = content;
						}

						/** Close data stream **/
						stream.Close();
					}
				}
				catch (Exception e)
				{
					Kernel.WriteLineERROR(e.Message);
				}







			}
			else if (command[0] == "web")
			{
				HtmlRender2.AddResource(@"skk.png", Kernel.skk);
				HtmlRender2.AddResource(@"ok.png", Kernel.ok);
				HtmlRender2.AddResource(@"https://i.creativecommons.org/l/by-nd/4.0/80x15.png", Kernel.cc);

				Explorer.CanvasMain = FullScreenCanvas.GetFullScreenCanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
				try
				{


					Explorer.CanvasMain.Clear(Color.Blue);
					Explorer.CanvasMain.Display();
					Explorer.CanvasMain.RenderHTML(Kernel.htmlcode);
					Explorer.CanvasMain.Display();
				}
				catch (Exception ex)
				{

					Explorer.CanvasMain.Disable();
					Console.WriteLine(ex.Message);
				}
			}
			else if (command[0] == "drivetest")
			{
				if (Kernel.diskReady)
				{
					if (command.Length > 1)
					{
						string aString = new string('a', int.Parse(command[1]));
						Kernel.WriteLineOK("Generated data! Saving...");
						DateTime dt = DateTime.Now;
						File.Create(@"0:\test.txt");
						File.WriteAllText(@"0:\test.txt", aString);
						Kernel.WriteLineOK("Saved " + int.Parse(command[1]) * 2 + "bytes. Done in " + (DateTime.Now - dt).TotalSeconds + "s");
					}
					else
					{
						DateTime dt = DateTime.Now;
						File.Create(@"0:\test.txt");
						File.WriteAllText(@"0:\test.txt", new string('a', 1000));
						Kernel.WriteLineOK("Saved 2000 bytes. Done in " + (DateTime.Now - dt).TotalSeconds + "s");
					}
				}
				else
				{
					ReportError(1);
				}

			}
			else if (command[0] == "help")
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
				Console.WriteLine("║               RadianceOS Console Command List              ║");
				Console.WriteLine("║                                                            ║");
				Console.WriteLine("║                        »FILE SYSTEM«                       ║");
				Console.WriteLine("║ ∙cat <file/path> -Reads the contents of a file.            ║");
				Console.WriteLine("║ ∙cd <directory> - Changes the current directory.           ║");
				Console.WriteLine("║ ∙del <file> - Deletes the file                             ║");
				Console.WriteLine("║ ∙dir - Lists the contents of the current directory         ║");
				Console.WriteLine("║ ∙echo> <file> - Creates and writes to a file.              ║");
				Console.WriteLine("║ ∙md <name> - Creates a new directory                       ║");
				Console.WriteLine("║                                                            ║");
				Console.WriteLine("║                         »RadianceOS«                       ║");
				Console.WriteLine("║ ∙drivetest <int> - Tests disk speed                        ║");
				Console.WriteLine("║ ∙display - Change GUI display settings                     ║");
				Console.WriteLine("║ ∙gui - Starts the explorer process                         ║");
				Console.WriteLine("║  ├∙guir - Starts GUI with RadianceOS installer process     ║");
				Console.WriteLine("║  └∙guid - Starts GUI without file system                   ║");
				Console.WriteLine("║ ∙info - Shows information about RadianceOS                 ║");
				Console.WriteLine("║ ∙repair - Runs RadianceOS repair (GUI mode)                ║");
				Console.WriteLine("║ ∙format - formats the disk                                 ║");
				Console.WriteLine("║ ∙restart - Restarts RadianceOS                             ║");
				Console.WriteLine("║ ∙shutdown - Shutdowns RadianceOS                           ║");
				Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else if (command[0] == "restart")
			{
				Cosmos.System.Power.Reboot();
			}
			else if (command[0] == "shutdown")
			{
				Cosmos.System.Power.Shutdown();
			}
			else if (command[0] == "info")
			{
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				DrawConsole.DrawLogo();
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(DrawConsole.CenterText("RadianceOS " + Kernel.version));
				Console.Write(DrawConsole.CenterText("Version: " + Kernel.subversion));
				Console.Write(DrawConsole.CenterText("RaSharp Version: " + Kernel.RasVersion));
				Console.Write(DrawConsole.CenterText("Created by Szymekk"));
				Console.Write(DrawConsole.CenterText("Szymekk.pl"));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(DrawConsole.CenterText("Disk space: " + Kernel.fs.Disks[0].Size / 1048576 + " MB"));
				Console.Write(DrawConsole.CenterText("Available RAM: " + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB"));
				Console.Write(DrawConsole.CenterText("Processor: " + CPU.GetCPUBrandString()));
				Console.ForegroundColor = ConsoleColor.White;
			}
			else if (command[0] == "display")
			{
				DisplaySizeSelector.Finished = false;
				DisplaySizeSelector.SelectMode();
			}
			else if (command[0] == "ras")
			{
				Console.WriteLine("Executing Ra# code...");
				List<string> code = new List<string>
		{
			"int intVar = 42",
			"bool boolVar = true",
			"string stringVar = \"Hello, World!\"",
			"Console.WriteLine(intVar);",
			"Console.WriteLine(boolVar);",
			"Console.WriteLine(stringVar);",
			"Console.WriteLine(123);", // Przykład użycia liczby bez przypisywania do zmiennej
            "Console.WriteLine(true);", // Przykład użycia boola bez przypisywania do zmiennej
        };


				//	RasInterpreter.Execute(code);
			}
		}

		public static void ReportError(int id)
		{
			switch (id)
			{
				case 0:
					Kernel.WriteLineERROR("Invalid command parameter. (0)");
					break;
				case 1:
					Kernel.WriteLineERROR("Read only file system! Please format your drive and create an account. (1)");
					break;
				case 2:
					Kernel.WriteLineERROR("Permissions denied. (2)");
					break;
			}
		}
	}
}
