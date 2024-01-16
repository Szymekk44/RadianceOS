using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
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
		public static void Render( int X, int Y, int SizeX, int SizeY, int id)
		{
			Window.DrawTop(id, X, Y, SizeX, "File Explorer - " + Process.Processes[id].temp, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, 200, SizeY - 25);
			bool clickedOn = false;
			if (Process.Processes[id].bitmap == null)
			{
				PreRender(X, Y,SizeX, SizeY, id);
			}
			else
			{
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap, X+4, Y+  28);
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap2, X + 202, Y + 28);
				Explorer.CanvasMain.DrawImage(Process.Processes[id].bitmap3, X + SizeX-150, Y + 28);
			}

			for (int i = 0; i < Process.Processes[id].FileExplorerDat.MainDirectories.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.MainDirectories.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + (i * 20) && Explorer.MY < Y + 48 + (i * 20))
				{
					if (Explorer.MX > X && Explorer.MX < X + 200)
					{
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X, Y + 28 + (i * 20) - 1, 200, 20);
						Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.MainDirectories[i].Name, Kernel.font18, Kernel.fontColor, X + 4, Y + 28 + (i * 20));
						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
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
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X + 200, Y + 28 + (i * 20) - 1, SizeX - 200, 20);
						Explorer.CanvasMain.DrawString(Process.Processes[id].FileExplorerDat.Directories[i].Name, Kernel.font18, Kernel.fontColor, X + 220, Y + 28 + (i * 20));
						Explorer.CanvasMain.DrawImage(Kernel.folder16, X + 202, Y + 28 + (i * 20));
						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
						{
							UpdateList(id, Process.Processes[id].FileExplorerDat.Directories[i].Path + @"\");
							PreRender(X, Y, SizeX, SizeY, id);
						}
					}
				}
				
			}
			int start = Process.Processes[id].FileExplorerDat.Directories.Count;
			for (int i = 0; i < Process.Processes[id].FileExplorerDat.Files.Count; i++)
			{
				if (Process.Processes[id].FileExplorerDat.Files.Count <= i)
					return;
				if (Explorer.MY > Y + 28 + ((i + start) * 20) && Explorer.MY < Y + 48 + ((i + start) * 20))
				{
					if (Explorer.MX > X + 200 && Explorer.MX < X + SizeX)
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
						if (MouseManager.MouseState == MouseState.Left && !Explorer.Clicked && !Process.Processes[id].tempBool)
						{
							string ext = Process.Processes[id].FileExplorerDat.Files[i].Extension;
							if (ext == "binary file")
							{
								MessageBoxCreator.CreateMessageBox("Error", "This file type cannot be opened!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);
							}
							else
								Notepad.OpenFile(Process.Processes[id].FileExplorerDat.Files[i].Path);
							Process.Processes[id].tempBool = false;
						}
						if (MouseManager.MouseState == MouseState.Right)
						{
							clickedOn = true;
							if (!Process.Processes[id].tempBool)
							{
								Process.Processes[id].tempBool = true;
								Process.Processes[id].tempInt3 = Explorer.MX - Process.Processes[id].X;
								Process.Processes[id].tempInt2 = i;

								if (Process.Processes[id].FileExplorerDat.Files[i].Extension == "text file")
								{
									Process.Processes[id].tempInt = 1;

								}

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
				Process.Processes[id].tempBool = false;
			}
			if (Process.Processes[id].tempBool)
			{
				switch(Process.Processes[id].tempInt)
				{
					case 1:
						{
							RenderMenu(Process.Processes[id].tempInt, Process.Processes[id].FileExplorerDat.Files[Process.Processes[id].tempInt2].Path, Process.Processes[id].tempInt3 + Process.Processes[id].X, Process.Processes[id].Y + ((Process.Processes[id].tempInt2 + start) * 20) - 25, start, id);
						}
						break;
				}

			}
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
				if (Process.Processes[id].bitmap == null)
				{
					int max = 0;
					for (int j = 0; j < Process.Processes[id].FileExplorerDat.MainDirectories.Count; j++)
					{
						if (max < Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length)
							max = Process.Processes[id].FileExplorerDat.MainDirectories[j].Name.Length;
					}
					Window.GetTempImage(X+4, Y + 28, max * 9, Process.Processes[id].FileExplorerDat.MainDirectories.Count * 20, "File Explorer1");
					Process.Processes[id].bitmap = Window.tempBitmap;
				}

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

				Window.GetTempImage(X + 202, Y + 28, maxLenght * 9 + 20, (Process.Processes[id].FileExplorerDat.Directories.Count + Process.Processes[id].FileExplorerDat.Files.Count) * 20, "File Explorer2");
				Process.Processes[id].bitmap2 = Window.tempBitmap;

				Window.GetTempImage(X + SizeX - 150, Y + 28, 150, (Process.Processes[id].FileExplorerDat.Files.Count + Process.Processes[id].FileExplorerDat.Directories.Count) * 20, "File Explorer1");
				Process.Processes[id].bitmap3 = Window.tempBitmap;
			}
		}

		public static void RenderMenu(int id, string path, int MX,int MY, int start, int ProcessID)
		{
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, MX, MY, 150, 64);
			DrawExplorerButton("Open", 1, 0, MX, MY, path);
            DrawExplorerButton("Rename", 1, 1, MX, MY, path);
            DrawExplorerButton("Delete", 1, 2, MX, MY, path);
        }

		public static void DrawExplorerButton(string text, int action, int num, int x, int y, string filePath)
		{
			int trueY = y + (num * 18) + 2;
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
			catch(Exception ex)
			{
				Kernel.Crash("File Explorer Error: " + ex.Message, 12); 
			}

		}

	}
}
