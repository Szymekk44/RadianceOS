using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2.Commands.Console
{
	public static class RasWrite
	{
		public static void WriteLine(int id, string text)
		{
			TextColor line = new TextColor
			{
				text = text,
				color = Process.Processes[id].RasData.TextColor
			};
			Process.Processes[id].RasData.lines.Add(line);
		}

	}
}
