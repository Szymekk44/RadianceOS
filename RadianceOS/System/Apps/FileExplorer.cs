using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System;
using RadianceOS.Render;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2;
using RadianceOS.System.Security.FileManagment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;

namespace RadianceOS.System.Apps
{
	public static class FileExplorer
	{
		static int pathIdx;
		public static void Render(int X, int Y, int SizeX, int SizeY, int id)
		{
			if(Process.Processes[id].tempBool2)
			{
				if (MessageBox.closedWith[Process.Processes[id].tempInt2] != 0)
				{
					switch (MessageBox.closedWith[Process.Processes[id].tempInt2])
					{
						case 1:
							{
								Process.Processes[id].tempBool2 = false;
							}
							break;
						case 2:
							{
								Process.Processes[id].tempBool2 = false;
								FileAction.DelteFile(Process.Processes[id].defaultLines[0]);
								Process.Processes[id].bitmap2 = null;
								Process.Processes[id].bitmap3 = null;
								
								if (Process.Processes[id].defaultLines[0].StartsWith(@"0:\Users\" + Kernel.loggedUser + @"\Desktop"))
								DrawDesktopApps.UpdateIcons();
								UpdateList(id, Process.Processes[id].temp + @"\");
								PreRender(X, Y, SizeX, SizeY, id);

							}
							break;
					}
				}
			}
			Window.DrawTop(id, X, Y, SizeX, "File Explorer - " + Process.Processes[id].temp, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, 200, SizeY - 25);
			bool clickedOn = false;
			if (Process.Processes[id].bitmap == null)
			{
				PreRender(X, Y, SizeX, SizeY, id);
			}
			else
			{
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap, X + 4, Y + 28);
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap2, X + 202, Y + 28);
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap3, X + SizeX - 150, Y + 28);
			}

			for (int i = 0; i < Process.Processes[id].FileExplorerDat.MainDirectories.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.MainDirectories.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + (i * 20) && Explorer.MY < Y + 48 + (i * 20))
				{
					if (Explorer.MX > X && Explorer.MX < X + 200)
					{
						if (!Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X, Y + 28 + (i * 20) - 1, 200, 20);
							Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.MainDirectories[i].Name, Kernel.font18, Kernel.fontColor, X + 4, Y + 28 + (i * 20));
						}
						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Process.Processes[id].tempBool)
						{
							UpdateList(id, Process.Processes[id].FileExplorerDat.MainDirectories[i].Path + @"\");
							PreRender(X, Y, SizeX, SizeY, id);
						}
					}
				}

			}
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.Directories.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.Directories.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + (i * 20) && Explorer.MY < Y + 48 + (i * 20))
				{
					if (Explorer.MX > X + 200 && Explorer.MX < X + SizeX)
					{
						if (!Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X + 200, Y + 28 + (i * 20) - 1, SizeX - 200, 20);
							Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.Directories[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + (i * 20));
							Explorer.CanvasMain.DrawImage(Kernel.folder16, X + 202, Y + 28 + (i * 20));
						}

						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Process.Processes[id].tempBool)
						{
							if (MouseHelpers.DoubleClick())
							{
								UpdateList(id, Process.Processes[id].FileExplorerDat.Directories[i].Path + @"\");
								PreRender(X, Y, SizeX, SizeY, id);
							}
						}
					}
				}

			}
			int start = Process.Processes[id].FileExplorerDat.Directories.Count;
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.Files.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.Files.Count <= i)
					return;
				pathIdx = i;
				if (Explorer.MY > Y + 28 + ((i + start) * 20) && Explorer.MY < Y + 48 + ((i + start) * 20))
				{
					if (Explorer.MX > X + 200 && Explorer.MX < X + SizeX)
					{
						if (!Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X + 200, Y + 28 + ((i + start) * 20) - 1, SizeX - 200, 20);
							string extension = Process.Processes[id].FileExplorerDat.Files[i].Extension;
							Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.Files[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + ((i + start) * 20));
							Explorer.CanvasMain.DrawString(extension, Kernel.font18, Kernel.fontColor, X + SizeX - 150, Y + 28 + ((i + start) * 20));
							if (extension == "text file")
							{
								Explorer.CanvasMain.DrawImage(Kernel.text16, X + 202, Y + 28 + ((i + start) * 20));
							}
							else if (extension == "data file" || extension == "configuration file")
							{
								Explorer.CanvasMain.DrawImage(Kernel.data16, X + 202, Y + 28 + ((i + start) * 20));
							}
							else if (extension == "system data file" || extension == "system file")
							{
								Explorer.CanvasMain.DrawImage(Kernel.sysData16, X + 202, Y + 28 + ((i + start) * 20));
							}
							else
							{
								Explorer.CanvasMain.DrawImage(Kernel.docuent16, X + 202, Y + 28 + ((i + start) * 20));
							}
						}

						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Process.Processes[id].tempBool)
						{
							if(MouseHelpers.DoubleClick())
							{
								string ext = Process.Processes[id].FileExplorerDat.Files[i].Extension;
								if (ext == "binary file")
								{
									MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be edited!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
                                }
                                else
								{
									if (Process.Processes[id].FileExplorerDat.ExplorerMode == 0)
									{
										Notepad.OpenFile(Process.Processes[id].FileExplorerDat.Files[i].Path);
										Process.Processes[id].tempBool = false;
									}
									else
									{
										switch (Process.Processes[id].FileExplorerDat.AppID)
										{
											case 2:
												switch (Process.Processes[id].FileExplorerDat.ExplorerMode)
												{
													case 1:
														Notepad.save(Process.Processes[id].FileExplorerDat.Files[i].Path);
														break;
													case 2:
														Notepad.OpenFile(Process.Processes[id].FileExplorerDat.Files[i].Path);
														break;
												}
												break;
										}
									}
								}
							}
							else if(ext == "bitmap image")
							{
								ImageViewer.OpenImage(Process.Processes[id].FileExplorerDat.Files[i].Path);
								Process.Processes[id].tempBool = false;
							}
							else
							{
								Process.Processes[id].tempBool = true;
								Process.Processes[id].FileExplorerDat.FileName = Process.Processes[id].FileExplorerDat.Files[i].Name;
                            }
						}
						if (MouseManager.MouseState == MouseState.Right)
						{
							clickedOn = true;

							Process.Processes[id].tempBool = true;
							Process.Processes[id].tempInt3 = Explorer.MX - Process.Processes[id].X;
							Process.Processes[id].tempInt2 = i;

							if (Process.Processes[id].FileExplorerDat.Files[i].Extension == "text file")
							{
								Process.Processes[id].tempInt = 1;
							}
							if (Process.Processes[id].FileExplorerDat.Files[i].Extension == "ra# file")
							{
								Process.Processes[id].tempInt = 2;
							}
						}

					}
				}

			}


			if (MouseManager.MouseState == MouseState.Right && !clickedOn)
			{
				Process.Processes[id].tempBool = false;
			}
			if (MouseManager.MouseState == MouseState.Left)
			{
				//Process.Processes[id].tempBool = false;
			}
			if (Process.Processes[id].tempBool)
			{
				RenderMenu(Process.Processes[id].tempInt, Process.Processes[id].FileExplorerDat.Files[Process.Processes[id].tempInt2].Path, Process.Processes[id].tempInt3 + Process.Processes[id].X, Process.Processes[id].Y + ((Process.Processes[id].tempInt2 + start) * 20) - 25, start, id);
			}
			RenderOptionsBar(id);
		}

		public static void PreRender(int X, int Y, int SizeX, int SizeY, int id)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, 200, SizeY - 25);
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.MainDirectories.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.MainDirectories.Count <= i)
					return;

				Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.MainDirectories[i].Name, Kernel.font18, Kernel.fontColor, X + 4, Y + 28 + (i * 20));
			}
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.Directories.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.Directories.Count <= i)
					return;

				Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.Directories[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + (i * 20));
				Explorer.CanvasMain.DrawImage(Kernel.folder16, X + 202, Y + 28 + (i * 20));
			}
			int start = Process.Processes[id].FileExplorerDat.Directories.Count;
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.Files.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.Files.Count <= i)
					return;

				string extension = Process.Processes[id].FileExplorerDat.Files[i].Extension;
				Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.Files[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + ((i + start) * 20));
				Explorer.CanvasMain.DrawString(extension, Kernel.font18, Kernel.fontColor, X + SizeX - 150, Y + 28 + ((i + start) * 20));
				if (extension == "text file")
				{
					Explorer.CanvasMain.DrawImage(Kernel.text16, X + 202, Y + 28 + ((i + start) * 20));
				}
				else if (extension == "data file" || extension == "configuration file")
				{
					Explorer.CanvasMain.DrawImage(Kernel.data16, X + 202, Y + 28 + ((i + start) * 20));
				}
				else if (extension == "system data file" || extension == "system file")
				{
					Explorer.CanvasMain.DrawImage(Kernel.sysData16, X + 202, Y + 28 + ((i + start) * 20));
				}
				else
				{
					Explorer.CanvasMain.DrawImage(Kernel.docuent16, X + 202, Y + 28 + ((i + start) * 20));
				}


			}

			if (Process.Processes[id].bitmap == null)
			{
				int max = 0;
				for (int j = 0; j < Process.Processes[id].FileExplorerDat.MainDirectories.Count; j++)
				{
					if (max < Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length)
						max = Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length;
				}
				Window.GetTempImage(X + 4, Y + 28, max * 9, Process.Processes[id].FileExplorerDat.MainDirectories.Count * 20, "File Explorer1");
				Process.Processes[id].bitmap = Window.tempBitmap;
			}
			int fcount = 0;
			fcount += Process.Processes[id].FileExplorerDat.Files.Count;
			int total = Process.Processes[id].FileExplorerDat.Files.Count + Process.Processes[id].FileExplorerDat.Directories.Count;
			if (total != 0)
			{
				int maxLenght = 0;
				for (int j = 0; j < Process.Processes[id].FileExplorerDat.Files.Count; j++)
				{
					if (maxLenght < Process.Processes[id].FileExplorerDat.Files[j].Name.Length)
						maxLenght = Process.Processes[id].FileExplorerDat.Files[j].Name.Length;
				}
				for (int j = 0; j < Process.Processes[id].FileExplorerDat.Directories.Count; j++)
				{
					if (maxLenght < Process.Processes[id].FileExplorerDat.Directories[j].Name.Length)
						maxLenght = Process.Processes[id].FileExplorerDat.Directories[j].Name.Length;
				}

				Window.GetTempImage(X + 202, Y + 28, maxLenght * 9 + 20, 20 * (fcount + Process.Processes[id].FileExplorerDat.Directories.Count), "File Explorer2");
				Process.Processes[id].bitmap2 = Window.tempBitmap;

				Window.GetTempImage(X + SizeX - 150, Y + 28, 150, (fcount + Process.Processes[id].FileExplorerDat.Directories.Count) * 20, "File Explorer3");
				Process.Processes[id].bitmap3 = Window.tempBitmap;
			}
			else
			{
				Window.GetTempImage(X + 202, Y + 28, 1, 1, "File Explorer2");
				Process.Processes[id].bitmap2 = Window.tempBitmap;

				Window.GetTempImage(X + SizeX - 150, Y + 28, 1, 1, "File Explorer3");
				Process.Processes[id].bitmap3 = Window.tempBitmap;
			}
		}

		public static void RenderMenu(int id, string path, int MX, int MY, int start, int ProcessID)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, MX, MY, 150, 59);
			switch (id)
			{
				default:
					DrawExplorerButton("Open", 0, 0, MX, MY, path, ProcessID);
					DrawExplorerButton("Rename", 1, 1, MX, MY, path, ProcessID);
					DrawExplorerButton("Delete", 2, 2, MX, MY, path, ProcessID);
					break;
				case 2:
					DrawExplorerButton("Run", 3, 0, MX, MY, path, ProcessID);
					DrawExplorerButton("Edit", 0, 1, MX, MY, path, ProcessID);
					DrawExplorerButton("Rename", 1, 2, MX, MY, path, ProcessID);
					DrawExplorerButton("Delete", 2, 2, MX, MY, path, ProcessID);
					break;
			}

			if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && Process.Processes[ProcessID].tempBool)
			{
				Process.Processes[ProcessID].tempBool = false;

			}
		}

		public static void DrawExplorerButton(string text, int action, int num, int x, int y, string filePath, int ProcessID)
		{
			int trueY = y + (num * 18) + 2;

			if (Explorer.MX > x && Explorer.MX < x + 150)
			{
				if (Explorer.MY > trueY && Explorer.MY < trueY + 18)
				{
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, x, trueY, 150, 18);
					if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
					{
						switch (action)
						{
							case 0:
								{
									Notepad.OpenFile(filePath);
									Process.Processes[ProcessID].tempBool = false;
								}
								break;
							case 2:
								{
									CallDelete(ProcessID, filePath);
									Process.Processes[ProcessID].tempBool = false;
								}
								break;
							case 3:
								{
									RasExecuter.StartScript(filePath);
									Process.Processes[ProcessID].tempBool = false;
								}
								break;
						}
					}
				}
			}
			Explorer.CanvasMain.DrawString(text, Kernel.font18, Kernel.fontColor, x + 3, trueY);
		}

		public static void UpdateList(int ProcessID, string path)
		{
			try
			{
				Process.Processes[ProcessID].FileExplorerDat = new FileExplorerData();
				Process.Processes[ProcessID].FileExplorerDat.Files = new List<FilePath>();
				Process.Processes[ProcessID].FileExplorerDat.Directories = new List<FilePath>();
				Process.Processes[ProcessID].FileExplorerDat.MainDirectories = new List<FilePath>();
				Process.Processes[ProcessID].temp = path;
				string[] files = Directory.GetFiles(path);
				for (int i = 0; i < files.Length; i++)
				{
					string tempExt;
					if (files[i].Contains('.'))
					{
						string curr = files[i].Substring(files[i].LastIndexOf('.'));
						if (curr == ".txt")
							tempExt = "text file";
						else if (curr == ".bmp")
							tempExt = "bitmap image";
						else if (curr == ".ras")
							tempExt = "ra# file";
						else if (curr == ".dat")
							tempExt = "data file";
						else if (curr == ".SysData")
							tempExt = "system data file";
						else if (curr == ".sys")
							tempExt = "system file";
						else if (curr == ".bin")
							tempExt = "binary file";
						else if (curr == ".cfg")
							tempExt = "configuration file";
						else
							tempExt = curr;
					}

					else
						tempExt = "file";
					FilePath newFile = new FilePath
					{
						Name = files[i],
						Path = path + files[i],

						Extension = tempExt
					};
					Process.Processes[ProcessID].FileExplorerDat.Files.Add(newFile);

				}
				string[] directories = Directory.GetDirectories(@"0:\Users\" + Kernel.loggedUser);
				FilePath zero = new FilePath
				{
					Name = @"0:\",
					Path = @"0:"
				};
				Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(zero);
				FilePath one = new FilePath
				{
					Name = @"1:\",
					Path = @"1:"
				};
				Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(one);
				for (int i = 0; i < directories.Length; i++)
				{
					FilePath newDir = new FilePath
					{
						Name = directories[i],
						Path = @"0:\Users\" + Kernel.loggedUser + @"\" + directories[i]
					};
					Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(newDir);
				}
				string[] directoriesInCur = Directory.GetDirectories(path);
				for (int i = 0; i < directoriesInCur.Length; i++)
				{
					FilePath newDir = new FilePath
					{
						Name = directoriesInCur[i],
						Path = path + directoriesInCur[i],
					};
					Process.Processes[ProcessID].FileExplorerDat.Directories.Add(newDir);
				}
			}
			catch (Exception ex)
			{
				Kernel.Crash("File Explorer Error: " + ex.Message, 12);
			}

		}


		public static void CallDelete(int ProdcessID, string path)
		{
			Process.Processes[ProdcessID].tempBool2 = true;
			MessageBoxCreator.CreateMessageBox("Delete", "Are you sure you want to delete\n" + path, MessageBoxCreator.MessageBoxIcon.warning, 500, 175, "Cancel", "Delete");
			Process.Processes[ProdcessID].tempInt2 = Process.Processes[Process.Processes.Count - 1].DataID;

			Process.Processes[ProdcessID].defaultLines = new List<string>
			{
				path
			};
		}

	}
  
		public static void RenderOptionsBar(int ProcessID)
		{
			int X = Process.Processes[ProcessID].X;
			int Y = Process.Processes[ProcessID].Y;
			int SizeX = Process.Processes[ProcessID].SizeX;
			int SizeY = Process.Processes[ProcessID].SizeY;
			if (ProcessID != 0)
			{
				Canvas.canvas.DrawFilledRectangle(Kernel.middark, X, Y + SizeY - 50, SizeX, 50);
				Canvas.canvas.DrawFilledRectangle(Kernel.lightMain, X + SizeX - 230, Y + SizeY - 40, 100, 30);
				StringsAcitons.DrawCenteredString("Cancel", 100, X + SizeX - 230, Y + SizeY - 34, 15, Kernel.fontColor, Kernel.fontDefault);
				Canvas.canvas.DrawFilledRectangle(Kernel.main, X + 10, Y + SizeY - 40, SizeX - 260, 30);
				Canvas.canvas.DrawRectangle(Kernel.lightMain, X + 10, Y + SizeY - 40, SizeX - 260, 30);
				Canvas.canvas.DrawString(Process.Processes[ProcessID].FileExplorerDat.FileName, Kernel.fontDefault, Kernel.fontColor, X + 15, Y + SizeY - 40 - Kernel.fontDefault.Height / 2 + 15);

			}
			switch (Process.Processes[ProcessID].FileExplorerDat.ExplorerMode)
			{
				case 1:
					Canvas.canvas.DrawFilledRectangle(Kernel.lightMain, X + SizeX - 110, Y + SizeY - 40, 100, 30);
					StringsAcitons.DrawCenteredString("Save", 100, X + SizeX - 110, Y + SizeY - 34, 15, Kernel.fontColor, Kernel.fontDefault);
                    string ext1 = Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Extension;
                    if (ext1 == "binary file")
                    {
                        MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be edited!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
                    }
					else
					{
                        switch (Process.Processes[ProcessID].FileExplorerDat.AppID)
                        {
                            case 2:
                                Notepad.save(Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Path);
                                break;
                        }
                    }
                    break;
				case 2:
					Canvas.canvas.DrawFilledRectangle(Kernel.lightMain, X + SizeX - 110, Y + SizeY - 40, 100, 30);
					StringsAcitons.DrawCenteredString("Select", 100, X + SizeX - 110, Y + SizeY - 34, 15, Kernel.fontColor, Kernel.fontDefault);
                    string ext2 = Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Extension;
                    if (ext2 == "binary file")
                    {
                        MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be edited!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
                    }
                    else
                    {
                        switch (Process.Processes[ProcessID].FileExplorerDat.AppID)
                        {
                            case 2:
                                Notepad.save(Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Path);
                                break;
                        }
                    }
                    break;
				case 3:
					Canvas.canvas.DrawFilledRectangle(Kernel.lightMain, X + SizeX - 110, Y + SizeY - 40, 100, 30);
					StringsAcitons.DrawCenteredString("Select", 100, X + SizeX - 110, Y + SizeY - 34, 15, Kernel.fontColor, Kernel.fontDefault);
					break;
			}
		}
		public static void enter(int ProcessID)
		{
            switch (Process.Processes[ProcessID].FileExplorerDat.ExplorerMode)
            {
                case 1:
                    string ext = Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Extension;
                    if (ext == "binary file")
                    {
                        MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be edited!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
                    }
                    else
                    {
                        switch (Process.Processes[ProcessID].FileExplorerDat.AppID)
                        {
                            case 2:
                                Notepad.save(Process.Processes[ProcessID].FileExplorerDat.Files[pathIdx].Path);
                                break;
                        }
                    }
					break;
				case 2:

					break;
            }
        }

    }
}
