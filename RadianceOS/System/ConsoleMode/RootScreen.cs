using RadianceOS.System.Managment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.ConsoleMode
{
	public static class RootScreen
	{
		public static void Display()
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.Clear();
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Write(DrawConsole.CenterText("ROOT"));
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(DrawConsole.CenterText("By logging in as Root you will remove all RadianceOS protection!"));
			Console.Write(DrawConsole.CenterText("If you want to continue, please enter password for " + Kernel.loggedUser));
			string Password = Console.ReadLine();
			string myUser = @"0:\Users\" + Kernel.loggedUser + @"\";
			if (Password == File.ReadAllText(myUser + @"AccountInfo\Password.SysData"))
			{
				Kernel.Root = true;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Clear();
				Kernel.WriteLineOK("Logged in as ROOT");
			}
			else
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Clear();
				Kernel.WriteLineERROR("Wrong Password!");
			}
		}
	}
}
