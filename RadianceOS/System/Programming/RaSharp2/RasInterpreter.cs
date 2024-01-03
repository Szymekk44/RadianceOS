using Cosmos.HAL.Drivers.Video.SVGAII;
using RadianceOS.System.Apps;
using RadianceOS.System.Programming.RaSharp2.Commands.Console;
using RadianceOS.System.Programming.RaSharp2.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace RadianceOS.System.Programming.RaSharp2
{

	public static class RasInterpreter
	{
		public static void RunCommand(string com, int ProcessID)
		{
			string[] commands = com.Split(';');
			List<string[]> paramets = new List<string[]>();
			List<string[]> dots = new List<string[]>();
			int datId = Process.Processes[ProcessID].DataID;
			// Poprawiona pętla, aby uniknąć błędów związanych z indeksowaniem
			for (int i = 0; i < commands.Length; i++)
			{
				string[] dotSplit = commands[i].Split('.');
				dots.Add(dotSplit);

				string[] spaceSplit = commands[i].Split(' ');
				paramets.Add(spaceSplit);
			}


			for (int i = 0; i < commands.Length-1; i++)
			{
				if (dots[i][0] == "Console")
				{
					string parametr = dots[i][1].Substring(0,dots[i][1].IndexOf("("));
					int startIndex = commands[i].IndexOf("(") + 1;//Dodaj 1, aby zacząć od znaku po "("
					int length = commands[i].LastIndexOf(")") - startIndex;//Określ długość, aby zakończyć na znaku przed ")"
					string textIn = commands[i].Substring(startIndex, length);
					string FinaleString = GetString.ReturnString(textIn.Split("+"), ProcessID, com, i);

					if (parametr == "Write")
					{
						RasWrite.Write(ProcessID, FinaleString);
					}
					else if (parametr == "WriteLine")
					{
						RasWrite.WriteLine(ProcessID, FinaleString);
					}
					else
						ReportError("Unknown parametr!", i, ProcessID);

				}
				else if (paramets[i][0].ToLower() == "string")
				{
					GetString.MakeString(paramets[i], ProcessID, com, i);
				}
				else if (RasExecuter.Data[datId].variables.ContainsKey(paramets[i][0]))
				{
					string temp = com.Substring(com.IndexOf("=") + 1);
					string[] fragments = temp.Split("+");
					string finaleString = GetString.ReturnString(fragments, ProcessID, com, i);
					if (finaleString != "null")
						RasExecuter.Data[datId].variables[paramets[i][0]] = finaleString;
				}
				else
				{
					ReportError("Unknown command!", i, ProcessID);
				}
			}
			Process.Processes[ProcessID].RasData.CurrentLine++;

		}


		public static void ReportError(string error, int line, int ProcessID)
		{
			TextColor err = new TextColor
			{
				text = error + " Line: " + line,
				color = Color.Red
			};
			Process.Processes[ProcessID].RasData.lines.Add(err);
		}

	}
}
