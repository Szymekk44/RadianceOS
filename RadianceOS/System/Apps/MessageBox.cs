using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RadianceOS.System.Managment;
using Cosmos.System;
using RadianceOS.System.Graphic;

namespace RadianceOS.System.Apps
{
	public static class MessageBox
	{
		public static List<int> closedWith = new List<int>();
		public static void Render(string Title, string[] Descriptions, string Meta, int X, int Y, int SizeX, int SizeY,int index, string Button1, string Button2)
		{
			try
			{
				Window.DrawTop(index, X, Y, SizeX, Title, false, Process.Processes[index].closeAble, true, Process.Processes[index].closeAble);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, SizeX, SizeY-25);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y+25, SizeX, SizeY-25);
				
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, X, Y + SizeY-40, SizeX, 40);
				if(Button1 != null)
				{
					
					if(Explorer.MX > X + SizeX - 125 && Explorer.MX < X + SizeX - 25)
					{
						if(Explorer.MY > Y + SizeY - 40 && Explorer.MY < Y + SizeY)
						{

							Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + SizeX - 125, Y + SizeY - 35, 100, 30);
							StringsAcitons.DrawCenteredString(Button1, 100, X + SizeX - 125, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
							if(MouseManager.MouseState == MouseState.Left)
							{
								if (Process.Processes[index].tempBool)
								closedWith[Process.Processes[index].DataID] = 1;
								Process.Processes.RemoveAt(index);
							}
						}
						else
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + SizeX - 125, Y + SizeY - 35, 100, 30);
							StringsAcitons.DrawCenteredString(Button1, 100, X + SizeX - 125, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
						}


					}
					else
					{
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + SizeX -125, Y + SizeY - 35, 100, 30);
						StringsAcitons.DrawCenteredString(Button1, 100, X + SizeX -125, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
					}
				}

				if (Button2 != null)
				{

					if (Explorer.MX > X + SizeX - 235 && Explorer.MX < X + SizeX - 135)
					{
						if (Explorer.MY > Y + SizeY - 40 && Explorer.MY < Y + SizeY)
						{

							Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, X + SizeX - 235, Y + SizeY - 35, 100, 30);
							StringsAcitons.DrawCenteredString(Button2, 100, X + SizeX - 235, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
							if (MouseManager.MouseState == MouseState.Left)
							{
								if (Process.Processes[index].tempBool)
									closedWith[Process.Processes[index].DataID] = 2;
								Process.Processes.RemoveAt(index);
	
							}
						}
						else
						{
							Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + SizeX - 235, Y + SizeY - 35, 100, 30);
							StringsAcitons.DrawCenteredString(Button2, 100, X + SizeX - 235, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
						}


					}
					else
					{
						Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + SizeX - 235, Y + SizeY - 35, 100, 30);
						StringsAcitons.DrawCenteredString(Button2, 100, X + SizeX - 235, Y + SizeY - 30, 15, Kernel.fontColor, Kernel.font18);
					}
				}



				if (Meta == "warning")
					Explorer.CanvasMain.DrawImageAlpha(Kernel.Error, X + 25, Y + 50);
				else if (Meta == "error")
                    Explorer.CanvasMain.DrawImage(Kernel.Stop, X + 20, Y + 40);
                else if (Meta == "info")
					Explorer.CanvasMain.DrawImage(Kernel.Info, X + 20, Y + 40);
				else if (Meta == "criticalStop")
					Explorer.CanvasMain.DrawImage(Kernel.CriticalStop, X + 20, Y + 40);
				else if (Meta == "diskError")
					Explorer.CanvasMain.DrawImageAlpha(Kernel.DiskError, X + 42, Y + 50);

				for (int i = 0; i < Descriptions.Length; i++)
				{
					Explorer.CanvasMain.DrawString(Descriptions[i], Kernel.font18, Kernel.fontColor, X + 90, i * 16 + 27 + Y);
				}
			}
			catch (Exception ex) 
			{
				Kernel.Crash("Application: " + Title + " performed an illegal operation.", 1);
			}
			
			
		}
	}
}
