using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Managment
{
	public static class Design
	{
		public static void ChangeTheme(int theme)
		{
			switch (theme)
			{
				case 0:
					Kernel.main = Color.FromArgb(34, 32, 48);
					Kernel.lightMain = Color.FromArgb(54, 51, 79);
					Kernel.lightlightMain = Color.FromArgb(63, 59, 99);
					Kernel.shadow = Color.FromArgb(26, 24, 36);
					Kernel.middark = Color.FromArgb(19, 18, 26);
					Kernel.dark = Color.FromArgb(16, 16, 20);
					Kernel.fontColor = Color.White;
					Kernel.terminalColor = Color.Black;
					Kernel.startDefault = Color.FromArgb(47, 44, 66);
					Kernel.startLight = Color.FromArgb(56, 51, 82);
					Kernel.startDefaultSelected = Color.FromArgb(41, 36, 66);
					Kernel.startLightSelected = Color.FromArgb(53, 48, 84);
					break;
				case 1:
					Kernel.main = Color.FromArgb(32, 32, 32);
					Kernel.lightMain = Color.FromArgb(61, 61, 61);
					Kernel.lightlightMain = Color.FromArgb(74, 74, 74);
					Kernel.shadow = Color.FromArgb(25, 25, 25);
					Kernel.middark = Color.FromArgb(21, 21, 21);
					Kernel.dark = Color.FromArgb(17, 17, 17);
					Kernel.fontColor = Color.White;
					Kernel.terminalColor = Color.Black;
					Kernel.startDefault = Color.FromArgb(52, 52, 52);
					Kernel.startLight = Color.FromArgb(63, 63, 63);
					Kernel.startDefaultSelected = Color.FromArgb(46, 46, 46);
					Kernel.startLightSelected = Color.FromArgb(61, 61, 61);
					break;
				case 2:
					Kernel.main = Color.FromArgb(240, 240, 240);
					Kernel.lightMain = Color.FromArgb(246, 246, 246);
					Kernel.lightlightMain = Color.FromArgb(255, 255, 255);
					Kernel.shadow = Color.FromArgb(230, 230, 230);
					Kernel.middark = Color.FromArgb(225, 225, 225);
					Kernel.dark = Color.FromArgb(220, 220, 220);
					Kernel.fontColor = Color.Black;
					Kernel.terminalColor = Kernel.main;
					Kernel.startDefault = Color.FromArgb(250, 250, 250);
					Kernel.startLight = Color.FromArgb(255, 255, 255);
					Kernel.startDefaultSelected = Color.FromArgb(245, 245, 245);
					Kernel.startLightSelected = Color.FromArgb(252, 252, 252);
					break;
			}
			Explorer.UpdateIcons();
		}
	}
}
