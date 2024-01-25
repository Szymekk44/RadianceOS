using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOSInstaller.System.Managment
{
	public static class DrawConsole
	{
		public static void DrawLogo()
		{
			Console.Write(CenterText("██████╗  █████╗ ██████╗ ██╗ █████╗ ███╗   ██╗ ██████╗███████╗     ██████╗ ███████╗"));
			Console.Write(CenterText("██╔══██╗██╔══██╗██╔══██╗██║██╔══██╗████╗  ██║██╔════╝██╔════╝    ██╔═══██╗██╔════╝"));
			Console.Write(CenterText("██████╔╝███████║██║  ██║██║███████║██╔██╗ ██║██║     █████╗      ██║   ██║███████╗"));
			Console.Write(CenterText("██╔══██╗██╔══██║██║  ██║██║██╔══██║██║╚██╗██║██║     ██╔══╝      ██║   ██║╚════██║"));
			Console.Write(CenterText("██║  ██║██║  ██║██████╔╝██║██║  ██║██║ ╚████║╚██████╗███████╗    ╚██████╔╝███████║"));
			Console.Write(CenterText("╚═╝  ╚═╝╚═╝  ╚═╝╚═════╝ ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝╚══════╝     ╚═════╝ ╚══════╝"));
		}

	public static string CenterText(string text)
		{
			int consoleWidth = 90;
			int padding = (consoleWidth - text.Length) / 2;
			string centeredText = text.PadLeft(padding + text.Length).PadRight(consoleWidth);
			return centeredText;
		}
	}
}
