using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using Cosmos.System.Graphics;
using RadianceOS.System.Graphic;
using RadianceOS.System.Programming.RaSharp2;
using System.IO;
using RadianceOS.System.Security.FileManagment;

namespace RadianceOS.System.Managment
{
	public static class DrawDesktopApps
	{
		public static List<DesktopIcon> Icons = new List<DesktopIcon>();

		public static bool clicked;
		public static bool stopperGoing;
		public static bool renaming, deleting;
		public static int renamingid, deletingid, monitoreid;
		static DateTime startTime;
		static bool clickedOn;
		static bool mainClick;
		static string lastName;
		static string firstName;
		public static int ReturnID(string extension)
		{
			if (extension == "txt")
				return 1;
			else if (extension == "ras")
				return 2;
			else if (extension == "SysData")
				return 99999;
			else
				return 0;
		}
		public static void UpdateIcons()
		{
			Icons.Clear();
			var files_list = Directory.GetFiles(@"0:\Users\" + Kernel.loggedUser + @"\Desktop");
			int pos = 0;
			for (int i = 0; i < files_list.Length; i++)
			{
				int id = 0;
				int lastDotIndex = files_list[i].LastIndexOf('.');
				if (lastDotIndex != -1 && lastDotIndex < files_list[i].Length - 1)
				{
					string extension = files_list[i].Substring(lastDotIndex + 1);
					id = ReturnID(extension);
					if (id == 99999)
					{
						id = 0;
						continue;
					}
						
				}



				DesktopIcon newicon = new DesktopIcon
				{
					ID = id,
					x = 20,
					y = 20 + (pos * 85),
					dateTime = DateTime.Now,
					path = @"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + files_list[i],
					Name = files_list[i]

				};
				pos++;
				Icons.Add(newicon);
			}
		}



		public static void RenderMenu(int index)
		{
			renaming = false;
			
			int addX = Icons[index].ClickedX;
			int addY = Icons[index].ClickedX;
			switch (Icons[index].ID)
			{
				case 2:
					{


						Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, Icons[index].x + addX + 2, Icons[index].y + addY + 2, 200, 100);
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, Icons[index].x + addX, Icons[index].y + addY, 200, 100);
						if (Cosmos.System.MouseManager.X > Icons[index].x + addX && Cosmos.System.MouseManager.X < Icons[index].x + addX + 200)
						{
							if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 2 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 20)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 2, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{

									RasExecuter.StartScript(Icons[index].path);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
							else if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 20 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 38)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 20, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{

									Notepad.OpenFile(Icons[index].path);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
							else if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 38 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 56)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 38, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{
									CallRename(index);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
							else if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 56 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 74)
							{
								Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 56, 200, 18);
								if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
								{
									CallDelete(index);
									clicked = true;
									Icons[index].selected = false;
									Icons[index].showMenu = false;
								}
							}
						}





