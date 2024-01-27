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

					Apps.Process.Processes[index].lines.Add(empty);
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
						location = Apps.Process.Processes[index].metaData;


					var folder_list = Directory.GetDirectories(location);

					TextColor folders = new TextColor
					{
						text = location + " - " + folder_list.Length + " Folder(s)",
						color = green
					};
					Apps.Process.Processes[index].lines.Add(folders);
					foreach (var file in folder_list)
					{
						TextColor folder = new TextColor
						{
							text = file,
							color = Apps.Process.Processes[index].color2
						};
						Apps.Process.Processes[index].lines.Add(folder);
					}
					var files_list = Directory.GetFiles(Apps.Process.Processes[index].metaData);
					TextColor files = new TextColor
					{
						text = location + " - " + files_list.Length + " File(s)",
						color = green
					};
					Apps.Process.Processes[index].lines.Add(files);



					foreach (var file in files_list)
					{

						TextColor folder = new TextColor
						{
							text = file,
							color = Apps.Process.Processes[index].color2
						};
						Apps.Process.Processes[index].lines.Add(folder);
					}

				}

				else if (commands[0] == "cd")
				{
					if (commands.Length > 1)
					{
						if (Directory.Exists(Apps.Process.Processes[index].metaData + commands[1]))
							Apps.Process.Processes[index].metaData += commands[1] + @"\";
						else
						{

							TextColor empty = new TextColor
							{
								text = "Directory " + commands[1] + " does not exist!",
								color = Color.Red
							};
							Apps.Process.Processes[index].lines.Add(empty);
						}
					}
					else
					{
						Apps.Process.Processes[index].metaData = @"0:\";
					}
				}
				else if (commands[0] == "cd..")
				{
					if (Apps.Process.Processes[index].metaData == @"0:\")
					{

						TextColor empty = new TextColor
						{
							text = "Could not exit disk 0!",
							color = Color.Red
						};
						Apps.Process.Processes[index].lines.Add(empty);
					}
					else
					{
						Apps.Process.Processes[index].metaData = Apps.Process.Processes[index].metaData.Remove(Apps.Process.Processes[index].metaData.Length - 1);
						int lastSlashIndex = Apps.Process.Processes[index].metaData.LastIndexOf(@"\");
						string result = Apps.Process.Processes[index].metaData.Substring(0, lastSlashIndex + 1);
						Apps.Process.Processes[index].metaData = result;
					}
				}
				else if (commands[0] == "size")
				{
					TextColor empty = new TextColor
					{
						text = Kernel.fs.GetTotalSize(Apps.Process.Processes[index].metaData).ToString() + " Bytes",
						color = Apps.Process.Processes[index].color1
					};
					Apps.Process.Processes[index].lines.Add(empty);
					TextColor empty2 = new TextColor
					{
						text = "(" + Kernel.fs.GetTotalSize(Apps.Process.Processes[index].metaData) / 1048576 + " MB)",
						color = Apps.Process.Processes[index].color1
					};
					Apps.Process.Processes[index].lines.Add(empty2);
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

								Apps.Process.Processes[index].lines.Add(error);
								return;
							}
							string[] parts = finale.Split('>');
							string fileName = parts[parts.Length - 1];
							fileName = fileName.Substring(0, fileName.Length - 1);
							var file_stream = File.Create(Apps.Process.Processes[index].metaData + fileName);
						
							file_stream.Close();
							string content = "";
							for (int i = 0; i < parts.Length - 1; i++)
							{
								content += parts[i];
							}
							File.WriteAllText(Apps.Process.Processes[index].metaData + fileName, content);
							TextColor empty = new TextColor
							{
								text = "File created in " + Apps.Process.Processes[index].metaData,
								color = green
							};
							Apps.Process.Processes[index].lines.Add(empty);

							if (Apps.Process.Processes[index].metaData.Contains(@"0:\Users\" + Kernel.loggedUser + @"\Desktop"))
								DrawDesktopApps.UpdateIcons();
						}
						else
						{
							TextColor empty = new TextColor
							{
								text = finale,
							};
							Apps.Process.Processes[index].lines.Add(empty);
						}

					}
					else
					{
						TextColor empty = new TextColor
						{
							text = "",
						};

						Apps.Process.Processes[index].lines.Add(empty);
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

						Apps.Process.Processes[index].lines.Add(error);
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
						location = Apps.Process.Processes[index].metaData + commands[1];

					if (File.Exists(location))
					{
						if (location.Contains(".SysData") && !Kernel.Root)
						{

							TextColor empty = new TextColor
							{
								text = "Permission denied",
								color = Color.Red
							};

							Apps.Process.Processes[index].lines.Add(empty);
						}
						else
						{
							try
							{

								TextColor empty = new TextColor
								{
									text = File.ReadAllText(location),
									color = Apps.Process.Processes[index].color2
								};
								Apps.Process.Processes[index].lines.Add(empty);
							}
							catch (Exception e)
							{


								TextColor empty = new TextColor
								{
									text = e.ToString(),
									color = Color.Red
								};

								Apps.Process.Processes[index].lines.Add(empty);

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

						Apps.Process.Processes[index].lines.Add(empty);
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

						Apps.Process.Processes[index].lines.Add(error);
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
						location = Apps.Process.Processes[index].metaData + fullLocation;

					if (File.Exists(location))
					{
						if (commands[1].Contains(".SysData") && !Kernel.Root)
						{

							TextColor empty = new TextColor
							{
								text = "Permission denied",
								color = Color.Red
							};

							Apps.Process.Processes[index].lines.Add(empty);
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
								Apps.Process.Processes[index].lines.Add(empty);
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

								Apps.Process.Processes[index].lines.Add(empty);

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

						Apps.Process.Processes[index].lines.Add(empty);
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

						Apps.Process.Processes[index].lines.Add(error);
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
						location = Apps.Process.Processes[index].metaData + fullLocation;

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
							Apps.Process.Processes[index].lines.Add(empty);
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

							Apps.Process.Processes[index].lines.Add(empty);

						}


					}
					else
					{
						TextColor empty = new TextColor
						{
							text = "Directory " + fullLocation + " already exist!",
							color = Color.Red
						};

						Apps.Process.Processes[index].lines.Add(empty);
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

						Apps.Process.Processes[index].lines.Add(exp);
					}
					for (int i = 0; i < Apps.Process.Processes.Count; i++)
					{
						string processName = "";
						switch (Apps.Process.Processes[i].ID)
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
								color = Apps.Process.Processes[index].color2
							};

							Apps.Process.Processes[index].lines.Add(task);
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
							switch (Apps.Process.Processes[int.Parse(commands[1])].ID)
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
								color = Apps.Process.Processes[index].color1,
							};
							TextColor empty2 = new TextColor
							{
								text = "",
								color = Apps.Process.Processes[index].color1,
							};
							int lineToAdd = Apps.Process.Processes[index].lines.Count - 1;
							string pathBefore = Apps.Process.Processes[index].metaData;
							Apps.Process.Processes[index].lines[lineToAdd].text = (pathBefore + ">" + Apps.Process.Processes[index].lines[lineToAdd].text);
							Apps.Process.Processes[index].lines.Add(empty);
							Apps.Process.Processes[index].lines.Add(empty2);
							InputSystem.CurrentString = "";
							Apps.Process.Processes.RemoveAt(int.Parse(commands[1]));

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
						for (int i = 0; i < Apps.Process.Processes.Count; i++)
						{
							string processName = "";
							switch (Apps.Process.Processes[i].ID)
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
									color = Apps.Process.Processes[index].color1,
								};
								TextColor empty2 = new TextColor
								{
									text = "",
									color = Apps.Process.Processes[index].color1,
								};
								int lineToAdd = Apps.Process.Processes[index].lines.Count - 1;
								string pathBefore = Apps.Process.Processes[index].metaData;
								Apps.Process.Processes[index].lines[lineToAdd].text = (pathBefore + ">" + Apps.Process.Processes[index].lines[lineToAdd].text);
								Apps.Process.Processes[index].lines.Add(empty);
								Apps.Process.Processes[index].lines.Add(empty2);
								InputSystem.CurrentString = "";
								Apps.Process.Processes.RemoveAt(i);
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
						color = Apps.Process.Processes[index].color2
					};

					Apps.Process.Processes[index].lines.Add(empty);
				}
				else if (commands[0] == "refresh")
				{
					DrawDesktopApps.UpdateIcons();
					TextColor empty = new TextColor
					{
						text = "Desktop refreshed",
						color = green
					};

					Apps.Process.Processes[index].lines.Add(empty);
				}
				else if (commands[0] == "system" || commands[0] == "info" || commands[0] == "about" || commands[0] == "sysinfo")
				{
					GenerateText("RadianceOS " + Kernel.version, Apps.Process.Processes[index].color1, index);
					GenerateText("Version: " + Kernel.subversion, Apps.Process.Processes[index].color1, index);
					GenerateText("Created by Szymekk", Apps.Process.Processes[index].color1, index);
					GenerateText("Szymekk.pl", Apps.Process.Processes[index].color1, index);
					GenerateText("Disk space: " + Kernel.fs.Disks[0].Size / 1048576 + " MB", Apps.Process.Processes[index].color2, index);
					TextColor ram = new TextColor
					{
						text = "Ram usage: " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "/" + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB",
						color = Apps.Process.Processes[index].color2
					};
					Apps.Process.Processes[index].lines.Add(ram);

					TextColor pro = new TextColor
					{
						text = "Processor: " + CPU.GetCPUBrandString(),
						color = Apps.Process.Processes[index].color2
					};


					Apps.Process.Processes[index].lines.Add(pro);

                    GenerateText("Display: " + Explorer.screenSizeX+"x"+Explorer.screenSizeY, Apps.Process.Processes[index].color2, index);
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
						Apps.Process.Processes.Add(MessageBox2);
						Apps.Process.UpdateProcess(Apps.Process.Processes.Count - 1);
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
                            location = Apps.Process.Processes[index].metaData + fullLocation;
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

                            Apps.Process.Processes[index].lines.Add(empty);
                        }
                    }
				}
				else if (commands[0] == "help")
				{
					GenerateText("=-=FILE SYSTEM=-=", Apps.Process.Processes[index].color1, index);
					GenerateText("cat <file/path> - Reads the contents of a file.", Apps.Process.Processes[index].color2, index);
					GenerateText("cd <directory> - Changes the current directory.", Apps.Process.Processes[index].color2, index);
					GenerateText("del <file> - Deletes the file", Apps.Process.Processes[index].color2, index);
					GenerateText("dir <directory> - Displays a list of files and subdirectories in a directory.", Apps.Process.Processes[index].color2, index);
					GenerateText("echo> <file> - Creates and writes to a file.", Apps.Process.Processes[index].color2, index);
					GenerateText("md <name> - Creates a new directory", Apps.Process.Processes[index].color2, index);
					GenerateText("=-=OTHER=-=", Apps.Process.Processes[index].color1, index);
					GenerateText("time - Displays the system time.", Apps.Process.Processes[index].color2, index);
					GenerateText("tasklist - Displays all currently running tasks.", Apps.Process.Processes[index].color2, index);
					GenerateText("taskkill - Kill or stop a running process or application.", Apps.Process.Processes[index].color2, index);
					GenerateText("=-=RADIANCE OS=-=", Apps.Process.Processes[index].color1, index);
					GenerateText("ram - Displays RAM usage.", Apps.Process.Processes[index].color2, index);
					GenerateText("sysinfo - Displays information about RadianceOS and computer", Apps.Process.Processes[index].color2, index);
				}
				else if (commands[0] == "color")
				{
					commands[1] = commands[1].Trim();
					if (commands[1].Length > 1)
					{
						Color col = Color.White;
						Color col2 = Color.Gray;
						char c1 = commands[1][1];
						switch (c1)
						{
							case '0':
								col = Color.Black;
								col2 = Color.FromArgb(50,50,50);
								break;
							case '1':
								col = Color.Blue;
								col2 = Color.DarkBlue;
								break;
							case '2':
								col = Color.LimeGreen;
								col2 = Color.Green;
								break;
							case '3':
								col = Color.Aqua;
								col2 = Color.DarkCyan;
								break;
							case '4':
								col = Color.Red;
								col2 = Color.DarkRed;
								break;
							case '5':
								col = Color.Purple;
								col2 = Color.MediumPurple;
								break;
							case '6':
								col = Color.Yellow;
								col2 = Color.GreenYellow;
								break;
							case '7':
								col = Color.FromArgb(240, 240, 240);
								col2 = Color.FromArgb(186, 186, 186);
								break;
							case '8':
								col = Color.Gray;
								col2 = Color.DarkGray;
								break;
							case '9':
								col = Color.Cyan;
								col2 = Color.Blue;
								break;
							case 'a':
								col = Color.GreenYellow;
								col2 = Color.LimeGreen;
								break;
							case 'b':
								col = Color.LightCyan;
								col2 = Color.Cyan;
								break;
							case 'c':
								col = Color.Pink;
								col2 = Color.LightPink;
								break;
							case 'd':
								col = Color.MediumPurple;
								col2 = Color.Purple;
								break;
							case 'e':
								col = Color.LightYellow;
								col2 = Color.LightGoldenrodYellow;
								break;
							case 'f':
								col = Color.FromArgb(255, 255, 255);
								col2 = gray;
								break;
						}
						Apps.Process.Processes[index].color1 = col;
						Apps.Process.Processes[index].color2 = col2;


						c1 = commands[1][0];
						switch (c1)
						{
							case '0':
								col = Color.Black;
								break;
							case '1':
								col = Color.Blue;
								break;
							case '2':
								col = Color.LimeGreen;
								break;
							case '3':
								col = Color.Aqua;
								break;
							case '4':
								col = Color.Red;
								break;
							case '5':
								col = Color.Purple;
								break;
							case '6':
								col = Color.Yellow;
								break;
							case '7':
								col = Color.FromArgb(240, 240, 240);
								break;
							case '8':
								col = Color.Gray;
								break;
							case '9':
								col = Color.Cyan;
								break;
							case 'a':
								col = Color.GreenYellow;
								break;
							case 'b':
								col = Color.LightCyan;
								break;
							case 'c':
								col = Color.Pink;

								break;
							case 'd':
								col = Color.MediumPurple;
								break;
							case 'e':
								col = Color.LightYellow;
								break;
							case 'f':
								col = Color.FromArgb(255, 255, 255);
								break;
						}
						Apps.Process.Processes[index].color3 = col;


					}
					else
					{
						Color col = Color.White;
						Color col2 = Color.Gray;
						char c1 = commands[1][0];
						switch(c1)
						{
							case '0':
								col = Color.Black;
								col2 = Color.FromArgb(50, 50, 50);
								break;
							case '1':
								col = Color.Blue;
								col2 = Color.DarkBlue;
								break;
							case '2':
								col = Color.LimeGreen;
								col2 = Color.Green;
								break;
							case '3':
								col = Color.Aqua;
								col2 = Color.DarkCyan;
								break;
							case '4':
								col = Color.Red;
								col2 = Color.DarkRed;
								break;
							case '5':
								col = Color.Purple;
								col2 = Color.MediumPurple;
								break;
							case '6':
								col = Color.Yellow;
								col2 = Color.GreenYellow;
								break;
							case '7':
								col = Color.FromArgb(240, 240, 240);
								col2 = Color.FromArgb(186, 186, 186);
								break;
							case '8':
								col = Color.Gray;
								col2 = Color.DarkGray;
								break;
							case '9':
								col = Color.Cyan;
								col2 = Color.Blue;
								break;
							case 'a':
								col = Color.GreenYellow;
								col2 = Color.LimeGreen;
								break;
							case 'b':
								col = Color.LightCyan;
								col2 = Color.Cyan;
								break;
							case 'c':
								col = Color.Pink;
								col2 = Color.LightPink;
								break;
							case 'd':
								col = Color.MediumPurple;
								col2 = Color.Purple;
								break;
							case 'e':
								col = Color.LightYellow;
								col2 = Color.LightGoldenrodYellow;
								break;
							case 'f':
								col = Color.FromArgb(255,255,255);
								col2 = gray;
								break;
						}
						Apps.Process.Processes[index].color1 = col;
						Apps.Process.Processes[index].color2 = col2;
					}
				}
				else if (commands[0] == "shutdown")
				{
					Screens.Shutdown.StartShutdown();
				}
				else if (commands[0] == "reboot")
				{

				}
				else if (commands[0] == "power")
				{
					// Start the power options UI
				}
				else if (commands[0] == "logout")
				{
					Security.Auth.Session.Logout();
				}
				else
				{
					TextColor empty = new TextColor
					{
						text = "'" + commands[0] + "' is not a valid Radiance Batch command.",
						color = Color.Red
					};

					Apps.Process.Processes[index].lines.Add(empty);
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
			Apps.Process.Processes[index].lines.Add(First);
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
