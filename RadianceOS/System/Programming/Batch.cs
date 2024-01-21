using Cosmos.Core;
using Cosmos.Core.Memory;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2;
using RadianceOS.System.Security.FileManagment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;


namespace RadianceOS.System.Programming
{
	public static class Batch
	{
		public static Color gray = Color.FromArgb(186, 186, 186), green = Color.FromArgb(42, 230, 0);
		public static void RunCommand(string com, int index)
		{
			try
			{
				string[] commands = com.Split(' ');
				commands[0] = commands[0].ToLower();
				if (commands[0] == "time")
				{
					TextColor empty = new TextColor
					{
						text = "The current time is: " + DateTime.Now,
					};

					Process.Processes[index].lines.Add(empty);
				}
				else if (commands[0] == "dir")
				{
					string location;
					if (commands.Length > 1)
					{
						if (commands.Contains(@"0:\"))
							location = commands[1];
						else
							location = @"0:\" + commands[1];
					}
					else
						location = Process.Processes[index].metaData;


					var folder_list = Directory.GetDirectories(location);

					TextColor folders = new TextColor
					{
						text = location + " - " + folder_list.Length + " Folder(s)",
						color = green
					};
					Process.Processes[index].lines.Add(folders);
					foreach (var file in folder_list)
					{
						TextColor folder = new TextColor
						{
							text = file,
							color = gray
						};
						Process.Processes[index].lines.Add(folder);
					}
					var files_list = Directory.GetFiles(Process.Processes[index].metaData);
					TextColor files = new TextColor
					{
						text = location + " - " + files_list.Length + " File(s)",
						color = green
					};
					Process.Processes[index].lines.Add(files);



					foreach (var file in files_list)
					{

						TextColor folder = new TextColor
						{
							text = file,
							color = gray
						};
						Process.Processes[index].lines.Add(folder);
					}

				}

				else if (commands[0] == "cd")
				{
					if (commands.Length > 1)
					{
						if (Directory.Exists(Process.Processes[index].metaData + commands[1]))
							Process.Processes[index].metaData += commands[1] + @"\";
						else
						{

							TextColor empty = new TextColor
							{
								text = "Directory " + commands[1] + " does not exist!",
								color = Color.Red
							};
							Process.Processes[index].lines.Add(empty);
						}
					}
					else
					{
						Process.Processes[index].metaData = @"0:\";
					}
				}
				else if (commands[0] == "cd..")
				{
					if (Process.Processes[index].metaData == @"0:\")
					{

						TextColor empty = new TextColor
						{
							text = "Could not exit disk 0!",
							color = Color.Red
						};
						Process.Processes[index].lines.Add(empty);
					}
					else
					{
						Process.Processes[index].metaData = Process.Processes[index].metaData.Remove(Process.Processes[index].metaData.Length - 1);
						int lastSlashIndex = Process.Processes[index].metaData.LastIndexOf(@"\");
						string result = Process.Processes[index].metaData.Substring(0, lastSlashIndex + 1);
						Process.Processes[index].metaData = result;
					}
				}
				else if (commands[0] == "size")
				{
					TextColor empty = new TextColor
					{
						text = Kernel.fs.GetTotalSize(Process.Processes[index].metaData).ToString() + " Bytes",
						color = Color.White
					};
					Process.Processes[index].lines.Add(empty);
					TextColor empty2 = new TextColor
					{
						text = "(" + Kernel.fs.GetTotalSize(Process.Processes[index].metaData) / 1048576 + " MB)",
						color = Color.White
					};
					Process.Processes[index].lines.Add(empty2);
				}
				else if (commands[0] == "echo")
				{
					if (commands.Length > 1)
					{
						string finale = "";
						for (int i = 1; i < commands.Length; i++)
						{
							finale += commands[i] + " ";
						}
						if (finale.Contains(">"))
						{
							if (!Kernel.diskReady)
							{
								TextColor error = new TextColor
								{
									text = "Read only file system! Please format your drive and create an account.",
									color = Color.Red
								};

								Process.Processes[index].lines.Add(error);
								return;
							}
							string[] parts = finale.Split('>');
							string fileName = parts[parts.Length - 1];
							fileName = fileName.Substring(0, fileName.Length - 1);
							var file_stream = File.Create(Process.Processes[index].metaData + fileName);
						
							file_stream.Close();
							string content = "";
							for (int i = 0; i < parts.Length - 1; i++)
							{
								content += parts[i];
							}
							File.WriteAllText(Process.Processes[index].metaData + fileName, content);
							TextColor empty = new TextColor
							{
								text = "File created in " + Process.Processes[index].metaData,
								color = green
							};
							Process.Processes[index].lines.Add(empty);

							if (Process.Processes[index].metaData.Contains(@"0:\Users\" + Kernel.loggedUser + @"\Desktop"))
								DrawDesktopApps.UpdateIcons();
						}
						else
						{
							TextColor empty = new TextColor
							{
								text = finale,
							};
							Process.Processes[index].lines.Add(empty);
						}

					}
					else
					{
						TextColor empty = new TextColor
						{
							text = "",
						};

						Process.Processes[index].lines.Add(empty);
					}

				}
				else if (commands[0] == "cat")
				{
					if (!Kernel.diskReady)
					{
						TextColor error = new TextColor
						{
							text = "Read only file system! Please format your drive and create an account.",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(error);
						return;
					}
					string location;

					if (commands[1].Contains(@"\"))
					{
						if (commands[1].Contains(@"0:\"))
							location = commands[1];
						else
							location = @"0:\" + commands[1];
					}
					else
						location = Process.Processes[index].metaData + commands[1];

					if (File.Exists(location))
					{
						if (location.Contains(".SysData") && !Kernel.Root)
						{

							TextColor empty = new TextColor
							{
								text = "Permission denied",
								color = Color.Red
							};

							Process.Processes[index].lines.Add(empty);
						}
						else
						{
							try
							{

								TextColor empty = new TextColor
								{
									text = File.ReadAllText(location),
									color = gray
								};
								Process.Processes[index].lines.Add(empty);
							}
							catch (Exception e)
							{


								TextColor empty = new TextColor
								{
									text = e.ToString(),
									color = Color.Red
								};

								Process.Processes[index].lines.Add(empty);

							}
						}

					}
					else
					{
						TextColor empty = new TextColor
						{
							text = "File does not exist!",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(empty);
					}

				}
				else if (commands[0] == "del")
				{
					if (!Kernel.diskReady)
					{
						TextColor error = new TextColor
						{
							text = "Read only file system! Please format your drive and create an account.",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(error);
						return;
					}
					string location;
					string fullLocation = "";
					if (commands.Length > 2)
					{
						for (int i = 1; i < commands.Length; i++)
						{
							if (i < commands.Length - 1)
								fullLocation += commands[i] + " ";
							else
								fullLocation += commands[i];
						}
					}
					else
					{
						fullLocation = commands[1];
					}

					if (fullLocation.Contains(@"\"))
					{
						if (fullLocation.Contains(@"0:\"))
							location = fullLocation;
						else
							location = @"0:\" + fullLocation;
					}
					else
						location = Process.Processes[index].metaData + fullLocation;

					if (File.Exists(location))
					{
						if (commands[1].Contains(".SysData") && !Kernel.Root)
						{

							TextColor empty = new TextColor
							{
								text = "Permission denied",
								color = Color.Red
							};

							Process.Processes[index].lines.Add(empty);
						}
						else
						{
							try
							{
								File.Delete(location);
								TextColor empty = new TextColor
								{
									text = "File " + location + " has been deleted",
									color = green
								};
								Process.Processes[index].lines.Add(empty);
								if (location.Contains(@"0:\Users\" + Kernel.loggedUser + @"\Desktop"))
									DrawDesktopApps.UpdateIcons();
							}
							catch (Exception e)
							{


								TextColor empty = new TextColor
								{
									text = e.ToString(),
									color = Color.Red
								};

								Process.Processes[index].lines.Add(empty);

							}
						}

					}

					else
					{
						TextColor empty = new TextColor
						{
							text = "File " + fullLocation + " does not exist!",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(empty);
					}
				}
				else if (commands[0] == "md")
				{
					if (!Kernel.diskReady)
					{
						TextColor error = new TextColor
						{
							text = "Read only file system! Please format your drive and create an account.",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(error);
						return;
					}
					string location;
					string fullLocation = "";
					if (commands.Length > 2)
					{
						for (int i = 1; i < commands.Length; i++)
						{
							if (i < commands.Length - 1)
								fullLocation += commands[i] + " ";
							else
								fullLocation += commands[i];
						}
					}
					else
					{
						fullLocation = commands[1];
					}

					if (fullLocation.Contains(@"\"))
					{
						if (fullLocation.Contains(@"0:\"))
							location = fullLocation;
						else
							location = @"0:\" + fullLocation;
					}
					else
						location = Process.Processes[index].metaData + fullLocation;

					if (!Directory.Exists(location))
					{

						try
						{
							Directory.CreateDirectory(location);

							TextColor empty = new TextColor
							{
								text = "Created directory " + location,
								color = green
							};
							Process.Processes[index].lines.Add(empty);
							if (location.Contains(@"0:\Users\" + Kernel.loggedUser + @"\Desktop"))
								DrawDesktopApps.UpdateIcons();
						}
						catch (Exception e)
						{


							TextColor empty = new TextColor
							{
								text = e.ToString(),
								color = Color.Red
							};

							Process.Processes[index].lines.Add(empty);

						}


					}
					else
					{
						TextColor empty = new TextColor
						{
							text = "Directory " + fullLocation + " already exist!",
							color = Color.Red
						};

						Process.Processes[index].lines.Add(empty);
					}
				}
				else if (commands[0] == "tasklist")
				{
					if (Kernel.render)
					{
						string space = new string(' ', 30 - "Explorer".Length);
						string space2 = new string(' ', 10 - 0.ToString().Length);
						TextColor exp = new TextColor
						{
							text = "Explorer" + space + "Process ID: " + 0 + space2,
							color = Color.Gray
						};

						Process.Processes[index].lines.Add(exp);
					}
					for (int i = 0; i < Process.Processes.Count; i++)
					{
						string processName = "";
						switch (Process.Processes[i].ID)
						{
							case 0:
								processName = "MessageBox";
								break;
							case 1:
								processName = "Terminal";
								break;
							case 2:
								processName = "Notepad";
								break;
							case 3:
								processName = "RaSharp";
								break;
							case 4:
								processName = "Login";
								break;
							case 5:
								processName = "Settings";
								break;
							case 6:
								processName = "Snake";
								break;
							case 7:
								processName = "SysPerformance";
								break;
							case 8:
								processName = "RadiantWave";
								break;
							case 9:
								processName = "Welcome";
								break;
							case 10:
								processName = "FileExplorer";
								break;
							case 100:
								processName = "Installer";
								break;
							case -1:
								processName = "Hidden";
								break;
						}
						string space = new string(' ', 30 - processName.Length);
						string space2 = new string(' ', 10 - i.ToString().Length);
						if (processName != "Hidden")
						{
							TextColor task = new TextColor
							{
								text = processName.ToString() + space + "Process ID: " + i + space2,
								color = gray
							};

							Process.Processes[index].lines.Add(task);
						}
						else
							continue;

					}
				}
				else if (commands[0] == "taskkill")
				{
					try
					{
						if (int.Parse(commands[1]) > 0)
						{
							string processName = "";
							switch (Process.Processes[int.Parse(commands[1])].ID)
							{
								case 0:
									processName = "MessageBox";
									break;
								case 1:
									processName = "Terminal";
									break;
								case 2:
									processName = "Notepad";
									break;
								case 3:
									processName = "RaSharp";
									break;
								case 5:
									processName = "Settings";
									break;
								case 6:
									processName = "Snake";
									break;
								case 7:
									processName = "SysPerformance";
									break;
								case 8:
									processName = "RadiantWave";
									break;
								case 9:
									processName = "Welcome";
									break;
								case 10:
									processName = "FileExplorer";
									break;
								case 100:
									processName = "Installer";
									break;
								case -1:
									processName = "Hidden";
									break;
							}
							TextColor empty = new TextColor
							{
								text = "",
								color = Color.White,
							};
							TextColor empty2 = new TextColor
							{
								text = "",
								color = Color.White,
							};
							int lineToAdd = Process.Processes[index].lines.Count - 1;
							string pathBefore = Process.Processes[index].metaData;
							Process.Processes[index].lines[lineToAdd].text = (pathBefore + ">" + Process.Processes[index].lines[lineToAdd].text);
							Process.Processes[index].lines.Add(empty);
							Process.Processes[index].lines.Add(empty2);
							InputSystem.CurrentString = "";
							Process.Processes.RemoveAt(int.Parse(commands[1]));

						}
						else if (int.Parse(commands[1]) == 0)
						{
							Explorer.CanvasMain.Disable();
							Kernel.render = false;
							Cosmos.System.Power.Reboot();
						}
					}
					catch (Exception e)
					{
						if (commands[1] == "Explorer")
						{
							Explorer.CanvasMain.Disable();
							Kernel.render = false;
							Cosmos.System.Power.Reboot();
							return;
						}
						for (int i = 0; i < Process.Processes.Count; i++)
						{
							string processName = "";
							switch (Process.Processes[i].ID)
							{
								case 0:
									processName = "MessageBox";
									break;
								case 1:
									processName = "Terminal";
									break;
								case 2:
									processName = "Notepad";
									break;
								case 3:
									processName = "RaSharp";
									break;
								case 5:
									processName = "Settings";
									break;
								case 6:
									processName = "Snake";
									break;
								case 7:
									processName = "SysPerformance";
									break;
								case 8:
									processName = "RadiantWave";
									break;
								case 9:
									processName = "Welcome";
									break;
								case 10:
									processName = "FileExplorer";
									break;
								case 100:
									processName = "Installer";
									break;
								case -1:
									processName = "Hidden";
									break;
							}
							if (processName == commands[1])
							{
								TextColor empty = new TextColor
								{
									text = "",
									color = Color.White,
								};
								TextColor empty2 = new TextColor
								{
									text = "",
									color = Color.White,
								};
								int lineToAdd = Process.Processes[index].lines.Count - 1;
								string pathBefore = Process.Processes[index].metaData;
								Process.Processes[index].lines[lineToAdd].text = (pathBefore + ">" + Process.Processes[index].lines[lineToAdd].text);
								Process.Processes[index].lines.Add(empty);
								Process.Processes[index].lines.Add(empty2);
								InputSystem.CurrentString = "";
								Process.Processes.RemoveAt(i);
								return;
							}


						}
					}

				}
				else if (commands[0] == "ram")
				{
					TextColor empty = new TextColor
					{
						text = "Ram usage: " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "/" + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB",
						color = gray
					};

					Process.Processes[index].lines.Add(empty);
				}
				else if (commands[0] == "refresh")
				{
					DrawDesktopApps.UpdateIcons();
					TextColor empty = new TextColor
					{
						text = "Desktop refreshed",
						color = green
					};

					Process.Processes[index].lines.Add(empty);
				}
				else if (commands[0] == "system" || commands[0] == "info" || commands[0] == "about" || commands[0] == "sysinfo")
				{
					GenerateText("RadianceOS " + Kernel.version, Color.White, index);
					GenerateText("Version: " + Kernel.subversion, Color.White, index);
					GenerateText("Created by Szymekk", Color.White, index);
					GenerateText("Szymekk.pl", Color.White, index);
					GenerateText("Disk space: " + Kernel.fs.Disks[0].Size / 1048576 + " MB", gray, index);
					TextColor ram = new TextColor
					{
						text = "Ram usage: " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "/" + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB",
						color = gray
					};
					Process.Processes[index].lines.Add(ram);

					TextColor pro = new TextColor
					{
						text = "Processor: " + CPU.GetCPUBrandString(),
						color = gray
					};


					Process.Processes[index].lines.Add(pro);

                    GenerateText("Display: " + Explorer.screenSizeX+"x"+Explorer.screenSizeY, gray, index);
                }
				else if (commands[0] == "start")
				{
					if(commands.Length < 2)
					{
						ReturnError(0, index);
						return;
					}
					if (commands[1] == "sysinfo" || commands[1] == "radver")
					{
						Processes MessageBox2 = new Processes
						{
							ID = 11,
							X = 200,
							Y = 100,
							SizeX = 500,
							SizeY = 305,
							moveAble = true
						};
						Process.Processes.Add(MessageBox2);
						Process.UpdateProcess(Process.Processes.Count - 1);
					}
					else
					{
                        string location;
                        string fullLocation = "";
						fullLocation = commands[1];
                        if (fullLocation.Contains(@"\"))
                        {
                            if (fullLocation.Contains(@"0:\"))
                                location = fullLocation;
                            else
                                location = @"0:\" + fullLocation;
                        }
                        else
                            location = Process.Processes[index].metaData + fullLocation;
                        if (File.Exists(location))
                        {
                            switch (Path.GetExtension(location))
                            {
                                case ".ras":
                                    RasExecuter.StartScript(location);
                                    break;
                                case ".txt":
                                    Notepad.OpenFile(location);
                                    break;
                                default:
                                    Notepad.OpenFile(location);
                                    break;
                            }
                        }
                        else
                        {
                            TextColor empty = new TextColor
                            {
                                text = "File" + fullLocation + " doesn't exists!",
                                color = Color.Red
                            };

                            Process.Processes[index].lines.Add(empty);
                        }
                    }
				}
				else if (commands[0] == "help")
				{
					GenerateText("=-=FILE SYSTEM=-=", Color.White, index);
					GenerateText("cat <file/path> - Reads the contents of a file.", gray, index);
					GenerateText("cd <directory> - Changes the current directory.", gray, index);
					GenerateText("del <file> - Deletes the file", gray, index);
					GenerateText("dir <directory> - Displays a list of files and subdirectories in a directory.", gray, index);
					GenerateText("echo> <file> - Creates and writes to a file.", gray, index);
					GenerateText("md <name> - Creates a new directory", gray, index);
					GenerateText("=-=OTHER=-=", Color.White, index);
					GenerateText("time - Displays the system time.", gray, index);
					GenerateText("tasklist - Displays all currently running tasks.", gray, index);
					GenerateText("taskkill - Kill or stop a running process or application.", gray, index);
					GenerateText("=-=RADIANCE OS=-=", Color.White, index);
					GenerateText("ram - Displays RAM usage.", gray, index);
					GenerateText("sysinfo - Displays information about RadianceOS and computer", gray, index);
				}
				else
				{
					TextColor empty = new TextColor
					{
						text = "'" + commands[0] + "' is not a valid Radiance Batch command.",
						color = Color.Red
					};

					Process.Processes[index].lines.Add(empty);
				}
			}
			catch(Exception ex)
			{
				Kernel.Crash("Batch: " + ex.Message, 5);
			}
			
		}

		public static void GenerateText(string text, Color color, int index)
		{
			TextColor First = new TextColor
			{
				text = text,
				color = color
			};
			Process.Processes[index].lines.Add(First);
		}

		public static void ReturnError(int errorID, int index)
		{
			switch(errorID)
			{
				case 0:
					{
						GenerateText("Invalid command parameter. (0)", Color.Red, index);
					}
					break;
				case 1:
					{
						GenerateText("Read only file system! Please format your drive and create an account. (1)", Color.Red, index);
					}
					break;
				case 2:
					{
						GenerateText("Permissions denied. (2)", Color.Red, index);
					}
					break;
			}
		}
	}
}
