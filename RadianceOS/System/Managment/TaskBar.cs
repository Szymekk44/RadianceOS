using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RadianceOS.System.Managment
{
	public static class TaskBar
	{
		public static int LastRefresh = 1000;
		static string formattedTime;
		public static void Render()
		{
			LastRefresh++;
			if(LastRefresh >= 1000)
			{
				formattedTime = DateTime.Now.ToString("HH:mm");
			}
			Explorer.CanvasMain.DrawString(formattedTime, Kernel.font18, Color.White, (int)Explorer.screenSizeX - 55, (int)Explorer.screenSizeY - 29);
			Explorer.CanvasMain.DrawString(formattedTime, Kernel.font18, Color.Black, (int)Explorer.screenSizeX - 54, (int)Explorer.screenSizeY - 28);
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
