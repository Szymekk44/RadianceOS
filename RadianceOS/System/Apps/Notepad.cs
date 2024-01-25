using Cosmos.System;
using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class Notepad
	{
		public static void Render(int X, int Y, int SizeX, int SizeY,int i, List<String> texts)
		{
			if (Process.Processes[i].saved)
				Window.DrawTop(i, X, Y, SizeX, "Notepad - " + Process.Processes[i].Name, true, true, false, true);
			else
				Window.DrawTop(i, X, Y, SizeX, "Notepad - " + Process.Processes[i].Name + " *", true, true, false, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY-25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y+25, SizeX, 25);
			DrawNotepadButton(0, X, Y, 60, i );

			int minusSize = 50;
			switch(Process.Processes[i].tempInt3)
			{
                case 1:
                    minusSize = 70;
                    Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(255, 244, 89), X, Y + 50, SizeX, 20);
					StringsAcitons.DrawCenteredString("You are currently editing a .dat file, be careful!", SizeX, X, Y+52, 20, Color.Black, Kernel.fontRuscii);
                    break;
                case 2:
                    minusSize = 70;
                    Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(255, 106, 89), X, Y + 50, SizeX, 20);
                    StringsAcitons.DrawCenteredString("You are currently viewing a read only file!", SizeX, X, Y + 52, 20, Color.Black, Kernel.fontRuscii);
                    break;
                case 3:
                    minusSize = 70;
                    Explorer.CanvasMain.DrawFilledRectangle(Color.FromArgb(255, 106, 89), X, Y + 50, SizeX, 20);
                    StringsAcitons.DrawCenteredString("You cannot view or edit this file!", SizeX, X, Y + 52, 20, Color.Black, Kernel.fontRuscii);
					return;
                    break;
            }


            int start = Process.Processes[i].StartLine;
			if (texts.Count == 0)
				Process.Processes[i].defaultLines.Add("");
			if(Process.Processes[i].defaultLines.Count < (Process.Processes[i].SizeY - minusSize) / 18)
			{
				for (int j = start; j < Process.Processes[i].defaultLines.Count; j++)
				{

					if (j != Process.Processes[i].CurrLine)
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize+2 + ((j - start) * 18));
					else if (Process.Processes[i].selected && !Process.Processes[i].saving)
					{
						string result = texts[j].Substring(0, Process.Processes[i].CurrChar) + "|" + texts[j].Substring(Process.Processes[i].CurrChar);
						Explorer.CanvasMain.DrawString(result, Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					}
					else
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize + 2 + ((j - start) * 18));
				}
			}
			else
			{
				for (int j = start; j < start+ (Process.Processes[i].SizeY - minusSize) / 18; j++)
				{
					if (j >= texts.Count)
						break;
					if (j != Process.Processes[i].CurrLine)
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					else if (Process.Processes[i].selected && !Process.Processes[i].saving)
					{
						string result = texts[j].Substring(0, Process.Processes[i].CurrChar) + "|" + texts[j].Substring(Process.Processes[i].CurrChar);
						Explorer.CanvasMain.DrawString(result, Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					}
					else
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
				}
			}
			
			if (Process.Processes[i].selected)
			{
				if (!Process.Processes[i].saving)
				{
					InputSystem.SpecialCharracters = true;
					InputSystem.AllowArrows = true;
					InputSystem.AllowUpDown = true;
					InputSystem.Monitore(2, Process.Processes[i].CurrChar, i);
					Process.Processes[i].tempInt2 = 0;
					Process.Processes[i].defaultLines[Process.Processes[i].CurrLine] = InputSystem.CurrentString;
					switch(Process.Processes[i].tempInt)
					{
						case 1:
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 50, 150, 40);
								DrawSmallNotepadButton(0, X, Y + 50, 150, i);
							}
							break;
					}
					if(Process.Processes[i].tempInt != 0 && Process.Processes[i].tempInt2 == 0)
					{
						if(!Explorer.Clicked)
						{
							if(MouseManager.MouseState == MouseState.Left)
							{
								Process.Processes[i].tempInt = 0;
							}
						}
					}
				}
				else
				{
					InputSystem.SpecialCharracters = true;
					InputSystem.AllowArrows = false;
					InputSystem.AllowUpDown = false;
					InputSystem.Monitore(3, 0, 0);

					if (Process.Processes[i].tempBool)
					{
						InputSystem.CurrentString = Process.Processes[i].temp;
						Process.Processes[i].tempBool = false;
					}

					Process.Processes[i].temp = InputSystem.CurrentString;
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + (SizeX / 2) - 248, Y + (SizeY / 2) - 98, 500, 200);
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + (SizeX / 2) - 250, Y + (SizeY / 2) - 100, 500, 200);
					StringsAcitons.DrawCenteredString("Save File", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 100, 18, Color.White, Kernel.font18);
					StringsAcitons.DrawCenteredString(">>> " + Process.Processes[i].temp + "|", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 60, 18, Color.White, Kernel.font18);
					//	StringsAcitons.DrawCenteredString(@"path: 0:\Users\" + Kernel.loggedUser + @"\Desktop", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 20, 18, Color.White, Kernel.font18);
					Process.Processes[i].tempInt = 0;

				}


			}
			else if (Process.Processes[i].saving)
			{
				Process.Processes[i].saving = false;
				Process.Processes[i].tempInt = 0;
			}
			else
				Process.Processes[i].tempInt = 0;


		}
		public static void DrawSmallNotepadButton(int id, int X, int Y, int ButtonSizeX, int ProcessID)
		{
			if (Explorer.MX > X && Explorer.MX < X + ButtonSizeX)
			{
				if (Explorer.MY > Y && Explorer.MY < Y + 20)
				{
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y, ButtonSizeX, 20);
					if (MouseManager.MouseState == MouseState.Left)
					{
						Process.Processes[ProcessID].tempInt2 = 1;
						switch (id)
						{
							case 0:
								save();
								break;
						}
					}

				}
			}
			switch (id)
			{
				case 0:
					Explorer.CanvasMain.DrawString("Save", Kernel.font18, Kernel.fontColor, X + 1, Y);
					break;
			}

		}



		public static void DrawNotepadButton(int id, int X, int Y, int ButtonSizeX, int ProcessID)
		{
			if (Explorer.MX > X && Explorer.MX < X + ButtonSizeX)
			{
				if(Explorer.MY > Y + 25 && Explorer.MY < Y+50)
				{
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightlightMain, X, Y + 25, ButtonSizeX, 25);
					if (MouseManager.MouseState == MouseState.Left)
					{
						switch(id)
						{
							case 0:
								Process.Processes[ProcessID].tempInt = 1;
								break;
						}
					}
						
				}
				else
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, ButtonSizeX, 25);
			}
			else
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, X, Y + 25, ButtonSizeX, 25);
			switch(id)
			{
				case 0:
					StringsAcitons.DrawCenteredString("File", 60, X, Y + 28, 20, Kernel.fontColor, Kernel.font18);
					break;
			}
		
		}




		public static void enter()
		{
			for (int i = 0; i < Process.Processes.Count; i++)
			{
				if (Process.Processes[i].ID == 2)
				{
					if (Process.Processes[i].selected)
					{
						int lineToAdd = Process.Processes[i].defaultLines.Count - 1;

						//Batch.RunCommand(Process.Processes[i].lines[Process.Processes[i].lines.Count - 1].text, i);
					
				
						Process.Processes[i].CurrLine++;
						if (Process.Processes[i].defaultLines.Count >= (Process.Processes[i].SizeY - 50) / 18)
						{
							Process.Processes[i].StartLine++;
							
						}
						
						if (Process.Processes[i].CurrLine > Process.Processes[i].defaultLines.Count)
						{
							
							Process.Processes[i].defaultLines.Add("");

							InputSystem.CurrentString = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine -1].Substring(Process.Processes[i].CurrChar); //TU TRZEBA ZMIENIĆ!!1
							Process.Processes[i].defaultLines[Process.Processes[i].CurrLine-1] = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine - 1].Remove(Process.Processes[i].CurrChar);
							Process.Processes[i].CurrChar = 0;
						}
						else
						{
							
							Process.Processes[i].defaultLines.Insert(Process.Processes[i].CurrLine, "");
							InputSystem.CurrentString = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine - 1].Substring(Process.Processes[i].CurrChar); //TU TRZEBA ZMIENIĆ!!1
							Process.Processes[i].defaultLines[Process.Processes[i].CurrLine - 1] = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine - 1].Remove(Process.Processes[i].CurrChar);
							Process.Processes[i].CurrChar = 0;
						}
					}
				}

			}
		}


		public static void save()
		{
			for (int i = 0; i < Process.Processes.Count; i++)
			{
				if (Process.Processes[i].ID == 2)
				{
					if (Process.Processes[i].selected)
					{
						if (Process.Processes[i].temp != null)
						{
							if(Process.Processes[i].tempInt3 == 0 || Process.Processes[i].tempInt3 == 1)
							{
                                if (Process.Processes[i].temp.Contains(".txt") || Process.Processes[i].temp.Contains(".ras") || Process.Processes[i].temp.Contains("."))
                                {
                                    if (!Process.Processes[i].saved)
                                    {
                                        if (Kernel.diskReady)
                                        {
                                            var file_stream = File.Create(Process.Processes[i].temp);
                                            string[] pathArg = Process.Processes[i].temp.Split(@"\");
                                            Process.Processes[i].Name = pathArg[pathArg.Length - 1];
                                            file_stream.Close();
                                            string finaleString = "";
                                            for (int j = 0; j < Process.Processes[i].defaultLines.Count; j++)
                                            {
                                                finaleString += Process.Processes[i].defaultLines[j] + '\n';
                                            }
                                            File.WriteAllText(Process.Processes[i].temp, finaleString);
                                            DrawDesktopApps.UpdateIcons();
                                        }
                                        else
                                        {
                                            MessageBoxCreator.CreateMessageBox("Error", "Read only file system!\nPlease format your drive\nand create an account.", MessageBoxCreator.MessageBoxIcon.error, 400);
                                        }
                                        Process.Processes[i].saved = true;
                                        Process.Processes[i].saving = false;
                                        InputSystem.CurrentString = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine];
                                        Process.Processes[i].tempInt = 0;

                                    }
                                }
                                else
                                {
                                    Process.Processes[i].saving = true;
                                    Process.Processes[i].tempBool = true;

                                }
                            }
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Cannot save read only file!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);

                            }
								
						}
						else
						{
							Process.Processes[i].saving = true;
							Process.Processes[i].tempBool = true;
						}

					}
				}

			}
		}

		public static void save(string path)
		{
			for (int i = 0; i < Process.Processes.Count; i++)
			{
				if (Process.Processes[i].ID == 2)
				{
					if (Process.Processes[i].selected)
					{
						if (Process.Processes[i].temp != null)
						{
							if(Process.Processes[i].tempInt3 == 0 || Process.Processes[i].tempInt3 == 1)
							{
                                if (Process.Processes[i].temp.Contains(".txt") || Process.Processes[i].temp.Contains(".ras") || Process.Processes[i].temp.Contains("."))
                                {
                                    if (!Process.Processes[i].saved)
                                    {
                                        if (Kernel.diskReady)
                                        {
                                            var file_stream = File.Create(Process.Processes[i].temp);
                                            string[] pathArg = Process.Processes[i].temp.Split(@"\");
                                            Process.Processes[i].Name = pathArg[pathArg.Length - 1];
                                            file_stream.Close();
                                            string finaleString = "";
                                            for (int j = 0; j < Process.Processes[i].defaultLines.Count; j++)
                                            {
                                                finaleString += Process.Processes[i].defaultLines[j] + '\n';
                                            }
                                            File.WriteAllText(Process.Processes[i].temp, finaleString);
                                            DrawDesktopApps.UpdateIcons();
                                        }
                                        else
                                        {
                                            MessageBoxCreator.CreateMessageBox("Error", "Read only file system!\nPlease format your drive\nand create an account.", MessageBoxCreator.MessageBoxIcon.error, 400);
                                        }
                                        Process.Processes[i].saved = true;
                                        Process.Processes[i].saving = false;
                                        InputSystem.CurrentString = Process.Processes[i].defaultLines[Process.Processes[i].CurrLine];
                                        Process.Processes[i].tempInt = 0;

                                    }
                                }
                                else
                                {
                                    Process.Processes[i].saving = true;
                                    Process.Processes[i].tempBool = true;

                                }
                            }
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Cannot save read only file!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);

                            }
								
						}

					}
				}

			}
		}
		
		public static void OpenFile(string path)
		{
			string[] content = File.ReadAllText(path).Split('\n');
			int lenght = 0;
			if (content.Length > 1)
			{
				for (int i = 0; i < content.Length; i++)
				{
					lenght += content[i].Length;
				}
				List<string> contentList = content.ToList();
				contentList.RemoveAt(contentList.Count - 1);
				content = contentList.ToArray();

			}
			else
				lenght = content[0].Length;
			if(lenght > 25000)
			{
				MessageBoxCreator.CreateMessageBox("Notepad Error", "File " + path + "\nis too large for notepad to open!\n(" + lenght + ")", MessageBoxCreator.MessageBoxIcon.error, 400, 175);
				return;
			}
			int minX = 200;
			string[] pathArg = path.Split(@"\");
			string[] dots = path.Split('.');
			string extension = dots[dots.Length -1];
			int warningLevel = 0;
			if (extension == "dat")
				warningLevel = 1;
            if (extension == "bin")
                warningLevel = 2;
			if(path.Contains(@"1:\") && !path.Contains(@"0:\"))
                warningLevel = 2;
            if (extension == "SysData" && !Kernel.Root)
			{
				MessageBoxCreator.CreateMessageBox("STOP", "RadianceOS blocked an attempt to open a system file.", MessageBoxCreator.MessageBoxIcon.STOP, 600, 150);
				return;
			}
			if (warningLevel != 0)
				minX = 400;
			 
            Processes MessageBox2 = new Processes
			{
				ID = 2,
				Name = pathArg[pathArg.Length-1],
				Description = "Notepad",
				metaData = @"0:\",
				defaultLines = content.ToList(),
				X = 150,
				Y = 150,
				SizeX = 800,
				SizeY = 500,
				saved = true,
				sizeAble = true,
				MinX = minX,
				CurrLine = content.Length - 1,
				temp = path,
				tempInt3 = warningLevel,
				CurrChar = content[content.Length - 1].Length,
				moveAble = true
			};
			Process.Processes.Add(MessageBox2);
			Process.UpdateProcess(Process.Processes.Count - 1);

		}
		
		public static void load(string path)
		{
			string[] content = File.ReadAllText(path).Split('\n');
			int lenght = 0;
			if (content.Length > 1)
			{
				for (int i = 0; i < content.Length; i++)
				{
					lenght += content[i].Length;
				}
				List<string> contentList = content.ToList();
				contentList.RemoveAt(contentList.Count - 1);
				content = contentList.ToArray();

			}
			else
				lenght = content[0].Length;
			if(lenght > 25000)
			{
				MessageBoxCreator.CreateMessageBox("Notepad Error", "File " + path + "\nis too large for notepad to open!\n(" + lenght + ")", MessageBoxCreator.MessageBoxIcon.error, 400, 175);
				return;
			}
			int minX = 200;
			string[] pathArg = path.Split(@"\");
			string[] dots = path.Split('.');
			string extension = dots[dots.Length -1];
			int warningLevel = 0;
			if (extension == "dat")
				warningLevel = 1;
            if (extension == "bin")
                warningLevel = 2;
			if(path.Contains(@"1:\") && !path.Contains(@"0:\"))
                warningLevel = 2;
            if (extension == "SysData" && !Kernel.Root)
			{
				MessageBoxCreator.CreateMessageBox("STOP", "RadianceOS blocked an attempt to open a system file.", MessageBoxCreator.MessageBoxIcon.STOP, 600, 150);
				return;
			}
			if (warningLevel != 0)
				minX = 400;
			 
            Processes MessageBox2 = new Processes
			{
				ID = 2,
				Name = pathArg[pathArg.Length-1],
				Description = "Notepad",
				metaData = @"0:\",
				defaultLines = content.ToList(),
				X = 150,
				Y = 150,
				SizeX = 800,
				SizeY = 500,
				saved = true,
				sizeAble = true,
				MinX = minX,
				CurrLine = content.Length - 1,
				temp = path,
				tempInt3 = warningLevel,
				CurrChar = content[content.Length - 1].Length,
				moveAble = true
			};
			Process.Processes.Add(MessageBox2);
			Process.UpdateProcess(Process.Processes.Count - 1);

		}


	

	}
}
