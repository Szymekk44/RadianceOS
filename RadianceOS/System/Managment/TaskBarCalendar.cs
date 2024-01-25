using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment
{
	public static class TaskBarCalendar
	{
		public static int Year = 2024;
		public static int Month = 1;

		public static void Render()
		{
		//	Explorer.CanvasMain.DrawString("Days: " + DateTime.DaysInMonth(Year, Month), Kernel.font18, Kernel.fontColor, (int)Explorer.screenSizeX - 200, (int)Explorer.screenSizeY - 400);
			DateTime firstDayOfMonth = new DateTime(Year, Month, 1);
			int dayOfWeekNumber = (int)firstDayOfMonth.DayOfWeek;
			int days = DateTime.DaysInMonth(Year, Month);
			//	Explorer.CanvasMain.DrawString("Starting: " + dayOfWeekNumber.ToString(), Kernel.font18, Kernel.fontColor, (int)Explorer.screenSizeX - 200, (int)Explorer.screenSizeY - 370);
			int currDay = DateTime.Now.Day;
			Explorer.CanvasMain.DrawString("Mon", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Tue", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 1, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Wed", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 2, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Thu", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 3, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Fri", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 4, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Sat", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 5, (int)Explorer.screenSizeY - 420);
			Explorer.CanvasMain.DrawString("Sun", Kernel.fontDefault, Kernel.fontColor, (int)Explorer.screenSizeX - 410 + 57 * 6, (int)Explorer.screenSizeY - 420);
			for (int i = 0; i < days; i++)
			{
				if (i+1 == currDay)
					Explorer.CanvasMain.DrawFilledCircle(Kernel.lightMain, (int)Explorer.screenSizeX - (410 - ((i - (i / 7) * 7) * 57)) + 9, (int)Explorer.screenSizeY - 390 + ((i / 7) * 40) + 9, 18);
				Explorer.CanvasMain.DrawString((i + 1).ToString(), Kernel.font18, Kernel.fontColor, (int)Explorer.screenSizeX - (410 - ((i - (i/7) * 7) * 57)), (int)Explorer.screenSizeY - 390 + ((i/7) * 40));
			}
		}
	}
}
