using RadianceOS.System.Apps;
using RadianceOS.System.Managment;
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
			if (paramets.Count() > 3)
			{
				string finaleString = "";
				string[] fragments = com.Split("+", com.IndexOf("="));
				ReturnString(fragments, ProcessID, com, i);
				Process.Processes[ProcessID].RasData.Variables.Add(paramets[1], finaleString);
			}
			else
			{
				Process.Processes[ProcessID].RasData.Variables.Add(paramets[1], "");
			}
			MessageBoxCreator.CreateMessageBox("Success!", "Success!", MessageBoxCreator.MessageBoxIcon.warning);
		}
		public static string ReturnString(string[] paramets, int ProcessID, string com, int i)
		{
			if (paramets.Length > 1)
			{
				string finaleString = "";
				string[] fragments = paramets;
				for (int j = 0; j < fragments.Length; j++)
				{
					try
					{
						if (fragments[j][0] == '"')
						{
							finaleString += fragments[j].Substring(1, fragments[j].LastIndexOf('"') - 1);
						}
						else
						{
							if (Process.Processes[i].RasData.Variables == null)
							{
								Process.Processes[i].RasData.Variables = new Dictionary<string, string>();
							}
							finaleString += Process.Processes[i].RasData.Variables[fragments[j].ToString()]; //THIS SHIT DOES NOT WORK
						}
						


					}
					catch(Exception e)
					{
						Kernel.Crash("wtf " + e.Message, 0);
					}
				}
				return finaleString;
			}
			else
			{
				return paramets[0];
			}
		}

	}

}