						Explorer.CanvasMain.DrawString("Run", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 2);
						Explorer.CanvasMain.DrawString("Edit", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 20);
						Explorer.CanvasMain.DrawString("Rename", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 38);
						Explorer.CanvasMain.DrawString("Delete", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 56);
					}
					break;
				default:
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, Icons[index].x + addX + 2, Icons[index].y + addY + 2, 200, 100);
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, Icons[index].x + addX, Icons[index].y + addY, 200, 100);
					if (Cosmos.System.MouseManager.X > Icons[index].x + addX && Cosmos.System.MouseManager.X < Icons[index].x + addX + 200)
					{
						if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 2 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 20)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 2, 200, 18);
							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{

								Notepad.OpenFile(Icons[index].path);
								clicked = true;
								Icons[index].selected = false;
								Icons[index].showMenu = false;
							}
						}
						else if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 20 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 38)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 20, 200, 18);
							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								CallRename(index);
								clicked = true;
								Icons[index].selected = false;
								Icons[index].showMenu = false;
							}
						}
						else if (Cosmos.System.MouseManager.Y > Icons[index].y + addY + 38 && Cosmos.System.MouseManager.Y < Icons[index].y + addY + 56)
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.lightMain, Icons[index].x + addX, Icons[index].y + addY + 38, 200, 18);
							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								CallDelete(index);
								clicked = true;
								Icons[index].selected = false;
								Icons[index].showMenu = false;
							}
						}
					}
					Explorer.CanvasMain.DrawString("Open", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 2);
					Explorer.CanvasMain.DrawString("Rename", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 20);
					Explorer.CanvasMain.DrawString("Delete", Kernel.font18, Color.White, Icons[index].x + addX + 6, Icons[index].y + addY + 38);
					break;


			}


		}


		public static void CallRename(int id)
		{
			Process.Processes[0].defaultLines = new List<string>();

			Process.Processes[0].defaultLines.Add(Icons[id].Name);
			InputSystem.SpecialCharracters = false;
			InputSystem.AllowArrows = true;
			InputSystem.AllowUpDown = false;
			InputSystem.onlyNumbers = false;
			InputSystem.allowDots = true;
			Process.Processes[0].selected = true;
			for (int i = 1; i < Process.Processes.Count; i++)
			{
				Process.Processes[i].selected = false;
			}
			InputSystem.CurrentString = Icons[id].Name;
			Process.Processes[0].CurrChar = Icons[id].Name.Length;
			firstName = Icons[id].FinaleName;
			renaming = true;
			renamingid = id;
		}

		public static void CallDelete(int id)
		{
			deleting = true;
			deletingid = id;
			MessageBoxCreator.CreateMessageBox("Delete", "Are you sure you want to delete\n" + Icons[id].path, MessageBoxCreator.MessageBoxIcon.warning, 500,175, "Cancel", "Delete");
			monitoreid = Process.Processes[Process.Processes.Count - 1].DataID;
		}

		public static void MonitoreDelete(int fileID, int delID)
		{
			if (MessageBox.closedWith[delID] != 0)
			{
				switch (MessageBox.closedWith[delID])
				{
					case 1:
						{
							deleting = false;
						}
						break;
					case 2:
						{
							deleting = false;
							FileAction.DelteFile(Icons[fileID].path);
							DrawDesktopApps.UpdateIcons();
						}
						break;
				}
			}
		}


		public static void Rename(int id)
		{
			Process.Processes[0].defaultLines[0] = InputSystem.CurrentString;
			InputSystem.Monitore(7, Process.Processes[0].CurrChar, 0, 20);
			string result = InputSystem.CurrentString.Substring(0, Process.Processes[0].CurrChar) + "|" + InputSystem.CurrentString.Substring(Process.Processes[0].CurrChar);
			Icons[id].Name = result;
			if(lastName != Icons[id].Name)
			{
				Icons[id].FinaleName = "";
				string[] commands = Icons[id].Name.Split(' ');
				string finale = "";

				foreach (string command in commands)
				{
					if (finale.Length + command.Length <= 10)
					{
						finale += (string.IsNullOrEmpty(finale) ? "" : " ") + command;
					}
					else
					{
						if (!string.IsNullOrEmpty(finale))
						{
							Icons[id].FinaleName += (string.IsNullOrEmpty(Icons[id].FinaleName) ? "" : "\n") + finale;
						}
						finale = command;
					}
				}

				if (!string.IsNullOrEmpty(finale))
				{
					Icons[id].FinaleName += (string.IsNullOrEmpty(Icons[id].FinaleName) ? "" : "\n") + finale;
				}
			}
		}

		public static void FinishRenaming()
		{

			Icons[renamingid].Name = Icons[renamingid].Name.Trim();
			Icons[renamingid].Name = Icons[renamingid].Name.Replace("|", "");
	
			string temp = Icons[renamingid].Name.Replace("|", "");
			InputSystem.CurrentString = temp;
			Process.Processes[0].defaultLines[0] = temp;
			Process.Processes[0].CurrChar = temp.Length;
			int indexOfPipe = Icons[renamingid].Name.IndexOf('|');
			if (indexOfPipe != -1)
			{
				Icons[renamingid].Name = Icons[renamingid].Name.Remove(indexOfPipe, 1);
			}
			renaming = false;

			if (!File.Exists(@"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + Icons[renamingid].Name))
			{
				//File.Move(Icons[renamingid].path, @"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + Icons[renamingid].Name); NO PLUG?!
				File.Copy(Icons[renamingid].path, @"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + Icons[renamingid].Name);
				File.Delete(Icons[renamingid].path);
				string extension = Icons[renamingid].Name.Substring(Icons[renamingid].Name.LastIndexOf(".") + 1);

				Icons[renamingid].FinaleName = "";
				string[] commands = temp.Split(' ');


				Icons[renamingid].FinaleName = "";

				int currID = ReturnID(extension);
				if (currID != Icons[renamingid].ID)
				{
					Icons[renamingid].ID = currID;
					Render();
				}
				Icons[renamingid].FinaleName = "";
				Icons[renamingid].FinaleName = "";
				DrawDesktopApps.UpdateIcons();
			}
			else
			{

				Icons[renamingid].FinaleName = "";
				Icons[renamingid].FinaleName = "";
				DrawDesktopApps.UpdateIcons();
				MessageBoxCreator.CreateMessageBox("Error", "File " + (@"0:\Users\" + Kernel.loggedUser + @"\Desktop\" + Icons[renamingid].Name) + "\nAlready exists!", MessageBoxCreator.MessageBoxIcon.error, 600);
			}
			
				
		}
	
		public static void Render()
		{
			bool refresh = false;
			clickedOn = false;
			int menuIndex = -1;
			for (int i = 0; i < Icons.Count; i++)
			{
				if (Icons[i].FinaleName == "")
				{
					StringsAcitons.DrawCenteredString("REFRESHING", 1920, 0, 540, 10, Color.White, Kernel.font18);
					string[] commands = Icons[i].Name.Split(' ');
					string finale = "";

					foreach (string command in commands)
					{
						if (finale.Length + command.Length <= 10)
						{
							finale += (string.IsNullOrEmpty(finale) ? "" : " ") + command;
						}
						else
						{
							if (!string.IsNullOrEmpty(finale))
							{
								Icons[i].FinaleName += (string.IsNullOrEmpty(Icons[i].FinaleName) ? "" : "\n") + finale;
							}
							finale = command;
						}
					}

					if (!string.IsNullOrEmpty(finale))
					{
						Icons[i].FinaleName += (string.IsNullOrEmpty(Icons[i].FinaleName) ? "" : "\n") + finale;
					}
				}

				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
					mainClick = true;
				//Explorer.CanvasMain.DrawString(Icons[i].Name, Kernel.font18, Color.White, Icons[i].x + 48, Icons[i].y + 48);
				if (!Icons[i].selected)
					StringsAcitons.DrawCenteredString(Icons[i].FinaleName, 48, Icons[i].x, Icons[i].y + 50, 15, Color.White, Kernel.font16);
				else
					StringsAcitons.DrawCenteredString(Icons[i].FinaleName, 48, Icons[i].x, Icons[i].y + 50, 15, Color.FromArgb(200, 200, 200), Kernel.font16);

				switch (Icons[i].ID)
				{
					case 0://NONE
						{
							if (Icons[i].alphaIcon != null)
							{

								Explorer.CanvasMain.DrawImage(Icons[i].alphaIcon, Icons[i].x, Icons[i].y);
							}
							else
							{
								if (!refresh)
								{
									refresh = true;
									Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
								}
								Explorer.CanvasMain.DrawImageAlpha(Kernel.unknownIcon, Icons[i].x, Icons[i].y);
								Window.GetTempImage(Icons[i].x, Icons[i].y, 48, 48, "File icon");
								Icons[i].alphaIcon = Window.tempBitmap;
							}


							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												Notepad.OpenFile(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}
						}
						break;
					case 1://TXT
						{
							if (Icons[i].alphaIcon != null)
							{
								Explorer.CanvasMain.DrawImage(Icons[i].alphaIcon, Icons[i].x, Icons[i].y);
							}
							else
							{
								if (!refresh)
								{
									refresh = true;
									Explorer.CanvasMain.DrawImage(Kernel.Wallpaper1, 0, 0);
								}
								Explorer.CanvasMain.DrawImageAlpha(Kernel.txtIcon, Icons[i].x, Icons[i].y);
								Window.GetTempImage(Icons[i].x, Icons[i].y, 48, 48, "File icon");
								Icons[i].alphaIcon = Window.tempBitmap;
							}

							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												// Obsługa double click
												//	Explorer.CanvasMain.DrawString("Double click! " + (Icons[i].dateTime - DateTime.Now).TotalMilliseconds, Kernel.font18, Color.White, 500, 500);
												Notepad.OpenFile(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}

						}
						break;

					case 2://Ra#
						{
							Explorer.CanvasMain.DrawImage(Kernel.rasIcon, Icons[i].x + 6, Icons[i].y);

							if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left)
							{
								if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
								{
									if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
									{
										if (!clicked)
										{
											if ((DateTime.Now - Icons[i].dateTime).TotalMilliseconds < 1000)
											{
												// Obsługa double click
												//	Explorer.CanvasMain.DrawString("Double click! " + (Icons[i].dateTime - DateTime.Now).TotalMilliseconds, Kernel.font18, Color.White, 500, 500);
												//	RasExecuter.StartScript(Icons[i].path);
												RasExecuter.StartScript(Icons[i].path);
												clicked = true;
												Icons[i].selected = false;

											}
											else
											{
												Icons[i].dateTime = DateTime.Now;
												clicked = true;
												Icons[i].selected = true;

											}
										}
										clickedOn = true;


									}

								}

							}

						}
						break;
				}

				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Right && !Icons[i].showMenu)
				{
					if (Cosmos.System.MouseManager.X > Icons[i].x && Cosmos.System.MouseManager.X < Icons[i].x + 48)
					{
						if (Cosmos.System.MouseManager.Y > Icons[i].y && Cosmos.System.MouseManager.Y < Icons[i].y + 48)
						{

							for (int j = 0; j < Icons.Count; j++)
							{
								Icons[j].showMenu = false;
							}
							Icons[i].showMenu = true;
							Icons[i].ClickedX = Explorer.MX - Icons[i].x;
							Icons[i].ClickedY = Explorer.MY - Icons[i].y;
						}

					}

				}



				if (Icons[i].showMenu)
					menuIndex = i;

			}

			if (renaming)
			{
				Rename(renamingid);
			}
			if(deleting)
			{
				MonitoreDelete(deletingid, monitoreid);
			}
			if (menuIndex != -1)
			{
				RenderMenu(menuIndex);
			}
			if (clicked)
			{
				if (Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.None)
				{
					clicked = false;
				}

			}
			if (mainClick)
			{
				mainClick = false;
				if (!clickedOn)
				{

					for (int i = 0; i < Icons.Count; i++)
					{
						Icons[i].selected = false;
						Icons[i].showMenu = false;
					}
				}
			}

		}

		public static void clearIcons()
		{
			for (int i = 0; i < Icons.Count; i++)
			{
				Icons[i].alphaIcon = null;
			}
		}
	}

	public class DesktopIcon
	{
		public int ID;
		public string Name;
		public string path;
		public string FinaleName = "";
		public bool selected;
		public int x, y;
		public DateTime dateTime;
		public bool showMenu;
		public Bitmap alphaIcon;
		public int ClickedX, ClickedY;
	}
}
