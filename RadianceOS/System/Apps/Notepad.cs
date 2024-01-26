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
			if (Apps.Process.Processes[i].saved)
				Window.DrawTop(i, X, Y, SizeX, "Notepad - " + Apps.Process.Processes[i].Name, true, true, false, true);
			else
				Window.DrawTop(i, X, Y, SizeX, "Notepad - " + Apps.Process.Processes[i].Name + " *", true, true, false, true);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY-25);
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y + 25, SizeX, SizeY - 25);

			Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X, Y+25, SizeX, 25);
			DrawNotepadButton(0, X, Y, 60, i );

			int minusSize = 50;
			switch(Apps.Process.Processes[i].tempInt3)
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


            int start = Apps.Process.Processes[i].StartLine;
			if (texts.Count == 0)
				Apps.Process.Processes[i].defaultLines.Add("");
			if(Apps.Process.Processes[i].defaultLines.Count < (Apps.Process.Processes[i].SizeY - minusSize) / 18)
			{
				for (int j = start; j < Apps.Process.Processes[i].defaultLines.Count; j++)
				{

					if (j != Apps.Process.Processes[i].CurrLine)
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize+2 + ((j - start) * 18));
					else if (Apps.Process.Processes[i].selected && !Apps.Process.Processes[i].saving)
					{
						string result = texts[j].Substring(0, Apps.Process.Processes[i].CurrChar) + "|" + texts[j].Substring(Apps.Process.Processes[i].CurrChar);
						Explorer.CanvasMain.DrawString(result, Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					}
					else
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Kernel.fontColor, X + 2, Y + minusSize + 2 + ((j - start) * 18));
				}
			}
			else
			{
				for (int j = start; j < start+ (Apps.Process.Processes[i].SizeY - minusSize) / 18; j++)
				{
					if (j >= texts.Count)
						break;
					if (j != Apps.Process.Processes[i].CurrLine)
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					else if (Apps.Process.Processes[i].selected && !Apps.Process.Processes[i].saving)
					{
						string result = texts[j].Substring(0, Apps.Process.Processes[i].CurrChar) + "|" + texts[j].Substring(Apps.Process.Processes[i].CurrChar);
						Explorer.CanvasMain.DrawString(result, Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
					}
					else
						Explorer.CanvasMain.DrawString(texts[j], Kernel.font18, Color.White, X + 2, Y + minusSize + 2 + ((j - start) * 18));
				}
			}
			
			if (Apps.Process.Processes[i].selected)
			{
				if (!Apps.Process.Processes[i].saving)
				{
					InputSystem.SpecialCharracters = true;
					InputSystem.AllowArrows = true;
					InputSystem.AllowUpDown = true;
					InputSystem.Monitore(2, Apps.Process.Processes[i].CurrChar, i);
					Apps.Process.Processes[i].tempInt2 = 0;
					Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine] = InputSystem.CurrentString;
					switch(Apps.Process.Processes[i].tempInt)
					{
						case 1:
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 50, 150, 40);
								DrawSmallNotepadButton(0, X, Y + 50, 150, i);
							}
							break;
					}
					if(Apps.Process.Processes[i].tempInt != 0 && Apps.Process.Processes[i].tempInt2 == 0)
					{
						if(!Explorer.Clicked)
						{
							if(MouseManager.MouseState == MouseState.Left)
							{
								Apps.Process.Processes[i].tempInt = 0;
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

					if (Apps.Process.Processes[i].tempBool)
					{
						InputSystem.CurrentString = Apps.Process.Processes[i].temp;
						Apps.Process.Processes[i].tempBool = false;
					}

					Apps.Process.Processes[i].temp = InputSystem.CurrentString;
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + (SizeX / 2) - 248, Y + (SizeY / 2) - 98, 500, 200);
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X + (SizeX / 2) - 250, Y + (SizeY / 2) - 100, 500, 200);
					StringsAcitons.DrawCenteredString("Save File", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 100, 18, Color.White, Kernel.font18);
					StringsAcitons.DrawCenteredString(">>> " + Apps.Process.Processes[i].temp + "|", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 60, 18, Color.White, Kernel.font18);
					//	StringsAcitons.DrawCenteredString(@"path: 0:\Users\" + Kernel.loggedUser + @"\Desktop", 500, X + (SizeX / 2) - 240, Y + (SizeY / 2) - 20, 18, Color.White, Kernel.font18);
					Apps.Process.Processes[i].tempInt = 0;

				}


			}
			else if (Apps.Process.Processes[i].saving)
			{
				Apps.Process.Processes[i].saving = false;
				Apps.Process.Processes[i].tempInt = 0;
			}
			else
				Apps.Process.Processes[i].tempInt = 0;


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
						Apps.Process.Processes[ProcessID].tempInt2 = 1;
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
								Apps.Process.Processes[ProcessID].tempInt = 1;
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
			for (int i = 0; i < Apps.Process.Processes.Count; i++)
			{
				if (Apps.Process.Processes[i].ID == 2)
				{
					if (Apps.Process.Processes[i].selected)
					{
						int lineToAdd = Apps.Process.Processes[i].defaultLines.Count - 1;

						//Batch.RunCommand(Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count - 1].text, i);
					
				
						Apps.Process.Processes[i].CurrLine++;
						if (Apps.Process.Processes[i].defaultLines.Count >= (Apps.Process.Processes[i].SizeY - 50) / 18)
						{
							Apps.Process.Processes[i].StartLine++;
							
						}
						
						if (Apps.Process.Processes[i].CurrLine > Apps.Process.Processes[i].defaultLines.Count)
						{
							
							Apps.Process.Processes[i].defaultLines.Add("");

							InputSystem.CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine -1].Substring(Apps.Process.Processes[i].CurrChar); //TU TRZEBA ZMIENIĆ!!1
							Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine-1] = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine - 1].Remove(Apps.Process.Processes[i].CurrChar);
							Apps.Process.Processes[i].CurrChar = 0;
						}
						else
						{
							
							Apps.Process.Processes[i].defaultLines.Insert(Apps.Process.Processes[i].CurrLine, "");
							InputSystem.CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine - 1].Substring(Apps.Process.Processes[i].CurrChar); //TU TRZEBA ZMIENIĆ!!1
							Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine - 1] = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine - 1].Remove(Apps.Process.Processes[i].CurrChar);
							Apps.Process.Processes[i].CurrChar = 0;
						}
					}
				}

			}
		}


		public static void save()
		{
			for (int i = 0; i < Apps.Process.Processes.Count; i++)
			{
				if (Apps.Process.Processes[i].ID == 2)
				{
					if (Apps.Process.Processes[i].selected)
					{
						if (Apps.Process.Processes[i].temp != null)
						{
							if(Apps.Process.Processes[i].tempInt3 == 0 || Apps.Process.Processes[i].tempInt3 == 1)
							{
                                if (Apps.Process.Processes[i].temp.Contains(".txt") || Apps.Process.Processes[i].temp.Contains(".ras") || Apps.Process.Processes[i].temp.Contains("."))
                                {
                                    if (!Apps.Process.Processes[i].saved)
                                    {
                                        if (Kernel.diskReady)
                                        {
                                            var file_stream = File.Create(Apps.Process.Processes[i].temp);
                                            string[] pathArg = Apps.Process.Processes[i].temp.Split(@"\");
                                            Apps.Process.Processes[i].Name = pathArg[pathArg.Length - 1];
                                            file_stream.Close();
                                            string finaleString = "";
                                            for (int j = 0; j < Apps.Process.Processes[i].defaultLines.Count; j++)
                                            {
                                                finaleString += Apps.Process.Processes[i].defaultLines[j] + '\n';
                                            }
                                            File.WriteAllText(Apps.Process.Processes[i].temp, finaleString);
                                            DrawDesktopApps.UpdateIcons();
                                        }
                                        else
                                        {
                                            MessageBoxCreator.CreateMessageBox("Error", "Read only file system!\nPlease format your drive\nand create an account.", MessageBoxCreator.MessageBoxIcon.error, 400);
                                        }
                                        Apps.Process.Processes[i].saved = true;
                                        Apps.Process.Processes[i].saving = false;
                                        InputSystem.CurrentString = Apps.Process.Processes[i].defaultLines[Apps.Process.Processes[i].CurrLine];
                                        Apps.Process.Processes[i].tempInt = 0;

                                    }
                                }
                                else
                                {
                                    Apps.Process.Processes[i].saving = true;
                                    Apps.Process.Processes[i].tempBool = true;

                                }
                            }
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Cannot save read only file!", MessageBoxCreator.MessageBoxIcon.error, 350, 175);

                            }
								
						}
						else
						{
							Apps.Process.Processes[i].saving = true;
							Apps.Process.Processes[i].tempBool = true;
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
			Apps.Process.Processes.Add(MessageBox2);
			Apps.Process.UpdateProcess(Apps.Process.Processes.Count - 1);

		}


	

	}
}
