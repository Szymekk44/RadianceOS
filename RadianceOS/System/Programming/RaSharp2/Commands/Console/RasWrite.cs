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
		public static void Write(int id, string text)
		{
			TextColor line = new TextColor
			{
				text = text,
				color = Apps.Process.Processes[id].RasData.TextColor
			};
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].color = line.color; // Modify empty line;
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text = Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text + line.text;
		}

		public static void WriteLine(int id, string text)
		{
			TextColor empt = new TextColor
			{
				text = "",
				color = Apps.Process.Processes[id].RasData.TextColor
			};
			TextColor line = new TextColor
			{
				text = text,
				color = Apps.Process.Processes[id].RasData.TextColor
			};
			Apps.Process.Processes[id].RasData.lines.Add(empt);
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].color = line.color; // Modify empty line;
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text = Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text + line.text;

		}

		public static void WriteLineError(int id, string text)
		{
			TextColor empt = new TextColor
			{
				text = "",
				color = Color.Red
			};
			TextColor line = new TextColor
			{
				text = text,
				color = Color.Red
			};
			Apps.Process.Processes[id].RasData.lines.Add(empt);
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].color = line.color; // Modify empty line;
			Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text = Apps.Process.Processes[id].RasData.lines[Apps.Process.Processes[id].RasData.lines.Count - 1].text + line.text;

		}

		public static void ReadLine(int id, string Variable)
		{
			Apps.Process.Processes[id].RasData.waitForUserInput = true;
			Apps.Process.Processes[id].RasData.toVariable = Variable;
		}

	}
}
