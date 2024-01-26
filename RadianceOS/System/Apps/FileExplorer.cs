using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;

namespace RadianceOS.System.Apps
{
	public static class FileExplorer
	{
		public static void Render(int X, int Y, int SizeX, int SizeY, int id)
		{
			Window.DrawTop(id, X, Y, SizeX, "File Explorer - " + Apps.Process.Processes[id].temp, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, 200, SizeY - 25);
			bool clickedOn = false;
			if (Apps.Process.Processes[id].bitmap == null)
			{
				PreRender(X, Y, SizeX, SizeY, id);
			}
			else
			{
				Explorer.CanvasMain.DrawImage(Apps.Process.Processes[id].bitmap, X + 4, Y + 28);
				Explorer.CanvasMain.DrawImage(Apps.Process.Processes[id].bitmap2, X + 202, Y + 28);
				Explorer.CanvasMain.DrawImage(Apps.Process.Processes[id].bitmap3, X + SizeX - 150, Y + 28);
			}

			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + (i * 20) && Explorer.MY < Y + 48 + (i * 20))
				{
					if (Explorer.MX > X && Explorer.MX < X + 200)
					{
						if (!Apps.Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X, Y + 28 + (i * 20) - 1, 200, 20);
							Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.MainDirectories[i].Name, Kernel.font18, Kernel.fontColor, X + 4, Y + 28 + (i * 20));
						}
						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Apps.Process.Processes[id].tempBool)
						{
							UpdateList(id, Apps.Process.Processes[id].FileExplorerDat.MainDirectories[i].Path + @"\");
							PreRender(X, Y, SizeX, SizeY, id);
						}
					}
				}

			}
			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.Directories.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.Directories.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + (i * 20) && Explorer.MY < Y + 48 + (i * 20))
				{
					if (Explorer.MX > X + 200 && Explorer.MX < X + SizeX)
					{
						if (!Apps.Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X + 200, Y + 28 + (i * 20) - 1, SizeX - 200, 20);
							Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.Directories[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + (i * 20));
							Explorer.CanvasMain.DrawImage(Kernel.folder16, X + 202, Y + 28 + (i * 20));
						}

						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Apps.Process.Processes[id].tempBool)
						{
							UpdateList(id, Apps.Process.Processes[id].FileExplorerDat.Directories[i].Path + @"\");
							PreRender(X, Y, SizeX, SizeY, id);
						}
					}
				}

			}
			int start = Apps.Process.Processes[id].FileExplorerDat.Directories.Count;
			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.Files.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.Files.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + ((i + start) * 20) && Explorer.MY < Y + 48 + ((i + start) * 20))
				{
					if (Explorer.MX > X + 200 && Explorer.MX < X + SizeX)
					{
						if (!Apps.Process.Processes[id].tempBool)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X + 200, Y + 28 + ((i + start) * 20) - 1, SizeX - 200, 20);
							string extension = Apps.Process.Processes[id].FileExplorerDat.Files[i].Extension;
							Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.Files[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + ((i + start) * 20));
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

						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Apps.Process.Processes[id].tempBool)
						{
							string ext = Apps.Process.Processes[id].FileExplorerDat.Files[i].Extension;
							if (ext == "binary file")
							{
								MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be opened!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
							}
							else
							{
								Notepad.OpenFile(Apps.Process.Processes[id].FileExplorerDat.Files[i].Path);
								Apps.Process.Processes[id].tempBool = false;
							}
						
							
						}
						if (MouseManager.MouseState == MouseState.Right)
						{
							clickedOn = true;

							Apps.Process.Processes[id].tempBool = true;
							Apps.Process.Processes[id].tempInt3 = Explorer.MX - Apps.Process.Processes[id].X;
							Apps.Process.Processes[id].tempInt2 = i;

							if (Apps.Process.Processes[id].FileExplorerDat.Files[i].Extension == "text file")
							{
								Apps.Process.Processes[id].tempInt = 1;
							}
							if (Apps.Process.Processes[id].FileExplorerDat.Files[i].Extension == "ra# file")
							{
								Apps.Process.Processes[id].tempInt = 2;
							}
						}

					}
				}

			}


			if (MouseManager.MouseState == MouseState.Right && !clickedOn)
			{
				Apps.Process.Processes[id].tempBool = false;
			}
			if (MouseManager.MouseState == MouseState.Left)
			{
				//Apps.Process.Processes[id].tempBool = false;
			}
			if (Apps.Process.Processes[id].tempBool)
			{
				RenderMenu(Apps.Process.Processes[id].tempInt, Apps.Process.Processes[id].FileExplorerDat.Files[Apps.Process.Processes[id].tempInt2].Path, Apps.Process.Processes[id].tempInt3 + Apps.Process.Processes[id].X, Apps.Process.Processes[id].Y + ((Apps.Process.Processes[id].tempInt2 + start) * 20) - 25, start, id);
			}
		}

		public static void PreRender(int X, int Y, int SizeX, int SizeY, int id)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, 200, SizeY - 25);
			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count <= i)
					return;

				Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.MainDirectories[i].Name, Kernel.font18, Kernel.fontColor, X + 4, Y + 28 + (i * 20));
			}
			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.Directories.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.Directories.Count <= i)
					return;

				Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.Directories[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + (i * 20));
				Explorer.CanvasMain.DrawImage(Kernel.folder16, X + 202, Y + 28 + (i * 20));
			}
			int start = Apps.Process.Processes[id].FileExplorerDat.Directories.Count;
			for (int i = 0; i < Apps.Process.Processes[id].FileExplorerDat.Files.Count; i++)
			{
				if (Apps.Process.Processes[id].FileExplorerDat.Files.Count <= i)
					return;

				string extension = Apps.Process.Processes[id].FileExplorerDat.Files[i].Extension;
				Explorer.CanvasMain.DrawString(Apps.Process.Processes[id].FileExplorerDat.Files[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + ((i + start) * 20));
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

			if (Apps.Process.Processes[id].bitmap == null)
			{
				int max = 0;
				for (int j = 0; j < Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count; j++)
				{
					if (max < Apps.Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length)
						max = Apps.Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length;
				}
				Window.GetTempImage(X + 4, Y + 28, max * 9, Apps.Process.Processes[id].FileExplorerDat.MainDirectories.Count * 20, "File Explorer1");
				Apps.Process.Processes[id].bitmap = Window.tempBitmap;
			}
			int fcount = 0;
			fcount += Apps.Process.Processes[id].FileExplorerDat.Files.Count;
			int total = Apps.Process.Processes[id].FileExplorerDat.Files.Count + Apps.Process.Processes[id].FileExplorerDat.Directories.Count;
			if (total != 0)
			{
				int maxLenght = 0;
				for (int j = 0; j < Apps.Process.Processes[id].FileExplorerDat.Files.Count; j++)
				{
					if (maxLenght < Apps.Process.Processes[id].FileExplorerDat.Files[j].Name.Length)
						maxLenght = Apps.Process.Processes[id].FileExplorerDat.Files[j].Name.Length;
				}
				for (int j = 0; j < Apps.Process.Processes[id].FileExplorerDat.Directories.Count; j++)
				{
					if (maxLenght < Apps.Process.Processes[id].FileExplorerDat.Directories[j].Name.Length)
						maxLenght = Apps.Process.Processes[id].FileExplorerDat.Directories[j].Name.Length;
				}

				Window.GetTempImage(X + 202, Y + 28, maxLenght * 9 + 20, 20 * (fcount + Apps.Process.Processes[id].FileExplorerDat.Directories.Count), "File Explorer2");
				Apps.Process.Processes[id].bitmap2 = Window.tempBitmap;

				Window.GetTempImage(X + SizeX - 150, Y + 28, 150, (fcount + Apps.Process.Processes[id].FileExplorerDat.Directories.Count) * 20, "File Explorer3");
				Apps.Process.Processes[id].bitmap3 = Window.tempBitmap;
			}
			else
			{
				Window.GetTempImage(X + 202, Y + 28, 1, 1, "File Explorer2");
				Apps.Process.Processes[id].bitmap2 = Window.tempBitmap;

				Window.GetTempImage(X + SizeX - 150, Y + 28, 1, 1, "File Explorer3");
				Apps.Process.Processes[id].bitmap3 = Window.tempBitmap;
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

			if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && Apps.Process.Processes[ProcessID].tempBool)
			{
				Apps.Process.Processes[ProcessID].tempBool = false;

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
									Apps.Process.Processes[ProcessID].tempBool = false;
								}
								break;
							case 2:
								{
									Notepad.OpenFile(filePath);
									Apps.Process.Processes[ProcessID].tempBool = false;
								}
								break;
							case 3:
								{
									RasExecuter.StartScript(filePath);
									Apps.Process.Processes[ProcessID].tempBool = false;
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
				Apps.Process.Processes[ProcessID].FileExplorerDat = new FileExplorerData();
				Apps.Process.Processes[ProcessID].FileExplorerDat.Files = new List<FilePath>();
				Apps.Process.Processes[ProcessID].FileExplorerDat.Directories = new List<FilePath>();
				Apps.Process.Processes[ProcessID].FileExplorerDat.MainDirectories = new List<FilePath>();
				Apps.Process.Processes[ProcessID].temp = path;
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
							tempExt = "bitmap file";
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
					Apps.Process.Processes[ProcessID].FileExplorerDat.Files.Add(newFile);

				}
				string[] directories = Directory.GetDirectories(@"0:\Users\" + Kernel.loggedUser);
				FilePath zero = new FilePath
				{
					Name = @"0:\",
					Path = @"0:"
				};
				Apps.Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(zero);
				FilePath one = new FilePath
				{
					Name = @"1:\",
					Path = @"1:"
				};
				Apps.Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(one);
				for (int i = 0; i < directories.Length; i++)
				{
					FilePath newDir = new FilePath
					{
						Name = directories[i],
						Path = @"0:\Users\" + Kernel.loggedUser + @"\" + directories[i]
					};
					Apps.Process.Processes[ProcessID].FileExplorerDat.MainDirectories.Add(newDir);
				}
				string[] directoriesInCur = Directory.GetDirectories(path);
				for (int i = 0; i < directoriesInCur.Length; i++)
				{
					FilePath newDir = new FilePath
					{
						Name = directoriesInCur[i],
						Path = path + directoriesInCur[i],
					};
					Apps.Process.Processes[ProcessID].FileExplorerDat.Directories.Add(newDir);
				}
			}
			catch (Exception ex)
			{
				Kernel.Crash("File Explorer Error: " + ex.Message, 12);
			}

		}

	}
}
