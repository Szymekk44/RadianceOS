using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cosmos.System.Network.Config;
using Cosmos.HAL;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4;
using System.Net.Sockets;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System.Drawing;
using Cosmos.Core;
using RadianceOSInstaller.System.ConsoleMode;

namespace RadianceOSInstaller.System.Managment
{
	public static class ConsoleCommands
	{
		public static void RunCommand(string input)
		{
			string[] command = input.Split(' ');
			if (Kernel.logged)
			{
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
						if (location.Contains(".SysData") && !Kernel.Root)
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
						if (command[1].Contains(".SysData") && !Kernel.Root)
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


				else if (command[0] == "format")
				{
					if (!Kernel.diskReady || Kernel.Root)
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


			}
			else if(command[0] == "dir" || command[0] == "del" || command[0] == "md" || command[0] == "cd" || command[0] == "cd.." || command[0] == "cat" || command[0] == "echo")
				Kernel.WriteLineERROR("Log in to use this command!");
				 if (command[0] == "drivetest")
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

			if (command[0] == "help")
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
				Console.WriteLine("║         RadianceOS Installer - Console Command List        ║");
				Console.WriteLine("║                                                            ║");
				Console.WriteLine("║                    »FILE SYSTEM (ROOT)«                    ║");
				Console.WriteLine("║ ∙login <password to main account>                          ║");
				Console.WriteLine("║ WARNING: These commands are only available after logging in║");
				Console.WriteLine("║ ∙cat <file/path> -Reads the contents of a file.            ║");
				Console.WriteLine("║ ∙cd <directory> - Changes the current directory.           ║");
				Console.WriteLine("║ ∙del <file> - Deletes the file                             ║");
				Console.WriteLine("║ ∙dir - Lists the contents of the current directory         ║");
				Console.WriteLine("║ ∙echo> <file> - Creates and writes to a file.              ║");
				Console.WriteLine("║ ∙md <name> - Creates a new directory                       ║");
				Console.WriteLine("║                                                            ║");
				Console.WriteLine("║                         »RadianceOS«                       ║");
				Console.WriteLine("║ ∙drivetest <int> - Tests disk speed                        ║");
				Console.WriteLine("║ ∙installG - Start GUI installer                            ║");
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
			else if (command[0] == "login")
			{
				if (Directory.Exists(@"0:\Users"))
				{

					var folder_list = Directory.GetDirectories(@"0:\Users\");
					if(folder_list.Length > 0)
					{
						string loggedUser = folder_list[0];
						string myUser = @"0:\Users\" + loggedUser + @"\";
						if (File.Exists(myUser + @"AccountInfo\Password.SysData"))
						{
							if (input == "login " + File.ReadAllText(myUser + @"AccountInfo\Password.SysData")) //imtolazy
							{
								Kernel.logged = true;
								Kernel.WriteLineOK("Logged as " + loggedUser);
							}
							else
							{
								Kernel.WriteLineERROR("Incorrect password!");
							}
						}
						else
						{
							Kernel.logged = true;
							Kernel.WriteLineOK("No user accounts!");
						}
					}
				
					else
					{
						Kernel.logged = true;
						Kernel.WriteLineOK("No user accounts!");
					}

				}
				else
				{
					Kernel.logged = true;
					Kernel.WriteLineOK("No user accounts!");
				}
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
				Console.Write(DrawConsole.CenterText("RadianceOS Installer " + Kernel.version));
				Console.Write(DrawConsole.CenterText("RadianceOS " + Kernel.RadianceOSver));
				Console.Write(DrawConsole.CenterText("Version: " + Kernel.RadianceOSsubVer));
				Console.Write(DrawConsole.CenterText("RaSharp Version: " + Kernel.RasVersion));
				Console.Write(DrawConsole.CenterText("Created by Szymekk"));
				Console.Write(DrawConsole.CenterText("Szymekk.pl"));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(DrawConsole.CenterText("Disk space: " + Kernel.fs.Disks[0].Size / 1048576 + " MB"));
				Console.Write(DrawConsole.CenterText("Available RAM: " + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB"));
				Console.Write(DrawConsole.CenterText("Processor: " + CPU.GetCPUBrandString()));
				Console.ForegroundColor = ConsoleColor.White;
			}
			else if (command[0] == "installG")
			{
				DisplaySizeSelector.Finished = false;
				DisplaySizeSelector.Last = -1;
				
				DisplaySizeSelector.SelectMode();
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
