using System;
using System.Collections.Generic;

namespace RadianceOS.System.Programming.RaSharp2
{

	public static class RasInterpreter
	{
		public static void RunCommand(string com, int RasID, int ProcessID)
		{
			String[] commands = com.Split(';');
			List<string[]> paramets = new List<string[]>();
			List<string[]> dots = new List<string[]>();
			for (int i = 0; i < commands.Length; i++)
			{
				if (commands[i].Contains('.')) 
				dots.Add(commands[i].Split('.'));

				paramets.Add(commands[i].Split(' '));
			}
		}


	}
}
