using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.Render
{
	public static class UICheckBox
	{
		public static void DrawCheckBox(int X, int Y, bool selected, bool mouseOn)
		{
			Color shadow = Kernel.dark;
			Color main = Kernel.middark;
			if (mouseOn)
			{
				main = Kernel.lightlightMain;
				shadow = Kernel.dark;
			}
			Explorer.CanvasMain.DrawFilledRectangle(shadow, X+2, Y+2, 20, 20);
			Explorer.CanvasMain.DrawFilledRectangle(main, X, Y, 20, 20);

			if (selected)
			{
				Explorer.CanvasMain.DrawLine(Kernel.fontColor, X + 3, Y + 10, X + 10, Y + 16);
				Explorer.CanvasMain.DrawLine(Kernel.fontColor, X + 4, Y + 10, X + 10, Y + 15);
				Explorer.CanvasMain.DrawLine(Kernel.fontColor, X + 10, Y + 16, X + 15, Y + 4);
				Explorer.CanvasMain.DrawLine(Kernel.fontColor, X + 10, Y + 15, X + 15, Y + 5);


			}
		}
	}
}
