using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cosmos.System;
using RadianceOS.System.Graphic;
using CosmosTTF;

namespace RadianceOS.System.Managment
{
	public static class TaskBar
	{
		public static int LastRefresh = 1000;
		static string formattedTime;
		static string formattedDate;

		public static bool showTimeMenu;
		public static void Render()
		{
			LastRefresh++;
			if(LastRefresh >= 1000)
			{
				DateTime now = DateTime.Now;
				formattedTime = now.ToString("HH:mm");
				formattedDate = now.ToString("dd/MM/yyyy");
			}
			if(Explorer.MY > (int)Explorer.screenSizeY - 38 && Explorer.MY < (int)Explorer.screenSizeY - 2)
			{
				if(Explorer.MX > (int)Explorer.screenSizeX - 95 && Explorer.MX < (int)Explorer.screenSizeX - 5)
				{
					Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, (int)Explorer.screenSizeX - 95, (int)Explorer.screenSizeY - 38, 91, 36);
					if(MouseManager.MouseState == MouseState.Left && !Explorer.Clicked)
					{
						showTimeMenu = !showTimeMenu;
					}
				}
			}
			Explorer.CanvasMain.DrawString(formattedTime, Kernel.font18, Color.DarkGray, (int)Explorer.screenSizeX - 50, (int)Explorer.screenSizeY - 37);
			Explorer.CanvasMain.DrawString(formattedTime, Kernel.font18, Color.White, (int)Explorer.screenSizeX - 51, (int)Explorer.screenSizeY - 38);

			Explorer.CanvasMain.DrawString(formattedDate, Kernel.font18, Color.DarkGray, (int)Explorer.screenSizeX - 89, (int)Explorer.screenSizeY - 19);
			Explorer.CanvasMain.DrawString(formattedDate, Kernel.font18, Color.White, (int)Explorer.screenSizeX - 90, (int)Explorer.screenSizeY - 20);
			if(showTimeMenu)
			{
				int y = (int)Explorer.screenSizeY - 440;
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.shadow, (int)Explorer.screenSizeX - 455, y, 450, 400);
				Window.DrawRoundedRectangle((int)Explorer.screenSizeX - 455, y - 25, 450, 25, 10, Kernel.shadow, 20);
				//Explorer.CanvasMain.DrawStringTTF(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"), "UMR", Kernel.fontColor, 24, (int)Explorer.screenSizeX - 410, y + 6);
				if (Settings.SettingAllowTTF)
					StringsAcitons.DrawCenteredTTFString(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"), 450, (int)Explorer.screenSizeX - 455, y, 20, Kernel.fontColor, "UMR", 24);
				else
					StringsAcitons.DrawCenteredString(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"), 450, (int)Explorer.screenSizeX - 455, y-20, 20, Kernel.fontColor, Kernel.fontDefault);
				TaskBarCalendar.Render();

			}
			for (int i = 1; i < Process.Processes.Count; i++)
			{
				DrawIcon(Process.Processes[i].Name, i);
			}
			if(Cosmos.System.MouseManager.MouseState == Cosmos.System.MouseState.Left && !Explorer.Clicked)
			{
				if (Explorer.MY > (int)Explorer.screenSizeY - 35 && Explorer.MY < (int)Explorer.screenSizeY - 5)
				{
					for (int i = 1; i < Process.Processes.Count; i++)
					{
						if (Explorer.MX > 100 + ((i - 1) * 210) && Explorer.MX < 300 + ((i - 1) * 210))
						{
							if(Process.Processes[i].hideAble)
							{
								if (Process.Processes[i].hidden)
								{
									Process.Processes[i].hidden = false;
								}
								else
								{
									Process.Processes[i].hidden = true;
								}
							}
						}

					}
				}
			}

		}

		public static void DrawIcon(string name,int ProcessIndex)
		{
			int startX = 100 + ((ProcessIndex-1) * 210);
			if (!Process.Processes[ProcessIndex].hidden)
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, startX + 2, (int)Explorer.screenSizeY - 33, 200, 30);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, startX, (int)Explorer.screenSizeY - 35, 200, 30);
			}
			else
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.dark, startX + 2, (int)Explorer.screenSizeY - 33, 200, 30);
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, startX, (int)Explorer.screenSizeY - 35, 200, 30);
			}
			if(name.Length > 21)
			{
				name = name.Substring(0, 18);
				name += "...";
			}
			Explorer.CanvasMain.DrawString(name, Kernel.font18, Kernel.fontColor, startX + 30, (int)Explorer.screenSizeY - 29);
		}
	}
}
