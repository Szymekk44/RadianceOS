using Cosmos.HAL.Drivers.Video.SVGAII;
using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp2.Commands.Console;
using RadianceOS.System.Programming.RaSharp2.Commands.Draw;
using RadianceOS.System.Programming.RaSharp2.Commands.Window;
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
			for (int i = 0; i < commands.Length; i++)
			{
				string[] dotSplit = commands[i].Split('.');
				dots.Add(dotSplit);

				string[] spaceSplit = commands[i].Split(' ');
				paramets.Add(spaceSplit);
			}
			com = com.Trim();
			if (com.StartsWith("void") || com.StartsWith("//"))
			{
				Process.Processes[ProcessID].RasData.CurrentLine++;
				return;
			}	
				
			for (int i = 0; i < commands.Length-1; i++)
			{
				if (dots[i][0] == "Console")
				{
					string parametr = dots[i][1].Substring(0,dots[i][1].IndexOf("("));
					int startIndex = commands[i].IndexOf("(") + 1;
					int length = commands[i].LastIndexOf(")") - startIndex;
					string textIn = commands[i].Substring(startIndex, length);
					string FinaleString = GetString.ReturnString(textIn.Split("+"), ProcessID, com);

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
				else if (dots[i][0] == "Draw")
				{
					DrawMain.RunCommand(com, ProcessID);
				}
				else if(dots[i][0] == "Window")
				{
					RasWindow.RunCommand(paramets[i].ToArray(), dots[i].ToArray(), com, ProcessID);
				}
				else if (paramets[i][0].ToLower() == "string")
				{
					GetString.MakeString(paramets[i], ProcessID, com, i);
				}
				else if (paramets[i][0].ToLower() == "int")
				{
					GetInt.MakeInt(paramets[i], ProcessID, com, i);
				}
				else if (paramets[i][0].ToLower() == "goto")
				{
					List<string> temp = new List<string>();
					for (int j = 1; j < paramets[i].Length; j++)
					{
						temp.Add(paramets[i][j]);
					}
					string name = GetString.ReturnString(temp.ToArray(), ProcessID, com).Trim();
					if (RasExecuter.Data[Process.Processes[i].DataID].voids.ContainsKey(name))
					{
						Process.Processes[ProcessID].RasData.CurrentLine = RasExecuter.Data[Process.Processes[i].DataID].voids[name];
					}
					else
						MessageBoxCreator.CreateMessageBox("Ra# Error", "Void " + name + " does not exist", MessageBoxCreator.MessageBoxIcon.error);
				}
				else if (RasExecuter.Data[datId].variables.ContainsKey(paramets[i][0]))
				{
					string temp = com.Substring(com.IndexOf("=") + 1);
					string[] fragments = temp.Split("+");
					if (RasExecuter.Data[datId].variables[paramets[i][0]] is string)
					{
						string finaleString = GetString.ReturnString(fragments, ProcessID, com);
						if (finaleString != "null")
							RasExecuter.Data[datId].variables[paramets[i][0]] = finaleString;
					}
					else if(RasExecuter.Data[datId].variables[paramets[i][0]] is int)
					{
						int finaleNumber = GetInt.ReturnInt(i, com, Process.Processes[i].DataID);
						RasExecuter.Data[datId].variables[paramets[i][0]] = finaleNumber;
					}

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
