using Cosmos.System.Graphics;
using RadianceOS.System.Apps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Programming.RaSharp
{
	public static class RasPerformer
	{
		public static string[] Commands;
		public static List<RasData> Data = new List<RasData>();
		public static void RunScript(string path)
		{
			if (File.Exists(path))
			{
				string temp = File.ReadAllText(path);
				
				Commands = temp.Split(';');

				string[] pathArg = path.Split(@"\");
				string Name = pathArg[pathArg.Length - 1];
				Processes RaScript = new Processes
				{
					ID = 3,
					Name = "RaSharp Script - " + Name,
					Description = "Script: " + path + " not found!",
					tempInt = Data.Count,
					X = 100,
					Y = 100,
					SizeX = 800,
					SizeY = 500,
					moveAble = true
				};
				for (int i = 1; i < Commands.Length; i++)
				{
					Commands[i] = Commands[i].Substring(1);

				}
			
						 Dictionary<string, int> methodss;
				methodss = GetMethodIndices(temp);
				if(methodss.ContainsKey("Start"))
				{
					Process.Processes.Add(RaScript);
					RasData rasData = new RasData
					{
						Commands = Commands,
						CurrLine = methodss["Start"],
						methods = methodss,
						currVoid = "Start()",
						output = new List<TextColor>()
					};

					Data.Add(rasData);
				}
				else
				{
					Processes MessageBox = new Processes
					{
						ID = 0,
						Name = "Ra# script error",
						Description = "Script: " + path + " is invalid.\nNo starting void!\nMake sure this script has 'void Start()'.",
						metaData = "error",
						X = 100,
						Y = 100,
						SizeX = 600,
						SizeY = 175,
						moveAble = true
					};
					Process.Processes.Add(MessageBox);
					Process.UpdateProcess(Process.Processes.Count - 1);
				}
				
			}
			else
			{
				Processes MessageBox = new Processes
				{
					ID = 0,
					Name = "Ra# script not found",
					Description = "Script: " + path + " not found!",
					metaData = "error",
					X = 100,
					Y = 100,
					SizeX = 300,
					SizeY = 175,
					moveAble = true
				};
				Process.Processes.Add(MessageBox);
				Process.UpdateProcess(Process.Processes.Count - 1);
			}

		}


		public static Dictionary<string, int> GetMethodIndices(string code)
		{
			Dictionary<string, int> methods = new Dictionary<string, int>();
			string[] lines = code.Split('\n');

			int currentIndex = 0;

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i].Trim();

				if (IsMethodDeclaration(line))
				{
					string methodName = GetMethodName(line);
					methods.Add(methodName, currentIndex);
				}

				currentIndex += line.Length + 1; // +1 to account for newline character
			}

			return methods;
		}

		static bool IsMethodDeclaration(string line)
		{
			line = line.Trim();
			return line.StartsWith("void ") && line.EndsWith(")");
		}

		static string GetMethodName(string line)
		{
			line = line.Trim();
			return line.Substring("void ".Length, line.IndexOf("(", StringComparison.Ordinal) - "void ".Length).Trim();
		}

		static string RemoveMethodDeclarations(string code)
		{
			string[] lines = code.Split('\n');
			StringBuilder filteredCode = new StringBuilder();

			foreach (string line in lines)
			{
				if (!IsMethodDeclaration(line))
				{
					filteredCode.AppendLine(line);
				}
			}

			return filteredCode.ToString();
		}

	

	}
}
