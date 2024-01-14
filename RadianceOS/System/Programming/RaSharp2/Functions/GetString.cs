using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming.RaSharp;
using RadianceOS.System.Programming.RaSharp2.Commands.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp2.Functions
{
	public static class GetString
	{

		public static void MakeString(string[] paramets, int ProcessID, string com, int i)
		{
			if (paramets.Count() > 2)
			{
				string finaleString = "";
				string temp = com.Substring(com.IndexOf("=") + 1);
				string[] fragments = temp.Split("+");
				finaleString = ReturnString(fragments, ProcessID, com);

				RasExecuter.Data[Process.Processes[ProcessID].DataID].variables.Add(paramets[1], finaleString);
			}
			else
			{
				RasExecuter.Data[Process.Processes[ProcessID].DataID].variables.Add(paramets[1], "empt0x");
			}
		}
		public static string ReturnString(string[] paramets, int ProcessID, string com)
		{
			if (paramets.Length > 1)
			{
				string finaleString = "";
				string[] fragments = paramets;
				for (int j = 0; j < fragments.Length; j++)
				{
					try
					{
						fragments[j] = fragments[j].Trim();
						if (fragments[j][0] == '"')
						{
							finaleString += fragments[j].Substring(1, fragments[j].LastIndexOf('"') - 1);

						}
						else
						{
							string without = fragments[j];
							if(fragments[j].Contains(';'))
							{
								without = without.Replace(';', ' ');
								without = without.Trim();
							}
							if (RasExecuter.Data[Process.Processes[ProcessID].DataID].variables.ContainsKey(without))
							{
								finaleString += RasExecuter.Data[Process.Processes[ProcessID].DataID].variables[without].ToString();
							}
							else if (without == "Console.ReadLine()")
							{
								string temp = com.Trim();
								RasWrite.ReadLine(ProcessID, temp.Substring(0, temp.IndexOf('=')));
								RasExecuter.Data[Process.Processes[ProcessID].DataID].variables[without] = "WaitForInput";
								return "null";
							}
							else
							{
								MessageBoxCreator.CreateMessageBox("Error", "Return String (>1)\nVariable: " + without + " Does not exist!", MessageBoxCreator.MessageBoxIcon.error, 500);
							}
						}
					}
					catch(Exception e)
					{
						Kernel.Crash("Error: " + e.Message, 0);
					}
				}
				return finaleString;
			}
			else
			{
				string finaleString = "";
				string[] fragments = paramets;
				fragments[0] = fragments[0].Trim();
				if (fragments[0][0] == '"')
				{
					finaleString += fragments[0].Substring(1, fragments[0].LastIndexOf('"') - 1);
				}
				else
				{
					string without = fragments[0];
					if (fragments[0].Contains(';'))
					{
						without = without.Replace(';', ' ');
						without = without.Trim();
					}
					if (RasExecuter.Data[Process.Processes[ProcessID].DataID].variables.ContainsKey(fragments[0]))
					{
						finaleString += RasExecuter.Data[Process.Processes[ProcessID].DataID].variables[fragments[0]].ToString();
					}
					else if (without == "Console.ReadLine()")
					{
						string temp = com.Trim();
						RasWrite.ReadLine(ProcessID, temp.Substring(0, temp.IndexOf('=')));
						return "null";
					}
					else
					{
						MessageBoxCreator.CreateMessageBox("Error", "Return String (<1)\nVariable: " + without + " Does not exist!", MessageBoxCreator.MessageBoxIcon.error, 500);
					}
				}
				return finaleString;
			}
		}
		public static void ChangeSrtingInput(int id, string varName, string varValue)
		{
			varName = varName.Trim();
			RasExecuter.Data[Process.Processes[id].DataID].variables[varName] = InputSystem.CurrentString;
		}
	}

}
